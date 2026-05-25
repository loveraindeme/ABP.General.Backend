using System.Text.RegularExpressions;

namespace General.Backend.Domain.Shared.Helper
{
    public static class ValidateHelper
    {
        /// <summary>
        /// 验证密码复杂度
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateComplexityOfPassword(string password)
        {
            return Regex.IsMatch(password, "^(?=.*[0-9])(?=.*[a-zA-Z])(?=.*[^a-zA-Z0-9]).*$");
        }

        /// <summary>
        /// 验证符号有效性，只能包含数字、字母和特殊字符
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static bool ValidateValidityOfSymbol(string account)
        {
            return Regex.IsMatch(account, "^[a-zA-Z0-9`!@#$%^&*()-_=+,.?:;'\"<>|~{}]*$");
        }
    }
}
