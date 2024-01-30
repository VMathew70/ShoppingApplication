using System.ComponentModel.DataAnnotations;

namespace ShoppingApps.Models
{
    public class Customer
    {
        [Required(ErrorMessage ="Enter Customer")]
        public string customername { get; set; }

        [Required(ErrorMessage = "Enter email")]
        public string email { get; set; }

        [Required,MaxLength(16,ErrorMessage = "Enter Card Number (16 digits)"),MinLength(16, ErrorMessage = "Enter Card Number (16 digits)")]
        public string cardnumber { get; set; }

        [Required, MaxLength(3, ErrorMessage = "Enter CVC(3 digits)"), MinLength(3, ErrorMessage = "Enter CVC (3 digits)")]
        public string cvc { get; set; }

        [Required, MaxLength(4, ErrorMessage = "Enter expirydate  (MMYY)"), MinLength(4, ErrorMessage = "Enter expirydate  (MMYY)")]        
        public string expirydate { get; set; }

    }

}
