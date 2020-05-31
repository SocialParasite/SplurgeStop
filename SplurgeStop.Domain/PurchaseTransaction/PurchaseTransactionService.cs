using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
using SplurgeStop.Domain.StoreProfile;
using static SplurgeStop.Domain.PurchaseTransaction.Commands;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public sealed class PurchaseTransactionService : IPurchaseTransactionService
    {
        private readonly IPurchaseTransactionRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public PurchaseTransactionService(IPurchaseTransactionRepository repository,
                                          IUnitOfWork unitOfWork)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                SetPurchaseTransactionDate cmd
                    => HandleUpdate(cmd.Id, c => c.UpdatePurchaseTransactionDate(cmd.TransactionDate)),
                SetPurchaseTransactionStore cmd
                    => HandleUpdateAsync(cmd.Id, async c => await UpdateStoreAsync(c, cmd.StoreId)),
                SetPurchaseTransactionLineItem cmd
                    => HandleUpdateAsync(cmd.Id, async c
                        => await UpdateLineItemAsync(c, cmd, LineItemBuilder
                        .LineItem(new Price(cmd.Price, cmd.Booking, cmd.Currency, cmd.CurrencySymbol, cmd.CurrencySymbolPosition),
                                  cmd.LineItemId)
                        .WithNotes(cmd.Notes)
                        .Build())),
                //UpdateLineItem cmd
                //    => HandleUpdate(cmd.Id, async c => await UpdateLineItemAsync(cmd.Id, cmd.LineItem)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newPurchaseTransaction = PurchaseTransaction.Create(cmd.Id);
            await repository.AddPurchaseTransactionAsync(newPurchaseTransaction);

            if (newPurchaseTransaction.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task UpdateStoreAsync(PurchaseTransaction pt, StoreId storeId)
        {
            await repository.ChangeStore(pt, storeId);
        }

        private async Task UpdateLineItemAsync(PurchaseTransaction transaction, SetPurchaseTransactionLineItem command, LineItem lineItem)
        {
            if (command.LineItemId is null)
            {
                transaction.UpdateLineItem(lineItem);
            }
            else
            {
                await repository.ChangeLineItem(transaction, lineItem);
            }
        }

        private async Task HandleUpdateAsync(Guid transactionId, Func<PurchaseTransaction, Task> operation)
        {
            var purchaseTransaction = await repository.LoadPurchaseTransactionAsync(transactionId);

            if (purchaseTransaction == null)
                throw new InvalidOperationException($"Entity with id {transactionId} cannot be found");

            await operation(purchaseTransaction);

            if (purchaseTransaction.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid transactionId, Action<PurchaseTransaction> operation)
        {
            var purchaseTransaction = await repository.LoadPurchaseTransactionAsync(transactionId);

            if (purchaseTransaction == null)
                throw new InvalidOperationException($"Entity with id {transactionId} cannot be found");

            operation(purchaseTransaction);

            if (purchaseTransaction.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactions()
        {
            return await repository.GetAllPurchaseTransactionsAsync();
        }

        public async Task<PurchaseTransaction> GetDetailedPurchaseTransaction(PurchaseTransactionId id)
        {
            return await repository.GetPurchaseTransactionFullAsync(id);
        }
    }
}
