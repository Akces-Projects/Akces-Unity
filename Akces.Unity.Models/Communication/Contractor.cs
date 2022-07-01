namespace Akces.Unity.Models.Communication
{
    public class Contractor
    {
        public ContractorType Type { get; set; }
        /// <summary>
        /// Imię osoby - jeżeli firma, zostawić puste
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Drugie imię osoby - jeżeli firma, zostawić puste
        /// </summary>
        public string SecondName { get; set; }
        /// <summary>
        /// Nazwisko osoby - jeżeli firma, zostawić puste
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Nazwa użytkownika kanału sprzedaży
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Skrocona nazwa firmy - jeżeli osoba, zostawić puste
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Pełna nazwa firmy - jeżeli osoba, zostawić puste
        /// </summary>
        public string FullName { get; set; }
        /// <summary>
        /// NIP firmy - jeżeli osoba, zostawić puste
        /// </summary>
        public string VATIN { get; set; }
        /// <summary>
        /// PESEL osoby - jeżeli firma, zostawić puste
        /// </summary>
        public string PESEL { get; set; }
        /// <summary>
        /// Adres email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Numer telefonu
        /// </summary>
        public string PhoneNumber { get; set; }
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
        /// Kraj
        /// </summary>
        public string Country { get; set; }
        /// <summary>
        /// Kod kraju
        /// </summary>
        public string CountryCode { get; set; }
    }

    public enum ContractorType
    {
        Company,
        Person,
        Batman
    }
}
