namespace General.Backend.Application.Contracts.Dtos.Users
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleDto : EntityDto<Guid>, IHasCreationTime
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public Guid RoleId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
