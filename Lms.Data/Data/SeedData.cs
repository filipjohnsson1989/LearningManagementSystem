using Bogus;
using Lms.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lms.Data.Data;

public class SeedData
{
    private static Faker faker = null!;
    private static async Task<IEnumerable<Course>> CourseInitAsync(LmsDbContext lmsDbContext)
    {
        IEnumerable<Course> courses = new List<Course>();
        if (await lmsDbContext.Courses.AnyAsync()) return courses;

        courses = GetCourses();
        await lmsDbContext.AddRangeAsync(courses);
        return courses;
    }
    private static async Task ModuleInitAsync(LmsDbContext lmsDbContext, IEnumerable<Course> courses)
    {
        if (await lmsDbContext.Modules.AnyAsync()) return;

        await lmsDbContext.AddRangeAsync(GetModules(courses));
    }
    public static async Task InitAsync(LmsDbContext lmsDbContext)
    {
        faker = new Faker("en");

        IEnumerable<Course> courses = await CourseInitAsync(lmsDbContext);
        await ModuleInitAsync(lmsDbContext, courses);

        await lmsDbContext.SaveChangesAsync();

        var a = lmsDbContext.Modules.ToList();
    }


    private static IEnumerable<Course> GetCourses()
    {
        var courses = new List<Course>();
        for (int i = 20; i > 0; i--)
        {
            var courseTitle = faker.Random.String(length: i, minChar: 'A', maxChar: 'z');
            var course = new Course()
            {
                Title = $"Course N{i}, {courseTitle}",
                StartDate = faker.Date.Past()
            };
            courses.Add(course);
        }

        return courses;
    }

    private static IEnumerable<Module> GetModules(IEnumerable<Course> courses)
    {
        Random random = new Random();
        var modules = new List<Module>();
        int coursesCount = courses.Count();
        var desierNumberOfModules = 80;
        for (int i = desierNumberOfModules; i > 0; i--)
        {

            var randomCourseIndex = random.Next(0, coursesCount - 1);
            var course = courses.ElementAt(randomCourseIndex)!;

            for (int j = i / coursesCount; j > 0; j--)
            {
                var moduleTitle = faker.Random.String(length: i + j, minChar: 'A', maxChar: 'z');
                var module = new Module()
                {
                    Course = course,
                    Title = $"Module N{j}, {moduleTitle}",
                    StartDate = faker.Date.Future(refDate: course.StartDate)
                };
                modules.Add(module);
            }
        }
        return modules;
    }
}