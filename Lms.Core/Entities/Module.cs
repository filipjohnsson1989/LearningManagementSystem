namespace Lms.Core.Entities;

public class Module
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public Course Course { get; set; } = null!;
}
