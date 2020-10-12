<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.UI.Index" ValidateRequest="false" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>
<%
    var action = Common.CommFun.GetParams("action") ?? "";
    var id = Common.CommFun.GetParams("id") ?? "";
    var parentId = Common.CommFun.GetParams("parentId") ?? "";
    var ChannelName = string.Empty;
    var t = Common.CommFun.GetParams("t") ?? "";

    if (!string.IsNullOrEmpty(id))
    {
        var list = new DAL.DataBase().List_1(t, Convert.ToInt32(id), 1);
        if (list.Count > 0)
        {
            foreach (var item in list)
            {
                ChannelName = item.Title;
            }
        }
    }
%>

<!DOCTYPE html>

<html>
<head>

    <!-- #include file="/Admin/include/ref.html" -->

</head>
<body class="layui-layout-body">

    <div class="layui-layout layui-layout-admin">

        <!-- #include file="/Admin/include/header.html" -->

        <!-- #include file="/Admin/include/leftmenu.html" -->

        <div class="layui-body layui-bg-#fff">
            <div style="padding: 15px;">
                <div>
                    <span style="font-size: 24px; color: #666;">栏目管理</span>
                </div>
                <br />
                <form class="layui-form" style="padding: 10px; width: 66%;">

                    <!-- 增加顶级栏目 -->
                    <% if (!string.IsNullOrEmpty(action) && action == "addtopchannel")
                        {%>
                    <div class="layui-form-item">
                        <label class="layui-form-label">父级栏目</label>
                        <div class="layui-input-block">
                            <select name="ParentId" lay-verify="required">
                                <option value="0" selected>顶级栏目</option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="Title" lay-verify="required" placeholder="请输入标题" autofocus="autofocus" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">副标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="SubTitle" lay-verify="" placeholder="请输入副标题" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">英文标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="EnTitle" lay-verify="" placeholder="请输入英文标题" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">添加时间</label>
                        <div class="layui-input-block">
                            <input type="text" name="AddTime" lay-verify="required" placeholder="请选择添加时间" autocomplete="off" class="layui-input" id="AddTime" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目状态</label>
                        <div class="layui-input-block">
                            <input type="checkbox" name="States" lay-verify="" lay-skin="switch" lay-text="发布|锁定" checked value="1" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目路径</label>
                        <div class="layui-input-block">
                            <input type="text" name="WebPath" lay-verify="" placeholder="请输入栏目路径" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <div class="layui-input-block">
                            <button class="layui-btn" lay-submit lay-filter="AddTopChannel">提交</button>
                            <a class="layui-btn layui-btn-primary" onclick="ReturnChannelList();">返回</a>
                        </div>
                    </div>
                    <%}%>
                    <!-- 增加顶级栏目 -->

                    <!-- 增加子栏目 -->
                    <% if (!string.IsNullOrEmpty(action) && action == "addchildchannel")
                        {%>
                    <div class="layui-form-item">
                        <label class="layui-form-label">父级栏目</label>
                        <div class="layui-input-block">
                            <select name="ParentId" lay-verify="required">
                                <option value="<%=id %>" selected><%=ChannelName %></option>
                            </select>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="Title" lay-verify="required" placeholder="请输入标题" autofocus="autofocus" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">副标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="SubTitle" lay-verify="" placeholder="请输入副标题" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">英文标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="EnTitle" lay-verify="" placeholder="请输入英文标题" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">添加时间</label>
                        <div class="layui-input-block">
                            <input type="text" name="AddTime" lay-verify="required" placeholder="请选择添加时间" autocomplete="off" class="layui-input" id="AddTime" value="<%=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目状态</label>
                        <div class="layui-input-block">
                            <input type="checkbox" name="States" lay-verify="" lay-skin="switch" lay-text="发布|锁定" checked value="1" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目路径</label>
                        <div class="layui-input-block">
                            <input type="text" name="WebPath" lay-verify="" placeholder="请输入栏目路径" autocomplete="off" class="layui-input" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <div class="layui-input-block">
                            <button class="layui-btn" lay-submit lay-filter="AddChildChannel">提交</button>
                            <a class="layui-btn layui-btn-primary" onclick="ReturnChannelList();">返回</a>
                        </div>
                    </div>
                    <%} %>
                    <!-- 增加子栏目 -->

                    <!-- 编辑栏目 -->
                    <% if (!string.IsNullOrEmpty(action) && action == "editchannel")
                        {
                            var editmodel = new DAL.ChannelServices().SelectChannelById(Convert.ToInt32(id));%>
                    <div class="layui-form-item" id="ChannelId" data-id="<%=Convert.ToInt32(id) %>" style="display: none;"></div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">父级栏目</label>
                        <div class="layui-input-block">
                            <select name="ParentId" lay-verify="required">
                                <% var ParentChannel = new DAL.ChannelServices().GetTopChannel();
                                    if (ParentChannel.Count > 0)
                                    {%>
                                <option value="0" <%=Convert.ToInt32(parentId)==0?"selected":"" %>>├ 顶级栏目</option>
                                <% foreach (var item in ParentChannel)
                                    {%>
                                <option value="<%=item.Id %>" <%=item.Id==Convert.ToInt32(parentId)?"selected":"" %>>&nbsp;&nbsp;├ <%=item.Title %></option>
                                <% var ChildChannel2 = new DAL.ChannelServices().GetChildChannel(item.Id);
                                    if (ChildChannel2.Count > 0)
                                    {
                                        foreach (var item2 in ChildChannel2)
                                        {%>
                                <option value="<%=item2.Id %>" <%=item2.Id==Convert.ToInt32(parentId)?"selected":"" %>>&nbsp;&nbsp;&nbsp;&nbsp;├ <%=item2.Title %></option>
                                <% var ChildChannel3 = new DAL.ChannelServices().GetChildChannel(item2.Id);
                                    if (ChildChannel3.Count > 0)
                                    {
                                        foreach (var item3 in ChildChannel3)
                                        {%>
                                <option value="<%=item3.Id %>" <%=item3.Id==Convert.ToInt32(parentId)?"selected":"" %>>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;├ <%=item3.Title %></option>
                                <%}
                                                    }
                                                }
                                            }
                                        }
                                    }%>
                            </select>
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="Title" lay-verify="required" placeholder="请输入标题" autofocus="autofocus" autocomplete="off" class="layui-input" value="<%=editmodel.Title %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">副标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="SubTitle" lay-verify="" placeholder="请输入副标题" autocomplete="off" class="layui-input" value="<%=editmodel.SubTitle %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">英文标题</label>
                        <div class="layui-input-block">
                            <input type="text" name="EnTitle" lay-verify="" placeholder="请输入英文标题" autocomplete="off" class="layui-input" value="<%=editmodel.EnTitle %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">添加时间</label>
                        <div class="layui-input-block">
                            <input type="text" name="AddTime" lay-verify="required" placeholder="请选择添加时间" autocomplete="off" class="layui-input" id="AddTime" value="<%=editmodel.AddTime.ToString("yyyy-MM-dd HH:mm:ss") %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目状态</label>
                        <div class="layui-input-block">
                            <input type="checkbox" name="States" lay-verify="" lay-skin="switch" lay-text="发布|锁定" <%=editmodel.States==1?"checked":"" %> value="<%=editmodel.States %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <label class="layui-form-label">栏目路径</label>
                        <div class="layui-input-block">
                            <input type="text" name="WebPath" lay-verify="" placeholder="请输入栏目路径" autocomplete="off" class="layui-input" value="<%=editmodel.WebPath %>" />
                        </div>
                    </div>
                    <div class="layui-form-item">
                        <div class="layui-input-block">
                            <button class="layui-btn" lay-submit lay-filter="EditChannel">更新</button>
                            <a class="layui-btn layui-btn-primary" onclick="ReturnChannelList();">返回</a>
                        </div>
                    </div>
                    <%} %>
                    <!-- 编辑栏目 -->
                </form>
            </div>
        </div>

        <!-- #include file="/Admin/include/footer.html" -->

    </div>

</body>
</html>
<script src="/Admin/dist/js/index.js"></script>
