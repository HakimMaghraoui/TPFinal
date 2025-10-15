using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using TPFinal.Business.Abstractions;
using TPFinal.Business.Services;
using TPFinal.DAL.Entities;
using TPFinal.Web.Models.Clients;

namespace TPFinal.Web.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;

        public ClientsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? throw new InvalidOperationException("API base URL not configured.");
            _httpClient.BaseAddress = new Uri(_apiBaseUrl);
        }

        // GET: Clients
        public async Task<IActionResult> Index(string search)
        {
            var response = await _httpClient.GetAsync("clients");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var clients = JsonSerializer.Deserialize<List<ClientViewModel>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (clients == null)
                return View("Error");

            if (!string.IsNullOrEmpty(search))
            {
                clients = clients.Where(c =>
                    c.NomEntreprise.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            ViewBag.Search = search;
            ViewData["Title"] = "Liste des Clients";
            return View(clients);
        }

        // GET: Clients/Create
        public IActionResult Create() => View();

        // POST: Clients/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClientViewModel client)
        {
            if (!ModelState.IsValid)
                return View(client);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(client),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("clients", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la création.");
                return View(client);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpClient.GetAsync($"clients/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var client = JsonSerializer.Deserialize<ClientViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (client == null)
                return View("Error");

            var viewModel = new ClientCreateOrUpdateViewModel
            {
                Id = client.Id,
                NomEntreprise = client.NomEntreprise,
                SecteurActivite = client.SecteurActivite,
                Adresse = client.Adresse,
                Email = client.Email,
                Missions = client.Missions
            };

            return View(viewModel);
        }
        // POST: Clients/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, ClientViewModel client)
        {
            if (id != client.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(client);

            var jsonContent = new StringContent(
                JsonSerializer.Serialize(client),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PutAsync($"clients/{id}", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Erreur lors de la mise à jour.");
                return View(client);
            }

            return RedirectToAction(nameof(Index));
        }
        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpClient.GetAsync($"clients/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var client = JsonSerializer.Deserialize<ClientViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (client == null)
                return View("Error");

            return View(client);
        }
        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"clients/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            return RedirectToAction(nameof(Index));
        }
        // GET: Clients/Details/5
        public async Task<IActionResult> Details(Guid id)
        {
            var response = await _httpClient.GetAsync($"clients/{id}");

            if (!response.IsSuccessStatusCode)
                return View("Error");

            var json = await response.Content.ReadAsStringAsync();
            var client = JsonSerializer.Deserialize<ClientViewModel>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (client == null)
                return View("Error");

            return View(client);
        }
    }
}
