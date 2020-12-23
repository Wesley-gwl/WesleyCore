$.ajaxSetup({
    error: function (jqXHR, textStatus, errorMsg) {
        errorHandler(jqXHR)
    }
});
//ajax错误处理
function errorHandler(xhr) {
    if (xhr.status == 401) {
        alert("登录超时或身份验证错误,请重新登录!");
        top.location.href = "/Home/Login";
    }
    else if (xhr.status == 403) {
        alert("你没有权限!");
    }
    else if (xhr.status == 0) {
        console.log({ xhr: xhr, message: 'xhr异常关闭' });
    }
    else {
        console.log("服务器错误： " + xhr.status + " " + xhr.statusText);
    }
}
function showPopupWindow(title, url, width, height) {
    layer.open({
        type: 2,
        title: title || '标题',
        shade: 0.1,
        area: [width || 800 + 'px', height || 600 + 'px'],
        content: url || '/Home/Error'
    });
}
function closePopupWindow() {
    layer.closeAll('iframe');
}
//站点最根级目录
function getRootPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos;
    if (strFullPath.indexOf('http') == 0)
        pos = strFullPath.substr(7).indexOf(strPath) + 7;
    else
        pos = strFullPath.substring(0, pos);
    var prePath = strFullPath.substring(0, pos);
    return (prePath + "/");
}
function getRootPathName() {
    return window.document.location.pathname;
}
//当前上级目录
function getCurrentPath() {
    var strFullPath = window.document.location.href;
    var strPath = window.document.location.pathname;
    var pos;
    if (strFullPath.indexOf('http') == 0)
        pos = strFullPath.substr(7).indexOf(strPath) + 7;
    else
        pos = strFullPath.substring(0, pos);
    var prePath = strFullPath.substring(0, pos);
    var postPath = strPath.substring(0, strPath.substr(1).indexOf('/') + 1);
    return (prePath + postPath);
}
function HasSbc(s) {//判断是否存在全角字符
    s = s.replace(":", "");
    if (s == "") {
        return true;
    }
    var reg = /^[\w\u4e00-\u9fa5\uf900-\ufa2d()（）-]*$/;
    if (reg.test(s)) {
        return false;
    } else {
        return true;
    }
};

function InLength(s, l) {
    if (l) {
        if (s.length > l) return false;
        else return true;
    }
    else
        return true;
};

function IsDecimal(s) { //检验是否为数字（包含小数点）modified by liujf 2011-11-24
    var reg = new RegExp(/^\d*\.?\d*$/);
    if (reg.test(s)) {
        var pos = s.indexOf(".");
        if (pos == 0 || (pos + 1 == s.length)) { return false; } //当值的末尾为"."或者开头为"."的，则返回false
        else { return true; }
    }
    else { return false; }
};

function IsNumber(s) { //适于校验非负整数
    var reg = /^[01233456789]{1,}$/;
    if (reg.test(s))
        return true;
    else
        return false;
};

function IsValidNum(s) { //是否有效数值
    if (s.trim() == "") {
        return false;
    }
    if (isNaN(s)) {
        return false;
    }
    if (parseFloat(s) <= 0) {
        return false;
    }
    return true;
};

//验证电话号码，只允许数字和“-”
function IsTel(s) {
    var reg = /[\d|-]{7,15}/;
    if (reg.test(s))
        return true;
    else
        return false;
};

