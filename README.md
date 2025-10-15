# TPFinal

TPFinal est une application web développée en ASP.NET Core Razor Pages (.NET 8) pour la gestion de missions de consultants, de clients et de compétences. Le projet est structuré en plusieurs couches (DAL, Business, API, Web) pour une architecture claire et évolutive.

## Fonctionnalités principales

- **Gestion des clients** : création, modification, suppression, consultation des clients et de leurs missions associées.
- **Gestion des missions** : gestion complète des missions (CRUD), association à un client et à un consultant.
- **Gestion des consultants et compétences** : gestion des consultants, de leurs compétences et de leur affectation aux missions.
- **API REST** : endpoints pour l’accès et la manipulation des données (clients, missions, consultants, compétences).
- **Interface web** : interface utilisateur basée sur Razor Pages pour l’administration et la consultation.

## Structure du projet

- `TPFinal.DAL` : couche d’accès aux données (Entity Framework Core, entités, DbContext).
- `TPFinal.Business` : logique métier, services, modèles de transfert de données (DTO).
- `TPFinal.API` : API RESTful pour l’accès aux données.
- `TPFinal.Web` : application web Razor Pages (MVC), modèles de vue, pages et vues partagées.

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server (ou autre SGBD compatible avec EF Core)
- Visual Studio 2022 ou supérieur

## Installation et démarrage

1. **Cloner le dépôt**
   
2. **Configurer la base de données**
   - Modifier la chaîne de connexion dans `appsettings.json` des projets `API` et `Web` si nécessaire.
   - Appliquer les migrations Entity Framework Core :
     ```bash
     dotnet ef database update --project TPFinal.DAL
     ```

3. **Lancer l’API**

4. **Lancer l’application web**

5. Accéder à l’interface web via [http://localhost:5000](http://localhost:5000) (ou le port configuré).

## Technologies utilisées

- ASP.NET Core Razor Pages
- Entity Framework Core
- C#
- SQL Server
- REST API

## Organisation du code

- **Modèles** : `TPFinal.Business.Models`, `TPFinal.DAL.Entities`
- **Services** : `TPFinal.Business.Services`
- **Contrôleurs API** : `TPFinal.API.Controllers`
- **Pages Razor** : `TPFinal.Web.Pages`
- **Vues partagées** : `TPFinal.Web.Views.Shared`

---

*Projet réalisé dans le cadre d’une formation AJC.*
