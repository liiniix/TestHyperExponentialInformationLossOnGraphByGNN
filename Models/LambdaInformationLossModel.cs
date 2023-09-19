using System;

namespace Models
{
    public class LambdaInformationLossModel
    {
        public float Lambda { get; set; }
        public int Step { get; set; }
        public IEnumerable<double> DiscountingWeight { get; set; } = Enumerable.Empty<double>();
    }
}
