using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using PKS.Core;
using PKS.Data;
using PKS.DbModels;
using PKS.Models;
using PKS.Utils;
using PKS.WebAPI.Models;

namespace PKS.DbServices.SysFrame
{
    /// <summary>用户身份认证服务</summary>
    public class IdentityService : AppService, IPerRequestAppService
    {
        /// <summary>登录请求验证</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<LoginResult> LoginAsync(LoginRequest request, TimeSpan tokenExpireInterval)
        {
            var result = new LoginResult();
            var userInfo = await GetUserInfo(request.UserName);
            if (userInfo == null)
            {
                result.ErrorMessage = $"用户名称{request.UserName}不存在";
                return result;
            }
            if (request.AuthenticationType == AuthenticationType.Forms && userInfo.PASSWORD != HashPassword(request.Password, userInfo.PASSWORDSALT))
            {
                result.ErrorMessage = $"用户密码错误";
                return result;
            }
            var principal= BuildPrincipal(userInfo, request.AuthenticationType.ToString());
            var userAuthSessionsRepository = GetService<IRepository<UserAuthSessions>>();
            var token = Guid.NewGuid().ToString();
            var now = DateTime.Now;
            var userAuthSession = new UserAuthSessions
            {
                SessionKey = token,
                AppKey = request.AppCode,
                UserName = userInfo.USERNAME,
                AuthenticationType = principal.Identity.AuthenticationType,
                Valid = true,
                CreateTime = now,
                InvalidTime = now + tokenExpireInterval,
                IPAddress = request.UserHostAddress,
            };
            userAuthSessionsRepository.Add(userAuthSession);
            principal.CreateTime = userAuthSession.CreateTime;
            principal.ExpireTime = userAuthSession.InvalidTime;
            result.Token = token;
            result.Principal = principal;
            result.Succeed = true;
            return result;
        }
        /// <summary>生成密码散列值</summary>
        private string HashPassword(string password, string salt)
        {
            var md5 = new MD5CryptoServiceProvider();
            var content = password + salt;
            var buffer = Encoding.UTF8.GetBytes(content);
            var hash = md5.ComputeHash(buffer);
            return BitConverter.ToString(hash).Replace("-", "");
        }
        /// <summary>续期</summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<LoginResult> RenewAsync(string token, IPKSPrincipal principal, TimeSpan tokenExpireInterval)
        {
            var userAuthSessionsRepository = GetService<IRepository<UserAuthSessions>>();
            var userSession = await LoadSessionFromToken(userAuthSessionsRepository, token);
            if (userSession == null || !userSession.Valid) return null;
            var userInfo = await GetUserInfo(userSession.UserName);
            var newPrincipal = BuildPrincipal(userInfo, userSession.AuthenticationType);
            var newToken = Guid.NewGuid().ToString();
            var now = DateTime.Now;
            var newUserSession = new UserAuthSessions
            {
                SessionKey = newToken,
                AppKey = userSession.AppKey,
                UserName = userInfo.USERNAME,
                AuthenticationType = userSession.AuthenticationType,
                Valid = true,
                CreateTime = now,
                InvalidTime = now + tokenExpireInterval,
                IPAddress = userSession.IPAddress,
            };
            userAuthSessionsRepository.Add(newUserSession);
            newPrincipal.CreateTime = newUserSession.CreateTime;
            newPrincipal.ExpireTime = newUserSession.InvalidTime;
            var result = new LoginResult();
            result.Token = newToken;
            result.Principal = newPrincipal;
            result.Succeed = true;
            return result;
        }

#if DEBUG
        /// <summary>调试用</summary>
        private PKSPrincipal DebugPrincipal { get; set; }
        /// <summary>调试用</summary>
        private async Task<PKSPrincipal> GetDebugPrincipal()
        {
            if (this.DebugPrincipal == null)
            {
                var userInfo = await GetUserInfo("admin");
                this.DebugPrincipal = BuildPrincipal(userInfo, AuthenticationType.Forms.ToString());
            }
            return this.DebugPrincipal;
        }
#endif
        /// <summary>
        /// 根据token获取用户认证信息
        /// </summary>
        public async Task<IPKSPrincipal> GetPrincipalAsync(string token)
        {
            PKSPrincipal principal = null;
            if (token == PKSWebConsts.Token_Debug)
            {
#if DEBUG
                principal = await GetDebugPrincipal();
                principal.CreateTime = DateTime.Now;
                principal.ExpireTime = principal.CreateTime.AddDays(1);
#endif
            }
            else
            {
                var userAuthSessionsRepository = GetService<IRepository<UserAuthSessions>>();
                var userSession = await LoadSessionFromToken(userAuthSessionsRepository, token);
                if (userSession == null || !userSession.Valid) return null;
                var userInfo = await GetUserInfo(userSession.UserName);
                if (userInfo == null) return null;
                principal = BuildPrincipal(userInfo, userSession.AuthenticationType);
                principal.CreateTime = userSession.CreateTime;
                principal.ExpireTime = userSession.InvalidTime;
            }
            return principal;
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        public async Task LogoutAsync(string token)
        {
            var userAuthSessionsRepository = GetService<IRepository<UserAuthSessions>>();
            var userSession = await LoadSessionFromToken(userAuthSessionsRepository, token);
            if (userSession == null || !userSession.Valid) return;
            userSession.Valid = false;
            userSession.LogoutTime = DateTime.Now;
            await userAuthSessionsRepository.SubmitAsync();
        }
        /// <summary>
        /// 生成用户认证信息
        /// </summary>
        private PKSPrincipal BuildPrincipal(VI_USERINFO userInfo, string authenticationType)
        {
            var principal = new PKSPrincipal
            {
                Roles = new[]
                {
                    new PKSRole {Id = userInfo.ROLEID.ToString(), Name = userInfo.ROLENAME, Description = userInfo.ROLEDESC}
                },
                Identity = new PKSIdentity
                {
                    Id = userInfo.USERID.ToString(),
                    Name = userInfo.USERNAME,
                    AuthenticationType = authenticationType,
                    IsAuthenticated = true
                }
            };
            return principal;
        }
        /// <summary>
        /// 根据用户名获取用户综合信息
        /// </summary>
        /// <param name="userName">LoginRequest->UserName</param>
        /// <returns></returns>
        private async Task<VI_USERINFO> GetUserInfo(string userName)
        {
            return await GetService<IRepository<VI_USERINFO>>()
                .GetQuery()
                .FirstOrDefaultAsync(v => v.USERNAME.ToLower() == userName.ToLower());
        }
        /// <summary>
        /// 根据用户Id获取用户综合信息
        /// </summary>
        public VI_USERINFO GetUserInfoById(int userId)
        {
            return GetService<IRepository<VI_USERINFO>>()
                .GetQuery()
                .FirstOrDefault(v => v.USERID == userId);
        }
        /// <summary>
        /// 从数据库中验证用户认证Session是否存在并且在有效期内
        /// </summary>
        /// <param name="appKey"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private async Task<UserAuthSessions> LoadSession(IRepository<UserAuthSessions> userAuthSessionsRepository, string appKey, string userName)
        {
            return await userAuthSessionsRepository
                .GetQuery()
                .OrderByDescending(e => e.CreateTime)
                .FirstOrDefaultAsync(p => p.AppKey == appKey && p.UserName == userName);
        }
        /// <summary>
        /// 从数据库中验证用户认证Session是否存在并且在有效期内
        /// </summary>
        private async Task<UserAuthSessions> LoadSessionFromToken(IRepository<UserAuthSessions> userAuthSessionsRepository, string token)
        {
            var now = DateTime.Now;
            return await userAuthSessionsRepository
                .GetQuery()
                .FirstOrDefaultAsync(p => p.SessionKey == token && p.InvalidTime > now);
        }

        /// <summary>
        /// 根据用户名称获取UserProfile
        /// </summary>
        /// <param name="username">LoginRequest->UserName</param>
        /// <returns></returns>
        private USERPROFILE GetUserProfile(string username)
        {
            return GetService<IRepository<USERPROFILE>>().GetQuery().Where(u => u.USERNAME == username)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据用户id获取MemberShip
        /// </summary>
        /// <param name="userid">USERPROFILE->USERID</param>
        /// <returns></returns>
        private WEBPAGES_MEMBERSHIP GetMemberShip(decimal userid)
        {
            return GetService<IRepository<WEBPAGES_MEMBERSHIP>>().GetQuery().Where(m => m.USERID == userid)
                .FirstOrDefault();
        }
    }
}