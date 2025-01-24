using Microsoft.EntityFrameworkCore;

namespace JobCandidateHubAPI
{
    public class JobCandidateDbContext(DbContextOptions<JobCandidateDbContext> options) : DbContext(options)
    {
        public DbSet<Entities.Candidate> Candidates { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Entities.Candidate>(entity =>
            {
                entity.HasKey(e => e.Email);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Comments).IsRequired();
                //add more configurations here
            });
        }
    }
}
