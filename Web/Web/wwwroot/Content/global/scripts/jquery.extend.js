//jQuery扩展
jQuery.extend({
    "ajaxSyncGet": function (url, data) {
        var ret = $.ajax({
            type: "GET",
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var e = JSON.parse(XMLHttpRequest.responseText);
                alert(e.Message, e.StackTrace);
            }
        });
        return JSON.parse(ret.responseText);
    },
    "ajaxSync": function (url, data) {
        var ret = $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            cache: false,
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                var e = JSON.parse(XMLHttpRequest.responseText);
                alert(e.Message, e.StackTrace);
            }
        });
        return JSON.parse(ret.responseText);
    },
    "ajaxAsync": function (url, data, callBack) {
        var calbacks = $.Callbacks("unique");
        calbacks.add(callBack);
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: true,
            cache: false,
            beforeSend: function (XMLHttpRequest) {
                loading(true);
            },
            complete: function (XMLHttpRequest, textStatus) {
                loading(false);
                calbacks.fire(JSON.parse(XMLHttpRequest.responseText));
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // 通常 textStatus 和 errorThrown 之中
                // 只有一个会包含信息
                //this; // 调用本次AJAX请求时传递的options参数
            }
        });
    },
    "ajaxAsyncForm": function (url, data, callBack) {
        var calbacks = $.Callbacks("unique");
        calbacks.add(callBack);
        $.ajax({
            type: "POST",
            url: url,
            data: data,
            async: true,
            cache: false,
            beforeSend: function (XMLHttpRequest) {
                loading(true);
            },
            complete: function (XMLHttpRequest, textStatus) {
                loading(false);
                calbacks.fire(JSON.parse(XMLHttpRequest.responseText));
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                // 通常 textStatus 和 errorThrown 之中
                // 只有一个会包含信息
                //this; // 调用本次AJAX请求时传递的options参数
            }
        });
    },
    //$.QueryString["ppp"]
    //$.addQueryString({"key":"value"});
    'QueryString':
        window.location.search.length <= 1 ?
            new Object() : function (a) {
                var b = new Object();
                for (var i = 0; i < a.length; ++i) {
                    var p = a[i].split('=');
                    if (p[0] == "_") {
                        continue;
                    }
                    b[p[0]] = decodeURIComponent(p[1]);
                }
                return b;
            }(window.location.search.substr(1).split('&')),
    'addQueryString': function (keyValues) {
        return this.param(this.extend({}, this.QueryString, keyValues));
    },
    'getQueryString': function (name) {
        var result = window.location.search.match(new RegExp("[\?\&]" + name
            + "=([^\&]+)", "i"));
        if (result == null || result.length < 1) {
            return "";
        }
        return result[1];
    },
    //页面数据变量缓存
    //getVarFun 通常为获取对象的ajax方法
    'CacheVarSetting': function (varName, getVarFun) {
        var f = $(document).data("VarFun");
        if (!f) {
            f = new Object();
        }
        f[varName] = getVarFun;
        $(document).data("VarFun", f);
    },
    'GetCacheVar': function (varName, key1, key2, key3, key4) {
        var dataKey = varName;
        if (key1) {
            dataKey += "|" + key1;
        }
        if (key2) {
            dataKey += "|" + key2;
        }
        if (key3) {
            dataKey += "|" + key3;
        }
        if (key4) {
            dataKey += "|" + key4;
        }
        var v = $(document).data(dataKey);
        if (v) {
            return v;
        }
        else {
            var f = $(document).data("VarFun");
            if (!f) {
                return null;
            }
            if ($.isFunction(f[varName])) {
                return f[varName].call(window, key1, key2, key3, key4);
            }
            else {
                return null;
            }
        }
    },
    //将页面上指定区域的带name属性的控件的值组装成对象
    'serializeObject': function (area) {
        var model = new Object();
        var items = $((area || "") + " input[name],textarea[name],select[name]");
        for (var i = 0; i < items.size(); i++) {
            model[items.eq(i).attr('name')] = items.eq(i).val();
        }
        return model;
        //alert(JSON.stringify(model));
    },
    'getFormJson': function (frm) {
        var o = {};
        var a = $(frm).serializeArray();
        $.each(a, function () {
            if (o[this.name] !== undefined) {
                if (!o[this.name].push) {
                    o[this.name] = [o[this.name]];
                }
                o[this.name].push(this.value || '');
            } else {
                o[this.name] = this.value || '';
            }
        });
        return o;
    },
    'setValueFromJson': function (jsonStr) {
        var o = JSON.parse(jsonStr);
        $("body").find("*").each(function () {
            if (o[this.id] != undefined) {
                $(this).html(o[this.id]);
            }
        });
    },
    //将字符串转换成日期格式
    'getStrDate': function (strDate) {
        var date = eval('new Date(' + strDate.replace(/\d+(?=-[^-]+$)/,
            function (a) { return parseInt(a, 10) - 1; }).match(/\d+/g) + ')');
        return date;
    },
    //获取指定日期之前几天
    'getBeforeDate': function (d, n) {
        var year = d.getFullYear();
        var mon = d.getMonth() + 1;
        var day = d.getDate();
        if (day <= n) {
            if (mon > 1) {
                mon = mon - 1;
            }
            else {
                year = year - 1;
                mon = 12;
            }
        }
        d.setDate(d.getDate() - n);
        year = d.getFullYear();
        mon = d.getMonth() + 1;
        day = d.getDate();
        s = year + "-" + (mon < 10 ? ('0' + mon) : mon) + "-" + (day < 10 ? ('0' + day) : day);
        return s;
    },
    'getAfterDate': function (d, n) {
        var year = d.getFullYear();
        var mon = d.getMonth() + 1;
        var day = d.getDate();
        if (day <= n) {
            if (mon > 1) {
                mon = mon - 1;
            }
            else {
                year = year - 1;
                mon = 12;
            }
        }
        d.setDate(d.getDate() + n);
        year = d.getFullYear();
        mon = d.getMonth() + 1;
        day = d.getDate();
        s = year + "-" + (mon < 10 ? ('0' + mon) : mon) + "-" + (day < 10 ? ('0' + day) : day);
        return s;
    },
    /** 树节点取消选中，请添加到树容器的 click 事件中使用 */
    treeUnSelect: function (tree) {
        var s = $(tree).tree('getSelected');
        if (!s) return false;
        $(tree).find('.tree-node-selected').removeClass('tree-node-selected');
        return true;
    },
    /**
     * 设定起始时间的最小值
     * @param minDate 最小时间，默认是 new Date()
     */
    beginDateCalendar: function (minDate) {
        var _minDate = minDate ? new Date(minDate) : new Date();
        return {
            validator: function (date) {
                var defaultDate = new Date(_minDate.getFullYear(), _minDate.getMonth(), _minDate.getDate());
                return defaultDate >= date;
            }
        }
    },
    /**
     * 设定起始时间的选择事件
     * @param selector 截止时间控件
     */
    beginDateOnSelect: function (selector) {
        return function (date) {
            var $dtEnd = $(selector);
            var strEndDate = $dtEnd.datebox('getValue');
            if (strEndDate) {
                var endDate = new Date(strEndDate);
                if (date > endDate) {
                    $dtEnd.datebox('clear');
                    $dtEnd.next().tooltip('error', {
                        content: '由于改变了起始日期，请重新选择截至日期！'
                    });
                }
            }
        }
    },
    /**
     * 设定结束时间的最小值
     * @param selector 起始时间控件
     */
    endDateCalendar: function (selector) {
        return {
            validator: function (date) {
                var now = new Date();
                var defaultDate = new Date(now.getFullYear(), now.getMonth(), now.getDate());
                var strBeginDate = $(selector).datebox('getValue');
                if (strBeginDate)
                    return new Date(strBeginDate) <= date;
                else
                    return defaultDate <= date;
            }
        }
    },
    /**
     * 初始化图片的预览功能
     * @param imgSelector 图片选择器
     */
    initImagePreview: function (imgSelector) {
        let xp = 10, yp = 20;//偏移量
        let css = {
            position: 'absolute',
            border: '1px solid #ccc',
            background: '#333',
            padding: '2px',
            color: '#fff',
            'z-index': 99999
        }
        $(imgSelector).mouseover(function (e) {
            var tooltip = '<div id="tooltip"><img src="' + this.src + '" /></div>';
            $('body').append(tooltip);
            var l = calImgLocation(this, e.pageX , e.pageY )
            //css.top = (e.pageY + y) + 'px';
            //css.left = (e.pageX + x) + 'px';
            css.top = l.top + 'px';
            css.left = l.left + 'px';
            $('#tooltip').css(css);

            $('#tooltip img').css({
                //'max-height': ($(window).height() - e.pageY - y - 10) + 'px',
                //'max-width': ($(window).width() - e.pageX - x - 10) + 'px'
                'max-height': l.heigth -10  + 'px',
                'max-width': l.width - 10 + 'px'
            })
        }).mouseout(function () {
            $('#tooltip').remove();
            }).mousemove(function (e) {
                var l = calImgLocation(this, e.pageX  , e.pageY )
                //css.top = (e.pageY + y) + 'px';
                //css.left = (e.pageX + x) + 'px';
                css.top = l.top + 'px';
                css.left = l.left + 'px';
                $('#tooltip').css(css);

                $('#tooltip img').css({
                    //'max-height': ($(window).height() - e.pageY - y - 10) + 'px',
                    //'max-width': ($(window).width() - e.pageX - x - 10) + 'px'
                    'max-height': l.heigth - 10 + 'px',
                    'max-width': l.width - 10 + 'px'
                })
            });

        function calImgLocation(imgTag, x, y) {
            var screenH = window.innerHeight;
            var screenW = window.innerWidth;
            var result = {
                top: y+yp,
                left: x+xp,
                width: imgTag.naturalWidth,
                height: imgTag.naturalHeight
            };

            //原图大小是否 适合默认位置
            if (result.width + x > screenW || result.height + y > screenH) {
                //根据给定坐标 计算适合的最大尺寸
                var x2 = screenW - x, y2 = screenH - y;
                //原始长宽比
                var bl = Math.floor(result.width / result.height *100)/100;
                
                //先处理高
                if (y2 < result.height) {
                    //下面显示不下 看下往上够不够
                    if (y < result.height+yp) {
                        //上面也不够
                        //缩放
                        if (y2 >= y) {
                            //下面空间更大
                            result.top = y+yp;
                            result.height = y2-yp;
                            result.width = Math.floor(result.height * bl);
                        } else {
                            result.top = 0;
                            result.height = y;
                            result.width = Math.floor(result.height * bl);
                        }

                    } else {
                        //上面可以显示
                        //设置定位点
                        result.top = y - result.height+yp;
                    }
                } else {
                    //不存在
                }
                //处理宽 如果上面已经缩放了 这里则可以判断缩放后是否足够 已经足够时 则 不会再缩放
                if (x2 < result.width) {
                    //右边显示不下 看下往左够不够
                    if (x < result.width) {
                        //上面也不够
                        //缩放
                        if (x2 >= x) {
                            //右面空间更大
                            result.left = x+xp;
                            result.width = screenW - x-xp;
                            result.height = Math.floor(result.width / bl);

                        } else {
                            result.left = 0;
                            result.width = x-xp;
                            result.height = Math.floor(result.width / bl);
                        }
                        //高度改变 top也要改变
                        if (result.top < y) {
                            //在上面显示
                            result.top = y - result.height - yp;
                        }

                    } else {
                        //上面可以显示
                        //设置定位点
                        result.left = x - result.width-xp;
                    }
                }  

            }
          
            return result;             
        }
    },
    /**
     * 异步下载文件 依赖easyui的进度条
     *
     */
    downloadAsync: function (method, url, data,contentType, displayfileName, callback) {
        if (!displayfileName)
            displayfileName = '';
        if (!method)
            method = "get";
        method = method.toLowerCase();
        if (!url)
            throw "url mast has value";

        if (method != "post" && method != "get")
            method = "get";
        var xhr = new XMLHttpRequest();

        var sendData = null;
        if (data && (typeof (data) == "object" || typeof (data) == "string")) {
            if (contentType.toLowerCase() ==="application/json") {
                sendData = JSON.stringify(data);
            } else if (method.toLowerCase() === "get") {
                //参数追加到url上
                var params;
                if (typeof (data) == "string")
                    params = data;
                else
                    params = $.param(data);
                var ls = url.indexOf(url.length);
                switch (ls) {
                    case "?":
                    case "&":
                        url += params
                        break;
                    default:
                        if (url.indexOf('?') < 0) {
                            url += "?" + params;
                        } else {
                            url += "&" + params;
                        }
                        break;
                }
            } else {
                //sendData赋值
                //sendData = JSON.stringify(data);
                sendData = $.param(data);
            }
        }
        xhr.open(method, url, true);    // 也可以使用POST方式，根据接口       
        xhr.setRequestHeader('content-type', contentType);
        //xhr.setRequestHeader("content-type","application/json");//json
        xhr.responseType = "blob";  // 返回类型blob

        $.messager.progress({ msg: '下载中。。。', interval: 0 });
        var bar = $.messager.progress('bar');

        //进度事件
        xhr.onprogress = function (ev) {
            if (ev.lengthComputable) {
                progress = ev.loaded / ev.total;
                //更新进度条
                bar.progressbar('setValue', parseFloat(progress, 2) * 100);
            }
        };

        // 定义请求完成的处理函数
        xhr.onload = function () {
            // 请求完成
            if (this.status === 200) {
                // 返回200
                var blob = this.response;
                var reader = new FileReader();
                reader.readAsDataURL(blob);  // 转换为base64，可以直接放入a表情href
                reader.onload = function (e) {
                    // 转换完成，创建一个a标签用于下载
                    var a = document.createElement('a');
                    if (!displayfileName || typeof (displayfileName) != 'string')
                        displayfileName = new Date().getTime + '.txt';
                    console.log(displayfileName);
                    a.download = displayfileName;
                    a.href = e.target.result;
                    $("body").append(a);  // 修复firefox中无法触发click
                    a.click();
                    $(a).remove();
                    if (typeof (callback) == "function")
                        callback();
                };
            }
            else if (this.status === 206) {//处理错误提示
                var result = this.response;
                if (result.type === 'application/json') {
                    var reader = new FileReader();
                    reader.readAsText(result, 'utf-8');
                    reader.onload = (e) => {
                        var jsonData = JSON.parse(reader.result);
                        if (!jsonData.success) {
                            $.messager.alert("警告", jsonData.message);
                        }
                    }
                }
            }
            else {
                errortitle = "错误";
                error = "导出错误";
                switch (this.status) {
                    case 404:
                        error += " 页面找不到";
                        break;
                    case 400:
                        error += " 导出参数不对";
                        break;
                    case 500:
                        error += " 导出数据失败";
                        break;
                    default:
                        break;
                }
                $.messager.alert(errortitle, error);
            }
            $.messager.progress('close');
        };
        xhr.onreadystatechange = function (ev) {
        }
        // 发送ajax请求
        xhr.send(sendData);
    },

    ajaxDownload: function (options) {
        var opt = $.extend({
            method: "get",
            url: "",
            data: "",
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            fileName: "",
            callback: ""
        }, options);
        $.downloadAsync(opt.method, opt.url, opt.data, opt.contentType, opt.fileName, opt.callback);
    },
    postbody: function (url, data, callback) {
        return jQuery.ajax({
            'type': 'POST',
            'url': url,
            'contentType': 'application/json',
            'data': JSON.stringify(data),
            'dataType': 'json',
            'success': callback
        });
    }
    
});