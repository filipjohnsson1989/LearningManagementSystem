﻿using AutoMapper;
using Lms.Core.Dto;
using Lms.Core.Entities;

namespace Lms.Data.Data;

public class LmsMappings : Profile
{
    public LmsMappings()
    {
        CreateMap<Course, CourseDto>().ReverseMap();
        
        CreateMap<Module, ModuleDto>().ReverseMap();
        CreateMap<ModuleForCreationDto, Module>();
    }

}
