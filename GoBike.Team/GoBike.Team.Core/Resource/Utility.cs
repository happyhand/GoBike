using System.Collections.Generic;
using System.Linq;

namespace GoBike.Team.Core.Resource
{
    /// <summary>
    /// 共用方法
    /// </summary>
    public class Utility
    {
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
    }
}