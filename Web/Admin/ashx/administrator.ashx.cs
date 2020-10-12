using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;
using Model;

namespace Web.Admin.ashx
{
    /// <summary>
    /// administrator 的摘要说明
    /// </summary>
    public class administrator : IHttpHandler, IRequiresSessionState
    {
        private string json = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string op = context.Request["op"] ?? "";

            switch (op)
            {
                case "login"://登录
                    json = Login(context);
                    break;
                case "register"://注册
                    json = Register(context);
                    break;
                case "Exit"://注销
                    Exit(context);
                    break;
                case "AddChannel"://增加栏目
                    json = AddChannel(context);
                    break;
                case "EditChannel"://编辑栏目
                    json = EditChannel(context);
                    break;
                case "LockingChannel"://锁定栏目
                    json = LockingChannel(context);
                    break;
                case "UpChannelSortId"://栏目排序上移
                    json = UpChannelSortId(context);
                    break;
                case "DownChannelSortId"://栏目排序下移
                    json = DownChannelSortId(context);
                    break;
                case "DeleteChannel"://删除栏目
                    json = DeleteChannel(context);
                    break;
                case "CopyChannel"://复制栏目
                    json = CopyChannel(context);
                    break;
                default:
                    break;
            }

            context.Response.Write(json);
        }

