using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TPFinal.DAL.Entities;

public class ConsultantCompetence
{
    [Key]
    public Guid Id { get; set; }
    [ForeignKey("Consultant")]
    public Guid ConsultantId { get; set; }
    public Consultant? Consultant { get; set; }
    [ForeignKey("Competence")]
    public Guid CompetenceId { get; set; }
    public Competence? Competence { get; set; }
    [Required(ErrorMessage = "Niveau d'expertise est obligatoire")]
    [Display(Name = "Niveau d'expertise")]
    public int Niveau { get; set; } = 1;
}
