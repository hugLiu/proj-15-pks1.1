using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jurassic.Com.DB;

namespace Jurassic.AppCenter
{
    public class MssqlSchemaScriptProvider : ISchemaScriptProvider
    {
        private static Dictionary<string, string> schemaScriptDictionary;
        private static string currentFrameSchemaVersion = "2.2";//当前框架数据结构版本

        public Dictionary<string, string> GetSchemaScriptDictionary()
        {
            if (schemaScriptDictionary == null)
            {
                schemaScriptDictionary = new Dictionary<string, string>();

                SchemaScriptDictionaryInitialization();
            }

            return schemaScriptDictionary;
        }

        /// <summary>
        /// 当前框架数据结构版本
        /// </summary>
        public string GetCurrentFrameSchemaVersion()
        {
            return currentFrameSchemaVersion;
        }

        /// <summary>
        /// 架构脚本初始化
        /// </summary>
        private void SchemaScriptDictionaryInitialization()
        {
            schemaScriptDictionary.Add("GetMaxVersion", @"if object_id(N'SchemaVersion') is not null select top 1 Version from SchemaVersion order by convert(float,Version) desc else select '-1' as Version ");
            schemaScriptDictionary.Add("CreateSchemaVersionTable", @"if object_id(N'SchemaVersion') is null create table SchemaVersion ( Id int identity(1,1) Primary key,[Version] nvarchar(50),UpdateTime datetime,Remark nvarchar(max) )");
            schemaScriptDictionary.Add("UpdateSchemaVersion", @"insert into [SchemaVersion]([Version],[UpdateTime],[Remark]) values('@Version','@UpdateTime','@Remark')");

            //schemaScriptDictionary.Add("2.0", @" IF COL_LENGTH('UserProfile', 'AvatarId') IS NULL alter table UserProfile add AvatarId int null; ");

            schemaScriptDictionary.Add("2.1", @" IF NOT EXISTS(select 1 FROM syscolumns where id=object_id('Dep_DepUser') AND NAME='PostId') BEGIN ALTER TABLE Dep_DepUser ADD PostId INT NULL END; IF NOT EXISTS(select 1 FROM syscolumns where id=object_id('Dep_Department') AND NAME='OrgNode') BEGIN ALTER TABLE Dep_Department ADD OrgNode NVARCHAR(200) NULL; END; ");
            //部门与用户关系变新增IsMain(是否主部门)与IsLeader(是否主管)字段
            schemaScriptDictionary.Add("2.2", @" IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE id = OBJECT_ID('Dep_DepUser') AND NAME = 'IsMain' ) BEGIN ALTER TABLE Dep_DepUser ADD IsMain INT NULL END; IF NOT EXISTS(SELECT 1 FROM syscolumns WHERE  id = OBJECT_ID('Dep_DepUser') AND NAME = 'IsLeader') BEGIN ALTER TABLE Dep_DepUser ADD IsLeader INT NULL; END; ");

       
        
        }
    }
}
