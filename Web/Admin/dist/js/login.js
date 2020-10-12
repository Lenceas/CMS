var onoff = true//根据此布尔值判断当前为注册状态还是登录状态
var confirm = document.getElementsByClassName("layui-form-item02")[0]

//自动居中title
var name_c = document.getElementById("title")
name = name_c.innerHTML.split("")
name_c.innerHTML = ""
for (i = 0; i < name.length; i++) {
    if (name[i] != ",") {
        name_c.innerHTML += name[i];
    }
};

layui.use('form', function () {

    var form = layui.form;

    form.on('submit(login)', function (data) {
        if (onoff) {
            var username = data.field.username;
            var userpwd = data.field.userpwd;
            var vcode = data.field.vcode;
            var Model = { op: "login", username: username, userpwd: userpwd, vcode: vcode };
            //console.log(Model);
            if (!Model.username) {
                alert("请输入用户名");
                return false;
            }
            if (Model.username.length < 5) {
                alert("请填写至少5位数用户名");
                return false;
            }
            if (!Model.userpwd) {
                alert("请输入密码");
                return false;
            }
            if (Model.userpwd.length < 6) {
                alert("请填写至少6位数密码");
                return false;
            }
            if (!Model.vcode) {
                alert("请输入验证码");
                return false;
            }
            if (Model.vcode.length != 4) {
                alert("请输入4位数验证码");
                return false;
            }
            //return false;
            $.post("/admin/ashx/administrator.ashx", Model, function (res) {
                res = JSON.parse(res);
                //console.log(res);
                if (res.id == 1) {
                    location.href = "/admin/index.aspx";
                }
                else {
                    alert(res.msg);
                    $("#vcodeimg").click();
                }
            });
        } else {
            let status = document.getElementById("status").getElementsByTagName("i")
            confirm.style.height = 0
            status[0].style.top = 0
            status[1].style.top = 35 + "px"
            onoff = !onoff
        }
        return false;
    });

    form.on('submit(register)', function (data) {
        if (onoff) {
            let status = document.getElementById("status").getElementsByTagName("i")
            confirm.style.height = 71 + "px"
            status[0].style.top = 35 + "px"
            status[1].style.top = 0
            onoff = !onoff
        } else {
            var username = data.field.username;
            var userpwd = data.field.userpwd;
            var userepwd = data.field.userepwd;
            var vcode = data.field.vcode;
            var Model = { op: "register", username: username, userpwd: userpwd, userepwd: userepwd, vcode: vcode };
            //console.log(Model);
            if (!Model.username) {
                alert("请输入用户名");
                return false;
            }
            if (Model.username.length < 5) {
                alert("请填写至少5位数用户名");
                return false;
            }
            if (!Model.userpwd) {
                alert("请输入密码");
                return false;
            }
            if (Model.userpwd.length < 6) {
                alert("请填写至少6位数密码");
                return false;
            }
            if (!Model.userepwd) {
                alert("请输入确认密码");
                return false;
            }
            if (Model.userepwd.length < 6) {
                alert("请填写至少6位数确认密码");
                return false;
            }
            if (Model.userpwd != Model.userepwd) {
                alert("两次输入的密码不一致");
                return false;
            }
            if (!Model.vcode) {
                alert("请输入验证码");
                return false;
            }
            if (Model.vcode.length != 4) {
                alert("请输入4位数验证码");
                return false;
            }
            $.post("/admin/ashx/administrator.ashx", Model, function (res) {
                res = JSON.parse(res);
                //console.log(res);
                alert(res.msg);
                if (res.id == 1) {
                    setTimeout(function () {
                        $(".login").click();
                    }, 1000);
                }
                else {
                    $("#vcodeimg").click();
                }
            });
        }
        return false;
    });
});

//点击验证码实时更换
function checktopimg(obj) {
    $(obj).attr("src", "/Admin/ashx/CheckCode.ashx?name=code&height=30&bgcolor=FFFFFFF&r=" + Math.random());
};