using System.ComponentModel.DataAnnotations;

namespace TPFinal.Business.Models;

public class ConsultantDTO
{
    public Guid ConsultantId { get; set; }
    [Required(ErrorMessage = "Nom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le nom ne peut pas dépasser 50 caractères.")]
    public string Nom { get; set; } = string.Empty;
    [Required(ErrorMessage = "Prenom est obligatoire")]
    [StringLength(50, ErrorMessage = "Le prenom ne peut pas dépasser 50 caractères.")]
    public string Prenom { get; set; } = string.Empty;
    [Required(ErrorMessage = "E-mail est obligatoire")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required(ErrorMessage = "Date d'embauche est obligatoire")]
    public DateTime DateEmbauche { get; set; }
    public List<ConsultantCompetenceDTO> Competences { get; set; } = new List<ConsultantCompetenceDTO>();
    public List<MissionDTO> Missions { get; set; } = new List<MissionDTO>();

}
