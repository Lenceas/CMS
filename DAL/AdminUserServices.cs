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
    public class AdminUserServices
    {
        public AdminUserServices()
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
        /// 登录验证
        /// </summary>
        /// <param name="adminName">用户名</param>
        /// <param name="adminPwd">密码</param>
        /// <returns></returns>
        public bool CheckIsLogin(string adminName, string adminPwd)
        {
            return Db.Queryable<AdminUser>().Where(ad => ad.AdminName == adminName && ad.AdminPwd == adminPwd).ToList().Count > 0;
        }

        /// <summary>
        /// 注册验证
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public bool CheckIsRegister(dynamic model)
        {
            return Convert.ToInt32(Db.Insertable(model).ExecuteReturnBigIdentity()) > 0;
        }

        /// <summary>
        /// 判断用户是否存在
        /// </summary>
        /// <param name="adminName">用户名</param>
        /// <returns>若存在返回true</returns>
        public bool CheckIsHad(string adminName)
        {
            return Db.Queryable<AdminUser>().Where(ad => ad.AdminName == adminName).ToList().Count > 0;
        }
    }
}
