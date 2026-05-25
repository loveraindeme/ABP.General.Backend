namespace General.Backend.Application.Contracts.Dtos
{
    /// <summary>
    /// 排序
    /// </summary>
    public class PageAndSortDto : PageDto
    {
        /// <summary>
        /// 排序类型：asc desc
        /// </summary>
        public string SortType { get; set; } = string.Empty;

        /// <summary>
        /// 排序字段
        /// </summary>
        public string SortField { get; set; } = string.Empty;
    }
}
