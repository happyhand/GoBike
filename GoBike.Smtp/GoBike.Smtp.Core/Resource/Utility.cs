using System.Reflection;

namespace GoBike.Smtp.Core.Resource
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public class Utility
    {
        #region 取得類別屬性資料

        /// <summary>
        /// 取得類別屬性資料
        /// </summary>
        /// <param name="data">data</param>
        /// <returns>string</returns>
        public static string GetPropertiesData(object data)
        {
            string propertiesData = string.Empty;
            if (data != null)
            {
                PropertyInfo[] properties = data.GetType().GetProperties();
                foreach (PropertyInfo propertie in properties)
                {
                    propertiesData += $"{(propertiesData.Length > 0 ? ", " : string.Empty)}{propertie.Name}:{propertie.GetValue(data)}";
                }
            }

            return propertiesData;
        }

        #endregion 取得類別屬性資料
    }
}