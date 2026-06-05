namespace General.InformationCollection.Domain
{
    public class InformationCollectionOptions
    {
        public const string InformationCollectionOption = "InformationCollection";
        
        /// <summary>
        /// 是否启用信息采集
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// 当前模块名称
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;
    }
}
