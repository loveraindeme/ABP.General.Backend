using General.Ftp;
using General.Ftp.Contracts;
using Volo.Abp.Settings;

namespace General.Backend.Domain.Settings
{
    public class SystemSettingFtpConnectionOptionsProvider : IFtpConnectionOptionsProvider
    {
        private readonly ISettingProvider _settingProvider;
        private readonly AppSettingsFtpConnectionOptionsProvider _appSettingsOptionsProvider;

        public SystemSettingFtpConnectionOptionsProvider(
            ISettingProvider settingProvider,
            AppSettingsFtpConnectionOptionsProvider appSettingsOptionsProvider)
        {
            _settingProvider = settingProvider;
            _appSettingsOptionsProvider = appSettingsOptionsProvider;
        }

        public async Task<FtpConnectionOptions> GetOptionsAsync(CancellationToken cancellationToken = default)
        {
            var account = await _settingProvider.GetOrNullAsync(SystemSettingNameConsts.FtpAccount) ?? string.Empty;
            var password = await _settingProvider.GetOrNullAsync(SystemSettingNameConsts.FtpPassword) ?? string.Empty;
            var server = await _settingProvider.GetOrNullAsync(SystemSettingNameConsts.FtpServer) ?? string.Empty;

            if (string.IsNullOrWhiteSpace(account)
                && string.IsNullOrWhiteSpace(password)
                && string.IsNullOrWhiteSpace(server)) 
            {
                return await _appSettingsOptionsProvider.GetOptionsAsync(cancellationToken);
            }

            return new FtpConnectionOptions(account, password, server);
        }
    }
}
