<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.UI.Index" ValidateRequest="false" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

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
                    <span style="font-size: 24px; color: #666; display: inline-block; vertical-align: middle;">栏目管理</span>&nbsp;&nbsp;
                    <button type="button" class="layui-btn layui-btn-normal" style="display: inline-block; vertical-align: middle;" onclick="AddTopChannel();">添加顶级栏目</button>
                </div>
                <br />
                <table class="layui-table" lay-even style="border: 1px solid gray;">
                    <colgroup>
                        <col width="75">
                        <col>
                        <col width="180">
                        <col width="80">
                        <col width="120">
                        <col width="250">
                    </colgroup>
                    <thead>
                        <tr>
                            <th class="manager_tab_center">编号</th>
                            <th>栏目名称</th>
                            <th class="manager_tab_center">发布时间</th>
                            <th class="manager_tab_center">属性</th>
                            <th class="manager_tab_center">排序</th>
                            <th class="manager_tab_center">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <% var cList = new DAL.ChannelServices().GetTopChannel();
                            if (cList.Count > 0)
                            {
                                foreach (var item in cList)
                                {%>
                        <tr>
                            <td class="manager_tab_center"><%=item.Id %></td>
                            <td>├ <%=item.Title %></td>
                            <td class="manager_tab_center"><%=item.AddTime.ToString("yyyy-MM-dd HH:mm:ss") %></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-<%=item.States==1?"primary":"danger" %> layui-btn-xs" onclick="Locking(<%=item.Id %>,<%=item.States %>);"><i class="layui-icon layui-icon-password"></i>锁定</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="UpChannel(<%=item.Id %>,<%=item.ParentId %>,<%=item.SortId %>);">上移</button>
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="DownChannel(<%=item.Id %>,<%=item.ParentId %>,<%=item.SortId %>);">下移</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-xs" onclick="EditChannel(<%=item.Id %>,<%=item.ParentId %>);">编辑</button>
                                <button type="button" class="layui-btn layui-btn-normal layui-btn-xs" onclick="AddChildChannel(<%=item.Id %>);">添加子栏目</button>
                                <button type="button" class="layui-btn layui-btn-danger layui-btn-xs" onclick="DeleteChannel(<%=item.Id %>);">删除</button>
                                <button type="button" class="layui-btn layui-btn-warm layui-btn-xs" onclick="CopyChannel(<%=item.Id %>);">复制</button></td>
                        </tr>
                        <% var cList2 = new DAL.ChannelServices().GetChildChannel(item.Id);
                            if (cList2.Count > 0)
                            {
                                foreach (var item2 in cList2)
                                {%>
                        <tr>
                            <td class="manager_tab_center"><%=item2.Id %></td>
                            <td style="padding-left: 30px;">├ <%=item2.Title %></td>
                            <td class="manager_tab_center"><%=item2.AddTime.ToString("yyyy-MM-dd HH:mm:ss") %></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-<%=item2.States==1?"primary":"danger" %> layui-btn-xs" onclick="Locking(<%=item2.Id %>,<%=item2.States %>);"><i class="layui-icon layui-icon-password"></i>锁定</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="UpChannel(<%=item2.Id %>,<%=item2.ParentId %>,<%=item2.SortId %>);">上移</button>
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="DownChannel(<%=item2.Id %>,<%=item2.ParentId %>,<%=item2.SortId %>);">下移</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-xs" onclick="EditChannel(<%=item2.Id %>,<%=item2.ParentId %>);">编辑</button>
                                <button type="button" class="layui-btn layui-btn-normal layui-btn-xs" onclick="AddChildChannel(<%=item2.Id %>);">添加子栏目</button>
                                <button type="button" class="layui-btn layui-btn-danger layui-btn-xs" onclick="DeleteChannel(<%=item2.Id %>);">删除</button>
                                <button type="button" class="layui-btn layui-btn-warm layui-btn-xs" onclick="CopyChannel(<%=item2.Id %>);">复制</button></td>
                        </tr>
                        <% var cList3 = new DAL.ChannelServices().GetChildChannel(item2.Id);
                            if (cList3.Count > 0)
                            {
                                foreach (var item3 in cList3)
                                {%>
                        <tr>
                            <td class="manager_tab_center"><%=item3.Id %></td>
                            <td style="padding-left: 45px;">├ <%=item3.Title %></td>
                            <td class="manager_tab_center"><%=item3.AddTime.ToString("yyyy-MM-dd HH:mm:ss") %></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-<%=item3.States==1?"primary":"danger" %> layui-btn-xs" onclick="Locking(<%=item3.Id %>,<%=item3.States %>);"><i class="layui-icon layui-icon-password"></i>锁定</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="UpChannel(<%=item3.Id %>,<%=item3.ParentId %>,<%=item3.SortId %>);">上移</button>
                                <button type="button" class="layui-btn layui-btn-primary layui-btn-xs" onclick="DownChannel(<%=item3.Id %>,<%=item3.ParentId %>,<%=item3.SortId %>);">下移</button></td>
                            <td class="manager_tab_center">
                                <button type="button" class="layui-btn layui-btn-xs" onclick="EditChannel(<%=item3.Id %>,<%=item3.ParentId %>);">编辑</button>
                                <button type="button" class="layui-btn layui-btn-normal layui-btn-xs" onclick="AddChildChannel(<%=item3.Id %>);">添加子栏目</button>
                                <button type="button" class="layui-btn layui-btn-danger layui-btn-xs" onclick="DeleteChannel(<%=item3.Id %>);">删除</button>
                                <button type="button" class="layui-btn layui-btn-warm layui-btn-xs" onclick="CopyChannel(<%=item3.Id %>);">复制</button></td>
                        </tr>
                        <%}
                                            }
                                        }
                                    }
                                }
                            }%>
                    </tbody>
                </table>
            </div>
        </div>

        <!-- #include file="/Admin/include/footer.html" -->

    </div>

</body>
</html>
<script src="/Admin/dist/js/index.js"></script>
