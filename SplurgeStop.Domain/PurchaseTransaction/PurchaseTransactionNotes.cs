namespace SplurgeStop.Domain.PurchaseTransaction
{
    public sealed class PurchaseTransactionNotes
    {
        public string Value { get; internal set; }

        internal PurchaseTransactionNotes() { }

        internal PurchaseTransactionNotes(string text) => Value = text;

        public static PurchaseTransactionNotes FromString(string text)
            => new PurchaseTransactionNotes(text);

        public static implicit operator string(PurchaseTransactionNotes text)
            => text.Value;

        public static implicit operator PurchaseTransactionNotes(string text)
            => text;

        public static PurchaseTransactionNotes NoNotes
            => new PurchaseTransactionNotes();
    }
}
