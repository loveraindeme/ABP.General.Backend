namespace General.Backend.Domain.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User : FullAuditedAggregateRoot<Guid>
    {
        /// <summary>
        /// 账号
        /// </summary>
        public virtual string Account { get; private set; } = string.Empty;

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; private set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public virtual string Name { get; private set; } = string.Empty;

        /// <summary>
        /// 联系方式
        /// </summary>
        public virtual string? Contact { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public virtual string? Address { get; set; }

        /// <summary>
        /// 登录错误次数
        /// </summary>
        public virtual byte LoginErrorCount { get; private set; }

        /// <summary>
        /// 是否冻结（1：未冻结，2：已冻结）
        /// </summary>
        public virtual FrozenStatus IsFrozen { get; private set; }

        /// <summary>
        /// 用户关联角色列表
        /// </summary>
        public virtual ICollection<UserRole> UserRoles { get; private set; } = [];

        protected User()
        {

        }

        public User(
            Guid id,
            [NotNull] string account,
            [NotNull] string password,
            [NotNull] string name,
            [CanBeNull] string? contact = null,
            [CanBeNull] string? address = null)
        {
            Id = id;
            SetAccount(account);
            SetPassword(password);
            SetName(name);
            IsFrozen = FrozenStatus.UnFrozen;
            Contact = Check.Length(contact, nameof(contact), UserConsts.MaxContactLength);
            Address = Check.Length(address, nameof(address), UserConsts.MaxAddressLength);
        }

        private void SetAccount([NotNull] string account)
        {
            Account = Check.Length(account, nameof(account), UserConsts.MaxAccountLength, UserConsts.MinAccountLength)!;
            ValidateAccountValidity(account);
        }

        internal virtual void SetPassword([NotNull] string password)
        {
            Password = Check.Length(password, nameof(password), UserConsts.MaxPasswordLength, UserConsts.MinPasswordLength)!;
            ValidatePasswordComplexity(password);
        }

        public virtual void SetName([NotNull] string name)
        {
            Name = Check.Length(name, nameof(name), UserConsts.MaxNameLength, UserConsts.MinNameLength)!;
        }

        internal virtual bool CheckPassword([NotNull] string password)
        {
            Check.NotNullOrEmpty(password, nameof(password));
            if (Password != password)
            {
                TryFrozen();
                return false;
            }
            UnFrozen();
            return true;
        }

        public virtual void UnFrozen()
        {
            LoginErrorCount = 0;
            IsFrozen = FrozenStatus.UnFrozen;
        }

        private bool TryFrozen()
        {
            var isSuccess = false;
            if (SecurityHelper.AESDecrypt(Account) != UserConsts.AdminAccount)
            {
                LoginErrorCount += 1;
                if (LoginErrorCount >= UserConsts.MaxLoginErrorCount)
                {
                    IsFrozen = FrozenStatus.Frozen;
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public virtual void SetRoles(ICollection<UserRole> userRoles)
        {
            UserRoles = userRoles;
        }

        private static void ValidateAccountValidity([NotNull] string account)
        {
            var decryptAccount = SecurityHelper.AESDecrypt(account);
            if (!ValidateHelper.ValidateValidityOfSymbol(decryptAccount))
            {
                throw new UserFriendlyException("账号只能包含字母、数字或其他特殊字符");
            }
        }

        private static void ValidatePasswordComplexity([NotNull] string password)
        {
            var decryptPassword = SecurityHelper.AESDecrypt(password);
            if (!ValidateHelper.ValidateComplexityOfPassword(decryptPassword))
            {
                throw new UserFriendlyException("密码需要包含字母、数字和其他特殊字符");
            }
        }
    }
}
