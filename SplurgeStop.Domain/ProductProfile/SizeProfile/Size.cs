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
    }
}