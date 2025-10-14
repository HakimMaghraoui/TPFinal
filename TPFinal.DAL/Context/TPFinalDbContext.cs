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

        modelBuilder.Entity<ConsultantCompetence>()
            .HasOne(cc => cc.Consultant)
            .WithMany(c => c.Competences)
            .HasForeignKey(cc => cc.ConsultantId);

        modelBuilder.Entity<ConsultantCompetence>()
            .HasOne(cc => cc.Competence)
            .WithMany()
            .HasForeignKey(cc => cc.CompetenceId);

        modelBuilder.Entity<Mission>()
            .HasOne<Consultant>()
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.ConsultantId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Mission>()
            .HasOne<Client>()
            .WithMany(c => c.Missions)
            .HasForeignKey(m => m.ClientId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
