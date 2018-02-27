using Jurassic.AppCenter.AppServices;
using Jurassic.Com.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;

namespace Jurassic.AppCenter
{
    /// <summary>
    /// 用户登录状态管理类，用于控制单点登录和用户个性化信息的缓存
    /// 在分布式环境下，必须保证一个服务进程始终处理一个用户的会话
    /// </summary>
    public class StateManager :IStateManager, IDisposable
    {
        private Dictionary<string, LoginResult> _usersDict;
        HeartBeat mCheckUserOnlineBeat;

        bool _singleLogin = false;

        /// <summary>
        /// 是否单点登录，默认值是False
        /// </summary>
        public bool SingleLogin
        {
            get { return _singleLogin; }
            set { _singleLogin = value; }
        }

        /// <summary>
        /// 存储用户会话的字典表
        /// </summary>
        protected Dictionary<string, LoginResult> UsersDict
        {
            get
            {                
                if (_usersDict == null)
                {
                    _usersDict = new Dictionary<string, LoginResult>();
                }
                return _usersDict;
            }
        }

        /// <summary>
        /// 利用缓存提供类创建一个状态管理类
        /// </summary>
        /// <param name="singleLogin">是否要支持单点登录</param>
        public StateManager(bool singleLogin)
        {
            _singleLogin = singleLogin;
            mCheckUserOnlineBeat = new HeartBeat("CheckUserOnline", 60, CheckUsersOnline);
            mCheckUserOnlineBeat.Start();
        }

        private void CheckUsersOnline()
        {
            var onlineUsers = GetOnlineUsers();
            var outTime = TimeHelper.ServerTime.AddSeconds(-AppManager.Instance.StateProvider.Timeout);
            onlineUsers.Where(uk => uk.Value.User.LastOpTime < outTime).Each(uk =>
            {
                uk.Value.User.IsOnline = false;
                _usersDict.Remove(uk.Key);
            });
        }

        /// <summary>
        /// 获取所有登录用户
        /// </summary>
        /// <returns>所有登录用户</returns>
        List<KeyValuePair<string, LoginResult>> GetOnlineUsers()
        {
            //修改人：卢英杰
            //修改于: 2015.8.26。
            //原因：在项目中应用框架的时候有时会出现_usersDict为null的异常，所以增加一个简单的null判断。
            //新增代码
            if (_usersDict == null)
            {
                return new Dictionary<string, LoginResult>().ToList();
            }

            var onLineUsers = _usersDict
                .OrderByDescending(u => u.Value.User.LastOpTime)
                .ToList();

            return onLineUsers;
        }

        private object synObj = new object();

        /// <summary>
        /// 新增用户会话信息到会话表
        /// </summary>
        /// <param name="result"></param>
        public void Add(LoginResult result)
        {
            lock (synObj)
            {
                // 如果是单点登录，则移除原来登录的sessionId
                if (_singleLogin)
                {
                    UsersDict.Where(ud => ud.Value.User.Id == result.User.Id)
                       .Select(kv => kv.Key).ToList()
                       .Each(key => UsersDict[key].State = LoginState.OtherSessionLogined);
                }
                if (!UsersDict.ContainsKey(result.SessionID))
                {
                    UsersDict.Add(result.SessionID, result);
                }
            }
        }

        /// <summary>
        /// 根据用户会话ID找到登录用户信息
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public LoginResult GetLoginResult(string sessionId)
        {
            var result = UsersDict[sessionId];
            return result;
        }

        public void Dispose()
        {
            UsersDict.Clear();
        }
    }
}
