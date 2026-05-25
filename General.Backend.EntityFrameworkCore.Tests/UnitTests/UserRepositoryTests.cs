namespace General.Backend.EntityFrameworkCore.Tests.UnitTests
{
    public class UserRepositoryTests : GeneralEntityFrameworkCoreTestBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IGuidGenerator _guidGenerator;

        public UserRepositoryTests()
        {
            _userRepository = GetRequiredService<IUserRepository>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task Should_Insert_A_Valid_User()
        {
            var user = new User(
                _guidGenerator.Create(),
                "krMWLfcpSEHXEVBzpFhhgQ==",
                "u8RSqTgo6+QXdtgIIoeaqQ==",
                "Hui");

            await WithUnitOfWorkAsync(async () =>
            {
                await _userRepository.InsertAsync(user);
            });
            var result = await WithUnitOfWorkAsync(async () =>
            {
                return await _userRepository.FirstOrDefaultAsync(item => item.Id == user.Id);
            });
            
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Update_User_With_Name()
        {
            var user = new User(
                _guidGenerator.Create(),
                "krMWLfcpSEHXEVBzpFhhgQ==",
                "u8RSqTgo6+QXdtgIIoeaqQ==",
                "Hui");

            await _userRepository.InsertAsync(user);
            user = await _userRepository.GetAsync(item => item.Id == user.Id);
            user.SetName("Rain");
            await _userRepository.UpdateAsync(user);
            var updatedUser = await _userRepository.FindAsync(item => item.Name == "Rain");

            updatedUser.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Insert_Some_Valid_User()
        {
            var user1 = new User(
                _guidGenerator.Create(),
                "krMWLfcpSEHXEVBzpFhhgQ==",
                "u8RSqTgo6+QXdtgIIoeaqQ==",
                "Hui",
                "00-12300",
                "深圳");
            var user2 = new User(
                _guidGenerator.Create(),
                "j9mE3fbCOj3RZToj8sxw9w==",
                "UgcYsv+xFU9wGfSYSpR1oQ==",
                "Rain",
                "00-12400");

            await WithUnitOfWorkAsync(async () =>
            {
                await _userRepository.InsertAsync(user1);
                await _userRepository.InsertAsync(user2);
            });
            var result = await _userRepository.GetListAsync();

            result.Count.ShouldBe(3);
        }

        [Fact]
        public async Task Should_Delete_Special_User()
        {
            var users = new List<User>()
            {
                new(
                    _guidGenerator.Create(),
                    "krMWLfcpSEHXEVBzpFhhgQ==",
                    "u8RSqTgo6+QXdtgIIoeaqQ==",
                    "Hui",
                    "00-12300",
                    "深圳"),
                new(
                    _guidGenerator.Create(),
                    "j9mE3fbCOj3RZToj8sxw9w==",
                    "UgcYsv+xFU9wGfSYSpR1oQ==",
                    "Rain",
                    "00-12400")
            };

            await WithUnitOfWorkAsync(async () =>
            {
                await _userRepository.InsertManyAsync(users);
            });
            await WithUnitOfWorkAsync(async () =>
            {
                await _userRepository.DeleteAsync(user => user.Name == "Rain");
            });
            var deletedUser = await WithUnitOfWorkAsync(async () =>
            {
                return await _userRepository.FirstOrDefaultAsync(user => user.Name == "Rain");
            });
            var notDeletedUser = await WithUnitOfWorkAsync(async () =>
            {
                return await _userRepository.FirstOrDefaultAsync(user => user.Name == "Hui");
            });

            deletedUser.ShouldBeNull();
            notDeletedUser.ShouldNotBeNull();
        }
    }
}
