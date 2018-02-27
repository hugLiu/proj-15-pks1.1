using PKS.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PKS.DbModels
{
    /// <summary>
    /// 用户认证会话
    /// </summary>
    public partial class UserAuthSessions
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        [StringLength(36)]
        [Required]
        public string SessionKey { get; set; }

        [Required]
        [StringLength(32)]
        public string AppKey { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; }

        [Required]
        [StringLength(10)]
        public string AuthenticationType { get; set; }

        [StringLength(30)]
        public string IPAddress { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime InvalidTime { get; set; }

        public bool Valid { get; set; }

        public DateTime? LogoutTime { get; set; }
    }
}
