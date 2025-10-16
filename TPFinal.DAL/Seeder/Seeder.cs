using TPFinal.DAL.Context;
using TPFinal.DAL.Entities;

namespace TPFinal.DAL;

public static class Seeder
{
    public static void SeedData(TPFinalDbContext context)
    {
        // Check if database already has data
        if (context.Clients.Any() || context.Missions.Any() || context.Consultants.Any() || context.Competences.Any() || context.ConsultantCompetences.Any())
            return;

        // Seed Competences
        var competences = new List<Competence>
        {
            new Competence
            {
                Id = Guid.NewGuid(),
                CompetenceTechnique = "C#",
                Categorie = "Développement"
            },
            new Competence
            {
                Id = Guid.NewGuid(),
                CompetenceTechnique = "Cybersécurité",
                Categorie = "Sécurité"
            },
            new Competence
            {
                Id = Guid.NewGuid(),
                CompetenceTechnique = "Gestion de Projet",
                Categorie = "Management"
            },
            new Competence
            {
                Id = Guid.NewGuid(),
                CompetenceTechnique = "Énergie Renouvelable",
                Categorie = "Énergie"
            }
        };
        context.Competences.AddRange(competences);
        context.SaveChanges();

        // Seed Consultants
        var consultants = new List<Consultant>
        {
            new Consultant
            {
                Id = Guid.NewGuid(),
                Nom = "Dupont",
                Prenom = "Jean",
                Email = "jean.dupont@consulting.fr",
                DateEmbauche = new DateTime(2020, 1, 15)
            },
            new Consultant
            {
                Id = Guid.NewGuid(),
                Nom = "Martin",
                Prenom = "Sophie",
                Email = "sophie.martin@consulting.fr",
                DateEmbauche = new DateTime(2019, 6, 1)
            },
            new Consultant
            {
                Id = Guid.NewGuid(),
                Nom = "Lefevre",
                Prenom = "Pierre",
                Email = "pierre.lefevre@consulting.fr",
                DateEmbauche = new DateTime(2021, 3, 10)
            }
        };
        context.Consultants.AddRange(consultants);
        context.SaveChanges();

        // Seed Clients
        var clients = new List<Client>
        {
            new Client
            {
                Id = Guid.NewGuid(),
                NomEntreprise = "TechCorp",
                SecteurActivite = "Technologie",
                Adresse = "123 Rue de l'Innovation, Paris",
                Email = "contact@techcorp.fr"
            },
            new Client
            {
                Id = Guid.NewGuid(),
                NomEntreprise = "HealthSolutions",
                SecteurActivite = "Santé",
                Adresse = "456 Avenue de la Santé, Lyon",
                Email = "info@healthsolutions.fr"
            },
            new Client
            {
                Id = Guid.NewGuid(),
                NomEntreprise = "GreenEnergy",
                SecteurActivite = "Énergie",
                Adresse = "789 Boulevard Éco, Marseille",
                Email = "support@greenenergy.fr"
            }
        };
        context.Clients.AddRange(clients);
        context.SaveChanges();

        // Seed ConsultantCompetences
        var consultantCompetences = new List<ConsultantCompetence>
        {
            new ConsultantCompetence
            {
                ConsultantId = consultants[0].Id,
                CompetenceId = competences[0].Id,
                Niveau = 3
            },
            new ConsultantCompetence
            {
                ConsultantId = consultants[0].Id,
                CompetenceId = competences[2].Id,
                Niveau = 2
            },
            new ConsultantCompetence
            {
                ConsultantId = consultants[1].Id,
                CompetenceId = competences[1].Id,
                Niveau = 4
            },
            new ConsultantCompetence
            {
                ConsultantId = consultants[1].Id,
                CompetenceId = competences[2].Id,
                Niveau = 3
            },
            new ConsultantCompetence
            {
                ConsultantId = consultants[2].Id,
                CompetenceId = competences[3].Id,
                Niveau = 4
            },
            new ConsultantCompetence
            {
                ConsultantId = consultants[2].Id,
                CompetenceId = competences[2].Id,
                Niveau = 2
            }
        };
        context.ConsultantCompetences.AddRange(consultantCompetences);
        context.SaveChanges();

        // Seed Missions
        var missions = new List<Mission>
        {
            new Mission
            {
                Id = Guid.NewGuid(),
                Titre = "Développement d'API",
                Description = "Création d'une API REST pour la gestion des clients.",
                DateDebut = DateTime.Now.AddDays(-30),
                DateFin = DateTime.Now.AddDays(30),
                Budget = 15000.00m,
                ClientId = clients[0].Id,
                ConsultantId = consultants[0].Id // Jean Dupont (C# expert)
            },
            new Mission
            {
                Id = Guid.NewGuid(),
                Titre = "Audit de Sécurité",
                Description = "Analyse de la sécurité du système existant.",
                DateDebut = DateTime.Now.AddDays(-15),
                DateFin = DateTime.Now.AddDays(15),
                Budget = 8000.00m,
                ClientId = clients[0].Id,
                ConsultantId = consultants[1].Id // Sophie Martin (Cybersécurité expert)
            },
            new Mission
            {
                Id = Guid.NewGuid(),
                Titre = "Optimisation de Processus",
                Description = "Amélioration des processus internes de gestion.",
                DateDebut = DateTime.Now.AddDays(-10),
                DateFin = DateTime.Now.AddDays(20),
                Budget = 12000.00m,
                ClientId = clients[1].Id,
                ConsultantId = null // No consultant assigned
            },
            new Mission
            {
                Id = Guid.NewGuid(),
                Titre = "Installation Solaire",
                Description = "Mise en place d'un système solaire pour l'énergie renouvelable.",
                DateDebut = DateTime.Now,
                DateFin = DateTime.Now.AddDays(60),
                Budget = 25000.00m,
                ClientId = clients[2].Id,
                ConsultantId = consultants[2].Id // Pierre Lefevre (Énergie Renouvelable expert)
            }
        };
        context.Missions.AddRange(missions);
        context.SaveChanges();
    }
}