//是否为电子邮件
function IsEmail(s) {
    var reg = /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/;
    if (reg.test(s))
        return true;
    else
        return false;
};
function IsMobile(s) { //适于校验非负整数
    var reg = /^^(13[0-9]|14[5|7]|15[0|1|2|3|5|6|7|8|9]|18[0|1|2|3|5|6|7|8|9])\d{8}$/;
    if (reg.test(s))
        return true;
    else
        return false;
};
Date.prototype.DateDiff = function (strInterval, dtEnd) {
    var dtStart = this;
    switch (strInterval) {
        case 's': return parseInt((dtEnd - dtStart) / 1000);
        case 'n': return parseInt((dtEnd - dtStart) / 60000);
        case 'h': return parseInt((dtEnd - dtStart) / 3600000);
        case 'd': return parseInt((dtEnd - dtStart) / 86400000);
        case 'w': return parseInt((dtEnd - dtStart) / (86400000 * 7));
        case 'm': return (dtEnd.getMonth() + 1) + ((dtEnd.getFullYear() - dtStart.getFullYear()) * 12) - (dtStart.getMonth() + 1);
        case 'y': return dtEnd.getFullYear() - dtStart.getFullYear();
    }
};
/* 得到日期年月日等加数字后的日期 */
Date.prototype.dateAdd = function (interval, number) {
    var d = this;
    var k = {
        'y': 'FullYear',
        'q': 'Month',
        'm': 'Month',
        'w': 'Date',
        'd': 'Date',
        'h': 'Hours',
        'n': 'Minutes',
        's': 'Seconds',
        'ms': 'MilliSeconds'
    };
    var n = {
        'q': 3,
        'w': 7
    };
    eval('d.set' + k[interval] + '(d.get' + k[interval] + '()+' + ((n[interval] || 1) * number) + ')');
    return d;
}
function strToDate(str) {
    if (!str)
        return null;

    var ms = /(\d{4}).(\d{2}).(\d{2})[^\d\n]*(?:(\d{2}))?(?:.(\d{2}))?(?:.(\d{2}))?/.exec(str);
    if (ms != null) {
        return new Date(ms[1], ms[2] - 1, ms[3], ms[4], ms[5], ms[6]);
    }
}
//时间格式转换 使用方法：var newTime= new Date(oldTime).format("yyyy-MM-dd hh:mm:ss");
Date.prototype.format = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1,                 //月份
        "d+": this.getDate(),                    //日
        "h+": this.getHours(),                   //小时
        "m+": this.getMinutes(),                 //分
        "s+": this.getSeconds(),                 //秒
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度
        "S": this.getMilliseconds()             //毫秒
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}

//实现字符串的trim功能-begin
function trim() {
    if (arguments.length < 1)
        return null;
    if (typeof (arguments[0]) == "string")
        return arguments[0].replace(/(^[\s　]*)|([\s　]*$)/g, "");
    else
        return "";
};
function ltrim() {
    if (arguments.length < 1)
        return null;
    if (typeof (arguments[0]) == "string")
        return arguments[0].replace(/(^[\s　]*)/g, "");
    else
        return "";
};
function rtrim() {
    if (arguments.length < 1)
        return null;
    if (typeof (arguments[0]) == "string")
        return arguments[0].replace(/([\s　]*$)/g, "");
    else
        return "";
};

