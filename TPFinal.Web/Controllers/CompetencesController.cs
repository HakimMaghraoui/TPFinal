using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using TPFinal.Web.Models;
using TPFinal.Business.Models;
using System.Text.Json;

using System.Text;

namespace TPFinal.Web.Controllers
{
    public class CompetencesController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public CompetencesController(IHttpClientFactory httpCompetenceFactory, IConfiguration configuration)
        {
            _httpClient = httpCompetenceFactory.CreateClient();
             _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not configured.");
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }
        // GET: CompetencesController
        public async Task<IActionResult> Index(string search)
        {
            var Competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");
            var competences = await _httpClient.GetFromJsonAsync<List<CompetenceDTO>>("Competences");

            var CompetenceViewModels = Competences?.Select(c => new CompetenceViewModel
            {
                Id =c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie,
              
            }).ToList() ?? new List<CompetenceViewModel>();

            if (!string.IsNullOrEmpty(search))
            {
                CompetenceViewModels = CompetenceViewModels.Where(c =>
                    $"{c.CompetenceTechnique} {c.Categorie}".Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    c.CompetenceTechnique.Contains(search, StringComparison.OrdinalIgnoreCase) ||  c.Categorie.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.Competences = competences?.Select(c => new CompetenceViewModel
            {
                Id = c.Id,
                CompetenceTechnique = c.CompetenceTechnique,
                Categorie = c.Categorie
            }).ToList() ?? new List<CompetenceViewModel>();
            ViewBag.Search = search;

            return View(CompetenceViewModels);
            
        }

        // GET: Competences/Create
        public IActionResult Create() => View();

        // POST: Competences/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetenceViewModel competence)
        {
            if (!ModelState.IsValid)
                return View(competence);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(competence),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("Competences", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la création.");
                return View(competence);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Competences/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetAsync($"Competences/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var Competence = JsonSerializer.Deserialize<CompetenceViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (Competence == null)
                return View("Error");

            var viewModel = new CompetenceViewModel
            {
                Id = Competence.Id,
                CompetenceTechnique = Competence.CompetenceTechnique,
                Categorie = Competence.Categorie
            };

            return View(viewModel);
        }
        // POST: Competences/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CompetenceViewModel competence)
        {
            if (id != competence.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(competence);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(competence),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"Competences/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour.");
                return View(competence);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Competences/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.GetAsync($"Competences/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var competence = JsonSerializer.Deserialize<CompetenceViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (competence == null)
                return View("Error");

            return View(competence);
        }
        // POST: Competences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"Competences/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            return RedirectToAction(nameof(Index));
        }
        // GET: Competences/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _httpClient.GetAsync($"Competences/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var competence = JsonSerializer.Deserialize<CompetenceViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (competence == null)
                return View("Error");

            return View(competence);
        }
    }
}
