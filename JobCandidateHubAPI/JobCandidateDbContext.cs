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
                //add other fluent api configurations as needed 

            });
        }
    }
}
