
namespace Akces.Unity.Models.SaleChannels
{
    public class Contractor
    {
        public ContractorType Type { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string FullName { get; set; }
        public string VATIN { get; set; }
        public string PESEL { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
    }

    public enum ContractorType
    {
        Company,
        Person,
        Batman
    }
}
