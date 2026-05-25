namespace General.Backend.Domain.Shared.Consts
{
    public static class UserConsts
    {
        public const string UserTableName = "user";

        public const string UserTableComment = "用户表";

        public const string UserRoleTableName = "user_role";

        public const string UserRoleTableComment = "用户角色表";

        public const string AdminAccount = "admin";

        public const string AdminName = "Admin";

        public const int MaxLoginErrorCount = 5;

        public const int MinAccountLength = 2;

        public const int MaxAccountLength = 32;

        public const int MinPasswordLength = 6;

        public const int MaxPasswordLength = 32;

        public const int MinNameLength = 1;

        public const int MaxNameLength = 32;

        public const int MaxContactLength = 64;

        public const int MaxAddressLength = 64;
    }
}
