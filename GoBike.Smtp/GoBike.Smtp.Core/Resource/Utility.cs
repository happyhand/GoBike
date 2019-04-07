using System.Collections.Generic;
using System.Linq;

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
        /// <returns>IEnumerable(string)</returns>
        public static IEnumerable<string> GetPropertiesData(object data)
        {
            return data.GetType().GetProperties().Select(x => $"{x.Name}:{x.GetValue(data)}");
        }

        #endregion 取得類別屬性資料
    }
}