<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.UI.Base" ValidateRequest="false" %>

<%@ Import Namespace="System" %>
<%@ Import Namespace="System.Collections" %>
<%@ Import Namespace="System.Collections.Generic" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="System.Web" %>
<%@ Import Namespace="System.Web.UI" %>
<%@ Import Namespace="System.Web.UI.WebControls" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <ul>
                <% var userlist = new DAL.UserInfoServices().SelectAll();
                    if (userlist.Count > 0)
                    {
                        foreach (var item in userlist)
                        {%>
                <li>编号:<%=item.UserId %>;用户名:<%=item.UserName %>;密码:<%=item.UserPwd %>;添加时间:<%=item.AddTime.ToString("yyyy.MM.dd") %></li>
                <%}
                    }%>
            </ul>
        </div>
    </form>
</body>
</html>
