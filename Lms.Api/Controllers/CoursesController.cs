using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;
using Lms.Data.Data;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lms.Api.Controllers;

[ApiController]
[Route("[controller]")]

public class CoursesController : ControllerBase
{
    private readonly LmsDbContext dbContext;
    private readonly IMapper mapper;

    public CoursesController(LmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext ??
            throw new ArgumentNullException(nameof(dbContext));
        this.mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CourseDto>>> GetCourses()
    {
        var model = mapper.ProjectTo<CourseDto>(dbContext.Courses.AsQueryable());
        return Ok(await model.ToListAsync());
    }


    [HttpGet("{courseId}", Name = "GetCourse")]
    public async Task<ActionResult<CourseDto>> GetCourse(int courseId)
    {
        var courseEntity = await dbContext.Courses.FindAsync(courseId);
        if (courseEntity is null)
        {
            return NotFound();
        }
        var courseToReturn = mapper.Map<CourseDto>(courseEntity);
        return Ok(courseToReturn);
    }

    [HttpPatch("{courseId}")]
    public async Task<IActionResult> PatchCourse(int courseId,
        JsonPatchDocument<CourseDto> patchDocument)
    {
        var courseEntity = await dbContext.Courses.FindAsync(courseId);
        if (courseEntity is null)
        {
            return NotFound();
        }
        var courseToPatch = mapper.Map<CourseDto>(courseEntity);
        // add validation
        patchDocument.ApplyTo(courseToPatch, ModelState);

        if (!TryValidateModel(courseToPatch))
        {
            return ValidationProblem(ModelState);
        }

        mapper.Map(courseToPatch, courseEntity);

        dbContext.Update(courseEntity);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{courseId}")]
    public async Task<IActionResult> PutCourse(int courseId, CourseDto courseDto)
    {
        var courseEntity = await dbContext.Courses.FindAsync(courseId);
        if (courseEntity is null)
        {
            return NotFound();
        }
        mapper.Map(courseDto, courseEntity);

        dbContext.Entry(courseEntity).State = EntityState.Modified;

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            return StatusCode(500);

        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<CourseDto>> PostCourse(CourseDto courseDto)
    {
        Course? courseEntity = default;
        try
        {
            //throw new NotImplementedException();
            courseEntity = mapper.Map<Course>(courseDto);
            await dbContext.AddAsync(courseEntity);
            await dbContext.SaveChangesAsync();

            var courseToReturn = mapper.Map<CourseDto>(courseEntity);
            return CreatedAtRoute("GetCourse",
                new { courseId = courseEntity.Id },
                courseToReturn);
        }
        catch (Exception ex)
        {
            if (!(courseEntity?.Id > 0))
                return StatusCode(500);
            else
                return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{courseId}")]
    public async Task<ActionResult> DeleteCourse(int courseId)
    {
        var authorEntity = await dbContext.Courses.FindAsync(courseId);
        if (authorEntity is null)
        {
            return NotFound();
        }
        dbContext.Remove(authorEntity);
        dbContext.SaveChanges();

        return NoContent();
    }
}