//查找数组中对象
function queryArray(arr, field, value) {
    var index = -1;
    $.each(arr, function (i, o) {
        if (o) {
            if (o[field] && o[field] == value) {
                index = i;
                return false;
            }
        }
    });
    return index;
}
function queryArrayText(arr, index) {
    var text = "";
    $.each(arr, function (i, o) {
        if (o) {
            if (index == o) {
                text = i;
            }
        }
    });
    return text;
}
//返回符合某一属性值的数组
function queryArrayData(arr, field, value) {
    var retArrarData = [];
    $.each(arr, function (i, o) {
        if (o) {
            if (o[field] && o[field].indexOf(value) >= 0) {
                retArrarData.push(o);
            }
        }
    })
    return retArrarData;
}
Array.prototype.del = function (n) {//n表示第几项，从0开始算起。
    //prototype为对象原型，注意这里为对象增加自定义方法的方法。
    // if (!(this instanceof Array)) return;
    if (n < 0)//如果n<0，则不进行任何操作。
        return this;
    else
        return this.slice(0, n).concat(this.slice(n + 1, this.length));
    /*
　　　concat方法：返回一个新数组，这个新数组是由两个或更多数组组合而成的。
　　　　　　　　　这里就是返回this.slice(0,n)/this.slice(n+1,this.length)
　　 　　　　　　组成的新数组，这中间，刚好少了第n项。
　　　slice方法： 返回一个数组的一段，两个参数，分别指定开始和结束的位置。
　　*/
}
Array.prototype.clear = function () {
    this.length = 0;
}
if (!Array.prototype.indexOf) {
    Array.prototype.indexOf = function (searchElement, fromIndex) {
        var k;
        if (this == null) throw new TypeError('"this" is null or not defined');
        var O = Object(this);
        var len = O.length >>> 0;
        if (len === 0) return -1;
        var n = +fromIndex || 0;
        if (Math.abs(n) === Infinity) n = 0;
        if (n >= len) return -1;
        k = Math.max(n >= 0 ? n : len - Math.abs(n), 0);
        while (k < len) {
            if (k in O && O[k] === searchElement) return k;
            k++;
        }
        return -1;
    };
}
Array.prototype.Contain = function (obj) {
    // if (!(this instanceof Array)) return;
    return this.indexOf(obj) !== -1;
}
//等待框
var __ishow = false;
function loading(isshow, message) {
    if (__ishow == isshow) return;
    __ishow = isshow;
    message = "";
    if ($("#loading") == undefined || $("#loading").css("display") == null) {
        var divload = '<div id="loading"/>';
        divload += '<div id="loadimg">' + message + '</div>';
        divload += '<div id="loadifr"><iframe width="100%" scrolling="no" height="100%" frameborder="0"></iframe></div>';
        $(document.body).append(divload);
    }
    if (isshow == true) {
        $("#loading").fadeTo('slow', 0.1);
        $("#loadimg").css("display", "block").animate('slow');
        $("#loadifr").fadeTo('slow', 0.1);
    }
    else {
        $("#loading").css("display", "none");
        $("#loadimg").css("display", "none");
        $("#loadifr").css("display", "none");
    }
    GoTop();
};
//转义字符格式化转换
String.prototype.formating = function () {
    if (!this) return this;
    var str = this;
    str = str.replace(/&lt;/g, "<");
    str = str.replace(/&gt;/g, ">");
    str = str.replace(/&quot;/g, "\"");
    str = str.replace(/&#39;/g, "'");
    str = str.replace(/&br;/g, "\n");
    return str;
}

//转义字符格式化转换
String.prototype.IsGuid = function () {
    if (!this) return false;
    var str = this;
    var reg = new RegExp("[0-9a-z]{8}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{4}-[0-9a-z]{12}");
    return reg.test(str);
}

//bootstrap自定义列
function GetFormatterButton(title, _class, index, event) {
    return "<a href='javascript:" + event + "(" + index + ")' title='" + title + "'><i class='" + _class + "'></i>" + title + "</a>";
}
function MouseButton(_class) {
    return "<div class='" + _class + "'>默认地址<div>";
}
function GetMouseButton(title, _class, index, event, a_class) {
    return "<a class='" + a_class + "' href='javascript:" + event + "(" + index + ")' title='" + title + "'><i class='" + _class + "'></i>" + title + "</a>";
}

//引入bootstrap-dialog.js
//提示前，先滚动到top，回到顶部才能看到进度条，及提示信息
if (typeof (msgBoxN) != "undefined") {
    window.alert = function (text, callback, hiddenMessage, embed) {
        GoTop();
        msgBoxN(text, callback, hiddenMessage, embed);
    }
}
//提示前，先滚动到top，回到顶部才能看到进度条，及提示信息
msgBox = function (text, callback, hiddenMessage, embed) {
    GoTop();
    msgBoxN(text, callback, hiddenMessage, embed);
}
//回到顶部
function GoTop() {
    if (typeof (parent.GoTopParent) != "undefined")
        parent.GoTopParent();//回到顶部才能看到进度条，及提示信息
    $('html,body').animate({ scrollTop: 0 }, 500);
}
//设置cookie
function setCookie(name, value, Days) {
    delCookie(name);
    var exp = new Date();
    exp.setTime(exp.getTime() + Days * 24 * 60 * 60 * 1000);
    document.cookie = name + "=" + encodeURIComponent(value) + ";expires=" + exp.toGMTString() + "; path=/;"
}
//读取cookie
function getCookie(name) {
    var arr, reg = new RegExp("(^| )" + name + "=([^;]*)(;|$)");
    if (arr = document.cookie.match(reg))
        return decodeURIComponent(arr[2]);//unescape
    else
        return null;
}
//删除cookie
function delCookie(name) {
    var exp = new Date();
    exp.setTime(exp.getTime() - 1);
    var cval = getCookie(name);
    if (cval != null)
        document.cookie = name + "=" + cval + ";expires=" + exp.toGMTString();
}
$(function () {
    //关闭编辑控件的语法检查功能HTML5
    $("input,textarea").attr("spellcheck", "false");
    //点击标签后在iframe中打开页面
    $('.a-partital').on("click", function (e) {
        href = $(e.target).closest('a').attr('href').replace(/.*(?=#[^\s]*$)/, '').replace('#', '');
        $('#ifrChild').attr("src", href + "?random=" + Math.random());
        GoTop();
    })
    //根据url激活对应标签
    var index_ = window.document.location.toString().indexOf('#');
    if (index_ > 0) {
        var url = window.document.location;
        var _active = url.toString().substr(index_);
        if (_active.length > 1) {
            //激活标签
            $('ul li>a[href="' + _active + '"]').closest('li').addClass('active');
            //刷新iframe页面
            $('#ifrChild').attr("src", url.toString().substr(index_ + 1) + "?random=" + Math.random());
            GoTop();
        }
    }

    //新菜单效果
    $(".xiala_menu").click(function () {
        //隐藏平级的菜单
        var menus = $(".xiala_menu").not($(this)[0]);
        if (menus.size() > 0) {
            for (var i = 0; i < menus.length; i++) {
                var node = menus.eq(i).next(".xiala_con");
                if (!node.is(':hidden')) {
                    node.hide();
                    menus.eq(i).removeClass("hout_menuh");
                }
            }
        }
        //处理当前点击的菜单
        var node = $(this).next(".xiala_con");
        if (node.is(':hidden')) {
            node.show();
            $(this).addClass("hout_menuh");
        } else {
            node.hide();
            $(this).removeClass("hout_menuh");
        }
    });
});

//图片预览
function PreviewImage(fileObj, imgPreviewId, divPreviewId) {
    if (fileObj.files.length <= 0) return;
    var allowExtention = ".jpg,.bmp,.gif,.png";//允许上传文件的后缀名document.getElementById("hfAllowPicSuffix").value; 
    var extention = fileObj.value.substring(fileObj.value.lastIndexOf(".") + 1).toLowerCase();
    var browserVersion = window.navigator.userAgent.toUpperCase();
    if (allowExtention.indexOf(extention) > -1) {
        if (fileObj.files) {//HTML5实现预览，兼容chrome、火狐7+等 
            if (window.FileReader) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById(imgPreviewId).setAttribute("src", e.target.result);
                }
                reader.readAsDataURL(fileObj.files[0]);
            } else if (browserVersion.indexOf("SAFARI") > -1) {
                alert("不支持Safari6.0以下浏览器的图片预览!");
            }
        } else if (browserVersion.indexOf("MSIE") > -1) {
            if (browserVersion.indexOf("MSIE 6") > -1) {//ie6 
                document.getElementById(imgPreviewId).setAttribute("src", fileObj.value);
            } else {//ie[7-9] 
                fileObj.select();
                if (browserVersion.indexOf("MSIE 9") > -1)
                    fileObj.blur();//不加上document.selection.createRange().text在ie9会拒绝访问 
                var newPreview = document.getElementById(divPreviewId + "New");
                if (newPreview == null) {
                    newPreview = document.createElement("div");
                    newPreview.setAttribute("id", divPreviewId + "New");
                    newPreview.style.width = document.getElementById(imgPreviewId).width + "px";
                    newPreview.style.height = document.getElementById(imgPreviewId).height + "px";
                    newPreview.style.border = "solid 1px #d2e2e2";
                }
                newPreview.style.filter = "progid:DXImageTransform.Microsoft.AlphaImageLoader(sizingMethod='scale',src='" + document.selection.createRange().text + "')";
                var tempDivPreview = document.getElementById(divPreviewId);
                tempDivPreview.parentNode.insertBefore(newPreview, tempDivPreview);
                tempDivPreview.style.display = "none";
            }
        } else if (browserVersion.indexOf("FIREFOX") > -1) {//firefox 
            var firefoxVersion = parseFloat(browserVersion.toLowerCase().match(/firefox\/([\d.]+)/)[1]);
            if (firefoxVersion < 7) {//firefox7以下版本 
                document.getElementById(imgPreviewId).setAttribute("src", fileObj.files[0].getAsDataURL());
            } else {//firefox7.0+                     
                document.getElementById(imgPreviewId).setAttribute("src", window.URL.createObjectURL(fileObj.files[0]));
            }
        } else {
            document.getElementById(imgPreviewId).setAttribute("src", fileObj.value);
        }
    } else {
        alert("仅支持" + allowExtention + "为后缀名的文件!");
        fileObj.value = "";//清空选中文件 
        if (browserVersion.indexOf("MSIE") > -1) {
            fileObj.select();
            document.selection.clear();
        }
        //fileObj.outerHTML = fileObj.outerHTML;
    }
}
//获取当前时间
function showTime(Time, pdVal) {
    var trans_day = "";
    //var cur_date = oDate.value == "" ? new Date() : new Date($('#date1').val());/
    var cur_date = Time == null ? new Date() : Time;/* 如果日期框内为空的话就获取系统的时间为输入框初始化赋值，如果有值（用户自己选择的时间），那就获取用户自己选择的时间 */
    var cur_year = cur_date.getFullYear();

    var cur_month = cur_date.getMonth() + 1;
    var real_date = cur_date.getDate();
    cur_month = cur_month > 9 ? cur_month : ("0" + cur_month);
    real_date = real_date > 9 ? real_date : ("0" + real_date);
    eT = cur_year + "-" + cur_month + "-" + real_date;
    if (pdVal != null && pdVal != 0) {
        trans_day = TransDate(eT, pdVal);
    }
    else {
        trans_day = eT;
    }
    //处理  
    return trans_day;
}
//加减天数
function TransDate(dateParameter, num) {
    var translateDate = "", dateString = "", monthString = "", dayString = "";
    translateDate = dateParameter.replace("-", "/").replace("-", "/");
    var newDate = new Date(translateDate);
    newDate = newDate.valueOf();
    newDate = newDate + num * 24 * 60 * 60 * 1000;
    newDate = new Date(newDate);
    //如果月份长度少于2，则前加 0 补位     
    if ((newDate.getMonth() + 1).toString().length == 1) {
        monthString = 0 + "" + (newDate.getMonth() + 1).toString();
    } else {
        monthString = (newDate.getMonth() + 1).toString();
    }
    //如果天数长度少于2，则前加 0 补位     
    if (newDate.getDate().toString().length == 1) {
        dayString = 0 + "" + newDate.getDate().toString();
    } else {
        dayString = newDate.getDate().toString();
    }
    dateString = newDate.getFullYear() + "-" + monthString + "-" + dayString;
    return dateString;
}
/*停止事件向上传播
  args: js 参数对象 arguments
*/
function eventStopProp(args) {
    var theEvent = window.event || args.callee.caller.arguments[0];
    theEvent.stopPropagation();
}

// 默认图片
const defaultImageUrl = '../content/image/defoultImage.png';

function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

/**
 * 前补字符
 * @param num 要格式化的参数
 * @param n 保留几位（太少会切掉原来的参数）
 * @param char 填充字符
 */
function prefixChar(num, n, char) {
    return (Array(n).join(char) + num).slice(-n);
}
//跳转到页面 url='/controller/action?query=666'
function go2Page(url) {
    if (url == null || url == '')
        return;
    $.ajax({
        url: '/Home/GetTabItem',
        type: 'POST',
        data: { url: url },
        success: function (re) {
            if (re.success) {
                parent.addNewTabItem(re.data);
            }
            else {
                $.messager.alert('错误', re.message);
            }
        }
    })
}

//从url获取参数
//调用：var args = GetUrlParms();var id=args["Guid"];
function GetUrlParms() {
    var args = new Object();
    var query = location.search.substring(1); //获取查询串
    var pairs = query.split("&");
    for (var i = 0; i < pairs.length; i++) {
        var pos = pairs[i].indexOf('='); //查找name=value
        if (pos == -1) continue; //如果没有找到就跳过
        var argname = pairs[i].substring(0, pos); //提取name
        var value = pairs[i].substring(pos + 1); //提取value
        args[argname] = unescape(value); //存为属性
    }
    return args;
}

/**
 * 下载文件
 * @param {any} url action地址
 * @param {any} data 参数
 * @param {any} method get/post
 */
function ajaxDownload(url, data, method) {
    if (url && data) {
        // data 是 string 或者 array/object
        data = typeof data == 'string' ? data : decodeURIComponent(jQuery.param(data));
        var inputs = '';
        jQuery.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });
        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>').appendTo('body').submit().remove();
    }
}
/**
 *
 * @param {any} file
 * @param {function} callback @argument url stirng
*Plupload中为我们提供了mOxie对象
*有关mOxie的介绍和说明请看：https://github.com/moxiecode/moxie/wiki/API
*file为plupload事件监听函数参数中的file对象,callback为预览图片准备完成的回调函数
*/
function previewImage(file, callback) {
    if (!file || !/image\//.test(file.type)) return; //确保文件是图片
    if (file.type == 'image/gif') { //gif使用FileReader进行预览,因为mOxie.Image只支持jpg和png
        var gif = new moxie.file.FileReader();
        gif.onload = function () {
            callback(gif.result);
            gif.destroy();
            gif = null;
        };
        gif.readAsDataURL(file.getSource());
    } else {
        var image = new moxie.image.Image();
        image.onload = function () {
            image.downsize(150, 150);//先压缩一下要预览的图片,宽300，高300
            var imgsrc = image.type == 'image/jpeg' ? image.getAsDataURL('image/jpeg', 80) : image.getAsDataURL(); //得到图片src,实质为一个base64编码的数据
            callback && callback(imgsrc); //callback传入的参数为预览图片的url
            image.destroy();
            image = null;
        };
        image.load(file.getSource());
    }
}

/**
 * 转换阿拉伯数字为中文大写
 * @param {number} cnIntLast 要转换的阿拉伯数字
 */
Number.prototype.toUpper = function (cnIntLast = "元") {
    money = this;
    var cnNums = new Array("零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖"); //汉字的数字
    var cnIntRadice = new Array("", "拾", "佰", "仟"); //基本单位
    var cnIntUnits = new Array("", "万", "亿", "兆"); //对应整数部分扩展单位
    var cnDecUnits = new Array("角", "分", "毫", "厘"); //对应小数部分单位
    var cnInteger = "整"; //整数金额时后面跟的字符
    //var cnIntLast = "元"; //整型完以后的单位
    var maxNum = 999999999999999.9999; //最大处理的数字

    var IntegerNum; //金额整数部分
    var DecimalNum; //金额小数部分
    var ChineseStr = ""; //输出的中文金额字符串
    var parts; //分离金额后用的数组，预定义
    if (money == "") {
        return "";
    }
    money = parseFloat(money);
    if (money >= maxNum) {
        console.log('超出最大处理数字');
        return "";
    }
    if (money == 0) {
        ChineseStr = cnNums[0] + cnIntLast + cnInteger;
        //document.getElementById("show").value=ChineseStr;
        return ChineseStr;
    }
    money = money.toString(); //转换为字符串
    if (money.indexOf(".") == -1) {
        IntegerNum = money;
        DecimalNum = '';
    } else {
        parts = money.split(".");
        IntegerNum = parts[0];
        DecimalNum = parts[1].substr(0, 4);
    }
    if (parseInt(IntegerNum, 10) > 0) {//获取整型部分转换
        zeroCount = 0;
        IntLen = IntegerNum.length;
        for (i = 0; i < IntLen; i++) {
            n = IntegerNum.substr(i, 1);
            p = IntLen - i - 1;
            q = p / 4;
            m = p % 4;
            if (n == "0") {
                zeroCount++;
            } else {
                if (zeroCount > 0) {
                    ChineseStr += cnNums[0];
                }
                zeroCount = 0; //归零
                ChineseStr += cnNums[parseInt(n)] + cnIntRadice[m];
            }
            if (m == 0 && zeroCount < 4) {
                ChineseStr += cnIntUnits[q];
            }
        }
        ChineseStr += cnIntLast;
        //整型部分处理完毕
    }
    if (DecimalNum != '') {//小数部分
        decLen = DecimalNum.length;
        for (i = 0; i < decLen; i++) {
            n = DecimalNum.substr(i, 1);
            if (n != '0') {
                ChineseStr += cnNums[Number(n)] + cnDecUnits[i];
            }
        }
    }
    if (ChineseStr == '') {
        ChineseStr += cnNums[0] + cnIntLast + cnInteger;
    } else if (DecimalNum == '') {
        ChineseStr += cnInteger;
    }
    //alert(ChineseStr);
    return ChineseStr;
}

/*
 *转换json 为 formdata 格式的键值对 数组
 * 未处理 json 数组 即 obj=[{}] 和 obj={a:[1，2，3]} 支持 obj={a:[{}]}
 * @@param obj
 * @@reurn [{k,v}...]
 */
function json2formDataKeyValues(obj) {
    //obj = { test: 123, obj: { idno: '1111', qut: 123 }, list: [{idno:'1111',qut:123}] };
    var kv = [];
    function objPropertyToKeyValue(obj, pNodeK) {
        for (var i in obj) {
            var kname = "";
            if (pNodeK && typeof (pNodeK) === "string")
                kname = `${pNodeK}[${i}]`;
            else
                kname = i;
            var t = typeof (obj[i]);
            if (obj[i] instanceof Array) {
                //Kname 开头固定+[索引]
                for (var j = 0; j < obj[i].length; j++) {
                    //kname = `${kname}[${j}]`;
                    objPropertyToKeyValue(obj[j], kname);
                }
            }
            //
            if (t === "object") {
                objPropertyToKeyValue(obj[i], kname);
            }
            else if (t === "string" || t == "number" || t == "boolean") {
                //单节点
                kv.push({ k: kname, v: obj[i] })
            } else {
                //console.log("空循环")
            }
        }
    }
    objPropertyToKeyValue(obj, '');
    //console.log(kv);
    return kv;
}

/**
 * 获取行上的 index 属性
 * @param {datagrid row html element} target
 */
function getRowIndexAttr(target) {
    var tr = $(target).closest('tr.datagrid-row');
    return parseInt(tr.attr('datagrid-row-index'));
}
/**
 * 获取菜单信息
 * @param {string} url
 */
function getFeature(url) {
    var result = null;
    $.ajax({
        url: '/Home/GetFeature',
        type: 'post',
        async: false,
        data: { url: url },
        success: (re) => { result = re.data; }
    });

    return result;
}
/**
 * 打开新菜单
 * @param {string} url
 * @param {{k:"",v:""}...} queryParams
 */
function OpenNewTab(url) {
    if (arguments.length > 1) {
        url += "?";
        var length = arguments.length;
        for (var i = 1; i < length; i++) {
            url += `${arguments[i].k}=${arguments[i].v}`;
            if (i + 1 < length) {
                url += "$";
            }
        }
    }
    var feature = getFeature(url);
    if (feature) {
        var item = { id: feature.id, name: feature.title, href: url };
        parent.addNewTabItem(item);
    }
}
/**
 *
 * @param {hex} a 16进制随机数 种子
 */
function getGuid(a) {
    return a // if the placeholder was passed, return
        ? ( // a random number from 0 to 15
            a ^ // unless b is 8,
            Math.random() // in which case
            * 16 // a random number from
            >> a / 4 // 8 to 11
        ).toString(16) // in hexadecimal
        : ( // or otherwise a concatenated string:
            [1e7] + // 10000000 +
            -1e3 + // -1000 +
            -4e3 + // -4000 +
            -8e3 + // -80000000 +
            -1e11 // -100000000000,
        ).replace( // replacing
            /[018]/g, // zeroes, ones, and eights with
            b // random hex digits
        )
}
/**
 * 日期字符串 转date
 * @param {datestr} fDate
 */
function stringToDate(fDate) {
    if (!fDate) return null;
    var fullDate = fDate.split("-");
    if (fullDate.length < 3) return null;

    return new Date(fullDate[0], fullDate[1] - 1, fullDate[2], 0, 0, 0);
}
//全屏（在哪个页面调用，就全屏化哪个页面）
function fullScreen() {
    element = document.documentElement;
    //判断浏览器是否支持全屏
    if ((document.fullscreenEnabled ||
        document.mozFullScreenEnabled ||
        document.webkitFullscreenEnabled ||
        document.msFullscreenEnabled || false) == false)
        return;
    //判断当前全屏状态
    if ((document.fullscreenElement ||
        document.msFullscreenElement ||
        document.mozFullScreenElement ||
        document.webkitFullscreenElement || false) == false) {//非全屏
        var requestMethod = element.requestFullScreen || //W3C
            element.webkitRequestFullScreen || //FireFox
            element.mozRequestFullScreen || //Chrome等
            element.msRequestFullScreen; //IE11
        if (requestMethod) {
            requestMethod.call(element);
        } else if (typeof window.ActiveXObject !== "undefined") { //for Internet Explorer
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
    } else {//已全屏
        var exitMethod = document.exitFullscreen || //W3C
            document.mozCancelFullScreen || //FireFox
            document.webkitExitFullscreen || //Chrome等
            document.webkitExitFullscreen; //IE11
        if (exitMethod) {
            exitMethod.call(document);
        } else if (typeof window.ActiveXObject !== "undefined") { //for Internet Explorer
            var wscript = new ActiveXObject("WScript.Shell");
            if (wscript !== null) {
                wscript.SendKeys("{F11}");
            }
        }
    }
}
//当前全屏状态
function isFullScreen() {
    return document.fullscreenElement ||
        document.msFullscreenElement ||
        document.mozFullScreenElement ||
        document.webkitFullscreenElement || false;
}