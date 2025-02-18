using AutoMapper;
using CollegeApp.Data;
using CollegeApp.Models;

namespace CollegeApp.Configurations
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() {
            //CreateMap<Student, StudentDTO>();
            //CreateMap<StudentDTO, Student>();

            //CreateMap<StudentDTO, Student>().ForMember(n=>n.StudentName,opt=>opt.MapFrom(x=>x.Name)).ReverseMap();
            //CreateMap<StudentDTO, Student>().ReverseMap().ForMember(n => n.StudentName, opt => opt.Ignore());
            CreateMap<StudentDTO, Student>().ReverseMap().AddTransform<string>(n=>string.IsNullOrEmpty(n)?"No address found": n);


            //CreateMap<StudentDTO, Student>().ReverseMap();
        }
    }
}
