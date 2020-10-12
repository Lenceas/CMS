//基本渲染
layui.use('element', function () {
    var element = layui.element;
    element.render();
});

//日期组件渲染
layui.use('laydate', function () {
    var laydate = layui.laydate;
    laydate.render({
        elem: '#AddTime'
        , type: 'datetime'
        , calendar: true
    });
});

//表单渲染
layui.use('form', function () {
    var form = layui.form;
    form.render();

    //监听提交,添加顶级栏目
    form.on('submit(AddTopChannel)', function (data) {
        $.post("/admin/ashx/administrator.ashx?op=AddChannel", data.field, function (res) {
            res = JSON.parse(res);
            if (res.Status == 1) {
                alert(res.msg, { icon: 6 });
                //成功后等待1.5秒跳转回栏目管理页面
                setTimeout(function () {
                    location.href = "/admin/page_channel/list.aspx?t=Channel";
                }, 1500);
            }
            else {
                alert(res.msg, { icon: 5 });
            }
        });
        return false;
    });

    //监听提交,添加子栏目
    form.on('submit(AddChildChannel)', function (data) {
        $.post("/admin/ashx/administrator.ashx?op=AddChannel", data.field, function (res) {
            res = JSON.parse(res);
            if (res.Status == 1) {
                alert(res.msg, { icon: 6 });
                //成功后等待1.5秒跳转回栏目管理页面
                setTimeout(function () {
                    location.href = "/admin/page_channel/list.aspx?t=Channel";
                }, 1500);
            }
            else {
                alert(res.msg, { icon: 5 });
            }
        });
        return false;
    });

    //监听提交,编辑栏目
    form.on('submit(EditChannel)', function (data) {
        var id = $("#ChannelId").attr("data-id");
        $.post("/admin/ashx/administrator.ashx?op=EditChannel&Id=" + id, data.field, function (res) {
            res = JSON.parse(res);
            if (res.Status == 1) {
                alert(res.msg, { icon: 6 });
                //成功后等待1.5秒跳转回栏目管理页面
                setTimeout(function () {
                    location.href = "/admin/page_channel/list.aspx?t=Channel";
                }, 1500);
            }
            else {
                alert(res.msg, { icon: 5 });
            }
        });
        return false;
    });
});

//返回栏目列表管理页面
function ReturnChannelList() {
    location.href = "/admin/page_channel/list.aspx?t=Channel";
};

//动态时间
$(function () {
    setInterval(function () {
        var now = (new Date()).toLocaleString();
        $('#now-time').text(now);
    }, 1000);
});

//后台退出登录状态
function Exit() {
    location.href = "/admin/ashx/administrator.ashx?op=Exit";
};

//后台默认页面
function Index() {
    location.href = "/admin/index.aspx";
};

//栏目管理
function Channel() {
    location.href = "/admin/page_channel/list.aspx?t=Channel";
};

//添加顶级栏目
function AddTopChannel() {
    location.href = "/admin/page_channel/edit.aspx?t=Channel&action=addtopchannel";
};

//锁定栏目
function Locking(id, sid) {
    var Model = { Id: id, States: sid }
    $.post("/admin/ashx/administrator.ashx?op=LockingChannel", Model, function (res) {
        res = JSON.parse(res);
        if (res.Status == 1) {
            alert(res.msg, { icon: 6 });
            //成功后等待1秒刷新页面
            setTimeout(function () {
                location.href = "/admin/page_channel/list.aspx?t=Channel";
            }, 1000);
        }
        else {
            alert(res.msg, { icon: 5 });
        }
    });
    return false;
};

//栏目排序上移
function UpChannel(id, parentid, sortid) {
    var Model = { Id: id, ParentId: parentid, SortId: sortid }
    $.post("/admin/ashx/administrator.ashx?op=UpChannelSortId", Model, function (res) {
        res = JSON.parse(res);
        if (res.Status == 1) {
            //成功后刷新页面
            location.href = "/admin/page_channel/list.aspx?t=Channel";
        }
        else {
            alert(res.msg, { icon: 5 });
        }
    });
    return false;
};

//栏目排序下移
function DownChannel(id, parentid, sortid) {
    var Model = { Id: id, ParentId: parentid, SortId: sortid }
    $.post("/admin/ashx/administrator.ashx?op=DownChannelSortId", Model, function (res) {
        res = JSON.parse(res);
        if (res.Status == 1) {
            //成功后刷新页面
            location.href = "/admin/page_channel/list.aspx?t=Channel";
        }
        else {
            alert(res.msg, { icon: 5 });
        }
    });
    return false;
};

//编辑栏目
function EditChannel(id, parentId) {
    location.href = "/admin/page_channel/edit.aspx?t=Channel&action=editchannel&id=" + id + "&parentId=" + parentId;
};

//添加子栏目
function AddChildChannel(id) {
    location.href = "/admin/page_channel/edit.aspx?t=Channel&action=addchildchannel&id=" + id;
};

//删除栏目
function DeleteChannel(id) {
    layer.msg('是否删除栏目?', {
        icon: 0,
        time: 5000,
        btn: ['确认', '取消'],
        btn1: function () {
            //确认删除
            var index = layer.load(0, { shade: false });
            var Model = { Id: id }
            $.post("/admin/ashx/administrator.ashx?op=DeleteChannel", Model, function (res) {
                res = JSON.parse(res);
                if (res.Status == 1) {
                    layer.close(index);
                    alert(res.msg, { icon: 6 });
                    //成功后等待1秒刷新页面
                    setTimeout(function () {
                        location.href = "/admin/page_channel/list.aspx?t=Channel";
                    }, 1000);
                }
                else {
                    alert(res.msg, { icon: 5 });
                }
            });
            return false;
        },
        cancel: function () {
            //取消删除
        }
    });
};

//复制栏目
function CopyChannel(id) {
    var Model = { Id: id}
    $.post("/admin/ashx/administrator.ashx?op=CopyChannel", Model, function (res) {
        res = JSON.parse(res);
        if (res.Status == 1) {
            //成功后刷新页面
            location.href = "/admin/page_channel/list.aspx?t=Channel";
        }
        else {
            alert(res.msg, { icon: 5 });
        }
    });
    return false;
};