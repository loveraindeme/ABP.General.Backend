namespace General.Backend.Application.Contracts.Dtos.Users
{
    /// <summary>
    /// 用户查询
    /// </summary>
    public class UserQueryDto : PageAndSortDto
    {
        /// <summary>
        /// 用户账号
        /// </summary>
        public string? Account { get; set; } = string.Empty;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string? Name { get; set; } = string.Empty;
    }
}
