namespace General.Backend.EntityFrameworkCore.Tests.UnitTests
{
    public class RoleRepositoryTests : GeneralEntityFrameworkCoreTestBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IGuidGenerator _guidGenerator;

        public RoleRepositoryTests()
        {
            _roleRepository = GetRequiredService<IRoleRepository>();
            _guidGenerator = GetRequiredService<IGuidGenerator>();
        }

        [Fact]
        public async Task Should_Insert_A_Valid_Role()
        {
            var role = new Role(
                _guidGenerator.Create(),
                "Rain",
                "Rain",
                "管理员");

            await _roleRepository.InsertAsync(role);
            var result = await _roleRepository.FindAsync(item => item.Id == role.Id);

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task Should_Delete_All_Role()
        {
            var roles = new List<Role>(){
                new (
                    _guidGenerator.Create(),
                    "Rain",
                    "Rain",
                    "管理员"),
                new (
                    _guidGenerator.Create(),
                    "Boss",
                    "Boss",
                    "头儿")
            };

            await _roleRepository.InsertManyAsync(roles);
            var result = await _roleRepository.GetPagedListAsync(0, 1, string.Empty);
            result.Count.ShouldBe(1);

            await _roleRepository.DeleteManyAsync(roles);
            var role = await _roleRepository.FindAsync(item => roles.Select(role => role.Id).Contains(item.Id));
            role.ShouldBeNull();
        }
    }
}
