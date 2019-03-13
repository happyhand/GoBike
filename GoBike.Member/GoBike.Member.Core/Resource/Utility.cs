using ClosedXML.Excel;
using System;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace GoBike.Member.Core.Resource
{
	public class Utility
	{
		#region AES 加解密功能

		public static string AES_KEY = "1234567890123456";
		public static string AES_IV = "6543210987654321";

		/// <summary>
		/// AES 加密
		/// </summary>
		/// <param name="text">text</param>
		/// <param name="key">key</param>
		/// <param name="iv">iv</param>
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
		/// AES 解密
		/// </summary>
		/// <param name="text">text</param>
		/// <param name="key">key</param>
		/// <param name="iv">iv</param>
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
		/// byte 轉 16 進制
		/// </summary>
		/// <param name="comByte"></param>
		/// <returns></returns>
		public static string ByteToHex(byte[] comByte)
		{
			StringBuilder builder = new StringBuilder(comByte.Length * 3);

			foreach (byte data in comByte)
				builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

			return builder.ToString().ToUpper().Replace(" ", "");
		}

		/// <summary>
		/// 16 進制轉 byte
		/// </summary>
		/// <param name="data">data</param>
		/// <returns>byte[]</returns>
		public static byte[] HexToByte(string data)
		{
			data = data.Replace(" ", "");

			byte[] comBuffer = new byte[data.Length / 2];

			for (int i = 0; i < data.Length; i += 2)
				comBuffer[i / 2] = (byte)Convert.ToByte(data.Substring(i, 2), 16);

			return comBuffer;
		}

		#endregion AES 加解密功能

		#region Excel 匯入:匯出

		/// <summary>
		/// 匯出 Excel
		/// </summary>
		/// <param name="dt">dt</param>
		/// <returns>FileStreamResult</returns>
		public static MemoryStream ExportExcel(DataTable dt, string sheetName)
		{
			#region 建立 Excel 資料串流

			MemoryStream memoryStream = new MemoryStream();

			#region 建立 Excel 檔案

			using (var workbook = new XLWorkbook())
			{
				var worksheet = workbook.Worksheets.Add(dt, sheetName);
				workbook.SaveAs(memoryStream);
			}

			#endregion 建立 Excel 檔案

			#endregion 建立 Excel 資料串流

			#region 回傳資料串流

			memoryStream.Seek(0, SeekOrigin.Begin);
			return memoryStream;
			//return new FileStreamResult(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

			#endregion 回傳資料串流
		}

		#endregion Excel 匯入:匯出
	}
}