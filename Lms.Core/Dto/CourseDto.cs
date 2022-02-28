namespace Lms.Core.Dto;

public class CourseDto
{
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get { return this.StartDate.AddMonths(3); } }
}
