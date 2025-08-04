using JobBoard.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobBoard.Models
{
    public class RecruiterProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [ValidateNever]
        public JobBoardUser User { get; set; }

        [Required(ErrorMessage = "The company name is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "The VAT number is required.")]
        public string VatNumber { get; set; }

        [Required(ErrorMessage = "The company email is required.")]
        [EmailAddress]
        public string CompanyEmail { get; set; }

        [Required(ErrorMessage = "The phone number is required.")]
        public string Phone { get; set; }

        [ValidateNever]
        public string IdentityDocumentPath { get; set; }

        [ValidateNever]
        public string CompanyCertificatePath { get; set; }

        [ValidateNever]
        public string VatCertificatePath { get; set; }
        public bool IsApproved { get; set; } = false;

        [NotMapped]
        [Required(ErrorMessage = "You must accept the privacy policy.")]
        [Display(Name = "I accept the privacy policy")]
        public bool AcceptPrivacyPolicy { get; set; }


    }
}
