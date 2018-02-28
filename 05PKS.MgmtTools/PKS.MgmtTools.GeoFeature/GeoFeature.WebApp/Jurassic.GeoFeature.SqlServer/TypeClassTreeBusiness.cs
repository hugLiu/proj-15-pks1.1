using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Jurassic.GeoFeature.IDAL;
using Jurassic.GeoFeature.Model;
using Jurassic.GeoFeature.DBUtility;
using System.Data.SqlClient;

namespace Jurassic.GeoFeature.SqlServer
{
    public class TypeClassTreeBusiness : ITypeClass
    {
        public IList<TypeClassTree> GetList()
        {
            List<TypeClassTree> list = new List<TypeClassTree>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT T1.ID AS ID,T1.NAME AS NAME ,'0' AS PID,'' AS FT,");
            strSql.Append(" T1.ISUSERDEFINE ,'' AS SHAPE,'' AS USEGEOMETRY,'TypeClass' AS TYPE,'' AS ENAME FROM OBJTYPECLASS T1 ");
            strSql.Append(" UNION ");
            strSql.Append(" SELECT O.BOTID AS ID,O.BOT AS NAME,T.ID AS PID,O.FT,O.ISUSERDEFINE,");
            strSql.Append(" O.SHAPE,O.USEGEOMETRY,'ObjType' AS TYPE,O.NAME AS ENAME  FROM OBJECTTYPE O JOIN OBJTYPECLASS T ON T.ID=O.ID ");
            list.AddRange(SqlServerDBHelper.ExecuteQueryText<TypeClassTree>(strSql.ToString()));
            return list;
        }
        public bool Exist(TypeClassTree model)
        {
            return false;
        }
        public int Insert(TypeClassTree model)
        {
            return 0;
        }
        public int Update(TypeClassTree model)
        {
            return 0;
        }
        public int Delete(TypeClassTree model)
        {
            return 0;
        }
        public IList<TypeClassTree> GetListByID(string ID)
        {
            return null;
        }
        public IList<TypeClassTree> GetListByName(string Name)
        { return null; }

