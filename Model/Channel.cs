using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace Model
{
    [SugarTable("Channel")]
    public class Channel
    {
        /// <summary>
        /// 栏目编号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true, IsOnlyIgnoreInsert = true)]
        public int Id { get; set; }
        /// <summary>
        /// 父级栏目编号
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 栏目标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 栏目副标题
        /// </summary>
        public string SubTitle { get; set; }
        /// <summary>
        /// 栏目英文标题
        /// </summary>
        public string EnTitle { get; set; }
        /// <summary>
        /// 栏目添加时间
        /// </summary>
        public DateTime AddTime { get; set; }
        /// <summary>
        /// 栏目修改时间
        /// </summary>
        public DateTime EditTime { get; set; }
        /// <summary>
        /// 栏目排序编号
        /// </summary>
        public int SortId { get; set; }
        /// <summary>
        /// 栏目状态
        /// </summary>
        public int States { get; set; }
        /// <summary>
        /// 栏目路径
        /// </summary>
        public string WebPath { get; set; }
    }
}
