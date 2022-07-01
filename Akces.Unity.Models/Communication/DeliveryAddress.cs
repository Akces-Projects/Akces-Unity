namespace Akces.Unity.Models.Communication
{
    public class DeliveryAddress
    {
        /// <summary>
        /// Nazwa dodatkowa
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ulica oraz numer budynku
        /// </summary>
        public string Line1 { get; set; }

        /// <summary>
        /// Kod pocztowy
        /// </summary>
        public string Line2 { get; set; }

        /// <summary>
        /// Miasto
        /// </summary>
        public string Line3 { get; set; }

        /// <summary>
        /// Kod kraju
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Kraj
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Chyba punkt odbioru (paczkomat??) Np: ""
        /// </summary>
        public string DeliveryPointId { get; set; }

        /// <summary>
        /// Chyba punkt odbioru (paczkomat??) Np: ""
        /// </summary>
        public string DeliveryPointName { get; set; }
    }
}
