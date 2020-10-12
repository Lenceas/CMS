using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Model
{
    [SugarTable("AdminUser")]
    public class AdminUser
    {
        /// <summary>
        /// 编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, IsOnlyIgnoreInsert = true)]
        public int AdminId { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string AdminName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string AdminPwd { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreInsert=true)]
        public DateTime AddTime { get; set; }
    }
}
