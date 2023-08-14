using AutoMapper;
using Sprout.Exam.Business.DataTransferObjects;
using Sprout.Exam.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.Business
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, BaseSaveEmployeeDto>().ReverseMap();
            CreateMap<Employee, EditEmployeeDto>()
                .ForMember(dest => dest.Birthdate, x => x.MapFrom(src => src.Birthdate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.TypeId, x => x.MapFrom(src => src.EmployeeTypeId))
                .ReverseMap();
            CreateMap<Employee, CreateEmployeeDto>()
                .ForMember(dest => dest.Birthdate, x => x.MapFrom(src => src.Birthdate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.TypeId, x => x.MapFrom(src => src.EmployeeTypeId))
                .ReverseMap();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Birthdate, x => x.MapFrom(src => src.Birthdate.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.TypeId, x => x.MapFrom(src => src.EmployeeTypeId))
                .ReverseMap();
            CreateMap<EmployeePayrollDto, EmployeePayroll>()
                .ReverseMap();

                    }
    }
}
