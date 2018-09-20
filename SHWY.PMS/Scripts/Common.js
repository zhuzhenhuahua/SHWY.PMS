﻿$.extend($.fn.datagrid.defaults, {
    onLoadError: function () {
        ButtonAuthentication();
    },
    onLoadSuccess: function () {
        ButtonAuthentication();
    }
});
function ButtonAuthentication() {
    //针对toolbar直接写到datagrid初始化中
    //$(".datagrid-toolbar").find("a").hide().each(function () {
    //    SetBtnDisplay();
    //});
    //针对toolbar写到html中
    //$(".easyui-linkbutton").hide().each(function () {
    //    SetBtnDisplay();
    //});
}
function SetBtnDisplay() {
    if ($("#user_buttons").val()) {
        var btns = JSON.parse($("#user_buttons").val());
        var id = $(this).attr("id");
        for (var i = 0; i < btns.length; i++) {
            if (id == btns[i].MenuOpid) {
                $(this).show();
            }
        }
    }
}
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
};
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