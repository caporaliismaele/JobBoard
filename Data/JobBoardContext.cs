using JobBoard.Areas.Identity.Data;
using JobBoard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace JobBoard.Data;

public class JobBoardContext : IdentityDbContext<JobBoardUser>
{
    public JobBoardContext(DbContextOptions<JobBoardContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CandidateProfile>()
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<CandidateProfile>(p => p.UserId)
            .HasPrincipalKey<JobBoardUser>(u => u.Id);

        builder.Entity<Candidature>()
            .HasOne(c => c.Candidate)
            .WithMany()
            .HasForeignKey(c => c.CandidateId)
            .HasPrincipalKey(p => p.UserId); // NOTA: UserId è stringa


    }


    public DbSet<JobOffer> JobOffers { get; set; }
    public DbSet<CandidateProfile> CandidateProfiles { get; set; }
    public DbSet<RecruiterProfile> RecruiterProfiles { get; set; }

    public DbSet<Candidature> Candidatures { get; set; }




}
