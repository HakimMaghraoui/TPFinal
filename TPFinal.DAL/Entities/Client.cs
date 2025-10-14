using System.ComponentModel.DataAnnotations;

namespace TPFinal.DAL.Entities;

public class Client
{
    [Key]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Nom de l'entreprise est obligatoire")]
    [Display(Name = "Nom de l'entreprise")]
    [StringLength(100, ErrorMessage = "Le nom de l'entreprise ne peut pas dépasser 100 caractères.")]
    public string NomEntreprise { get; set; } = string.Empty;
    [Required(ErrorMessage = "Secteur d'activité est obligatoire")]
    [Display(Name = "Secteur d'activité")]
    [StringLength(50, ErrorMessage = "Le secteur d'activité ne peut pas dépasser 100 caractères.")]
    public string SecteurActivite { get; set; } = string.Empty;
    [Required(ErrorMessage = "Adresse est obligatoire")]
    [Display(Name = "Adresse")]
    public string Adresse { get; set; } = string.Empty;
    [Required(ErrorMessage = "E-mail est obligatoire")]
    [EmailAddress(ErrorMessage = "E-mail est pas valide")]
    [Display(Name = "E-mail")]
    public string Email { get; set; } = string.Empty;
    public ICollection<Mission> Missions { get; set; }
}
