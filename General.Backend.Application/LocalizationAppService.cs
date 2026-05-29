using General.Backend.Domain.Shared.Localization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;

namespace General.Backend.Application
{
    /// <summary>
    /// 本地化应用服务
    /// </summary>
    [AllowAnonymous]
    public class LocalizationAppService : ApplicationService, ILocalizationAppService
    {
        private readonly IStringLocalizer<GeneralBackendResource> _stringLocalizer;

        public LocalizationAppService(
            IStringLocalizer<GeneralBackendResource> stringLocalizer)
        {
            LocalizationResource = typeof(GeneralBackendResource);
            _stringLocalizer = stringLocalizer;
        }

        /// <summary>
        /// 根据指定文化和本地化键获取本地化文本
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object GetHelloWorld(string culture = "zh-CN")
        {
            string localizationKey = "HelloWorld";
            var localizedString1 = _stringLocalizer[localizationKey];
            var localizedString2 = _stringLocalizer.GetString(localizationKey);
            var localizedString3 = L[localizationKey];
            return new 
            {
                Index = localizedString1,
                GetString = localizedString2,
                L = localizedString3
            };
        }

        /// <summary>
        /// 根据指定文化和本地化键获取本地化文本
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetHelloName(string culture = "en-US", string name = "LiHua")
        {
            string localizationKey = "HelloName";
            var localizedString1 = _stringLocalizer[localizationKey, name];
            var localizedString2 = _stringLocalizer.GetString(localizationKey, name);
            var localizedString3 = L[localizationKey, name];
            return new
            {
                Index = localizedString1,
                GetString = localizedString2,
                L = localizedString3
            };
        }
    }
}
