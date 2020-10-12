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
    public class ChannelServices
    {
        public ChannelServices()
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
        /// 查询所有顶级栏目，States>-1
        /// </summary>
        /// <returns></returns>
        public List<Channel> GetTopChannel()
        {
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == 0 && i.States > -1).OrderBy(i => i.SortId, OrderByType.Asc).OrderBy(i => i.AddTime, OrderByType.Asc).ToList();
            return list;
        }

        /// <summary>
        /// 查询所有顶级栏目，States=1
        /// </summary>
        /// <returns></returns>
        public List<Channel> GetTopChannel_1()
        {
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == 0 && i.States == 1).OrderBy(i => i.SortId, OrderByType.Asc).OrderBy(i => i.AddTime, OrderByType.Asc).ToList();
            return list;
        }

        /// <summary>
        /// 查询所有子级栏目，States>-1
        /// </summary>
        /// <param name="ParentId">父级栏目编号</param>
        /// <returns></returns>
        public List<Channel> GetChildChannel(int ParentId)
        {
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States > -1).OrderBy(i => i.SortId, OrderByType.Asc).OrderBy(i => i.AddTime, OrderByType.Asc).ToList();
            return list;
        }

        /// <summary>
        /// 查询所有子级栏目，States=1
        /// </summary>
        /// <param name="ParentId">父级栏目编号</param>
        /// <returns></returns>
        public List<Channel> GetChildChannel_1(int ParentId)
        {
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States == 1).OrderBy(i => i.SortId, OrderByType.Asc).OrderBy(i => i.AddTime, OrderByType.Asc).ToList();
            return list;
        }

        /// <summary>
        /// 根据栏目Id查询栏目是否存在,存在则返回实体类否则为null
        /// </summary>
        /// <param name="Id">栏目编号</param>
        /// <returns>存在则返回实体类否则为null</returns>
        public Model.Channel SelectChannelById(int Id)
        {
            return Db.Queryable<Channel>().InSingle(Id);
        }

        /// <summary>
        /// 增加栏目
        /// </summary>
        /// <param name="model">实体类</param>
        /// <returns></returns>
        public bool AddChannel(dynamic model)
        {
            bool result = false;
            //修改时间默认当前时间
            model.EditTime = DateTime.Now;
            //插入成功返回自增列Id
            int Id = Convert.ToInt32(Db.Insertable(model).ExecuteReturnBigIdentity());
            if (Id > 0)
            {
                //获取自增列的实体类
                var Model = SelectChannelById(Id);
                //排序编号与自增列Id保持一致
                Model.SortId = Model.Id;
                //更新实体类
                if (UpdateChannel(Model))
                {
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 通用栏目更新
        /// </summary>
        /// <param name="model">栏目实体类</param>
        /// <returns>true/false</returns>
        public bool UpdateChannel(dynamic model)
        {
            return Db.Updateable(model).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 是否当前层级最靠前的栏目
        /// </summary>
        /// <param name="Id">栏目编号</param>
        /// <param name="ParentId">父级栏目编号</param>
        /// <returns>true/false</returns>
        public bool IsTopChannelSortId(int Id, int ParentId)
        {
            var Model = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States == 1).OrderBy(i => i.SortId, OrderByType.Asc).First();
            return Model != null && Model.Id == Id;
        }

        /// <summary>
        /// 上移栏目
        /// </summary>
        /// <param name="Id">栏目编号</param>
        /// <param name="ParentId">父级栏目编号</param>
        /// <param name="SortId">栏目排序编号</param>
        /// <returns>true/false</returns>
        public bool UpChannelSortId(int Id, int ParentId, int SortId)
        {
            bool result = false;
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States == 1 && i.SortId <= SortId).OrderBy(i => i.SortId, OrderByType.Desc).Take(2).ToList();
            if (list.Count > 1)
            {
                int TopSortId = list[1].SortId;
                list[0].SortId = TopSortId;
                list[1].SortId = SortId;
                result = UpdateChannel(list[0]) && UpdateChannel(list[1]);
            }
            return result;
        }

        /// <summary>
        /// 是否当前层级最靠后的栏目
        /// </summary>
        /// <param name="Id">栏目编号</param>
        /// <param name="ParentId">父级栏目编号</param>
        /// <returns>true/false</returns>
        public bool IsBottomChannelSortId(int Id, int ParentId)
        {
            var Model = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States == 1).OrderBy(i => i.SortId, OrderByType.Desc).First();
            return Model != null && Model.Id == Id;
        }

        /// <summary>
        /// 下移栏目
        /// </summary>
        /// <param name="Id">栏目编号</param>
        /// <param name="ParentId">父级栏目编号</param>
        /// <param name="SortId">栏目排序编号</param>
        /// <returns>true/false</returns>
        public bool DownChannelSortId(int Id, int ParentId, int SortId)
        {
            bool result = false;
            var list = Db.Queryable<Channel>().Where(i => i.ParentId == ParentId && i.States == 1 && i.SortId >= SortId).OrderBy(i => i.SortId, OrderByType.Asc).Take(2).ToList();
            if (list.Count > 1)
            {
                int BottomSortId = list[1].SortId;
                list[0].SortId = BottomSortId;
                list[1].SortId = SortId;
                result = UpdateChannel(list[0]) && UpdateChannel(list[1]);
            }
            return result;
        }
    }
}
