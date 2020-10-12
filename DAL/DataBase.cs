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
    /// <summary>
    /// 数据库通用操作方法
    /// </summary>
    public class DataBase
    {
        public DataBase()
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
        /// 查询(表名、栏目Id)
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ChannelId">栏目Id</param>
        /// <returns>列表</returns>
        public dynamic List(string TableName, int ChannelId)
        {
            return Db.Queryable(TableName, "t").Where("t.Id=@Id").AddParameters(new { Id = ChannelId }).Select("t.*").ToList();
        }

        /// <summary>
        /// 查询(表名、栏目Id、几条数据)
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="ChannelId">栏目Id</param>
        /// <param name="Top">几条数据</param>
        /// <returns>列表</returns>
        public dynamic List_1(string TableName, int ChannelId, int Top)
        {
            return Db.Queryable(TableName, "t").Where("t.Id=@Id").AddParameters(new { Id = ChannelId }).Select("t.*").Take(Top).ToList();
        }
    }
}
