namespace Akces.Unity.Models.Communication
{
    public class Product
    {
        /// <summary>
        /// Id w bazie danych
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Symbol towaru
        /// </summary>
        public string Symbol { get; set; }
        /// <summary>
        /// Nazwa towaru
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Kod kreskowy podstawowej jednostki miary
        /// </summary>
        public string EAN { get; set; }
        /// <summary>
        /// Miedzynarodowy kod CN
        /// </summary>
        public string CombinedNomenclature { get; set; }
        /// <summary>
        /// Ilość towaru w magazynie lub na zamówieniu
        /// </summary>
        public decimal Quantity { get; set; }
        /// <summary>
        /// Cena brutto przed rabatem - zamówienie. Cena magazynowa - magazyn
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// Stawka podatku VAT
        /// </summary>
        public string Tax { get; set; }
        /// <summary>
        /// Jednostka miary
        /// </summary>
        public string Currency { get; set; }
        public string Unit { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}
