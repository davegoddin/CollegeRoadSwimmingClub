using CollegeRoadSwimmingClub.Models;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Data
{
    public class CRSCContext : DbContext
    {
        public CRSCContext(DbContextOptions<CRSCContext> options)
            : base(options)
        { }

        public DbSet<Class> Classes { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Gala> Galas { get; set; }
        public DbSet<Member> Members { get; set; }

        public DbSet<MemberSquad> MembersSquads { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<RaceResult> RaceResults { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Squad> Squads { get; set; }
        public DbSet<TrainingResult> TrainingResults { get; set; }
        public DbSet<User> Users { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Squad>()
                .HasMany(s => s.Members)
                .WithMany(s => s.Squads)
                .UsingEntity<MemberSquad>(
                j => j
                .HasOne(ms => ms.Member)
                .WithMany(m => m.MemberSquad)
                .HasForeignKey(ms => ms.MemberId),
                j => j
                .HasOne(ms => ms.Squad)
                .WithMany(s => s.MemberSquad)
                .HasForeignKey(ms => ms.SquadId),
                j => j
                .HasKey(m => new { m.SquadId, m.MemberId, m.MemberRole }));

            

        }

    }
}
