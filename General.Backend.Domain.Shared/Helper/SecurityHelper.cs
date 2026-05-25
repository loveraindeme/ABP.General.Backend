using System.Security.Cryptography;
using System.Text;

namespace General.Backend.Domain.Shared.Helper
{
    public static class SecurityHelper
    {
        #region AES加密

        private readonly static string AESKEY = "GeneralBackend123456789~!@#$%^&*";
        private readonly static string KEY = "GeneralBackend**";

        /// <summary>
        /// 对称加密算法AES加密(（AES）算法是块式加密算法)
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <returns>加密结果字符串</returns>
        public static string AESEncrypt(string encryptString)
        {
            if (string.IsNullOrEmpty(encryptString))
            {
                return string.Empty;
            }
            return AESEncrypt(encryptString, AESKEY);
        }

        /// <summary>
        /// 对称加密算法AES加密(（AES）算法是块式加密算法)
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <param name="encryptKey">加密密钥，须半角字符</param>
        /// <returns>加密结果字符串</returns>
        public static string AESEncrypt(string encryptString, string encryptKey)
        {
            encryptKey = GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');
            Aes aesAlg = Aes.Create();
            aesAlg.Key = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 32));
            aesAlg.IV = Encoding.UTF8.GetBytes(KEY);
            aesAlg.Mode = CipherMode.CBC;
            aesAlg.Padding = PaddingMode.PKCS7;
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            byte[] inputData = Encoding.UTF8.GetBytes(encryptString);
            byte[] encryptedData = encryptor.TransformFinalBlock(inputData, 0, inputData.Length);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 对称加密算法AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns>
        public static string AESDecrypt(string decryptString)
        {
            if (string.IsNullOrEmpty(decryptString))
            {
                return string.Empty;
            }
            return AESDecrypt(decryptString, AESKEY);
        }

        /// <summary>
        /// 对称加密算法AES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥，和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串，失败返回空</returns>
        public static string AESDecrypt(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');
                Aes aesAlg = Aes.Create();
                aesAlg.Key = Encoding.UTF8.GetBytes(decryptKey);
                aesAlg.IV = Encoding.UTF8.GetBytes(KEY);
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = decryptor.TransformFinalBlock(inputData, 0, inputData.Length);
                return Encoding.UTF8.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")</param>
        /// <returns>某字符串的一部分</returns>
        private static string GetSubString(string sourceString, int length, string tailString)
        {
            return GetSubString(sourceString, 0, length, tailString);
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="startIndex">索引位置，以0开始</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")</param>
        /// <returns>某字符串的一部分</returns>
        private static string GetSubString(string sourceString, int startIndex, int length, string tailString)
        {
            string result = sourceString;
            if (System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (startIndex >= sourceString.Length)
                {
                    return string.Empty;
                }
                else
                {
                    return sourceString.Substring(startIndex, length + startIndex > sourceString.Length ? sourceString.Length - startIndex : length);
                }
            }
            //中文字符
            if (length <= 0)
            {
                return string.Empty;
            }
            byte[] bytesSource = Encoding.Default.GetBytes(sourceString);
            //当字符串长度大于起始位置
            if (bytesSource.Length > startIndex)
            {
                int endIndex = bytesSource.Length;
                //当要截取的长度在字符串的有效长度范围内
                if (bytesSource.Length > startIndex + length)
                {
                    endIndex = length + startIndex;
                }
                else
                {   //当不在有效范围内时,只取到字符串的结尾
                    length = bytesSource.Length - startIndex;
                    tailString = "";
                }

                int[] anResultFlag = new int[length];
                int nFlag = 0;
                //字节大于127为双字节字符
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (bytesSource[i] > 127)
                    {
                        nFlag++;
                        if (nFlag == 3)
                        {
                            nFlag = 1;
                        }
                    }
                    else
                    {
                        nFlag = 0;
                    }
                    anResultFlag[i] = nFlag;
                }
                if (bytesSource[endIndex - 1] > 127 && anResultFlag[length - 1] == 1)
                {
                    length++;
                }
                byte[] bsResult = new byte[length];
                Array.Copy(bytesSource, startIndex, bsResult, 0, length);
                result = Encoding.Default.GetString(bsResult);
                result += tailString;
                return result;
            }
            return string.Empty;
        }

        #endregion
    }
}
