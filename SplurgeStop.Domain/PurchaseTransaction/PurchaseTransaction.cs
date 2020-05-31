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

        public static PurchaseTransaction Create(PurchaseTransactionId id)
        {
            var purchaseTransaction = new PurchaseTransaction();

            purchaseTransaction.Apply(new Events.PurchaseTransactionCreated
            {
                Id = id
            });

            return purchaseTransaction;
        }

        public PurchaseTransaction()
        {
            if (LineItems is null)
            {
                LineItems = new List<LineItem>();
            }
        }

        public PurchaseTransactionId Id { get; private set; }

        public PurchaseDate PurchaseDate
        {
            get => purchaseDate.Value.Date;
            private set { purchaseDate = value; }
        }

        public Store Store { get; set; }
        public List<LineItem> LineItems { get; private set; }

        public PurchaseTransactionNotes Notes { get; private set; }

        public string TotalPrice => GetTotalSum();

        private string GetTotalSum()
        {
            var credit = LineItems.Where(i => i.Price.Booking == Booking.Credit).Sum(i => i.Price.Amount);
            var debit = LineItems.Where(i => i.Price.Booking == Booking.Debit).Sum(i => i.Price.Amount);
            var currency = LineItems.Select(i => i.Price.Currency).First();

            return new Money((credit - debit), currency).ToString();
        }

        internal void UpdateLineItem(LineItem lineItem)
        {
            Apply(new Events.PurchaseTransactionLineItemChanged
            {
                Id = Id,
                LineItem = lineItem
            });
        }

        internal void UpdatePurchaseTransactionDate(PurchaseDate date)
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
                case Events.LineItemChanged e:
                    var test = this;
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
            return true;
            //return Id.Value != default
            //    && PurchaseDate != default
            //    && Store != null
            //    && LineItems.Count >= 1;
        }
    }
}