        #region 登录
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Login(HttpContext context)
        {
            string result = string.Empty;

            string username = context.Request["username"].ToString().Trim();
            string userpwd = context.Request["userpwd"].ToString().Trim();
            string vcode = context.Request["vcode"].ToString().Trim();

            try
            {
                //检查code
                string vcode_server = Common.CommFun.ReadSession("code") as string;

                if (!string.IsNullOrEmpty(vcode_server) && !string.IsNullOrEmpty(vcode) && vcode_server.ToUpper() == vcode.ToUpper())
                {

                    if (new DAL.AdminUserServices().CheckIsLogin(username, userpwd))
                    {
                        result = new JavaScriptSerializer().Serialize(new { id = 1, msg = "登录成功" });
                        Common.CommFun.AddSession("Lenceas"
                            , "lujiesheng0122-" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff")
                            , 480);
                    }
                    else
                    {
                        result = new JavaScriptSerializer().Serialize(new { id = 0, msg = "用户名或密码错误" });
                    }

                }
                else
                {
                    result = new JavaScriptSerializer().Serialize(new { id = -2, msg = "验证码错误" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { id = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 注册
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string Register(HttpContext context)
        {
            string result = string.Empty;

            string username = context.Request["username"].ToString().Trim();
            string userpwd = context.Request["userpwd"].ToString().Trim();
            string vcode = context.Request["vcode"].ToString().Trim();

            try
            {
                //检查code
                string vcode_server = Common.CommFun.ReadSession("code") as string;

                if (!string.IsNullOrEmpty(vcode_server) && !string.IsNullOrEmpty(vcode) && vcode_server.ToUpper() == vcode.ToUpper())
                {
                    //判断用户是否存在
                    bool ishad = new DAL.AdminUserServices().CheckIsHad(username);
                    if (ishad)
                    {
                        return new JavaScriptSerializer().Serialize(new { id = 2, msg = "用户名已存在" });
                    }

                    var Model = new AdminUser();
                    Model.AdminName = username;
                    Model.AdminPwd = userpwd;

                    if (new DAL.AdminUserServices().CheckIsRegister(Model))
                    {
                        result = new JavaScriptSerializer().Serialize(new { id = 1, msg = "注册成功" });
                    }
                    else
                    {
                        result = new JavaScriptSerializer().Serialize(new { id = 0, msg = "注册失败" });
                    }
                }
                else
                {
                    result = new JavaScriptSerializer().Serialize(new { id = -2, msg = "验证码错误" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { id = -1, msg = "操作异常,请稍后重试" });
                return result;
                throw;
            }
            return result;
        }
        #endregion

        #region 注销
        /// <summary>
        /// 退出后台登录状态
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public void Exit(HttpContext context)
        {
            try
            {
                Common.CommFun.DelSession("Lenceas");
                context.Response.Redirect("/admin/login.aspx");
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion        

        #region 增加栏目
        protected string AddChannel(HttpContext context)
        {
            string result = string.Empty;

            int ParentId = Convert.ToInt32(Common.CommFun.GetParams("ParentId") ?? "0");
            string Title = Common.CommFun.GetParams("Title");
            string SubTitle = Common.CommFun.GetParams("SubTitle") ?? "";
            string EnTitle = Common.CommFun.GetParams("EnTitle") ?? "";
            string AddTime = Common.CommFun.GetParams("AddTime") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int SortId = Convert.ToInt32(Common.CommFun.GetParams("SortId") ?? "100");
            int States = Convert.ToInt32(Common.CommFun.GetParams("States") ?? "0");
            string WebPath = Common.CommFun.GetParams("WebPath") ?? "";

            Channel Model = new Channel();
            Model.ParentId = ParentId;
            Model.Title = Title;
            Model.SubTitle = SubTitle;
            Model.EnTitle = EnTitle;
            Model.AddTime = DateTime.Parse(AddTime);
            Model.SortId = SortId;
            Model.States = States;
            Model.WebPath = WebPath;

            try
            {
                if (new DAL.ChannelServices().AddChannel(Model))
                {
                    result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "提交成功" });
                }
                else
                {
                    result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "提交失败" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 编辑栏目
        protected string EditChannel(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");
            int ParentId = Convert.ToInt32(Common.CommFun.GetParams("ParentId") ?? "0");
            string Title = Common.CommFun.GetParams("Title");
            string SubTitle = Common.CommFun.GetParams("SubTitle") ?? "";
            string EnTitle = Common.CommFun.GetParams("EnTitle") ?? "";
            string AddTime = Common.CommFun.GetParams("AddTime") ?? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int SortId = Convert.ToInt32(Common.CommFun.GetParams("SortId") ?? "100");
            int States = Convert.ToInt32(Common.CommFun.GetParams("States") ?? "0");
            string WebPath = Common.CommFun.GetParams("WebPath") ?? "";

            //获取要编辑的栏目实体
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    Model.ParentId = ParentId;
                    Model.Title = Title;
                    Model.SubTitle = SubTitle;
                    Model.EnTitle = EnTitle;
                    Model.AddTime = DateTime.Parse(AddTime);
                    Model.SortId = SortId;
                    Model.States = States;
                    Model.WebPath = WebPath;

                    if (new DAL.ChannelServices().UpdateChannel(Model))
                    {
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "更新成功" });
                    }
                    else
                    {
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "更新失败" });
                    }
                }
                else
                {
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 锁定栏目
        protected string LockingChannel(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");
            int States = Convert.ToInt32(Common.CommFun.GetParams("States") ?? "0");

            //根据栏目Id查询Model实体类
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    //栏目存在

                    //替换栏目状态
                    Model.States = States == 0 ? 1 : 0;
                    if (new DAL.ChannelServices().UpdateChannel(Model))
                    {
                        //锁定或解锁成功
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = Model.States == 0 ? "锁定成功" : "解锁成功" });
                    }
                    else
                    {
                        //锁定或解锁失败
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = Model.States == 0 ? "锁定失败" : "解锁失败" });
                    }
                }
                else
                {
                    //栏目不存在
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 栏目排序上移
        protected string UpChannelSortId(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");
            int ParentId = Convert.ToInt32(Common.CommFun.GetParams("ParentId") ?? "0");
            int SortId = Convert.ToInt32(Common.CommFun.GetParams("SortId") ?? "0");

            //根据栏目Id查询Model实体类
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    //栏目存在

                    if (new DAL.ChannelServices().IsTopChannelSortId(Id, ParentId))
                    {
                        //已经是当前层级最靠前的栏目了
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "已经是当前层级最靠前的栏目" });
                    }
                    else if (new DAL.ChannelServices().UpChannelSortId(Id, ParentId, SortId))
                    {
                        //上移成功
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "上移成功" });
                    }
                    else
                    {
                        //上移失败
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "上移失败" });
                    }
                }
                else
                {
                    //栏目不存在
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 栏目排序下移
        protected string DownChannelSortId(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");
            int ParentId = Convert.ToInt32(Common.CommFun.GetParams("ParentId") ?? "0");
            int SortId = Convert.ToInt32(Common.CommFun.GetParams("SortId") ?? "0");

            //根据栏目Id查询Model实体类
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    //栏目存在

                    if (new DAL.ChannelServices().IsBottomChannelSortId(Id, ParentId))
                    {
                        //已经是当前层级最靠后的栏目了
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "已经是当前层级最靠后的栏目" });
                    }
                    else if (new DAL.ChannelServices().DownChannelSortId(Id, ParentId, SortId))
                    {
                        //下移成功
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "下移成功" });
                    }
                    else
                    {
                        //下移失败
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "下移失败" });
                    }
                }
                else
                {
                    //栏目不存在
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 删除栏目
        public string DeleteChannel(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");

            //根据栏目Id查询Model实体类
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    //栏目存在则删除该栏目,注意这里是逻辑删除！！！即改变栏目状态States
                    Model.States = -1;
                    if (new DAL.ChannelServices().UpdateChannel(Model))
                    {
                        //删除成功
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "删除成功" });
                    }
                    else
                    {
                        //删除失败
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "删除失败" });
                    }
                }
                else
                {
                    //栏目不存在
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        #region 复制栏目
        public string CopyChannel(HttpContext context)
        {
            string result = string.Empty;

            int Id = Convert.ToInt32(Common.CommFun.GetParams("Id") ?? "0");

            //根据栏目Id查询Model实体类
            var Model = new DAL.ChannelServices().SelectChannelById(Id);

            try
            {
                if (Model != null)
                {
                    Channel NewModel = new Channel();
                    NewModel.ParentId = Model.ParentId;
                    NewModel.Title = Model.Title;
                    NewModel.SubTitle = Model.SubTitle;
                    NewModel.EnTitle = Model.EnTitle;
                    NewModel.AddTime = DateTime.Now;
                    NewModel.SortId = Model.SortId;
                    NewModel.States = Model.States;
                    NewModel.WebPath = Model.WebPath;

                    if (new DAL.ChannelServices().AddChannel(NewModel))
                    {
                        result = new JavaScriptSerializer().Serialize(new { Status = 1, msg = "复制成功" });
                    }
                    else
                    {
                        result = new JavaScriptSerializer().Serialize(new { Status = 0, msg = "复制失败" });
                    }
                }
                else
                {
                    //栏目不存在
                    result = new JavaScriptSerializer().Serialize(new { Status = -2, msg = "该栏目不存在" });
                }
            }
            catch (Exception)
            {
                result = new JavaScriptSerializer().Serialize(new { Status = -1, msg = "操作异常,请稍后重试" });
                throw;
            }

            return result;
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}