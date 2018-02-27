using System.Data.Entity.SqlServer;
using System.Diagnostics;
using Ninject;
using Ninject.Web.Common;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.EntityFramework;
using PKS.Core;

namespace PKS.Data
{
    /// <summary>仓储数据访问组件加载器</summary>
    public class EFRepositoryLoader : IServiceLoader<RepositoryLoaderConfig>
    {
        /// <summary>初始化</summary>
        public void Initialize(IKernel kernel, RepositoryLoaderConfig config)
        {
            kernel.Bind<IDbContextConfig>().ToConstant(config);
            if (config.EnableWeb)
            {
                kernel.Bind<IDbContext>().To<EFDbContext>().InRequestScope();
                kernel.Bind(typeof(IRepository<>)).To(typeof(EFRepository<>)).InRequestScope();
            }
            else
            {
                kernel.Bind<IDbContext>().To<EFDbContext>().InThreadScope();
                kernel.Bind(typeof(IRepository<>)).To(typeof(EFRepository<>)).InThreadScope();
            }
            TestConnection(config);
        }

        /// <summary>加入Oracle数据库支持</summary>
        public void UseOracle()
        {
        }

        /// <summary>测试连接</summary>
        public void TestConnection(IDbContextConfig config)
        {
            var context = new EFDbContext(config, null);
            context.Database.Initialize(false);
            var connection = context.Database.Connection;
            connection.Open();
            connection.Close();
        }

        /// <summary>加载间接DLL</summary>
        private void LoadIndirectAssemblies()
        {
            Debug.WriteLine(typeof(SqlProviderServices));
            Debug.WriteLine(typeof(OracleClientFactory));
            Debug.WriteLine(typeof(EFOracleProviderServices));
        }
    }
}