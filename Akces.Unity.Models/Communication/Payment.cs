using System;

namespace Akces.Unity.Models.Communication
{
    public class Payment
    {
        /// <summary>
        /// Sposób płatności
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// Waluta
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Kwota
        /// </summary>
        public decimal Value { get; set; }

        /// <summary>
        /// Termin płatności
        /// </summary>
        public DateTime? TimeLimit { get; set; }
    }
}
