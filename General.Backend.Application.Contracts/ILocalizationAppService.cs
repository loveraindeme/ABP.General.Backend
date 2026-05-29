namespace General.Backend.Application.Contracts
{
    public interface ILocalizationAppService : IApplicationService
    {
        /// <summary>
        /// 根据指定文化和本地化键获取本地化文本
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object GetHelloWorld(string culture = "zh-CN");

        /// <summary>
        /// 根据指定文化和本地化键获取本地化文本
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public object GetHelloName(string culture = "en-US", string name = "LiHua");
    }
}
