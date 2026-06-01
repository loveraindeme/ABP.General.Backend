namespace General.Backend.Application.Contracts
{
    /// <summary>
    /// Ftp应用服务
    /// </summary>
    public interface IFtpAppService : IApplicationService
    {
        /// <summary>
        /// 获取FTP目录下的文件名称列表
        /// </summary>
        /// <param name="filePathDirectory"></param>
        /// <returns></returns>
        public Task<List<string>> GetFileNamesAsync(string filePathDirectory);

        /// <summary>
        /// 判断FTP目录下的文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public Task<bool> ExistAsync(string filePath);
    }
}
