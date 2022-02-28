using Lms.Core.Validations;
using System.ComponentModel.DataAnnotations;

namespace Lms.Core.Dto;

public class ModuleForCreationDto
{
    [Required]
    [StringLength(maximumLength: 25, MinimumLength = 5)]
    public string Title { get; set; } = null!;
    
    [Required]
    [FutureDate]
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get { return this.StartDate.AddMonths(3); } }
    public int CourseId { get; set; }


}
