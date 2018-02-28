using Json_Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Juarssic.Server.Comm
{
    public class MongoJsonToSql
    {
        /// <summary>
        /// json to Sql
        /// </summary>
        /// <param name="json"></param>
        /// <param name="ft"></param>
        /// <returns></returns>
        public static string JsonToSql(string json, string bot)
        {
            JsonObjectTree tree = JsonObjectTree.Parse(json);
            string sql = "";
            string sqlFormt = "";
            if (!tree.ListMode[0].Leaives)
            {
                if (tree.ListMode[0].JsonType == JsonType.Object)
                {
                    for (int i = 0; i < tree.ListMode[0].Nodes.Count; i++)
                    {
                        if (i != tree.ListMode[0].Nodes.Count - 1)
                        {
                            sqlFormt += "({" + i + "}" + tree.ListMode[0].Relations;
                        }
                        else
                        {
                            sqlFormt += "{" + i + "})";
                        }
                    }
                }
                else if (tree.ListMode[0].JsonType == JsonType.Array)
                {
                    for (int i = 0; i < tree.ListMode[0].Nodes.Count; i++)
                    {
                        if (i != tree.ListMode[0].Nodes.Count - 1)
                        {
                            sqlFormt += "({" + i + "}" + tree.ListMode[0].Relations;
                        }
                        else
                        {
                            sqlFormt += "{" + i + "})";
                        }
                    }
                }
            }
            else
            {
                //数据类型如何处理问题
                sqlFormt = tree.ListMode[0].FieldN + tree.ListMode[0].Opt + tree.ListMode[0].Value + ")";
            }

            sql += sqlFormt;
            string str = "";
            foreach (ObjModel item in tree.ListMode)
            {
                str += tree.GetSQL(item, bot);
            }
            return str;
        }
    }
}
