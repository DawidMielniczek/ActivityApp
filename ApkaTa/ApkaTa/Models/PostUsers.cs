using System.ComponentModel.DataAnnotations;

namespace ApkaTa.Models
{
    public class PostUsers
    {
        public int IdPost { get; set; }
        [Required(ErrorMessage = "Wprowadź Opis")]
        [MinLength(35, ErrorMessage = "Opis nie moze zawierać mniej niż 35 znaków.")]
        [MaxLength(500, ErrorMessage = "Opis nie moze zawierać więcej niż 500 znaków.")]
        public string Opis { get; set; }
        public DateTime DataWpisu { get; set; }
        public int IdU { get; set; }
        public int IdAktywności { get; set; }
        public int Wyświetlenia { get; set; }
        [Required(ErrorMessage = "Wprowadź Temat")]
        [MinLength(5, ErrorMessage = "Temat nie moze zawierać mniej niż 5 znaków.")]
        [MaxLength(25, ErrorMessage = "Opis nie moze zawierać więcej niż 25 znaków.")]
        public string Temat { get; set; }
    }
}
