namespace General.Backend.Domain.Shared.Extensions
{
    /// <summary>
    /// 数据扩展
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// 时间戳转换为时间
        /// </summary>
        /// <param name="timestampMillis"></param>
        /// <param name="dateTimeKind"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this long timestampMillis, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, dateTimeKind).AddMilliseconds(timestampMillis);
            return dateTime;
        }

        /// <summary>
        /// 时间转换为时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="dateTimeKind"></param>
        /// <returns></returns>
        public static long ToTimestampMillis(this DateTime dateTime, DateTimeKind dateTimeKind = DateTimeKind.Utc)
        {
            var utcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, dateTimeKind);
            TimeSpan timeSpan = dateTime - utcDateTime;
            long timestampMillis = Convert.ToInt64(timeSpan.TotalMilliseconds);
            return timestampMillis;
        }
    }
}
