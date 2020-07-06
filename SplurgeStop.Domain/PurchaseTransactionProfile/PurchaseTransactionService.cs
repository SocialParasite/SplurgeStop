using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;
using SplurgeStop.Domain.Shared;
using SplurgeStop.Domain.StoreProfile;
using static SplurgeStop.Domain.PurchaseTransactionProfile.Commands;

namespace SplurgeStop.Domain.PurchaseTransactionProfile
{
    public sealed class PurchaseTransactionService : IPurchaseTransactionService
    {
        private readonly IPurchaseTransactionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public PurchaseTransactionService(IPurchaseTransactionRepository repository,
                                          IUnitOfWork unitOfWork)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this._unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task Handle(object command)
        {
            return command switch
            {
                CreateFull cmd => HandleCreateFull(cmd),
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
                        .WithProduct(cmd.Product)
                        .Build())),
                DeletePurchaseTransaction cmd => HandleUpdateAsync(cmd.Id, _ => this._repository.RemoveAsync(cmd.Id)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreateFull(CreateFull cmd)
        {
            if (await _repository.ExistsAsync(cmd.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var newPurchaseTransaction = PurchaseTransaction.Create(cmd.Id);
            newPurchaseTransaction.UpdatePurchaseTransactionDate(cmd.TransactionDate);
            var store = await _repository.GetStoreAsync(cmd.StoreId);
            newPurchaseTransaction.UpdateStore(store);

            foreach (var lineItem in cmd.LineItems)
            {
                var prod = await _repository.GetProductAsync(lineItem.Product.Id);

                var newLineItem = LineItemBuilder
                            .LineItem(new Price(decimal.Parse(lineItem.Price, CultureInfo.InvariantCulture),
                            lineItem.Booking,
                            lineItem.CurrencyCode,
                            lineItem.CurrencySymbol,
                            lineItem.CurrencySymbolPosition))
                            .WithNotes(lineItem.Notes)
                            .WithProduct(prod)
                            .Build();

                newPurchaseTransaction.UpdateLineItem(newLineItem);
            }

            await _repository.AddAsync(newPurchaseTransaction);

            if (newPurchaseTransaction.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task UpdateStoreAsync(PurchaseTransaction pt, StoreId storeId)
        {
            await _repository.ChangeStore(pt, storeId);
        }

        private async Task UpdateLineItemAsync(PurchaseTransaction transaction, SetPurchaseTransactionLineItem command, LineItem lineItem)
        {
            if (command.LineItemId is null)
            {
                transaction.UpdateLineItem(lineItem);
            }
            else
            {
                await _repository.ChangeLineItem(transaction, lineItem);
            }
        }

        private async Task HandleUpdateAsync(Guid transactionId, Func<PurchaseTransaction, Task> operation)
        {
            var purchaseTransaction = await _repository.LoadAsync(transactionId);

            if (purchaseTransaction == null)
                throw new InvalidOperationException($"Entity with id {transactionId} cannot be found");

            await operation(purchaseTransaction);

            if (purchaseTransaction.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        private async Task HandleUpdate(Guid transactionId, Action<PurchaseTransaction> operation)
        {
            var purchaseTransaction = await _repository.LoadAsync(transactionId);

            if (purchaseTransaction == null)
                throw new InvalidOperationException($"Entity with id {transactionId} cannot be found");

            operation(purchaseTransaction);

            if (purchaseTransaction.EnsureValidState())
            {
                await _unitOfWork.Commit();
            }
        }

        public async Task<IEnumerable<PurchaseTransactionStripped>> GetAllPurchaseTransactions()
        {
            return await _repository.GetAllDtoAsync();
        }

        public async Task<PurchaseTransaction> GetDetailedPurchaseTransaction(PurchaseTransactionId id)
        {
            var purchaseTransaction = await _repository.GetAsync(id);

            //HACK: ?? to prevent PurchaseTransaction nav prop causing problem in serializatio to json
            foreach (var item in purchaseTransaction.LineItems)
            {
                item.PurchaseTransaction = null;
            }

            return purchaseTransaction;
        }
    }
}
