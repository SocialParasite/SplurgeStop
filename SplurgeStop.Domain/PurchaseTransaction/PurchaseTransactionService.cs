using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction.DTO;
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
                    => HandleUpdate(cmd.Id, c => c.UpdateStore(cmd.Store)),
                SetPurchaseTransactionLineItem cmd
                    => HandleUpdate(cmd.Id, c => c.UpdateLineItem(cmd.LineItem)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.ExistsAsync(cmd.Transaction.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Transaction.Id} already exists");

            await repository.AddPurchaseTransactionAsync(cmd.Transaction);
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
