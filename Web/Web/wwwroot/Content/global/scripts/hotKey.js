/*用法：
 *1、页面引入hotKey.js
 *2、注册HotKeys定义过的事件（若不够可自己新增）HotKey.on(HotKeys热键,自定义页面函数名)
 * HotKey.on(HotKeys.Add, main.add);
 * HotKey.on(HotKeys.Edit, main.edit);
 * HotKey.on(HotKeys.Delete, main.del);
 * HotKey.on(HotKeys.Fresh, main.search);
*/

//热键定义，可自行添加
const HotKeys = {
    Tab: 9,//Tab
    Enter: 13,//回车
    Ctrl: 17,
    Alt: 18,
    Left: 37,//左
    UP: 38,//上
    Right: 39,//右
    Down: 40,//下
    Delete: 46,//删除
    Help: 112,//帮助F1
    Add: 113,//新增F2
    Edit: 114,//编辑F3
    Fresh: 116,//刷新F5
    One: 49,
    Two: 50,//数字以此类推
    A: 65,
    B: 66,//字母以此类推（无视大小写）
    Ext1: '78|true',//组合键【N|Ctrl】
}

//监听键盘事件
//如需拦截事件，不再冒泡，返回false
$(document).keydown(function (e) {
    var keys = e.keyCode;
    if (e.altKey || e.ctrlKey)
        keys += '|' + e.altKey;
    console.log(keys);

    //如热键冲突需要拦截的，或特殊处理的，统一写在switch中
    switch (keys) {
        //无需特殊处理的，直接在各自页面中注册对应的事件即可
        //case HotKeys.UP:
        //    HotKey.fire(keys);
        //    break;
        case HotKeys.Help://帮助F1 拦截
            HotKey.fire(keys);
            return false;
        case HotKeys.Edit://编辑F3 拦截
            HotKey.fire(keys);
            return false;
        case HotKeys.Fresh://刷新F5 拦截
            HotKey.fire(keys);
            return false;
        default:
            HotKey.fire(keys);
            break;
    }
});
//初始化全局事件委托
const HotKey = new HotKeyEventHandler();
//键盘事件委托
function HotKeyEventHandler(name) {
    this.name = name;
    var events = [];
    this.on = function (eventName, func) {
        if (events[eventName] == undefined) {
            events[eventName] = [];
        }
        events[eventName].push(func);
    };
    this.off = function (eventName, func) {
        var index = events[eventName].indexOf(func);
        if (index > -1) {
            events[eventName].splice(index, 1);
        }
    }
    this.fire = function (eventName) {
        if (events[eventName] == undefined)
            return;
        var args = [];
        var len = arguments.length;
        for (var i = 1; i < len; i++) {
            args[i - 1] = arguments[i];// 复制参数
        }
        len = events[eventName].length;
        for (var i = 0; i < len; i++) {
            events[eventName][i](args);
        }
    }
}