using General.Backend.Application.Contracts;
using General.Backend.Application.Contracts.Dtos.Users;
using Shouldly;
using Volo.Abp;

namespace General.Backend.Application.Tests.UnitTests
{
    public class UserAppServiceTests : GeneralApplicationTestBase
    {
        private readonly IUserAppService _userAppService;
        
        public UserAppServiceTests() 
        {
            _userAppService = GetRequiredService<IUserAppService>();
        }

        [Fact]
        public async Task Should_Create_A_Valid_User()
        {
            var input = new UserCreateDto
            {
                Account = "krMWLfcpSEHXEVBzpFhhgQ==",
                Password = "u8RSqTgo6+QXdtgIIoeaqQ==",
                Name = "Hui",
                Address = "深圳",
                Contact = null
            };
            await Should.NotThrowAsync(async () =>
            {
                await _userAppService.CreateAsync(input);
            });
        }

        [Fact]
        public async Task Should_Update_A_Valid_User()
        {
            var createInput = new UserCreateDto
            {
                Account = "j9mE3fbCOj3RZToj8sxw9w==",
                Password = "UgcYsv+xFU9wGfSYSpR1oQ==",
                Name = "Rain",
                Address = "深圳",
                Contact = "0023-684"
            };
            await Should.NotThrowAsync(async () =>
            {
                var userDto = await _userAppService.CreateAsync(createInput);
                var updateInput = new UserUpdateDto
                {
                    Id = userDto.Id,
                    Password = userDto.Password,
                    Name = userDto.Name,
                    Address = "广州",
                    Contact = "0025-797"
                };
                await _userAppService.UpdateAsync(updateInput);
            });
        }

        [Fact]
        public async Task Should_Delete_A_User()
        {
            var createInput = new UserCreateDto
            {
                Account = "j9mE3fbCOj3RZToj8sxw9w==",
                Password = "UgcYsv+xFU9wGfSYSpR1oQ==",
                Name = "Rain",
                Address = "深圳",
                Contact = "0023-684"
            };
            var userDto = new UserDto();
            await Should.NotThrowAsync(async () =>
            {
                userDto = await _userAppService.CreateAsync(createInput);
                await _userAppService.DeleteAsync([ userDto.Id ]);
                
            });
            await Should.ThrowAsync<UserFriendlyException>(async () =>
            {
                await _userAppService.GetAsync(userDto.Id);
            });
        }

        [Fact]
        public async Task Should_Get_Some_User()
        {
            var createInput1 = new UserCreateDto
            {
                Account = "krMWLfcpSEHXEVBzpFhhgQ==",
                Password = "u8RSqTgo6+QXdtgIIoeaqQ==",
                Name = "Hui",
                Address = "深圳",
                Contact = "0023-684"
            };
            var createInput2 = new UserCreateDto
            {
                Account = "j9mE3fbCOj3RZToj8sxw9w==",
                Password = "UgcYsv+xFU9wGfSYSpR1oQ==",
                Name = "Rain",
                Address = "广州",
                Contact = "0018-461"
            };

            var result = await WithUnitOfWorkAsync(async () =>
            {
                await Should.NotThrowAsync(async () =>
                {
                    await _userAppService.CreateAsync(createInput1);
                    await _userAppService.CreateAsync(createInput2);
                });
                return await _userAppService.GetListAsync(new UserQueryDto
                {
                    Name = "Rain",
                    PageSize = 10,
                    PageIndex = 1
                });
            });

            result.TotalCount.ShouldBe(1);
        }
    }
}
