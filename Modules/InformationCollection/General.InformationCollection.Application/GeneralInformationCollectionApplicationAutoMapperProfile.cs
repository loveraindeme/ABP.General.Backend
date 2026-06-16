using AutoMapper;
using General.InformationCollection.Application.Contracts.Dtos.Modules;
using General.InformationCollection.Domain.Entities;

namespace General.InformationCollection.Application
{
    /// <summary>
    /// Profile
    /// </summary>
    public class GeneralInformationCollectionApplicationAutoMapperProfile : Profile
    {
        public GeneralInformationCollectionApplicationAutoMapperProfile()
        {
            CreateMap<Module, ModuleDto>();
        }
    }
}
