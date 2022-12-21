using AutoMapper;

namespace {{cookiecutter.ProjectName}}.Persistence.Mapper      

{
    public class BankProfile : Profile

    {
        public  BankProfile()
        {
            CreateMap<Domain.Core.Bank,DAO.Bank>();
        }
    }
}