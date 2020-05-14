using System;
using System.Collections.Generic;
using System.Linq;
using GuidHelpers;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.PurchaseTransaction
{
    public class PurchaseTransaction
    {
        private PurchaseDate purchaseDate;

        public PurchaseTransaction()
        {
            Apply(new Events.PurchaseTransactionCreated
            {
                Id = new PurchaseTransactionId(SequentialGuid.NewSequentialGuid())
            });
        }

        public PurchaseTransactionId Id { get; private set; }

        public PurchaseDate PurchaseDate
        {
            get => purchaseDate.Value.Date;
            private set { purchaseDate = value; }
        }

        public Store Store { get; private set; }
        public List<LineItem> LineItems { get; private set; }

        public PurchaseTransactionNotes Notes { get; private set; }

        public IEnumerable<PurchaseTotalSum> TotalPrice => GetTotalSums();

        public IEnumerable<PurchaseTotalSum> GetTotalSums()
        {
            return LineItems.GroupBy(i => i.Price.CurrencyCode)
                .Select(g => new PurchaseTotalSum
                {
                    CurrencyCode = g.Key,
                    TotalSum = g.Sum(x => x.Price.Amount)
                });
        }

        public void AddLineItem(LineItem lineItem)
        {
            if (LineItems is null)
                LineItems = new List<LineItem>();

            LineItems.Add(lineItem);
        }

        public void SetStore(Store store)
            => Apply(new Events.PurchaseTransactionStoreChanged
            {
                Id = Id,
                Store = store
            });

        public void UpdatePurchaseTransactionDate(PurchaseDate date) 
            => Apply(new Events.PurchaseTransactionDateChanged
            {
                Id = Id,
                TransactionDate = date.Value
            });

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.PurchaseTransactionCreated e:
                    Id = new PurchaseTransactionId(e.Id);
                    PurchaseDate = PurchaseDate.Now;
                    Store = null;
                    LineItems = new List<LineItem>();
                    Notes = PurchaseTransactionNotes.NoNotes;
                    break;
                case Events.PurchaseTransactionDateChanged e:
                    PurchaseDate = new PurchaseDate(e.TransactionDate);
                    break;
                case Events.PurchaseTransactionStoreChanged e:
                    Store = e.Store;
                    break;
                case Events.PurchaseTransactionLineItemsChanged e:
                    LineItems.Add(e.LineItem); // TODO: 
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                && PurchaseDate != default
                && Store != null
                && LineItems.Count >= 1;
        }
    }
}
