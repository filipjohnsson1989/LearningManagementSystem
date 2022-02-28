using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lms.Data.Data;
using Lms.Core.Entities;
using AutoMapper;
using Lms.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Lms.Api.Controllers;

[Route("courses/{courseId}/[controller]")]
[ApiController]
public class ModulesController : ControllerBase
{
    private readonly LmsDbContext dbContext;
    private readonly IMapper mapper;


    public ModulesController(LmsDbContext dbContext, IMapper mapper)
    {
        this.dbContext = dbContext ??
            throw new ArgumentNullException(nameof(dbContext));
        this.mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ModuleDto>>> GetModulesForCourse(int courseId)
    {
        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        var model = mapper.ProjectTo<ModuleDto>(dbContext.Modules
            .Where(m => m.CourseId == courseId)
            .AsQueryable());

        return Ok(await model.ToListAsync());
    }

    [HttpGet("{moduleId}", Name = "GetModule")]
    public async Task<ActionResult<ModuleDto>> GetModuleForCourse(int courseId,
        int moduleId)
    {
        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        var moduleEntity = await dbContext.Modules
            .Where(m =>
            m.CourseId == courseId &&
            m.Id == moduleId)
            .FirstOrDefaultAsync();

        if (moduleEntity == null)
        {
            return NotFound();
        }
        var moduleToReturn = mapper.Map<ModuleDto>(moduleEntity);

        return moduleToReturn;
    }

    [HttpPatch("{moduleId}")]
    public async Task<IActionResult> PatchModule(int courseId,
        int moduleId,
        JsonPatchDocument<ModuleDto> patchDocument)
    {
        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        var moduleEntity = await dbContext.Modules
            .Where(m =>
            m.CourseId == courseId &&
            m.Id == moduleId)
            .FirstOrDefaultAsync();

        if (moduleEntity is null)
        {
            return NotFound();
        }
        var moduleToPatch = mapper.Map<ModuleDto>(moduleEntity);
        // add validation
        patchDocument.ApplyTo(moduleToPatch, ModelState);

        if (!TryValidateModel(moduleToPatch))
        {
            return ValidationProblem(ModelState);
        }

        mapper.Map(moduleToPatch, moduleEntity);

        dbContext.Update(moduleEntity);

        await dbContext.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{moduleId}")]
    public async Task<IActionResult> PutModule(int courseId,
        int moduleId,
        ModuleDto moduleDto)
    {
        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        var moduleEntity = await dbContext.Modules
            .Where(m =>
            m.CourseId == courseId &&
            m.Id == moduleId)
            .FirstOrDefaultAsync();

        if (moduleEntity is null)
        {
            return NotFound();
        }
        mapper.Map(moduleDto, moduleEntity);

        dbContext.Entry(moduleEntity).State = EntityState.Modified;

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
    public async Task<ActionResult<ModuleDto>> PostModule(int courseId,
        ModuleForCreationDto moduleForCreationDto)
    {
        if (courseId != moduleForCreationDto.CourseId)
        {
            return BadRequest();
        }

        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        Module? moduleEntity = default;
        try
        {
            //throw new NotImplementedException();
            moduleEntity = mapper.Map<Module>(moduleForCreationDto);
            await dbContext.AddAsync(moduleEntity);
            await dbContext.SaveChangesAsync();

            var moduleToReturn = mapper.Map<ModuleDto>(moduleEntity);
            return CreatedAtRoute("GetModuleForCourse",
                new
                {
                    courseId = courseId,
                    moduleId = moduleEntity.Id
                },
                moduleToReturn);
        }
        catch (Exception ex)
        {
            if (!(moduleEntity?.Id > 0))
                return StatusCode(500);
            else
                return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete("{moduleId}")]
    public async Task<IActionResult> DeleteModule(int courseId, int moduleId)
    {
        var courseExist = await dbContext.Courses
            .AnyAsync(c => c.Id == courseId);

        if (!courseExist)
        {
            return NotFound();
        }

        var moduleEntity = await dbContext.Modules
            .Where(m =>
            m.CourseId == courseId &&
            m.Id == moduleId)
            .FirstOrDefaultAsync();


        if (moduleEntity == null)
        {
            return NotFound();
        }

        dbContext.Remove(moduleEntity);
        await dbContext.SaveChangesAsync();

        return NoContent();
    }

}
