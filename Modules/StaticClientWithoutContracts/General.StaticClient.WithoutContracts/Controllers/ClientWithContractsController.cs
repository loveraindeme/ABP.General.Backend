using General.Backend.Application.Contracts;
using General.Backend.Application.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;

namespace General.StaticClient.WithoutContracts.Controllers
{
    [Route("api/[Controller]")]
    public class ClientWithContractsController : AbpController
    {
        private readonly IUserAppService _userAppService;

        public ClientWithContractsController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet("user/{id}")]
        public async Task<UserDto> GetUserByIdAsync(Guid id)
        {
            var user = await _userAppService.GetAsync(id);
            return user;
        }

        [HttpGet("users")]
        public async Task<List<UserDto>> GetUsersAsync()
        {
            var users = await _userAppService.GetListAsync(new UserQueryDto
            {
                SortType = "asc",
                SortField = "Id"
            });
            return users.Items.ToList();
        }
    }
}
