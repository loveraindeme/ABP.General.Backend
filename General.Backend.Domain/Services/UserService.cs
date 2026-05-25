namespace General.Backend.Domain.Services
{
    public class UserService : DomainService
    {
        private readonly IUserRepository _userRepository;

        public UserService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> LoginCheckAsync(
            string account,
            string password)
        {
            var user = await _userRepository.FindUserByAccountAsync(account);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            if (user.IsFrozen == FrozenStatus.Frozen)
            {
                throw new UserFriendlyException("用户已被冻结");
            }
            var checkResult = user.CheckPassword(password);

            await _userRepository.UpdateAsync(user);

            if (!checkResult)
            {
                throw new UserFriendlyException("用户密码错误");
            }
            return user;
        }

        public async Task<User> AddAsync(
            string account,
            string password,
            string name,
            string? contact = null,
            string? address = null)
        {
            var sameUsers = await _userRepository.GetSameUserAsync(new SameUserSpecification(name, account));
            if (sameUsers.Count > 0)
            {
                if (sameUsers.Any(user => user.Account == account))
                {
                    throw new UserFriendlyException("已存在相同账号的用户");
                }
                if (sameUsers.Any(user => user.Name == name))
                {
                    throw new UserFriendlyException("已存在相同名称的用户");
                }
            }
            var user = new User(
                GuidGenerator.Create(),
                account,
                password,
                name,
                contact,
                address);
            return user;
        }

        public async Task<User> ModifyAsync(
            Guid id,
            string password,
            string name,
            string? contact = null,
            string? address = null)
        {
            var user = await _userRepository.FindAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            if (SecurityHelper.AESDecrypt(user.Account) == UserConsts.AdminAccount)
            {
                throw new UserFriendlyException("不可操作管理员用户");
            }
            var sameUsers = await _userRepository.GetSameUserAsync(new SameUserSpecification(name));
            if (sameUsers.Any(item => item.Name == name && item.Id != id))
            {
                throw new UserFriendlyException("已存在相同名称的用户");
            }
            user.SetName(name);
            user.SetPassword(password);
            user.Contact = contact;
            user.Address = address;
            return user;
        }

        public async Task<User> UnfreezeAsync(Guid id)
        {
            var user = await _userRepository.FindAsync(id);
            if (user == null)
            {
                throw new UserFriendlyException("用户不存在");
            }
            user.UnFrozen();
            return user;
        }

        public void BindRoles(User user, List<Guid> roleIds)
        {
            var userRoles = new List<UserRole>();
            roleIds.ForEach(roleId =>
            {
                var userRole = new UserRole(
                    GuidGenerator.Create(),
                    user.Id,
                    roleId);
                userRoles.Add(userRole);
            });
            user.SetRoles(userRoles);
        }
    }
}
