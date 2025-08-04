using System.ComponentModel.DataAnnotations;

namespace JobBoard.Models
{
    public class JobOffer
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Company is required.")]
        public string Company { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Position is required.")]
        public string Position { get; set; }

        public DateTime PublishedDate { get; set; } = DateTime.Now;

        public string RecruiterId { get; set; }

    }
}
