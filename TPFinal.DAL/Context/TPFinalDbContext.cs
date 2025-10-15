using Microsoft.EntityFrameworkCore;
using TPFinal.DAL.Entities;

namespace TPFinal.DAL.Context;

public sealed class TPFinalDbContext : DbContext
{
    public DbSet<Client> Clients { get; set; }
    public DbSet<Consultant> Consultants { get; set; }
    public DbSet<Mission> Missions { get; set; }
    public DbSet<ConsultantCompetence> ConsultantCompetences { get; set; }
    public DbSet<Competence> Competences { get; set; }

    public TPFinalDbContext(DbContextOptions<TPFinalDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure ConsultantCompetence with composite key
        modelBuilder.Entity<ConsultantCompetence>()
            .HasKey(cc => new { cc.ConsultantId, cc.CompetenceId });

        // Configure Consultant relationship
        modelBuilder.Entity<ConsultantCompetence>()
            .HasOne(cc => cc.Consultant)
            .WithMany(c => c.Competences)
            .HasForeignKey(cc => cc.ConsultantId);

        // Configure Competence relationship
        modelBuilder.Entity<ConsultantCompetence>()
            .HasOne(cc => cc.Competence)
            .WithMany(c => c.ConsultantCompetences)
            .HasForeignKey(cc => cc.CompetenceId);

        // Configure Mission relationships
        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Consultant) // Explicitly specify navigation property
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.ConsultantId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Mission>()
            .HasOne(m => m.Client) // Explicitly specify navigation property
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
