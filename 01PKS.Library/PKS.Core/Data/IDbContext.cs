using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PKS.Data
{
    /// <summary>数据访问配置接口</summary>
    public interface IDbContextConfig
    {
        /// <summary>连接名称</summary>
        string ConnectionName { get; }
        /// <summary>实体映射配置集合</summary>
        List<IDbEntityMappingConfiguration> MappingConfigs { get; }
        /// <summary>是否发布变化</summary>
        bool PublishChange { get; }
    }

    /// <summary>数据访问实体映射配置接口</summary>
    public interface IDbEntityMappingConfiguration
    {
        /// <summary>初始化实体映射</summary>
        void OnModelCreating(DbModelBuilder modelBuilder);
        /// <summary>映射实体集合</summary>
        List<object> MapTypes { get; }
        /// <summary>变化发布实体集合</summary>
        List<Type> ChangePublishTypes { get; }
    }

    /// <summary>数据访问上下文接口</summary>
    public interface IDbContext
    {
    }

    /// <summary>数据库提供者</summary>
    public static class DbProvider
    {
        /// <summary>Odbc</summary>
        public const string Odbc = "System.Data.Odbc";
        /// <summary>OleDb</summary>
        public const string OleDb = "System.Data.OleDb";
        /// <summary>SQLServer</summary>
        public const string SqlClient = "System.Data.SqlClient";
        /// <summary>MS提供的Oracle客户端</summary>
        public const string MSOracleClient = "System.Data.OracleClient";
        /// <summary>Oracle客户端</summary>
        public const string OracleClient = "Oracle.ManagedDataAccess.Client";
        /// <summary>Oracle原生客户端</summary>
        public const string OracleUnmanagedClient = "Oracle.DataAccess.Client";
        /// <summary>是否Oracle客户端</summary>
        public static bool IsOracleClient(string connectionName)
        {
            return IsOracleClient(ConfigurationManager.ConnectionStrings[connectionName]);
        }
        /// <summary>是否Oracle客户端</summary>
        public static bool IsOracleClient(this ConnectionStringSettings settings)
        {
            var provider = settings.ProviderName;
            return provider.Equals(OracleClient, StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>获得Oracle Schema</summary>
        public static string GetOracleSchema(this ConnectionStringSettings settings)
        {
            var builder = new DbConnectionStringBuilder();
            builder.ConnectionString = settings.ConnectionString;
            return builder["USER ID"].ToString();
        }
    }
}
