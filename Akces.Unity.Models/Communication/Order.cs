using System;
using System.Collections.Generic;

namespace Akces.Unity.Models.Communication
{
    public class Order
    {
        public string SourceSaleChannelName { get; set; }

        /// <summary>
        /// Numer oryginału, Numer zewnętrzny. Dla Arttoru imię i nazwisko zamawiającego. Dla Mossy numer zamówienia. Najlepiej jako "{nazwa kanału}/{id zamowienia z kanału}
        /// </summary>
        public string Original { get; set; }

        /// <summary>
        /// Podmiot - zamawiający. Wymagany!
        /// </summary>
        public Contractor Purchaser { get; set; }

        /// <summary>
        /// Data oryginału, jako data żłożenia zamówienia w sklepie internetowym
        /// </summary>
        public DateTime OriginalDate { get; set; }

        /// <summary>
        /// Termin realizacji. Musi być późniejszy niż moment dodania dokumentu
        /// </summary>
        public DateTime? CompletionDate { get; set; }

        /// <summary>
        /// Miejsce sprzedaży oryginału. Najlepiej InvoiceCity
        /// </summary>
        public string OriginalPlace { get; set; }

        /// <summary>
        /// Oddział. Jeżeli brak oddziału w zamówieniu, podać nazwę lub symbol kraju. W ostateczności można zostawić puste
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Magazyn. Jeżeli brak, zostawić puste
        /// </summary>
        public string Warehouse { get; set; }

        /// <summary>
        /// Tytuł
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Uwagi
        /// </summary>
        public string Annotation { get; set; }

        /// <summary>
        /// Waluta zamówienia
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Pozycje (towary) na zamówieniu
        /// </summary>
        public List<Product> Products { get; set; }

        /// <summary>
        /// Dane płatności (obsługuje płatność podzieloną)
        /// </summary>
        public List<Payment> Payments { get; set; }

        /// <summary>
        /// Dane dostawy
        /// </summary>
        public Delivery Delivery { get; set; }
        public string Subtitle { get; set; }
        public bool Confirmed { get; set; }
    }
}
