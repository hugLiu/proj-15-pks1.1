using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PKS.DbModels
{
    /// <summary>IAuditedModel</summary>
    public interface IAuditedModel
    {
        ///<summary>
        /// CREATEDBY (length: 50)
        ///</summary>
        string CreatedBy { get; set; }

        ///<summary>
        /// CREATEDDATE
        ///</summary>
        DateTime? CreatedDate { get; set; }

        ///<summary>
        /// LASTUPDATEDBY (length: 50)
        ///</summary>
        string LastUpdatedBy { get; set; }

        ///<summary>
        /// LASTUPDATEDDATE
        ///</summary>
        DateTime? LastUpdatedDate { get; set; }
    }

    /// <summary>PKS_AuditedModel</summary>
    [Serializable]
    public class PKS_AuditedModel : IAuditedModel
    {
        ///<summary>
        /// Id (Primary key)
        ///</summary>
        public int Id { get; set; }

        ///<summary>
        /// CREATEDBY (length: 50)
        ///</summary>
        public string CreatedBy { get; set; }

        ///<summary>
        /// CREATEDDATE
        ///</summary>
        public DateTime? CreatedDate { get; set; }

        ///<summary>
        /// LASTUPDATEDBY (length: 50)
        ///</summary>
        public string LastUpdatedBy { get; set; }

        ///<summary>
        /// LASTUPDATEDDATE
        ///</summary>
        public DateTime? LastUpdatedDate { get; set; }
    }

    /// <summary>IAuditedModel扩展</summary>
    public static class PKS_AuditedModelExtension
    {
        ///<summary>插入赋值</summary>
        public static void SetInsert(this IAuditedModel model)
        {
            model.CreatedDate = DateTime.Now;
            model.LastUpdatedDate = model.CreatedDate;
        }
        ///<summary>更新赋值</summary>
        public static void SetUpdate(this IAuditedModel model)
        {
            model.LastUpdatedDate = DateTime.Now;
        }
    }
}
