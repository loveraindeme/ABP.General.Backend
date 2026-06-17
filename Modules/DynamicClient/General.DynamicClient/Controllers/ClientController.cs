using General.Backend.Application.Contracts;
using General.Backend.Application.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Http.Client.DynamicProxying;

namespace General.DynamicClient.Controllers
{
    [Route("api/[Controller]")]
    public class ClientController : AbpController
    {
        private readonly IUserAppService _userAppService;
        private readonly IHttpClientProxy<IUserAppService> _httpClientProxy;

        public ClientController(IUserAppService userAppService, 
            IHttpClientProxy<IUserAppService> httpClientProxy)
        {
            _userAppService = userAppService;
            _httpClientProxy = httpClientProxy;
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
            var users = await _httpClientProxy.Service.GetListAsync(new UserQueryDto
            {
                SortType = "asc",
                SortField = "Id"
            });
            return users.Items.ToList();
        }
    }
}
