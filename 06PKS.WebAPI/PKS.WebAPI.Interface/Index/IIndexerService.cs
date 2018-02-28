using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Jurassic.PKS.Service;
using Nest;
using PKS.Models;
using PKS.WebAPI.Models;

namespace PKS.WebAPI.Services
{
    /// <summary>索引数据服务接口</summary>
    public interface IIndexerService
    {
        /// <summary>插入</summary>
        string[] Insert(IndexInsertRequest request);

        /// <summary>插入</summary>
        Task<string[]> InsertAsync(IndexInsertRequest request);
        /// <summary>保存</summary>
        string[] Save(IndexSaveRequest request);

        /// <summary>保存</summary>
        Task<string[]> SaveAsync(IndexSaveRequest request);
        /// <summary>删除</summary>
        string[] Delete(List<string> iiids);

        /// <summary>删除</summary>
        Task<string[]> DeleteAsync(List<string> iiids);
   }
}