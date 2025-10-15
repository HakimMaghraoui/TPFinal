// TPFinal.Web/Controllers/MissionsController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TPFinal.Web.Models;
using TPFinal.Web.Models.Clients;
using TPFinal.Business.Models;
using TPFinal.Web.Models.Missions;
using TPFinal.Web.Models.Consultants;

namespace TPFinal.Web.Controllers;

public class MissionsController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public MissionsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not configured.");
        _httpClient.BaseAddress = new Uri(_apiBaseUrl);
    }

    // GET: Missions
    public async Task<IActionResult> Index(string search)
    {
        var missions = await _httpClient.GetFromJsonAsync<List<MissionDTO>>("Missions");
        var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

        var missionViewModels = missions?.Select(m => new MissionViewModel
        {
            Id = m.Id,
            Titre = m.Titre,
            Description = m.Description,
            DateDebut = m.DateDebut,
            DateFin = m.DateFin,
            Budget = m.Budget,
            ClientId = m.ClientId,
            ConsultantId = m.ConsultantId,
            ClientNomEntreprise = clients?.FirstOrDefault(c => c.Id == m.ClientId)?.NomEntreprise ?? "Unknown",
            ConsultantNomPrenom = m.ConsultantId.HasValue
                ? $"{consultants?.FirstOrDefault(c => c.ConsultantId == m.ConsultantId)?.Nom} {consultants?.FirstOrDefault(c => c.ConsultantId == m.ConsultantId)?.Prenom}"
                : "Aucun"
        }).ToList() ?? new List<MissionViewModel>();

        if (!string.IsNullOrEmpty(search))
        {
            missionViewModels = missionViewModels.Where(m =>
                m.Titre.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        ViewBag.Search = search;
        ViewData["Title"] = "Liste des Missions";
        return View(missionViewModels);
    }

    // GET: Missions/Create
    public async Task<IActionResult> Create()
    {
        var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

        ViewBag.Clients = clients?.Select(c => new ClientViewModel
        {
            Id = c.Id,
            NomEntreprise = c.NomEntreprise
        }).ToList() ?? new List<ClientViewModel>();

        ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
        {
            ConsultantId = c.ConsultantId,
            Nom = c.Nom,
            Prenom = c.Prenom
        }).ToList() ?? new List<ConsultantViewModel>();

        return View(new MissionViewModel { DateDebut = DateTime.Today, DateFin = DateTime.Today});
    }

    // POST: Missions/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MissionViewModel missionViewModel)
    {
        if (!ModelState.IsValid)
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
            var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

            ViewBag.Clients = clients?.Select(c => new ClientViewModel
            {
                Id = c.Id,
                NomEntreprise = c.NomEntreprise
            }).ToList() ?? new List<ClientViewModel>();

            ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
            {
                ConsultantId = c.ConsultantId,
                Nom = c.Nom,
                Prenom = c.Prenom
            }).ToList() ?? new List<ConsultantViewModel>();

            return View(missionViewModel);
        }

        try
        {
            var missionDto = new MissionDTO
            {
                Id = missionViewModel.Id,
                Titre = missionViewModel.Titre,
                Description = missionViewModel.Description,
                DateDebut = missionViewModel.DateDebut,
                DateFin = missionViewModel.DateFin,
                Budget = missionViewModel.Budget,
                ClientId = missionViewModel.ClientId,
                ConsultantId = missionViewModel.ConsultantId
            };

            var response = await _httpClient.PostAsJsonAsync("Missions", missionDto);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Erreur lors de la création de la mission : {ex.Message}");
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
            var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

            ViewBag.Clients = clients?.Select(c => new ClientViewModel
            {
                Id = c.Id,
                NomEntreprise = c.NomEntreprise
            }).ToList() ?? new List<ClientViewModel>();

            ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
            {
                ConsultantId = c.ConsultantId,
                Nom = c.Nom,
                Prenom = c.Prenom
            }).ToList() ?? new List<ConsultantViewModel>();

            return View(missionViewModel);
        }
    }

    // GET: Missions/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        var mission = await _httpClient.GetFromJsonAsync<MissionDTO>($"Missions/{id}");
        if (mission == null)
            return NotFound();

        var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");
        var client = clients?.FirstOrDefault(c => c.Id == mission.ClientId);
        var consultant = consultants?.FirstOrDefault(c => c.ConsultantId == mission.ConsultantId);

        var missionViewModel = new MissionViewModel
        {
            Id = mission.Id,
            Titre = mission.Titre,
            Description = mission.Description,
            DateDebut = mission.DateDebut,
            DateFin = mission.DateFin,
            Budget = mission.Budget,
            ClientId = mission.ClientId,
            ConsultantId = mission.ConsultantId,
            ClientNomEntreprise = client?.NomEntreprise ?? "Unknown",
            ConsultantNomPrenom = mission.ConsultantId.HasValue
                ? $"{consultant?.Nom} {consultant?.Prenom}" : "Aucun"
        };

        ViewBag.Clients = clients?.Select(c => new ClientViewModel
        {
            Id = c.Id,
            NomEntreprise = c.NomEntreprise
        }).ToList() ?? new List<ClientViewModel>();

        ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
        {
            ConsultantId = c.ConsultantId,
            Nom = c.Nom,
            Prenom = c.Prenom
        }).ToList() ?? new List<ConsultantViewModel>();

        ViewData["Title"] = $"Modifier Mission : {missionViewModel.Titre}";
        return View(missionViewModel);
    }

    // POST: Missions/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, MissionViewModel missionViewModel)
    {
        if (id != missionViewModel.Id)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
            var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

            ViewBag.Clients = clients?.Select(c => new ClientViewModel
            {
                Id = c.Id,
                NomEntreprise = c.NomEntreprise
            }).ToList() ?? new List<ClientViewModel>();

            ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
            {
                ConsultantId = c.ConsultantId,
                Nom = c.Nom,
                Prenom = c.Prenom
            }).ToList() ?? new List<ConsultantViewModel>();

            ViewData["Title"] = $"Modifier Mission : {missionViewModel.Titre}";
            return View(missionViewModel);
        }

        try
        {
            var missionDto = new MissionDTO
            {
                Id = missionViewModel.Id,
                Titre = missionViewModel.Titre,
                Description = missionViewModel.Description,
                DateDebut = missionViewModel.DateDebut,
                DateFin = missionViewModel.DateFin,
                Budget = missionViewModel.Budget,
                ClientId = missionViewModel.ClientId,
                ConsultantId = missionViewModel.ConsultantId
            };

            var response = await _httpClient.PutAsJsonAsync($"Missions/{id}", missionDto);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Erreur lors de la mise à jour de la mission : {ex.Message}");
            var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
            var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");

            ViewBag.Clients = clients?.Select(c => new ClientViewModel
            {
                Id = c.Id,
                NomEntreprise = c.NomEntreprise
            }).ToList() ?? new List<ClientViewModel>();

            ViewBag.Consultants = consultants?.Select(c => new ConsultantViewModel
            {
                ConsultantId = c.ConsultantId,
                Nom = c.Nom,
                Prenom = c.Prenom
            }).ToList() ?? new List<ConsultantViewModel>();

            ViewData["Title"] = $"Modifier Mission : {missionViewModel.Titre}";
            return View(missionViewModel);
        }
    }

    // GET: Missions/Details/{id}
    public async Task<IActionResult> Details(Guid id)
    {
        var mission = await _httpClient.GetFromJsonAsync<MissionDTO>($"Missions/{id}");
        if (mission == null)
            return NotFound();

        var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");
        var client = clients?.FirstOrDefault(c => c.Id == mission.ClientId);
        var consultant = consultants?.FirstOrDefault(c => c.ConsultantId == mission.ConsultantId);

        var missionViewModel = new MissionViewModel
        {
            Id = mission.Id,
            Titre = mission.Titre,
            Description = mission.Description,
            DateDebut = mission.DateDebut,
            DateFin = mission.DateFin,
            Budget = mission.Budget,
            ClientId = mission.ClientId,
            ConsultantId = mission.ConsultantId,
            ClientNomEntreprise = client?.NomEntreprise ?? "Unknown",
            ConsultantNomPrenom = mission.ConsultantId.HasValue
                ? $"{consultant?.Nom} {consultant?.Prenom}" : "Aucun"
        };

        ViewData["Title"] = $"Détails de la Mission : {missionViewModel.Titre}";
        return View(missionViewModel);
    }

    // GET: Missions/Delete/{id}
    public async Task<IActionResult> Delete(Guid id)
    {
        var mission = await _httpClient.GetFromJsonAsync<MissionDTO>($"Missions/{id}");
        if (mission == null)
            return NotFound();

        var clients = await _httpClient.GetFromJsonAsync<List<ClientDTO>>("Clients");
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");
        var client = clients?.FirstOrDefault(c => c.Id == mission.ClientId);
        var consultant = consultants?.FirstOrDefault(c => c.ConsultantId == mission.ConsultantId);

        var missionViewModel = new MissionViewModel
        {
            Id = mission.Id,
            Titre = mission.Titre,
            Description = mission.Description,
            DateDebut = mission.DateDebut,
            DateFin = mission.DateFin,
            Budget = mission.Budget,
            ClientId = mission.ClientId,
            ConsultantId = mission.ConsultantId,
            ClientNomEntreprise = client?.NomEntreprise ?? "Unknown",
            ConsultantNomPrenom = mission.ConsultantId.HasValue
                ? $"{consultant?.Nom} {consultant?.Prenom}" : "Aucun"
        };

        ViewData["Title"] = $"Supprimer Mission : {missionViewModel.Titre}";
        return View(missionViewModel);
    }

    // POST: Missions/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Missions/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}