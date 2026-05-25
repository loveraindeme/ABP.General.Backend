namespace General.Backend.Application.Contracts.Dtos
{
    /// <summary>
    /// 分页
    /// </summary>
    public class PageDto
    {
        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// 分页下标
        /// </summary>
        public int PageIndex { get; set; } = 1;
    }
}
