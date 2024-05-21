using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace ApkaTa.Models
{
    public class AktywnośćUser
    {
        public int AktywnośćId { get; set; }
        [Required(ErrorMessage = "Wprowadź poprawny czas!")]
        public TimeSpan? GodzinaOd { get; set; }
        [Required(ErrorMessage = "Wprowadź godzine zakończenia!")]

        public TimeSpan? GodzinaDo { get; set; }
        [Required(ErrorMessage = "Wprowadź Date Wydarzenia!")]
        
        public DateTime? Data { get; set; }


        [Required(ErrorMessage = "Wprowadź Opis")]
        [MinLength(10, ErrorMessage = "Opis nie moze zawierać mniej niż 10 znaków.")]
        [MaxLength(250, ErrorMessage = "Opis nie moze zawierać więcej niż 250 znaków.")]
        public string Opis { get; set; }


        [Required(ErrorMessage = "Wprowadź Miejsce Docelowe")]
        public string MiejsceDoc { get; set; }

        [Required(ErrorMessage = "Wprowadź Ilość Miejsc")]
        [Range(2, 14, ErrorMessage = "Liczba musi być zakresu 2-14 uczestników")]
        public int? IlośćMiejsc { get; set; }
        public int IdAktywności { get; set; }
        public int Count { get; set; }
    }
}
