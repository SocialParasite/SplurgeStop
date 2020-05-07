using System;
using System.Threading.Tasks;
using SplurgeStop.Domain.DA_Interfaces;
using static SplurgeStop.Domain.PurchaseTransaction.Commands;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public sealed class PurchaseTransactionService
    {
        private readonly IPurchaseTransactionRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public PurchaseTransactionService(IPurchaseTransactionRepository repository,
                                          IUnitOfWork unitOfWork)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        // TODO:
        // Create(Guid/PurchaseTransaction)
        // SetStore(Guid, Store)
        // UpdateNotes(Guid, string)
        // Update LineItems(Guid, LineItem)
        // Publish?? (Guid) - if valid
        // UpdateDate(Guid, date)

        public Task Handle(object command)
        {
            return command switch
            {
                Create cmd => HandleCreate(cmd),
                SetPurchaseTransactionDate cmd
                    => HandleUpdate(cmd.Id, c => c.SetTransactionDate(cmd.TransactionDate)),
                _ => Task.CompletedTask
            };
        }

        private async Task HandleCreate(Create cmd)
        {
            if (await repository.Exists(cmd.Transaction.Id))
                throw new InvalidOperationException($"Entity with id {cmd.Transaction.Id} already exists");

            await repository.Add(cmd.Transaction);
        }

        private async Task HandleUpdate(Guid transactionId, Action<PurchaseTransaction> operation)
        {
            var purchaseTransaction = await repository.Load(transactionId);

            if (purchaseTransaction == null)
                throw new InvalidOperationException($"Entity with id {transactionId} cannot be found");

            operation(purchaseTransaction);

            if (purchaseTransaction.EnsureValidState())
            {
                await unitOfWork.Commit();
            }
        }
    }
}