        public bool Save(List<TypeClassTree> tcl)
        {
            List<TypeClassTree> oldTcl = GetList().ToList();

            //删去的
            List<TypeClassTree> d = oldTcl.Except(tcl, new TCLComparer()).ToList();

            //新增的
            List<TypeClassTree> a = tcl.Except(oldTcl, new TCLComparer()).ToList();

            //修改的
            List<TypeClassTree> u = tcl.Except(oldTcl, new TCLComparer1()).ToList().Except(a, new TCLComparer()).ToList();

            List<SqlCommand> oracleCommList = new List<SqlCommand>();

            foreach (TypeClassTree tc in d)
            {
                SqlCommand oracleCommD = new SqlCommand();
                if (tc.Type == "ObjType")
                {
                    oracleCommD.CommandText = "DELETE FROM OBJECTTYPE WHERE BOTID=@ID";
                }
                else
                {
                    oracleCommD.CommandText = "DELETE FROM OBJTYPECLASS WHERE ID=@ID";
                }
                var oraParams = new List<SqlParameter>();
                oraParams.Add(new SqlParameter("ID", SqlDbType.VarChar) { Value = tc.Id });
                oracleCommD.Parameters.AddRange(oraParams.ToArray());
                oracleCommList.Add(oracleCommD);
            }
            foreach (TypeClassTree tc in a)
            {
                SqlCommand oracleCommD = new SqlCommand();
                var oraParams = new List<SqlParameter>();
                if (tc.Type == "ObjType")
                {
                    oracleCommD.CommandText = "INSERT INTO OBJECTTYPE(BOTID,BOT,FT,ID,ISUSERDEFINE,SHAPE,USEGEOMETRY,NAME) VALUES(@BOTID,@BOT,@FT,@ID,@ISUSERDEFINE,@SHAPE,@USEGEOMETRY,@ENAME)";
                    oraParams.Add(new SqlParameter("BOTID", SqlDbType.VarChar) { Value = tc.Id });
                    oraParams.Add(new SqlParameter("BOT", SqlDbType.VarChar) { Value = tc.Name });
                    oraParams.Add(new SqlParameter("FT", SqlDbType.VarChar) { Value = tc.FT });
                    oraParams.Add(new SqlParameter("ID", SqlDbType.VarChar) { Value = tc.PId });
                    oraParams.Add(new SqlParameter("ISUSERDEFINE", SqlDbType.VarChar) { Value = tc.IsUserDefine });
                    oraParams.Add(new SqlParameter("SHAPE", SqlDbType.VarChar) { Value = tc.Shape});
                    oraParams.Add(new SqlParameter("USEGEOMETRY", SqlDbType.VarChar) { Value = tc.UseGeometry });
                    oraParams.Add(new SqlParameter("ENAME", SqlDbType.VarChar) { Value = tc.EName });

                    SqlCommand oracleCommDP = new SqlCommand();

                    oracleCommList.AddRange(BuildPSetComm(tc.OPL, tc.Id));
                }
                else
                {
                    oracleCommD.CommandText = "INSERT INTO OBJTYPECLASS(ID,NAME,ISUSERDEFINE) VALUES(@ID,@NAME,@ISUSERDEFINE)";
                    oraParams.Add(new SqlParameter("ID", SqlDbType.VarChar) { Value = tc.Id });
                    oraParams.Add(new SqlParameter("NAME", SqlDbType.VarChar) { Value = tc.Name });
                    oraParams.Add(new SqlParameter("ISUSERDEFINE", SqlDbType.VarChar) { Value = tc.IsUserDefine });
                }
                oracleCommD.Parameters.AddRange(oraParams.ToArray());
                oracleCommList.Add(oracleCommD);
            }
            foreach (TypeClassTree tc in u)
            {
                SqlCommand oracleCommD = new SqlCommand();
                var oraParams = new List<SqlParameter>();
                if (tc.Type == "ObjType")
                {
                    oracleCommD.CommandText = "UPDATE OBJECTTYPE SET BOT=@BOT,FT=@FT,ID=@ID,ISUSERDEFINE=@ISUSERDEFINE,SHAPE=@SHAPE,USEGEOMETRY=@USEGEOMETRY,NAME=@ENAME WHERE BOTID=@BOTID";
                    oraParams.Add(new SqlParameter("BOT", SqlDbType.VarChar) { Value = tc.Name });
                    oraParams.Add(new SqlParameter("FT", SqlDbType.VarChar) { Value = tc.FT });
                    oraParams.Add(new SqlParameter("ID", SqlDbType.VarChar) { Value = tc.PId });
                    oraParams.Add(new SqlParameter("ISUSERDEFINE", SqlDbType.VarChar) { Value = tc.IsUserDefine });
                    oraParams.Add(new SqlParameter("SHAPE", SqlDbType.VarChar) { Value = tc.Shape });
                    oraParams.Add(new SqlParameter("USEGEOMETRY", SqlDbType.VarChar) { Value = tc.UseGeometry });
                    oraParams.Add(new SqlParameter("ENAME", SqlDbType.VarChar) { Value = tc.EName });
                    oraParams.Add(new SqlParameter("BOTID", SqlDbType.VarChar) { Value = tc.Id });

                    SqlCommand oracleCommDP = new SqlCommand();

                    oracleCommList.AddRange(BuildPSetComm(tc.OPL, tc.Id));
                }
                else
                {
                    oracleCommD.CommandText = "UPDATE OBJTYPECLASS SET NAME=@NAME WHERE ID=@ID";
                    oraParams.Add(new SqlParameter("NAME", SqlDbType.VarChar) { Value = tc.Name });
                    oraParams.Add(new SqlParameter("ID", SqlDbType.VarChar) { Value = tc.Id });
                }
                oracleCommD.Parameters.AddRange(oraParams.ToArray());
                oracleCommList.Add(oracleCommD);
            }
            CommComparer com = new CommComparer();
            oracleCommList.Sort(com);
            DBUtility.SqlServerDBHelper.ExecuteCommand(oracleCommList);

            return true;
        }
        private List<SqlCommand> BuildPSetComm(List<ObjTypePropertyModel> opl, string BotId)
        {
            //更新或新增
            List<SqlCommand> oracleCommList = new List<SqlCommand>();
            foreach (ObjTypePropertyModel otp in opl)
            {
                SqlCommand oracleCommP = new SqlCommand();
                var oraParamsP = new List<SqlParameter>();
                StringBuilder strq = new StringBuilder();
                strq.Append(" MERGE INTO OBJTYPEPROPERTY D ");
                strq.Append(" USING (SELECT @BOTID BOTID, @NS NS,@MD MD,@ISUSERDEFINE ISUSERDEFINE ) S ");
                strq.Append(" ON (D.BOTID = S.BOTID and D.NS=S.NS) ");
                strq.Append(" WHEN MATCHED THEN UPDATE SET D.MD = S.MD ");
                strq.Append(" WHEN NOT MATCHED THEN INSERT (BOTID,NS,MD,ISUSERDEFINE) VALUES (S.BOTID,S.NS,S.MD,S.ISUSERDEFINE) ;");
                oracleCommP.CommandText = strq.ToString();
                oraParamsP.Add(new SqlParameter("BOTID", SqlDbType.VarChar) { Value = otp.Botid });
                oraParamsP.Add(new SqlParameter("NS", SqlDbType.VarChar) { Value = otp.Ns });
                oraParamsP.Add(new SqlParameter("MD", SqlDbType.VarChar) { Value = otp.Md });
                oraParamsP.Add(new SqlParameter("ISUSERDEFINE", SqlDbType.VarChar) { Value = otp.IsUserDefine });

                oracleCommP.Parameters.AddRange(oraParamsP.ToArray());
                oracleCommList.Add(oracleCommP);
            }
            //删除
            List<ObjTypePropertyModel> oldopl = new ObjTypePropertyBusiness().GetListByID(BotId).ToList();
            List<ObjTypePropertyModel> d = oldopl.Except(opl, new OPLComparer()).ToList();
            foreach (ObjTypePropertyModel otp in d)
            {
                SqlCommand oracleCommD = new SqlCommand();
                oracleCommD.CommandText = "DELETE FROM OBJTYPEPROPERTY WHERE BOTID=@BOTID AND NS=@NS";
                var oraParams = new List<SqlParameter>();
                oraParams.Add(new SqlParameter("BOTID", SqlDbType.VarChar) { Value = otp.Botid });
                oraParams.Add(new SqlParameter("NS", SqlDbType.VarChar) { Value = otp.Ns });
                oracleCommD.Parameters.AddRange(oraParams.ToArray());
                oracleCommList.Add(oracleCommD);
            }

            return oracleCommList;
        }


    }
    class TCLComparer : IEqualityComparer<TypeClassTree>
    {
        public bool Equals(TypeClassTree x, TypeClassTree y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Id == y.Id;
        }
        public int GetHashCode(TypeClassTree tc)
        {
            if (Object.ReferenceEquals(tc, null)) return 0;
            int hashName = tc.Name == null ? 0 : tc.Name.GetHashCode();
            int hashCode = tc.Id.GetHashCode();
            return hashCode;
        }
    }
    class TCLComparer1 : IEqualityComparer<TypeClassTree>
    {
        public bool Equals(TypeClassTree x, TypeClassTree y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            if (x.Id == y.Id &&
                x.PId == y.PId &&
                x.Name == y.Name &&
                x.FT == y.FT &&
                x.IsUserDefine == y.IsUserDefine &&
                x.Shape == y.Shape &&
                x.UseGeometry == y.UseGeometry &&
                x.Type == y.Type)
            {
                if ((x.OPL != null && x.OPL.Count > 0) || (y.OPL != null && y.OPL.Count > 0))
                    return false;
                else return true;
            }
            else return false;
        }
        public int GetHashCode(TypeClassTree tc)
        {
            if (Object.ReferenceEquals(tc, null)) return 0;
            int hashName = tc.Name == null ? 0 : tc.Name.GetHashCode();
            int hashCode = tc.Id.GetHashCode();
            return hashCode;
        }
    }
    class OPLComparer : IEqualityComparer<ObjTypePropertyModel>
    {
        public bool Equals(ObjTypePropertyModel x, ObjTypePropertyModel y)
        {
            if (Object.ReferenceEquals(x, y)) return true;
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;
            return x.Botid == y.Botid && x.Ns == y.Ns;
        }
        public int GetHashCode(ObjTypePropertyModel tc)
        {
            if (Object.ReferenceEquals(tc, null)) return 0;
            int hashName = tc.Ns == null ? 0 : tc.Ns.GetHashCode();
            int hashCode = tc.Botid.GetHashCode();
            return hashName ^ hashCode;
        }
    }
    //调整command的执行顺序，避免子对象插入比父对象插入早执行
    class CommComparer : IComparer<SqlCommand>
    {
        public int Compare(SqlCommand y, SqlCommand x)
        {
            if (x.CommandText.Contains("INSERT INTO OBJTYPECLASS"))
            {
                if (y.CommandText.Contains("INSERT INTO OBJTYPECLASS")) return 0;
                else return 1;
            }
            else if (x.CommandText.Contains("INSERT INTO OBJECTTYPE"))
            {
                if (y.CommandText.Contains("INSERT INTO OBJTYPECLASS")) return -1;
                else if (y.CommandText.Contains("INSERT INTO OBJECTTYPE")) return 0;
                else return 1;
            }
            else if (x.CommandText.Contains("MERGE INTO OBJTYPEPROPERTY"))
            {
                if (y.CommandText.Contains("INSERT INTO OBJTYPECLASS") || y.CommandText.Contains("INSERT INTO OBJECTTYPE")) return -1;
                else if (y.CommandText.Contains("MERGE INTO OBJTYPEPROPERTY")) return 0;
                else return 1;
            }
            else
            {
                if (y.CommandText.Contains("INSERT INTO OBJTYPECLASS") || y.CommandText.Contains("INSERT INTO OBJECTTYPE") || y.CommandText.Contains("MERGE INTO OBJTYPEPROPERTY"))
                    return -1;
                else return 0;
            }
        }
    }
}
