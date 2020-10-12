using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using SqlSugar;

namespace DAL
{
    public class UserInfoServices
    {
        public UserInfoServices()
        {
            Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString(),
                DbType = DbType.SqlServer,
                InitKeyType = InitKeyType.Attribute,//从特性读取主键和自增列信息
                IsAutoCloseConnection = true,//开启自动释放模式和EF原理一样我就不多解释了

            });
        }

        //用来处理事务多表查询和复杂的操作
        public SqlSugarClient Db;

        /// <summary>
        /// 查询所有用户信息
        /// </summary>
        /// <returns>返回list</returns>
        public List<UserInfo> SelectAll()
        {
            return Db.Queryable<UserInfo>().ToList();
        }
    }
}
