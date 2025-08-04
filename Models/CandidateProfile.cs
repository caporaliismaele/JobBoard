using JobBoard.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;

namespace JobBoard.Models
{
    public class CandidateProfile
    {
        [Key]
        public int Id { get; set; }

        [ValidateNever]
        public string UserId { get; set; }

        [ValidateNever]
        public JobBoardUser User { get; set; }


        [Required(ErrorMessage = "The name is required.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The name must contain 2 to 15 characters.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "The name cannot contain numbers or symbols.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The surname is required.")]
        [StringLength(15, MinimumLength = 2, ErrorMessage = "The last name must contain 2 to 15 characters.")]
        [RegularExpression(@"^[a-zA-ZÀ-ÿ\s]+$", ErrorMessage = "The surname cannot contain numbers or symbols.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "The email is required")]
        [RegularExpression(@"^[\w\.\-]+@([\w\-]+\.)+[a-zA-Z]{2,4}$", ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The telephone number is required.")]
        [RegularExpression(@"^\+?[0-9]{8,15}$", ErrorMessage = "Please enter a valid phone number (numbers only, and optionally the + prefix).")]
        public string Telephone { get; set; }

        [ValidateNever]
        [Display(Name = "CV (PDF)")]
        public string CvFileName { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "You must accept the privacy policy.")]
        [Display(Name = "I accept the privacy policy")]
        public bool AcceptPrivacyPolicy { get; set; }


    }
}
