using Jurassic.AppCenter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebMatrix.WebData;
using Jurassic.Com.Tools;
using Jurassic.AppCenter.Models;
using System.Data.Entity;
using Jurassic.AppCenter.AppServices;
using Jurassic.AppCenter.Resources;
using System.Threading;
using System.Web.Security;
using Ninject;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Jurassic.CommonModels.EFProvider
{

    /// <summary>
    /// 实际系统实现自己的DataProvider
    /// </summary>
    public class MyRoleProvider : IDataProvider<AppRole>
    {
        public MyRoleProvider()
        {

        }

        public IEnumerable<AppRole> GetData()
        {
            using (var _context = new ModelContext())
            {
                return _context.Set<Role>().Select(r => new
                    {
                        Id = r.RoleId,
                        Name = r.RoleName,
                        Description = r.Description
                    }).ToList().Select(r => new AppRole
                    {
                        Id = r.Id.ToString(),
                        Name = r.Name,
                        Description = r.Description
                    }).ToList();
            }
        }

        public int Add(AppRole t)
        {
            using (var _context = new ModelContext())
            {

                var dbRole = new Role()
                {
                    RoleName = t.Name,
                    Description = t.Description,
                };
                _context.Set<Role>().Add(dbRole);
                var r = _context.SaveChanges();
                t.Id = dbRole.RoleId.ToString();
                return r;
            }
        }

        public int Change(AppRole t)
        {
            using (var _context = new ModelContext())
            {
                var dbRole = new Role()
                           {
                               RoleId = t.Id.ToInt(),
                               RoleName = t.Name,
                               Description = t.Description,
                           };
                _context.Entry<Role>(dbRole).State = EntityState.Modified;
                return _context.SaveChanges();
            }
        }

        public int Delete(AppRole t)
        {
            var roleId = t.Id.ToInt();
            using (var _context = new ModelContext())
            {
                var dbRole = new Role()
                {
                    RoleId = roleId,
                    RoleName = t.Name,
                    Description = t.Description,
                };
                var userInRoles = _context.Set<UserInRole>().Where(uir => uir.RoleId == roleId);
                var trans = _context.Database.BeginTransaction();
                userInRoles.Each(uir => _context.Set<UserInRole>().Remove(uir));
                _context.Entry<Role>(dbRole).State = EntityState.Deleted;
                var r = _context.SaveChanges();
                trans.Commit();
                return r;
            }
        }
    }

    public class MyUserProvider : IDataProvider<AppUser>
    {
        static MyUserProvider()
        {
        }

        public MyUserProvider()
        {
        }

        public IEnumerable<AppUser> GetData()
        {
            using (var _context = new ModelContext())
            {
                /*
                 * 修改原有用户查询对象UserProfiles为MemberShips,对已删除用户的过滤
                 * IsConfirmed = true 有效客户 IsConfirmed = false 无效客户(删除)
                 *  by_zjf
                 */
                var query = _context.MemberShips.Include("UserInRoles")
                    .ToList()
                .Select(u => new AppUser()
                {
                    Id = u.UserId.ToString(),
                    Name = u.UserName,
                    // 此处加入自定义属性
                    Email = u.Email,
                    PhoneNumber = u.PhoneNumber,

                    RoleIds = u.UserInRoles.Select(ur => ur.RoleId.ToString())
                });
                return query.ToList();
            }
        }

        public int Delete(AppUser user)
        {
            using (var _context = new ModelContext())
            {

                var dbuser = _context.Set<MemberShip>().Find(user.Id.ToInt());

                if (dbuser != null)
                {
                    //此处设置删除用户不做物理删除,修改状态进行控制用户无效 by_zjf
                    dbuser.IsConfirmed = false;
                    //_context.UserProfiles.Remove(dbuser);
                    _context.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        public int Add(AppUser user)
        {
            //WebSecurity.CreateUserAndAccount(user.Name, "123456", new { Email = ((AppUserEx)user).Email });
            MemberShip member = new MemberShip()
            {
                UserName = user.Name,
                CreateDate = DateTime.Now,
                IsConfirmed = true,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                PasswordSalt = CommOp.NewId(),
            };

            member.Password = MyStateProvider.EncryptPassword("123456", member.PasswordSalt);

            foreach (string roleId in user.RoleIds)
            {
                UserInRole uir = new UserInRole
                {
                    RoleId = roleId.ToInt(),
                };
                member.UserInRoles.Add(uir);
            }
            using (var _context = new ModelContext())
            {
                _context.Set<MemberShip>().Add(member);
                _context.SaveChanges();
            }
            user.Id = member.UserId.ToString();
            return 1;
        }

        public int Change(AppUser t)
        {
            using (var _context = new ModelContext())
            {
                int tid = t.Id.ToInt();
                UserProfile user = _context.UserProfiles.Include("UserInRoles").FirstOrDefault(u => u.UserId == tid);
                if (user == null) return 0;

                user.UserName = t.Name;
                 
                //此处加入自定义属性
                user.Email = t.Email;
                user.PhoneNumber = t.PhoneNumber;

                //下面是写用户对应的角色
                var existIds = user.UserInRoles.Select(r => r.RoleId.ToString());
                var roleIdsToAdd = t.RoleIds.Except(existIds);
                var rolesIdsToDelete = existIds.Except(t.RoleIds);

                user.UserInRoles.Where(ur => rolesIdsToDelete.Contains(ur.RoleId.ToString()))
                   .ToArray().Each(ur => _context.Entry(ur).State = EntityState.Deleted);

                roleIdsToAdd.Each(rid => user.UserInRoles
                    .Add(new UserInRole { RoleId = rid.ToInt(), UserId = user.UserId }));

                 _context.SaveChanges(); //context会判断到底有没有数据修改，从而可能返回0
                 return 1;
            }
        }
    }

    public class MyStateProvider : IStateProvider
    {
        public MyStateProvider()
        {
        }

        internal static string EncryptPassword(string password, string salt)
        {
            return Encryption.MD5(password + salt);
        }

        public virtual LoginState Login(LoginModel model)
        {
            // var user = _context.UserProfiles.Find(2);
            //var art = _context.Set<Role>().ToList();
            using (var _context = new ModelContext())
            {
                //添加m.IsConfirmed==true验证判断该用户是否被删除 by_zjf
                MemberShip member =  _context.Set<MemberShip>().FirstOrDefault(m => m.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) && m.IsConfirmed == true);

                if (member == null)
                {
                    return LoginState.UserOrPasswordError;
                }

                if (EncryptPassword(model.Password, member.PasswordSalt) != member.Password)
                {
                    member.LastPasswordFailureDate = DateTime.Now;
                    _context.SaveChanges();
                    return LoginState.UserOrPasswordError;
                }

                //验证是否强制修改密码_by_zjf
                model.IsChangedPassword = false;
                if (member.PasswordChangedDate == null)
                {
                    model.IsChangedPassword = true;
                } 
            } 
            ////if (HttpContext.Current == null) HttpContext.Current = new HttpContext(new HttpRequest("File.aspx", "http://localhost", ""), new HttpResponse(new System.IO.StreamWriter(new System.IO.MemoryStream())));
            return LoginState.OK;
        }
        /// <summary>
        /// 设置授权Cookie
        /// </summary>
        /// <param name="model">登录数据实体</param>
        public virtual bool SetAuthCookie(LoginModel model)
        {
            return false;
        }

        public virtual LoginState SendPasswordResetMessage(string userName, string email, string resetUrl)
        {
            var _context = new ModelContext();

            MemberShip member = _context.Set<MemberShip>().FirstOrDefault(m => m.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) && m.IsConfirmed == true);

            if (member == null)
            {
                return LoginState.UserNotExist;
            }
            if (!member.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            {
                return LoginState.EmailError;
            }
            member.ConfirmationToken = CommOp.NewId();
            string url = resetUrl + "?username=" + userName + "&confirmToken=" + member.ConfirmationToken;
            string subject = ResHelper.GetStr("Password+Reset");
            string body = "<p>" + ResHelper.GetStr("Click the link below to reset your password") + "</p>"
                + "<p>" + "<a href='" + url + "' target='_blank'>" + url + "</a></p>";
            SMTPMail mail = new SMTPMail(member.Email, subject, body);
            _context.SaveChanges();
            mail.Send();
            if (mail.ErrorMessage.IsEmpty())
            {
                return LoginState.OK;
            }
            else
            {
#if DEBUG
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Session["ResetPasswordEmailBody"] = body;
                }
#endif
                return LoginState.EmailSendError;
            }
        }

        public virtual LoginState ResetPassword(PasswordResetModel model)
        {
            var _context = new ModelContext();

            // var user = _context.UserProfiles.Find(2);
            MemberShip member = _context.Set<MemberShip>().FirstOrDefault(m => m.UserName.Equals(model.UserName, StringComparison.OrdinalIgnoreCase));

            if (member == null)
            {
                return LoginState.UserNotExist;
            }

            if (model.Password != model.ConfirmPassword && model.IsResetPass != 1)
            {
                return LoginState.PasswordError;
            }

            if (model.ConfirmToken != member.ConfirmationToken && model.IsResetPass != 1)
            {
                return LoginState.TokenError;
            }
            member.PasswordSalt = CommOp.NewId();
            member.Password = Encryption.MD5(model.Password + member.PasswordSalt);
            member.PasswordChangedDate = DateTime.Now;
            //对于系统重置的密码,强制吧修改密码日期移除让用户登陆重新修改面貌
            if (model.IsResetPass == 1)
                member.PasswordChangedDate = null;
            _context.SaveChanges();
            return LoginState.OK;
        }

        public virtual LoginState ChangePassword(PasswordChangeModel model)
        {
            var _context = new ModelContext();

            // var user = _context.UserProfiles.Find(2);
            MemberShip member = _context.Set<MemberShip>().FirstOrDefault(m => m.UserName.Equals(
                Thread.CurrentPrincipal.Identity.Name,
                StringComparison.OrdinalIgnoreCase));

            if (member == null)
            {
                return LoginState.UserNotExist;
            }

            if (model.Password != model.ConfirmPassword)
            {
                return LoginState.PasswordError;
            }

            if (EncryptPassword(model.OldPassword, member.PasswordSalt) != member.Password)
            {
                return LoginState.OldPasswordError;
            }

            member.PasswordSalt = CommOp.NewId();
            member.Password = Encryption.MD5(model.Password + member.PasswordSalt);
            member.PasswordChangedDate = DateTime.Now;
            _context.SaveChanges();
            return LoginState.OK;
        }

        public virtual void Logout(string userName)
        {
            //WebSecurity.Logout();
        }

        /// <summary>
        /// 指定登录过期时间(以秒为单位)
        /// </summary>
        public virtual int Timeout
        {
            get { return (int)FormsAuthentication.Timeout.TotalSeconds; }
        }
    }

    public class MyTypeProvider : ITypeProvider
    {
        public IEnumerable<Type> GetTypes()
        {
            yield return typeof(AppUser);
        }
    }

}