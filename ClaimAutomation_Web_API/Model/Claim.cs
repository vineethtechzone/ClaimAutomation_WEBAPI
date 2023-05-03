

namespace ClaimAutomation.Model
{
    public class ClaimModel
    {
        public string cost_centre { get; set; }
        public string payment_method { get; set; }

        public string vendor { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public decimal taxrate { get; set; }
        public decimal totalExcludingTax { get; set; }
        public decimal salestaxamt { get; set; }
        public decimal total { get; set; }
    }
}
