function ShowDialog(title, width, height, url, btnSaveFun) {
    var div = $("#PopWindow");
    if (div.length <= 0) {
        div = "<div id=\"PopWindow\" style=\"margin: 0; padding-top: 5px; border: 0\"></div>";
        $("body").append(div);
    }
    $("#PopWindow").dialog({
        title: title,
        width: width,
        height: height,
        closed: true,
        cache: true,
        href: url,
        buttons: [{
            iconCls: 'icon-ok',
            text: '保存',
            handler: btnSaveFun
        }, {
            iconCls: 'icon-cancel',
            text: '取消',
            handler: function () { $("#PopWindow").dialog("close"); }
        }]
    }).dialog({
        onClose: function () {
            $("#PopWindow").remove();
        }
    }).dialog("open");
}
function AlertMsg(msg) {
    $.messager.show({
        title: '提示',
        msg: msg,
        showType: 'slide',
        timeout: 5000
    });
}