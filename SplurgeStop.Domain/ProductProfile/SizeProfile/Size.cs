using System;

namespace SplurgeStop.Domain.ProductProfile.SizeProfile
{
    public class Size
    {
        public SizeId Id { get; private set; }
        public string Amount { get; set; }
        public string Unit { get; set; }
        public int PackageSize { get; set; }

        //public string SizeType { get; set; }
        // ??
        // size, measurement, volume, weight, height, length, width, ...

        public static Size Create(SizeId id, string amount)
        {
            var size = new Size();

            size.Apply(new Events.SizeCreated
            {
                Id = id,
                Amount = amount
            });

            return size;
        }

        public void UpdateSizeAmount(string amount)
        {
            Amount = amount ?? throw new ArgumentNullException(nameof(amount), "A valid amount for size must be provided.");
        }

        private void Apply(object @event)
        {
            When(@event);
        }

        private void When(object @event)
        {
            switch (@event)
            {
                case Events.SizeCreated e:
                    Id = new SizeId(e.Id);
                    Amount = e.Amount;
                    break;
                case Events.SizeAmountChanged e:
                    Amount = e.Amount;
                    break;
                case Events.SizeDeleted e:
                    Id = new SizeId(e.Id);
                    Amount = e.Amount;
                    break;
            }
        }

        internal bool EnsureValidState()
        {
            return Id.Value != default
                   && !string.IsNullOrWhiteSpace(Amount);
        }
    }
}