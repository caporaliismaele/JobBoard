using JobBoard.Areas.Identity.Data;
using JobBoard.Models;
using System.ComponentModel.DataAnnotations.Schema;


namespace JobBoard.Models
{
    public class Candidature
    {
        public int Id { get; set; }

        public int JobOfferId { get; set; }
        public JobOffer JobOffer { get; set; }

        public string CandidateId { get; set; } // contiene UserId

        [ForeignKey("CandidateId")]
        public CandidateProfile Candidate { get; set; }

        public DateTime ApplicationDate { get; set; }
    }

}
