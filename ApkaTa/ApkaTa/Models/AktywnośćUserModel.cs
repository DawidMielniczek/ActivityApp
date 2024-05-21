using System.ComponentModel.DataAnnotations;

namespace ApkaTa.Models
{
    public class AktywnośćUserModel
    {
        public int AktywnośćId { get; set; }
        [Required(ErrorMessage = "Wprowadź poprawny czas!")]
        [DataType(DataType.Time)]
        [EmailAddress(ErrorMessage = "Wprowadź poprawny czas!")]
        public TimeSpan GodzinaOd { get; set; }

        public TimeSpan GodzinaDo { get; set; }

        public DateTime Data { get; set; }

        public string Opis { get; set; }
        public string MiejsceDoc { get; set; }
        public int IlośćMiejsc { get; set; }
        public int IdAktywności { get; set; }
        public int Count { get; set; }
        public string Nazwa { get; set; }
        public string Obrazek { get; set; }
    }
}
