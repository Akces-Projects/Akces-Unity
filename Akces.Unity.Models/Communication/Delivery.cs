namespace Akces.Unity.Models.Communication
{
    public class Delivery
    {
        /// <summary>
        /// Adres dostawy
        /// </summary>
        public DeliveryAddress DeliveryAddress { get; set; }

        /// <summary>
        /// Koszt dostawy
        /// </summary>
        public decimal DeliveryCost { get; set; }

        /// <summary>
        /// Stawka VAT usługi dostawy
        /// </summary>
        public string DeliveryTax { get; set; }

        /// <summary>
        /// Sposób dostawy
        /// </summary>
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Numer przesyłki
        /// </summary>
        public string PackageNumber { get; set; }
    }
}
