using PKS.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PKS.WebAPI.Services
{
    public interface IBO2Service
    {
        BO2 GetBO(string bot, string bo);
        Task<BO2> GetBOAsync(string bot,string bo);
        BOT GetBOT(string bot);
        Task<BOT> GetBOTAsync(string bot);

        List<BO2> FindBOs(string bot, string bo);
        Task<List<BO2>> FindBOsAsync(string bot, string bo);

        List<BO2> FilterBOs(FilterRequest request);
        Task<List<BO2>> FilterBOsAsync(FilterRequest request);

        long CountBOs(object query);
        Task<long> CountBOsAsync(object query);

        List<BOT> FilterBOTs(FilterRequest request);
        Task<List<BOT>> FilterBOTsAsync(FilterRequest request);
        long CountBOTs(object query);
        Task<long> CountBOTsAsync(object query);

        void InsertBOTs(List<BOT> bots);
        Task InsertBOTsAsync(List<BOT> bots);
        object SaveBOTs(List<BOT> bots);
        Task<object> SaveBOTsAsync(List<BOT> bots);
        object DeleteBOTs(List<string> bots);
        Task<object> DeleteBOTsAsync(List<string> bots);

        void InsertBOs(List<BO2> bos);
        Task InsertBOsAsync(List<BO2> bost);
        object SaveBOs(List<BO2> bos);
        Task<object> SaveBOsAsync(List<BO2> bos);
        object DeleteBOs(BO2DeleteRequest request);
        Task<object> DeleteBOsAsync(BO2DeleteRequest request);

        object Near(NearRequest request);
        Task<object> NearAsync(NearRequest request);

    }
}
