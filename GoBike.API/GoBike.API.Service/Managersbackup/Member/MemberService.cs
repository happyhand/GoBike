//namespace GoBike.API.Service.Managersbackup.Member
//{
//    /// <summary>
//    /// 會員服務
//    /// </summary>
//    public class MemberService
//    {
//        /// <summary>
//        /// logger
//        /// </summary>
//        private readonly ILogger<MemberService> logger;

//        /// <summary>
//        /// mapper
//        /// </summary>
//        private readonly IMapper mapper;

//        /// <summary>
//        /// 建構式
//        /// </summary>
//        /// <param name="logger">logger</param>
//        /// <param name="mapper">mapper</param>
//        public MemberService(ILogger<MemberService> logger, IMapper mapper)
//        {
//            this.logger = logger;
//            this.mapper = mapper;
//        }

//        /// <summary>
//        /// 會員編輯
//        /// </summary>
//        /// <param name="memberInfo">memberInfo</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> EditData(MemberInfoDto memberInfo)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberInfo);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/EditData", postData);
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = true,
//                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberSettingInfoViewDto>()
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(memberInfo)}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "會員編輯發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 取得會員資訊
//        /// </summary>
//        /// <param name="memberBaseCommand">memberBaseCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> GetMemberInfo(MemberBaseCommandDto memberBaseCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberBaseCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = true,
//                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberSettingInfoViewDto>()
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Get Member Info Error >>> MemberID:{memberBaseCommand.MemberID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "取得會員資訊發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 會員登入 (normal)
//        /// </summary>
//        /// <param name="httpContext">httpContext</param>
//        /// <param name="email">email</param>
//        /// <param name="password">password</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> Login(HttpContext httpContext, string email, string password)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(new MemberBaseCommandDto() { Email = email, Password = password });
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Login", postData);
//                string result = await httpResponseMessage.Content.ReadAsAsync<string>();
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    httpContext.Session.SetObject(CommonFlagHelper.CommonFlag.SessionFlag.MemberID, result);
//                    return new ResponseResultDto()
//                    {
//                        Ok = true,
//                        Data = $"{Utility.EncryptAES(email)}{CommonFlagHelper.CommonFlag.SeparateFlag}{Utility.EncryptAES(password)}"
//                    };
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = result
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Login Error >>> Email:{email} Password:{password}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "會員登入發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 會員登入 (token)
//        /// </summary>
//        /// <param name="httpContext">httpContext</param>
//        /// <param name="token">token</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> Login(HttpContext httpContext, string token)
//        {
//            try
//            {
//                string[] dataArr = token.Split(CommonFlagHelper.CommonFlag.SeparateFlag);
//                string email = Utility.DecryptAES(dataArr[0]);
//                string password = Utility.DecryptAES(dataArr[1]);
//                return await this.Login(httpContext, email, password);
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Token Login Error >>> Token:{token}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "自動登入驗證碼發生錯誤，無法編譯."
//                };
//            }
//        }

//        /// <summary>
//        /// 會員註冊
//        /// </summary>
//        /// <param name="memberBaseCommand">memberBaseCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> Register(MemberBaseCommandDto memberBaseCommand)
//        {
//            try
//            {
//                string postData = JsonConvert.SerializeObject(memberBaseCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/Register", postData);
//                return new ResponseResultDto()
//                {
//                    Ok = httpResponseMessage.IsSuccessStatusCode,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Register Error >>> Email:{memberBaseCommand.Email} Password:{memberBaseCommand.Password}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "會員註冊發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 重設密碼
//        /// </summary>
//        /// <param name="memberBaseCommand">memberBaseCommand</param>
//        /// <returns>HttpResponseMessage</returns>
//        public async Task<ResponseResultDto> ResetPassword(MemberBaseCommandDto memberBaseCommand)
//        {
//            try
//            {
//                memberBaseCommand.Password = Guid.NewGuid().ToString().Substring(0, 8);
//                string postData = JsonConvert.SerializeObject(memberBaseCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/ResetPassword", postData);
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    EmailContext emailContext = EmailContext.GetResetPasswordEmailContext(memberBaseCommand.Email, memberBaseCommand.Password);
//                    postData = JsonConvert.SerializeObject(emailContext);
//                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.SmtpService, "api/SendEmail", postData);
//                    if (httpResponseMessage.IsSuccessStatusCode)
//                    {
//                        return new ResponseResultDto()
//                        {
//                            Ok = httpResponseMessage.IsSuccessStatusCode,
//                            Data = "已重設密碼，並發送郵件成功."
//                        };
//                    }
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Reset Password Error >>> Email:{memberBaseCommand.Email}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "重設密碼發生錯誤."
//                };
//            }
//        }

//        #region TODO

//        /// <summary>
//        /// 查詢會員資訊
//        /// </summary>
//        /// <param name="memberID">memberID</param>
//        /// <param name="memberSearchCommand">memberSearchCommand</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> InquireMemberInfo(string memberID, MemberSearchCommandDto memberSearchCommand)
//        {
//            try
//            {
//                //// 判斷 Search Key
//                MemberBaseCommandDto memberBaseCommand = new MemberBaseCommandDto() { MemberID = memberID };
//                string searchKey = memberSearchCommand.SearchKey;
//                if (searchKey.Contains("@"))
//                {
//                    memberBaseCommand.Email = searchKey;
//                }
//                else if (searchKey.Length == 6) //// 目前只能先寫死，待思考有沒有其他更好的方式
//                {
//                    memberBaseCommand.MemberID = searchKey;
//                }
//                else if (searchKey.Length == 10) //// 目前只能先寫死，待思考有沒有其他更好的方式
//                {
//                    memberBaseCommand.Mobile = searchKey;
//                }
//                else
//                {
//                    return new ResponseResultDto()
//                    {
//                        Ok = false,
//                        Data = "無效的查詢參數."
//                    };
//                }

//                //// 取得會員資料
//                string postData = JsonConvert.SerializeObject(memberBaseCommand);
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    MemberCardInfoViewDto memberCardInfoView = await httpResponseMessage.Content.ReadAsAsync<MemberCardInfoViewDto>();
//                    if (memberID.Equals(memberCardInfoView.MemberID))
//                    {
//                        return new ResponseResultDto()
//                        {
//                            Ok = false,
//                            Data = "無法查詢會員本身資料."
//                        };
//                    }

//                    //// 取得互動資料
//                    postData = JsonConvert.SerializeObject(new MemberInteractiveCommandDto() { InitiatorID = memberID, ReceiverID = memberCardInfoView.MemberID });
//                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/GetMemberInteractiveStatus", postData);
//                    if (httpResponseMessage.IsSuccessStatusCode)
//                    {
//                        memberCardInfoView.InteractiveStatus = await httpResponseMessage.Content.ReadAsAsync<int>();
//                        //// TODO 會員騎乘資料
//                        memberCardInfoView.RideDataList = new List<MemberRideRecordDto>();
//                        //// 車隊設定資料
//                        if (string.IsNullOrEmpty(memberSearchCommand.TeamID))
//                        {
//                            memberCardInfoView.TeamJoinSetting = (int)TeamJoinSettingType.None;
//                            memberCardInfoView.TeamKickOutSetting = (int)TeamKickOutSettingType.None;
//                            memberCardInfoView.TeamViceLeaderSetting = (int)TeamViceLeaderSettingType.None;
//                            return new ResponseResultDto()
//                            {
//                                Ok = true,
//                                Data = memberCardInfoView
//                            };
//                        }

//                        postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = memberSearchCommand.TeamID, TargetID = memberID });
//                        httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/GetTeamInfo", postData);
//                        TeamInfoDto teamInfo = await httpResponseMessage.Content.ReadAsAsync<TeamInfoDto>();
//                        int memberIdentity = this.GetTeamIdentity(teamInfo, memberID);
//                        int targetIdentity = this.GetTeamIdentity(teamInfo, memberCardInfoView.MemberID);
//                        if (targetIdentity == (int)TeamIdentityType.None)
//                        {
//                            if (memberIdentity == (int)TeamIdentityType.Leader || memberIdentity == (int)TeamIdentityType.ViceLeader)
//                            {
//                                if (teamInfo.TeamInviteJoinIDs.Contains(memberCardInfoView.MemberID))
//                                {
//                                    memberCardInfoView.TeamJoinSetting = (int)TeamJoinSettingType.CancelInviteJoin;
//                                }
//                                else if (teamInfo.TeamApplyForJoinIDs.Contains(memberCardInfoView.MemberID))
//                                {
//                                    memberCardInfoView.TeamJoinSetting = (int)TeamJoinSettingType.HandleApplyFor;
//                                }
//                                else
//                                {
//                                    memberCardInfoView.TeamJoinSetting = (int)TeamJoinSettingType.InviteJoin;
//                                }
//                            }
//                            memberCardInfoView.TeamKickOutSetting = (int)TeamKickOutSettingType.None;
//                            memberCardInfoView.TeamViceLeaderSetting = (int)TeamViceLeaderSettingType.None;
//                        }
//                        else
//                        {
//                            memberCardInfoView.TeamJoinSetting = (int)TeamJoinSettingType.None;
//                            memberCardInfoView.TeamKickOutSetting = this.GetTeamKickOutSetFlag(memberIdentity, targetIdentity);
//                            memberCardInfoView.TeamViceLeaderSetting = this.GetTeamViceLeaderSetFlag(memberIdentity, targetIdentity);
//                        }

//                        return new ResponseResultDto()
//                        {
//                            Ok = true,
//                            Data = memberCardInfoView
//                        };
//                    }
//                    else
//                    {
//                        return new ResponseResultDto()
//                        {
//                            Ok = false,
//                            Data = "無會員資料."
//                        };
//                    }
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Inquire Member Info Error >>> MemberID:{memberID} SearchKey:{memberSearchCommand.SearchKey} TeamID:{memberSearchCommand.TeamID}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "查詢會員資訊發生錯誤."
//                };
//            }
//        }

//        /// <summary>
//        /// 上傳頭像
//        /// </summary>
//        /// <param name="memberID">memberID</param>
//        /// <param name="file">file</param>
//        /// <returns>ResponseResultDto</returns>
//        public async Task<ResponseResultDto> UploadPhoto(string memberID, IFormFile file)
//        {
//            if (string.IsNullOrEmpty(memberID))
//            {
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "會員編號無效."
//                };
//            }

//            if (file == null)
//            {
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "無檔案上傳."
//                };
//            }

//            //// TODO 檔案大小判斷
//            //this.logger.LogInformation($"UploadPhoto >>> {file.Length}");
//            try
//            {
//                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.UploadFilesService, "api/UploadFiles/Images", new FormFileCollection() { file });
//                if (httpResponseMessage.IsSuccessStatusCode)
//                {
//                    string photoUrl = (await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>()).FirstOrDefault();

//                    ResponseResultDto responseResult = await this.EditData(new MemberInfoDto() { MemberID = memberID, Photo = photoUrl });
//                    responseResult.Data = responseResult.Ok ? photoUrl : responseResult.Data;
//                    return responseResult;
//                }

//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
//                };
//            }
//            catch (Exception ex)
//            {
//                this.logger.LogError($"Upload Photo Error >>> MemberID:{memberID} File:{file}\n{ex}");
//                return new ResponseResultDto()
//                {
//                    Ok = false,
//                    Data = "上傳頭像發生錯誤."
//                };
//            }
//        }

//        #endregion TODO

//        /// <summary>
//        /// 取得車隊身分
//        /// </summary>
//        /// <param name="teamInfo">teamInfo</param>
//        /// <param name="memberID">memberID</param>
//        /// <returns>int</returns>
//        private int GetTeamIdentity(TeamInfoDto teamInfo, string memberID)
//        {
//            if (teamInfo.TeamLeaderID.Equals(memberID))
//            {
//                return (int)TeamIdentityType.Leader;
//            }
//            else if (teamInfo.TeamViceLeaderIDs.Contains(memberID))
//            {
//                return (int)TeamIdentityType.ViceLeader;
//            }
//            else if (teamInfo.TeamPlayerIDs.Contains(memberID))
//            {
//                return (int)TeamIdentityType.Normal;
//            }

//            return (int)TeamIdentityType.None;
//        }

//        /// <summary>
//        /// 取得車隊踢除設定
//        /// </summary>
//        /// <param name="examinerIdentity">examinerIdentity</param>
//        /// <param name="targetIdentity">targetIdentity</param>
//        /// <returns>int</returns>
//        private int GetTeamKickOutSetFlag(int examinerIdentity, int targetIdentity)
//        {
//            switch (examinerIdentity)
//            {
//                case (int)TeamIdentityType.Leader:
//                case (int)TeamIdentityType.ViceLeader:
//                    switch (targetIdentity)
//                    {
//                        case (int)TeamIdentityType.Leader:
//                            return (int)TeamKickOutSettingType.None;

//                        default:
//                            return (int)TeamKickOutSettingType.KickOut;
//                    }
//                default:
//                    return (int)TeamKickOutSettingType.None;
//            }
//        }

//        /// <summary>
//        /// 取得車隊副隊長委任設定
//        /// </summary>
//        /// <param name="examinerIdentity">examinerIdentity</param>
//        /// <param name="targetIdentity">targetIdentity</param>
//        /// <returns>int</returns>
//        private int GetTeamViceLeaderSetFlag(int examinerIdentity, int targetIdentity)
//        {
//            switch (examinerIdentity)
//            {
//                case (int)TeamIdentityType.Leader:
//                    switch (targetIdentity)
//                    {
//                        case (int)TeamIdentityType.Leader:
//                            return (int)TeamViceLeaderSettingType.None;

//                        case (int)TeamIdentityType.ViceLeader:
//                            return (int)TeamViceLeaderSettingType.Cancel;

//                        default:
//                            return (int)TeamViceLeaderSettingType.Appoint;
//                    }
//                default:
//                    return (int)TeamViceLeaderSettingType.None;
//            }
//        }
//    }
//}