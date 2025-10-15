// TPFinal.Web/Controllers/ConsultantsController.cs
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TPFinal.Web.Models;
using TPFinal.Business.Models;
using System.Text.Json;
using TPFinal.Web.Models.Consultants;
using TPFinal.Web.Models.ConsultantCompetences;
using TPFinal.Web.Models.Competences;

namespace TPFinal.Web.Controllers;

public class ConsultantsController : Controller
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;

    public ConsultantsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient();
        _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not configured.");
        _httpClient.BaseAddress = new Uri(_apiBaseUrl);
    }

    // GET: Consultants
    public async Task<IActionResult> Index(string search)
    {
        var consultants = await _httpClient.GetFromJsonAsync<List<ConsultantDTO>>("Consultants");
        var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");

        var consultantViewModels = consultants?.Select(c => new ConsultantViewModel
        {
            ConsultantId = c.ConsultantId,
            Nom = c.Nom,
            Prenom = c.Prenom,
            Email = c.Email,
            DateEmbauche = c.DateEmbauche,
            Competences = c.Competences.Select(cc => new ConsultantCompetenceViewModel
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau,
                CompetenceTechnique = competences?.FirstOrDefault(comp => comp.Id == cc.CompetenceId)?.CompetenceTechnique ?? "Unknown"
            }).ToList()
        }).ToList() ?? new List<ConsultantViewModel>();

        if (!string.IsNullOrEmpty(search))
        {
            consultantViewModels = consultantViewModels.Where(c =>
                $"{c.Nom} {c.Prenom}".Contains(search, StringComparison.OrdinalIgnoreCase) ||
                c.Email.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
        {
            Id = c.Id,
            CompetenceTechnique = c.CompetenceTechnique,
            Categorie = c.Categorie
        }).ToList() ?? new List<CompetenceViewModel>();
        ViewBag.Search = search;

        return View(consultantViewModels);
    }

    // GET: Consultants/Details/{id}
    public async Task<IActionResult> Details(Guid id)
    {
        var consultant = await _httpClient.GetFromJsonAsync<ConsultantDTO>($"Consultants/{id}");
        if (consultant == null)
            return NotFound();

        var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");
        var consultantViewModel = new ConsultantViewModel
        {
            ConsultantId = consultant.ConsultantId,
            Nom = consultant.Nom,
            Prenom = consultant.Prenom,
            Email = consultant.Email,
            DateEmbauche = consultant.DateEmbauche,
            Competences = consultant.Competences.Select(cc => new ConsultantCompetenceViewModel
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau,
                CompetenceTechnique = competences?.FirstOrDefault(c => c.Id == cc.CompetenceId)?.CompetenceTechnique ?? "Unknown"
            }).ToList()
        };

        ViewData["Title"] = $"Détails du Consultant : {consultantViewModel.Nom} {consultantViewModel.Prenom}";
        return View(consultantViewModel);
    }

    // GET: Consultants/Create
    public async Task<IActionResult> Create()
    {
        var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");
        ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
        {
            Id = c.Id,
            CompetenceTechnique = c.CompetenceTechnique,
            Categorie = c.Categorie
        }).ToList() ?? new List<CompetenceViewModel>();
        return View(new ConsultantViewModel { DateEmbauche = DateTime.Today });
    }

    // POST: Consultants/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ConsultantViewModel consultantViewModel, Guid[] selectedCompetenceIds, int[] niveaux)
    {
        if (!ModelState.IsValid)
        {
            var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");
            ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
            {
                Id = c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie
            }).ToList() ?? new List<CompetenceViewModel>();
            return View(consultantViewModel);
        }

        try
        {
            var consultantDto = new ConsultantDTO
            {
                ConsultantId = consultantViewModel.ConsultantId,
                Nom = consultantViewModel.Nom,
                Prenom = consultantViewModel.Prenom,
                Email = consultantViewModel.Email,
                DateEmbauche = consultantViewModel.DateEmbauche
            };

            var response = await _httpClient.PostAsJsonAsync("Consultants", consultantDto);
            response.EnsureSuccessStatusCode();
            var consultantId = await response.Content.ReadFromJsonAsync<Guid>();

            for (int i = 0; i < selectedCompetenceIds.Length; i++)
            {
                var consultantCompetence = new ConsultantCompetenceDTO
                {
                    ConsultantId = consultantId,
                    CompetenceId = selectedCompetenceIds[i],
                    Niveau = niveaux[i]
                };
                await _httpClient.PostAsJsonAsync("ConsultantCompetences", consultantCompetence);
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Erreur lors de la création du consultant : {ex.Message}");
            var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");
            ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
            {
                Id = c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie
            }).ToList() ?? new List<CompetenceViewModel>();
            return View(consultantViewModel);
        }
    }

    // GET: Consultants/Edit/{id}
    public async Task<IActionResult> Edit(Guid id)
    {
        var consultant = await _httpClient.GetFromJsonAsync<ConsultantDTO>($"Consultants/{id}");
        if (consultant == null)
            return NotFound();

        List<CompetenceDTO>? competences = null;
        try
        {
            var response = await _httpClient.GetAsync("Competences");
            if (response.IsSuccessStatusCode)
            {
                competences = await response.Content.ReadFromJsonAsync<List<CompetenceDTO>>();
            }
            else
            {
                Console.WriteLine($"Failed to fetch competences: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching competences: {ex.Message}");
        }

        var consultantViewModel = new ConsultantViewModel
        {
            ConsultantId = consultant.ConsultantId,
            Nom = consultant.Nom,
            Prenom = consultant.Prenom,
            Email = consultant.Email,
            DateEmbauche = consultant.DateEmbauche,
            Competences = consultant.Competences.Select(cc => new ConsultantCompetenceViewModel
            {
                ConsultantId = cc.ConsultantId,
                CompetenceId = cc.CompetenceId,
                Niveau = cc.Niveau,
                CompetenceTechnique = competences?.FirstOrDefault(c => c.Id == cc.CompetenceId)?.CompetenceTechnique ?? "Unknown"
            }).ToList()
        };

        ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
        {
            Id = c.Id,
            CompetenceTechnique = c.CompetenceTechnique,
            Categorie = c.Categorie
        }).ToList() ?? new List<CompetenceViewModel>();
        ViewData["Title"] = $"Modifier Consultant : {consultantViewModel.Nom} {consultantViewModel.Prenom}";
        return View(consultantViewModel);
    }

    // POST: Consultants/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, ConsultantViewModel consultantViewModel, Guid[] selectedCompetenceIds, int[] niveaux)
    {
        if (id != consultantViewModel.ConsultantId)
            return BadRequest();

        if (!ModelState.IsValid)
        {
            List<CompetenceDTO>? competences = null;
            try
            {
                var response = await _httpClient.GetAsync("Competences");
                if (response.IsSuccessStatusCode)
                {
                    competences = await response.Content.ReadFromJsonAsync<List<CompetenceDTO>>();
                }
                else
                {
                    Console.WriteLine($"Failed to fetch competences: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching competences: {ex.Message}");
            }

            ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
            {
                Id = c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie
            }).ToList() ?? new List<CompetenceViewModel>();
            ViewData["Title"] = $"Modifier Consultant : {consultantViewModel.Nom} {consultantViewModel.Prenom}";
            return View(consultantViewModel);
        }

        try
        {
            // Update consultant details
            var consultantDto = new ConsultantDTO
            {
                ConsultantId = consultantViewModel.ConsultantId,
                Nom = consultantViewModel.Nom,
                Prenom = consultantViewModel.Prenom,
                Email = consultantViewModel.Email,
                DateEmbauche = consultantViewModel.DateEmbauche
            };

            var response = await _httpClient.PutAsJsonAsync($"Consultants/{id}", consultantDto);
            response.EnsureSuccessStatusCode();

            // Fetch existing competences
            var existingCompetences = await _httpClient.GetFromJsonAsync<List<ConsultantCompetenceDTO>>($"ConsultantCompetences/{id}") ?? new List<ConsultantCompetenceDTO>();

            // Create a dictionary of submitted competences
            var submittedCompetences = new Dictionary<Guid, int>();
            for (int i = 0; i < selectedCompetenceIds.Length; i++)
            {
                if (i < niveaux.Length)
                {
                    submittedCompetences[selectedCompetenceIds[i]] = niveaux[i];
                }
            }

            // Add or update competences
            foreach (var submitted in submittedCompetences)
            {
                var existing = existingCompetences.FirstOrDefault(c => c.CompetenceId == submitted.Key);
                var consultantCompetence = new ConsultantCompetenceDTO
                {
                    ConsultantId = id,
                    CompetenceId = submitted.Key,
                    Niveau = submitted.Value
                };
                await _httpClient.PostAsJsonAsync("ConsultantCompetences", consultantCompetence);
            }

            // Remove competences that are no longer selected
            foreach (var existing in existingCompetences)
            {
                if (!submittedCompetences.ContainsKey(existing.CompetenceId))
                {
                    await _httpClient.DeleteAsync($"ConsultantCompetences/{id}/{existing.CompetenceId}");
                }
            }

            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException ex)
        {
            ModelState.AddModelError("", $"Erreur lors de la mise à jour du consultant : {ex.Message}");
            List<CompetenceDTO>? competences = null;
            try
            {
                var response = await _httpClient.GetAsync("Competences");
                if (response.IsSuccessStatusCode)
                {
                    competences = await response.Content.ReadFromJsonAsync<List<CompetenceDTO>>();
                }
                else
                {
                    Console.WriteLine($"Failed to fetch competences: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine($"Error fetching competences: {ex2.Message}");
            }

            ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
            {
                Id = c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie
            }).ToList() ?? new List<CompetenceViewModel>();
            ViewData["Title"] = $"Modifier Consultant : {consultantViewModel.Nom} {consultantViewModel.Prenom}";
            return View(consultantViewModel);
        }
    }

    // GET: Consultants/Delete/{id}
    public async Task<IActionResult> Delete(Guid id)
    {
        var consultant = await _httpClient.GetFromJsonAsync<ConsultantDTO>($"Consultants/{id}");
        if (consultant == null)
            return NotFound();

        var consultantViewModel = new ConsultantViewModel
        {
            ConsultantId = consultant.ConsultantId,
            Nom = consultant.Nom,
            Prenom = consultant.Prenom,
            Email = consultant.Email,
            DateEmbauche = consultant.DateEmbauche
        };

        ViewData["Title"] = $"Supprimer Consultant : {consultantViewModel.Nom} {consultantViewModel.Prenom}";
        return View(consultantViewModel);
    }

    // POST: Consultants/Delete/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"Consultants/{id}");
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
        catch (HttpRequestException)
        {
            return NotFound();
        }
    }
}