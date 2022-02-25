namespace Lms.Core.Entities;

public class Course
{
    public int Id { get; set; }
    public string Title { get; set; }
    public DateTime StartDate { get; set; }
    public ICollection<Module> Modules { get; set; }
}
