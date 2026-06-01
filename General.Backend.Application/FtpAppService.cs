using General.Ftp.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace General.Backend.Application
{
    /// <summary>
    /// Ftp应用服务
    /// </summary>
    public class FtpAppService : ApplicationService, IFtpAppService
    {
        private readonly FtpFileServiceBuilder _ftpFileServiceBuilder;

        public FtpAppService(FtpFileServiceBuilder ftpFileServiceBuilder)
        {
            _ftpFileServiceBuilder = ftpFileServiceBuilder;
        }

        /// <summary>
        /// 获取FTP目录下的文件名称列表
        /// </summary>
        /// <param name="filePathDirectory"></param>
        /// <returns></returns>
        public async Task<List<string>> GetFileNamesAsync([FromQuery] string filePathDirectory)
        {
            var fileNames = new List<string>();
            await using var ftpFileService = await _ftpFileServiceBuilder.CreateAsync();
            var fileGetResult = await ftpFileService.GetFilesAsync(filePathDirectory);
            if (fileGetResult.IsSuccess && fileGetResult.Data != null)
            {
                fileNames = fileGetResult.Data.Select(f => f.Name).ToList();
            }
            return fileNames;
        }

        /// <summary>
        /// 判断FTP目录下的文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public async Task<bool> ExistAsync([FromQuery] string filePath)
        {
            var result = false;
            await using var ftpFileService = await _ftpFileServiceBuilder.CreateAsync();
            var fileGetResult = await ftpFileService.ExistAsync(filePath);
            if (fileGetResult.IsSuccess)
            {
                result = fileGetResult.Data;
            }
            return result;
        }
    }
}
