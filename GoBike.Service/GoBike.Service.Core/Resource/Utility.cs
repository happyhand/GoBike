using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace GoBike.Service.Core.Resource
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public class Utility
    {
        #region AES 加解密功能

        /// <summary>
        /// AES_IV
        /// </summary>
        public static string AES_IV = "2244668800113355";

        /// <summary>
        /// AES_KEY
        /// </summary>
        public static string AES_KEY = "1133557799224466";

        /// <summary>
        /// byte 轉 16 進制
        /// </summary>
        /// <param name="comByte">comByte</param>
        /// <returns>string</returns>
        public static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);

            foreach (byte data in comByte)
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

            return builder.ToString().ToUpper().Replace(" ", string.Empty);
        }

        /// <summary>
        /// AES 解密
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>string</returns>
        public static string DecryptAES(string text)
        {
            //byte[] encryptBytes = Convert.FromBase64String(text);
            byte[] encryptBytes = HexToByte(text);
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(AES_KEY);
            aes.IV = Encoding.UTF8.GetBytes(AES_IV);
            ICryptoTransform cryptoTransform = aes.CreateDecryptor();
            byte[] bResult = cryptoTransform.TransformFinalBlock(encryptBytes, 0, encryptBytes.Length);
            return Encoding.UTF8.GetString(bResult);
        }

        /// <summary>
        /// AES 加密
        /// </summary>
        /// <param name="text">text</param>
        /// <returns>string</returns>
        public static string EncryptAES(string text)
        {
            byte[] sourceBytes = Encoding.UTF8.GetBytes(text);
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Encoding.UTF8.GetBytes(AES_KEY);
            aes.IV = Encoding.UTF8.GetBytes(AES_IV);
            ICryptoTransform cryptoTransform = aes.CreateEncryptor();
            byte[] bResult = cryptoTransform.TransformFinalBlock(sourceBytes, 0, sourceBytes.Length);
            return ByteToHex(bResult);
            //return Convert.ToBase64String(bResult);
        }

        /// <summary>
        /// 16 進制轉 byte
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>byte[]</returns>
        public static byte[] HexToByte(string data)
        {
            data = data.Replace(" ", string.Empty);

            byte[] comBuffer = new byte[data.Length / 2];

            for (int i = 0; i < data.Length; i += 2)
                comBuffer[i / 2] = (byte)Convert.ToByte(data.Substring(i, 2), 16);

            return comBuffer;
        }

        #endregion AES 加解密功能

        #region Email

        /// <summary>
        /// Email 格式驗證
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>bool</returns>
        public static bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email,
                @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        #endregion Email

        #region 行動電話驗證

        /// <summary>
        /// 行動電話格式驗證
        /// </summary>
        /// <param name="mobile">mobile</param>
        /// <returns>bool</returns>
        public static bool IsValidMobile(string mobile)
        {
            return Regex.IsMatch(mobile, @"^(09|8869|\+8869)\d{8}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }

        #endregion 行動電話驗證

        #region 流水號

        /// <summary>
        /// 取得流水號 ID
        /// </summary>
        /// <param name="createDate">createDate</param>
        /// <returns>string</returns>
        public static string GetSerialID(DateTime createDate)
        {
            return $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
        }

        #endregion 流水號

        #region 列表更新

        /// <summary>
        /// 名單更新處理
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="targetID">targetID</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>bool</returns>
        public static bool UpdateListHandler(IEnumerable<string> list, string targetID, bool isAdd)
        {
            if (isAdd)
            {
                if (!list.Contains(targetID))
                {
                    (list as List<string>).Add(targetID);
                    return true;
                }

                return false;
            }
            else
            {
                if (list.Contains(targetID))
                {
                    (list as List<string>).Remove(targetID);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 名單更新處理
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="targetIDs">targetIDs</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>bool</returns>
        public static bool UpdateListHandler(IEnumerable<string> list, IEnumerable<string> targetIDs, bool isAdd)
        {
            bool isUpdate = false;
            foreach (string targetID in targetIDs)
            {
                if (isAdd && !list.Contains(targetID))
                {
                    (list as List<string>).Add(targetID);
                    isUpdate = true;
                }
                else if (!isAdd && list.Contains(targetID))
                {
                    (list as List<string>).Remove(targetID);
                    isUpdate = true;
                }
            }

            return isUpdate;
        }

        #endregion 列表更新
    }
}