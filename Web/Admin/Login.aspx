<%@ Page Language="C#" AutoEventWireup="true" Inherits="Web.UI.Base" ValidateRequest="false" %>

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
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
    <meta name="renderer" content="webkit">
    <meta name="format-detection" content="telephone=no">
    <meta name="apple-mobile-web-app-capable" content="yes">
    <meta name="apple-mobile-web-app-status-bar-style" content="black">
    <meta name="applicable-device" content="pc,mobile">
    <title>管理员登录 - Lenceas.Admin</title>
    <meta name="Keywords" content="" />
    <meta name="Description" content="" />
    <link rel="icon" href="/favicon.ico" type="image/x-icon" />
    <link rel="styleSheet" href="/Admin/dist/css/main.css" />
    <link rel="stylesheet" href="/Content/layui/css/layui.css">
    <script src="/Admin/dist/js/jquery.min.js"></script>
    <script src="/Content/layui/layui.all.js"></script>
    <script>
        window.alert = function (mes, option) {
            if (option == undefined) {
                layer.msg(mes);
            }
            else {
                layer.msg(mes, 1, option);
            }
        }
    </script>
</head>
<body ondragstart="window.event.returnValue=false" onselectstart="event.returnValue=false">
    <div id="bg">
        <div id="login_wrap">
            <div id="login">
                <!-- 登录注册切换动画 -->
                <div id="status">
                    <i style="top: 0">登录</i>
                    <i style="top: 35px">注册</i>
                    <%--<i style="right: 5px">in</i>--%>
                </div>
                <span>
                    <form class="layui-form" method="post">
                        <div class="layui-form-item">
                            <div class="form">
                                <input type="text" name="username" placeholder="用户名" autofocus="autofocus" autocomplete="off" />
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="form">
                                <input type="password" name="userpwd" placeholder="密码" autocomplete="off" />
                            </div>
                        </div>
                        <div class="layui-form-item layui-form-item02">
                            <div class="form">
                                <input type="password" name="userepwd" placeholder="确认密码" autocomplete="off" id="userepwd" />
                            </div>
                        </div>
                        <div class="layui-form-item">
                            <div class="form" style="position: relative;">
                                <input type="text" name="vcode" placeholder="验证码" autocomplete="off" />
                                <img id="vcodeimg" src="/Admin/ashx/CheckCode.ashx?name=code&height=35&bgcolor=FFFFFFF" onclick="checktopimg(this)" alt="验证码" style="position: absolute; right: 2px; bottom: 22px;">
                            </div>
                        </div>                        
                        <input type="submit" lay-submit lay-filter="login" value="登录" class="btn login" style="margin-right: 20px;" />
                        <input type="submit" lay-submit lay-filter="register" value="注册" class="btn register" id="btn" />                        
                    </form>
                </span>
            </div>
            <div id="login_img">
                <!-- 图片绘制框 -->
                <span class="circle">
                    <span></span>
                    <span></span>
                </span>
                <span class="star">
                    <span></span>
                    <span></span>
                    <span></span>
                    <span></span>
                    <span></span>
                    <span></span>
                    <span></span>
                    <span></span>
                </span>
                <span class="fly_star">
                    <span></span>
                    <span></span>
                </span>
                <p id="title">CopyRight © 2019 卢杰晟</p>
            </div>
        </div>
    </div>
</body>
</html>
<script src="/Admin/dist/js/login.js"></script>
