using General.Backend.Domain.Shared.Localization;
using Volo.Abp.Localization;
using Volo.Abp.Settings;

namespace General.Backend.Domain.Settings
{
    public class SystemSettingDefinitionProvider : SettingDefinitionProvider
    {
        private const string FtpServerIpv4WithPortRegex = @"^((2[0-4]\d|25[0-5]|[1]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[1]?\d\d?)\:([1-9]|[1-9][0-9]|[1-9][0-9][0-9]|[1-9][0-9][0-9][0-9]|[1-6][0-5][0-5][0-3][0-5])$";

        public override void Define(ISettingDefinitionContext context)
        {
            context.Add(
                CreateSettingDefinition(
                    SystemSettingNameConsts.FtpAccount,
                    "SystemSetting__FtpAccount",
                    SystemSettingValueTypeConsts.ValueTypeString,
                    true,
                    "FTP",
                    false,
                    1),
                CreateSettingDefinition(
                    SystemSettingNameConsts.FtpPassword,
                    "SystemSetting__FtpPassword",
                    SystemSettingValueTypeConsts.ValueTypeString,
                    true,
                    "FTP",
                    true,
                    2),
                CreateSettingDefinition(
                    SystemSettingNameConsts.FtpServer,
                    "SystemSetting__FtpServer",
                    SystemSettingValueTypeConsts.ValueTypeString,
                    true,
                    "FTP",
                    false,
                    3,
                    FtpServerIpv4WithPortRegex));
        }

        private static SettingDefinition CreateSettingDefinition(
            string name,
            string displayNameKey,
            string valueType,
            bool required,
            string group,
            bool isSecret,
            int sort = 1,
            string regex = "",
            string min = "",
            string max = "",
            string options = "")
        {
            return new SettingDefinition(
                    name,
                    displayName: LocalizableString.Create<GeneralBackendResource>(displayNameKey),
                    isEncrypted: isSecret)
                .WithProperty(SystemSettingMetaConsts.ValueType, valueType)
                .WithProperty(SystemSettingMetaConsts.Required, required)
                .WithProperty(SystemSettingMetaConsts.Group, group)
                .WithProperty(SystemSettingMetaConsts.IsSecret, isSecret)
                .WithProperty(SystemSettingMetaConsts.Sort, sort)
                .WithProperty(SystemSettingMetaConsts.Regex, regex)
                .WithProperty(SystemSettingMetaConsts.Min, min)
                .WithProperty(SystemSettingMetaConsts.Max, max)
                .WithProperty(SystemSettingMetaConsts.Options, options);
        }
    }

    public static class SystemSettingNameConsts
    {
        /// <summary>
        /// FTP账号
        /// </summary>
        public const string FtpAccount = "SystemSetting.FtpAccount";

        /// <summary>
        /// FTP密码
        /// </summary>
        public const string FtpPassword = "SystemSetting.FtpPassword";

        /// <summary>
        /// FTP服务地址
        /// </summary>
        public const string FtpServer = "SystemSetting.FtpServer";
    }
}
