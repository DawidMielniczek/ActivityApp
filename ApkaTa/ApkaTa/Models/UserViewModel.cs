using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApkaTa.Models
{
    public class UserViewModel
    {
        public int IdU { get; set; }
        [Required(ErrorMessage ="Wprowadź poprawny Email!")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage ="Wprowadź poprawny Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Wprowadź hasło!")]
        [DataType(DataType.Password)]
        [PasswordPropertyText]
        public string Password { get; set; }
        [Required(ErrorMessage = "Wprowadź Nick")]
        [StringLength(12,ErrorMessage ="Nick nie moze być dłuższy niż 12 znaków.")]
        public string Nick { get; set; }
        
        public string Płeć { get; set; }
        [Required(ErrorMessage = "Wprowadź Wage")]
        [Range(40,200, ErrorMessage ="Waga musi być zakresu 40-200 kg")]
        public double Waga { get; set; }
        public string Bmi { get; set; }
        [Required(ErrorMessage = "Wprowadź Wzrost")]
        [Range(120, 200, ErrorMessage = "Wzrost musi być zakresu 120-250 cm")]
        public int Wzrost { get; set; }
        
        public DateTime RokUrodzenia { get; set; }
        
        public bool czyZalogowany { get; set; }
    }
}
