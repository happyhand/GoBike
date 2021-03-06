﻿using GoBike.MGT.Repository.Models.Data;
using System;
using System.Threading.Tasks;

namespace GoBike.MGT.Service.Interface
{
    /// <summary>
    /// 後台服務
    /// </summary>
    public interface IMgtService
    {
        /// <summary>
        /// 新增代理商資料
        /// </summary>
        /// <returns>Tuple(AgentData, string)</returns>
        void AddAgent(string nickname, string password);

        /// <summary>
        /// 取得代理商資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Tuple(AgentData, string)</returns>
        Task<Tuple<AgentData, string>> GetAgent(long id);

        /// <summary>
        /// 代理商登入
        /// </summary>
        /// <param name="account">account</param>
        /// <param name="password">password</param>
        /// <returns>bool</returns>
        Task<string> AgentLogin(string account, string password);
    }
}