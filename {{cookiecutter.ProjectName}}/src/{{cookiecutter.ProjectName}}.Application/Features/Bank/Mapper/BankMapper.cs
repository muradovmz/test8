using AutoMapper;
using {{cookiecutter.ProjectName}}.Application.Features.Bank.Response;

namespace {{cookiecutter.ProjectName}}.Application.Features.Bank.Mapper
{
    /// <summary>
    /// We can define all mapping in these mapping classes
    /// </summary>
    public class BankMapper : Profile
    {
        public BankMapper() =>
            CreateMap<Persistence.DAO.Bank, BankResponse>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.IfscCode, opt => opt.MapFrom(src => src.IfscCode));
    }
}