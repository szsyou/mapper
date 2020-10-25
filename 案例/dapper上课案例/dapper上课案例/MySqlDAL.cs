using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using MySql.Data.MySqlClient;

namespace dapper上课案例
{
    public class MySqlDAL
    {
        readonly string conn = "server=localhost;database=myshare;uid=root;pwd=1qa@WS";

        //返回一个List集合
        public List<Account> GetList()
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "select * from account";
                return db.Query<Account>(sql).ToList();
            }
        }

        //新增
        public int Add(Account account)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "insert into account(name,rest) select @name,@rest";
                return db.Execute(sql, account);
            }
        }

        //新增(批量)
        public int Add(List<Account> accounts)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "insert into account(name,rest) select @name,@rest";
                return db.Execute(sql, accounts);
            }
        }

        //修改
        public int Update(Account account)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "update account set name=@Name,rest=@Rest where id=@Id";
                return db.Execute(sql, account);
            }
        }

        //删除
        public int Del(int id)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "delete from account where id=@Id";
                return db.Execute(sql, new { @Id=id});
            }
        }

        //获取人员数量
        public int GetEmpCount()
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "select count(*) from emp";
                //return db.ExecuteScalar<int>(sql);//方案一
                //return db.QueryFirst<int>(sql);//方案二
                //return db.QuerySingle<int>(sql);//方案三
                //return db.QueryFirstOrDefault<int>(sql);//方案四
                return db.QuerySingleOrDefault<int>(sql);//方案五
            }
        }

        //根据ID获取账户信息
        public Account GetAccountInfoByID(int id)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "select * from account where id=@id";               
                //return db.QueryFirst<Account>(sql,new { @id=id});//方案一
                //return db.QuerySingle<Account>(sql, new { @id = id });//方案二
                //return db.QueryFirstOrDefault<Account>(sql, new { @id = id });//方案三
                return db.QuerySingleOrDefault<Account>(sql, new { @id = id });//方案四
            }
        }

        //返回一个DataTable
        public DataTable GetAccountList()
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "select * from account";
                IDataReader idr = db.ExecuteReader(sql);
                DataTable dt = new DataTable();
                dt.Load(idr);
                return dt;
            }
        }

        //返回多个结果集
        public void GetMultiList(out List<Account> accounts,out List<Emp> emps)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "select * from account;";
                sql += "select * from emp";
                var multiResult = db.QueryMultiple(sql);
                accounts=(List<Account>)multiResult.Read<Account>();
                emps = (List<Emp>)multiResult.Read<Emp>();
            }
        }

        //新增(存储过程)
        public int AddByProc(Account account)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("_name", account.Name);
                dp.Add("_rest", account.Rest);
                return db.Execute("sp_account_add", dp, null, null, CommandType.StoredProcedure);
            }
        }

        //获取记录集(存储过程)
        public IList<Account> GetAccountListByProc(string content,out int rowCount)
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                DynamicParameters dp = new DynamicParameters();
                dp.Add("_content", content);
                dp.Add("_recordCount",null,DbType.Int32,ParameterDirection.Output);
                IList<Account> lst = db.Query<Account>("sp_accountInfo", dp, null, true, null, CommandType.StoredProcedure).ToList();
                rowCount = dp.Get<int>("_recordCount");
                return lst;
            }
        }

        //事务(AB转账)
        public bool ABCount()
        {
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql1 = "update account set rest=rest-1000 where id=1";//jack
                string sql2 = "update account set rest=rest+1000 where id=2";//tom
                db.Open();
                IDbTransaction tran = db.BeginTransaction();
                try
                {
                    db.Execute(sql1, null, tran);
                    db.Execute(sql2, null, tran);
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    return false;
                }
                return true;
            }
        }


        //异步
        public async void InsertRecordAsync()
        {
            await Task.Delay(10000);
            using (IDbConnection db = new MySqlConnection(conn))
            {
                string sql = "insert into account(name,rest) select '刘备',30000";
                await db.ExecuteAsync(sql).ConfigureAwait(false);
            }
        }

    }
}
