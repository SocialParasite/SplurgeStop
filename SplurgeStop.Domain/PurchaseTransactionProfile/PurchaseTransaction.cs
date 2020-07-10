using System;
using System.Collections.Generic;
using System.Linq;
using SplurgeStop.Domain.PurchaseTransactionProfile.LineItemProfile;
using SplurgeStop.Domain.PurchaseTransactionProfile.PriceProfile;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.Domain.PurchaseTransactionProfile
{
    public class PurchaseTransaction
    {
        private PurchaseDate _purchaseDate;

        internal static PurchaseTransaction Create(PurchaseTransactionId id)
        {
            var purchaseTransaction = new PurchaseTransaction();

            purchaseTransaction.Apply(new Events.PurchaseTransactionCreated
            {
                Id = id
            });

            return purchaseTransaction;
        }

        public static PurchaseTransaction CreateFull(PurchaseTransactionId id,
            Store store,
            ICollection<LineItemStripped> lineItems,
            PurchaseDate transactionDate)
        {
            ValidateParameters();

            var purchaseTransaction = new PurchaseTransaction();

            purchaseTransaction.Apply(new Events.PurchaseTransactionFullCreated
            {
                Id = id,
                Store = store,
                LineItems = lineItems,
                TransactionDate = (DateTime)transactionDate
            });

            return purchaseTransaction;

            void ValidateParameters()
            {
                if (id is null)
                    throw new ArgumentNullException(nameof(id), "Purchase transaction without valid identifier cannot be created.");
                if (store is null)
                    throw new ArgumentNullException(nameof(id), "Purchase transaction without valid store cannot be created.");
                if (lineItems is null)
                    throw new ArgumentNullException(nameof(id), "Purchase transaction without at least one line item cannot be created.");
                if (transactionDate is null)
                    throw new ArgumentNullException(nameof(id), "Purchase transaction without purchase date cannot be created.");
            }
        }


        public PurchaseTransaction()
        {
            LineItems ??= new List<LineItem>();
        }

        public PurchaseTransactionId Id { get; private set; }

        public PurchaseDate PurchaseDate
        {
            get => _purchaseDate.Value.Date;
            private set => _purchaseDate = value;
        }

        public Store Store { get; private set; }
        public List<LineItem> LineItems { get; private set; }

        public PurchaseTransactionNotes Notes { get; private set; }

        public string TotalPrice => GetTotalSum();

        private string GetTotalSum()
        {
            if (LineItems.Count > 0)
            {
                var credit = LineItems.Where(i => i.Price.Booking == Booking.Credit).Sum(i => i.Price.Amount);
                var debit = LineItems.Where(i => i.Price.Booking == Booking.Debit).Sum(i => i.Price.Amount);
                var currency = LineItems.Select(i => i.Price.Currency).First();

                return new Money((credit - debit), currency).ToString();
            }

            return "N/A";
        }

        public void UpdateStore(Store store)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store), "Invalid store provided. Update failed.");
        }

        public void UpdateLineItem(LineItem lineItem)
        {
            Apply(new Events.PurchaseTransactionLineItemChanged
            {
                Id = Id,
                LineItem = lineItem
            });
        }

        public void UpdatePurchaseTransactionDate(PurchaseDate date)
        {
            Apply(new Events.PurchaseTransactionDateChanged
            {
                Id = Id,
                TransactionDate = date.Value
            });
        }

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
                case Events.PurchaseTransactionFullCreated e:
                    Id = new PurchaseTransactionId((Guid)e.Id);
                    PurchaseDate = PurchaseDate.Now;
                    Store = e.Store;
                    LineItems = new List<LineItem>();
                    Notes = PurchaseTransactionNotes.NoNotes;
                    break;
                case Events.PurchaseTransactionDateChanged e:
                    PurchaseDate = new PurchaseDate(e.TransactionDate);
                    break;
                case Events.PurchaseTransactionLineItemChanged e:
                    if (LineItems.Any(l => l.Id == e.LineItem.Id))
                    {
                        LineItems.Remove(LineItems.Find(l => l.Id == e.LineItem.Id));
                    }
                    LineItems.Add(e.LineItem);
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                && PurchaseDate != default;
            //&& Store != null
            //&& LineItems.Count >= 1;
        }
    }
}
