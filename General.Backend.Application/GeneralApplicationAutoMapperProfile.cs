using AutoMapper;
using General.Backend.Application.Contracts.Dtos.Auths;
using General.Backend.Application.Contracts.Dtos.Menus;
using General.Backend.Application.Contracts.Dtos.Roles;
using General.Backend.Application.Contracts.Dtos.Users;

namespace General.Backend.Application
{
    /// <summary>
    /// Profile
    /// </summary>
    public class GeneralApplicationAutoMapperProfile : Profile
    {
        /// <summary>
        /// 
        /// </summary>
        public GeneralApplicationAutoMapperProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<Role, RoleDto>();
            CreateMap<Role, RoleMapDto>();

            CreateMap<Menu, MenuDto>();
            CreateMap<UserPermissionCacheItem, UserPermissionDto>();
            CreateMap<MenuRouterCacheItem, MenuRouterDto>();
            CreateMap<Meta, MetaDto>();
        }
    }
}
