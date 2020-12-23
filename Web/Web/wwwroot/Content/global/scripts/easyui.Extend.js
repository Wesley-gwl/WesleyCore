// easyui messager扩展
$.extend($.messager, {
    /** 在屏幕上方中间显示一个提示框 */
    info: function () {
        if (arguments.length < 1) {
            throw "info method need least 1 argument";
        }
        var message = {
            title: "提示",
            msg: null,
            timeout: 1000,
            closable: false,
            showType: "side",
            style: {
                right: '',
                top: '',//document.body.scrollTop + document.documentElement.scrollTop,
                bottom: '10%'
            }
        };
        if (arguments.length == 1) {
            if (typeof (arguments[0]) === "object") {
                var arg = arguments[0];
                for (var i in message) {
                    if (arg.hasOwnProperty(i)) {
                        message[i] = arg[i];
                    }
                }
                return $.messager.show(message);
            } else if (typeof (arguments[0]) === "string") {
                message.msg = arguments[0];
            }
        }
        if (arguments.length > 1) {
            message.title = arguments[0];
            message.msg = arguments[1];
        }
        if (arguments.length > 2) {
            message.timeout = arguments[2] * 1000;
        }
        if (arguments.length > 3) {
            message.showType = arguments[3];
        }
        return $.messager.show(message);
    }
});
$.extend($.fn.datagrid.defaults, {
    editCellIndex: -1,
    editCellField: ""
});

//重写相关控件的loader
[
    'datagrid',
    'tree',
    'combobox',
    'combotree',
    'combogrid',
    'treegrid',
    'datalist',
    'combotreegrid',
].forEach(function (controlName) {
    $.extend($.fn[controlName].defaults, {
        method: "get",
        loader: function (param, success, error) {
            var that = $(this);
            if (controlName === "combogrid" && that.hasClass('datagrid-f')) {
                controlName = "datagrid"
            }
            if (controlName === "combotree" && that.hasClass('tree')) {
                controlName = "tree"
            }
            var opts = that[controlName]("options");
            if (!opts.url) {
                return false;
            }

            var ajaxOpts = {
                type: opts.method,
                url: opts.url,
                data: param,
                dataType: "json",
                success: function (data) {
                    if (!data.hasOwnProperty('success') || !data.hasOwnProperty('data')) {
                        $.messager.alert("提示", "返回的数据格式错误！不是BizResult类型")
                        //错误处理
                        error.apply(this, arguments);
                        return;
                    }

                    if (data.success) {
                        success(data.data);
                    }
                    else {
                        //错误处理
                        error.apply(this, arguments);
                        $.messager.alert("提示", data.message)
                    }
                },
                error: function () {
                    error.apply(this, arguments);
                }
            };
            if (opts.method.toLowerCase() == "get") {
                ajaxOpts.traditional = true;
            } else if (opts.method.toLowerCase() == "post") {
                opts.contentType = 'application/json';
                opts.data = JSON.stringify(opts.data)
            }

            $.ajax(ajaxOpts);
        }
    });
})

var htmlInputType = ["button"
    , "checkbox"
    , "file"
    , "hidden"
    , "image"
    , "password"
    , "radio"
    , "reset"
    , "submit"
    , "text"];

$.extend($.fn.datagrid.methods, {
    /** 获取指定行的数据 从0开始 */
    getIndexRow: function (jq, rowIndex) {
        //获取所有列
        var rows = $(jq).datagrid('getRows');
        if (rowIndex >= rows.length)
            return null;
        return rows[rowIndex];
    },
    /** 编辑单元格 */
    editCell: function (jq, param) {
        return jq.each(function () {
            var dg = this;
            //选中当前行
            //判断当前列是否可编辑
            var editCol = $(dg).datagrid('getColumnOption', param.field);

            //获取所有列的编辑项的配置
            var opts = $(dg).datagrid('options');
            //结束正在编辑的列 如果点击的不是正在编辑的单元格
            if (opts.editCellIndex > -1 && opts.editCellField) {
                //同一个单元格
                if (opts.editCellIndex == param.index && opts.editCellField == param.field)
                    return;
                // 验证是否符合结束条件
                if (!$(dg).datagrid('validateRow', param.index)) return;
                $(dg).datagrid('endCellEdit');
            }
            //当前列不可编辑
            if (!editCol.editor) return;

            opts.editCellIndex = param.index;
            opts.editCellField = param.field;

            //把除了指定列之外的列的编辑项置空
            var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
            //var fields = $(dg).datagrid('getColumnFields');
            for (var i = 0; i < fields.length; i++) {
                var col = $(dg).datagrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            //编辑指定行
            $(dg).datagrid('beginEdit', param.index);

            //还原每列的编辑项
            for (var i = 0; i < fields.length; i++) {
                var col = $(dg).datagrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }

            var i = $(dg).datagrid('getEditor', param);//获取当前编辑器
            if (!i) {
                $(dg).datagrid('endEdit', param.index);
                return;
            }

            //当前单元格编辑时触发
            if (param.OnCellEdit && typeof (param.OnCellEdit) == "function")
                param.OnCellEdit(i, param);

            var o;
            if (i.target[0].tagName == "INPUT" && htmlInputType.indexOf(i.type) > -1) {
                o = i.target;
                if (!i.target[0].id)
                    i.target[0].id = i.type + "_" + (new Date().getTime());
            }
            else
                eval("o = $(i.target)." + i.type + "('textbox')");

            if (!o) return
            o.focus()//获取焦点
            o.select()//获取焦点
            //判断点击的元素的标位置 结束编辑
            //console.log("add click event %s", o[0].id);
            var gridElement = $(this).parents(".datagrid");
            gridElement.on("mousedown." + o[0].id, "", { cellInfo: param, targetId: o[0].id, eventElement: gridElement }, function (e) {
                //return true 防止会阻止事件冒泡
                et = $(e.target);
                var data = e.data.cellInfo;
                var targetId = e.data.targetId;
                //debugger
                //是否是table内部的数据行
                var isIntable = et.is("table.datagrid-btable") || et.parents("table.datagrid-btable").is("table.datagrid-btable");
                //是否是同一列
                var isSameField = et.closest("td[field=" + data.field + "]").length > 0;
                if (isIntable) {
                    if (isSameField) {
                        //同一个单元格不会触发oncellclick 就不会结束编辑 所以要继续监听
                        //是否是同一个单元格
                        var index = et.parents("tr.datagrid-row-editing[datagrid-row-index]").attr("datagrid-row-index");
                        if (index == data.index) return true;

                        // console.log("remove click event %s", targetId);
                        //不是同一个单元格会触发oncellclick 也可能禁用的oncellclick  还是要处理

                        //会在 oncellclick 中结束编辑 所以绑定的事件就没用了
                        //$("body").off("mousedown." + targetId);
                        //return true;
                    }
                }
                // 验证是否符合结束条件
                if (!$(dg).datagrid('validateRow', data.index)) return true;
                $(dg).datagrid('endCellEdit');
                e.data.eventElement.off("mousedown." + targetId);
            });
            //console.log('edit cell %s %s', param.field ,param.index);
            return $(dg);
        });
    },
    /* 结束单元格编辑
     * @param data {index = 行（索引），field=列名}
     */
    endCellEdit: function (jq) {
        var opts = $(jq).datagrid('options');

        // 获取当前编辑框的target
        var col = $(jq).datagrid('getEditor', { index: opts.editCellIndex, field: opts.editCellField });
        if (col && col.target[0].classList.contains('combo-f')) {
            col.target.combo('hidePanel')
        }

        // 结束或取消编辑
        if ($(jq).datagrid('validateRow', opts.editCellIndex)) {
            $(jq).datagrid('endEdit', opts.editCellIndex);
        } else {
            $(jq).datagrid('cancelEdit', opts.editCellIndex);
        }
        opts.editCellIndex = -1;
        opts.editCellField = "";
        return jq;
    },
    /* 结束单元格编辑
     */
    resetCellEditInfo: function (jq, param) {
        var opts = $(jq).datagrid('options');
        opts.editCellIndex = -1;
        opts.editCellField = undefined;
    },
    /**
     * 展示tooltip提示信息
     * @param option position=出现方向，content=消息内容, index=出现的位置（索引）
     */
    showTip: function (jq, option) {
        if (!option) option = {};
        // 计算并找到滚动之后的第一条
        var scrollTop = jq.prevAll().find('.datagrid-body').scrollTop();
        var dgvRowHeight = $('.datagrid-row').height();
        var dfRow = Math.ceil(scrollTop / dgvRowHeight);

        var row = jq.prevAll().find('.datagrid-row').eq(option.index || dfRow);
        option.content = option.content || '请选择一行数据';
        row.tooltip('error', option);
    },
    fixRownumber: function (jq) {
        return jq.each(function () {
            var panel = $(this).datagrid("getPanel");
            //获取最后一行的number容器,并拷贝一份
            var clone = $(".datagrid-cell-rownumber", panel).last().clone();
            //由于在某些浏览器里面,是不支持获取隐藏元素的宽度,所以取巧一下
            clone.css({
                "position": "absolute",
                left: -1000
            }).appendTo("body");
            var width = clone.width("auto").width();
            //默认宽度是25,所以只有大于25的时候才进行fix
            if (width > 25) {
                //多加5个像素,保持一点边距
                $(".datagrid-header-rownumber,.datagrid-cell-rownumber", panel).width(width + 5);
                //修改了宽度之后,需要对容器进行重新计算,所以调用resize
                $(this).datagrid("resize");
                //一些清理工作
                clone.remove();
                clone = null;
            } else {
                //还原成默认状态
                $(".datagrid-header-rownumber,.datagrid-cell-rownumber", panel).removeAttr("style");
            }
        });
    },
    cellTip: function (jq) {
        function showTip(data, td, e) {
            if ($(td).text() == "" || $(td).hasClass('datagrid-td-rownumber'))
                return;
            data.tooltip.text($(td).text()).css({
                top: (e.pageY + 10) + 'px',
                left: (e.pageX + 20) + 'px',
                'z-index': $.fn.window.defaults.zIndex,
                display: 'block'
            });
        };
        return jq.each(function () {
            var grid = $(this);
            var options = $(this).data('datagrid');
            if (!options.tooltip) {
                var panel = grid.datagrid('getPanel').panel('panel');
                var defaultCls = {
                    'border': '1px solid #ccc',
                    'padding': '1px',
                    'color': '#333',
                    'background': '#f7f5d1',
                    'position': 'absolute',
                    'max-width': '500px',
                    'border-radius': '4px',
                    '-moz-border-radius': '4px',
                    '-webkit-border-radius': '4px',
                    'display': 'none'
                }
                var tooltip = $("<div id='celltip'></div>").appendTo('body');
                tooltip.css($.extend({}, defaultCls));
                options.tooltip = tooltip;
                panel.find('.datagrid-body').each(function () {
                    var delegateEle = $(this).find('> div.datagrid-body-inner').length
                        ? $(this).find('> div.datagrid-body-inner')[0]
                        : this;
                    $(delegateEle).undelegate('td', 'mouseover').undelegate(
                        'td', 'mouseout').undelegate('td', 'mousemove')
                        .delegate('td', {
                            'mouseover': function (e) {
                                if (options.tipDelayTime)
                                    clearTimeout(options.tipDelayTime);
                                var that = this;
                                options.tipDelayTime = setTimeout(
                                    function () {
                                        showTip(options, that, e);
                                    }, 1000);
                            },
                            'mouseout': function (e) {
                                if (options.tipDelayTime)
                                    clearTimeout(options.tipDelayTime);
                                options.tooltip.css({
                                    'display': 'none'
                                });
                            }
                        });
                });
            }
        });
    },
    /**
     * 关闭消息提示功能
     * @param {} jq
     * @return {}
     */
    cancelCellTip: function (jq) {
        return jq.each(function () {
            var data = $(this).data('datagrid');
            if (data.tooltip) {
                data.tooltip.remove();
                data.tooltip = null;
                var panel = $(this).datagrid('getPanel').panel('panel');
                panel.find('.datagrid-body').undelegate('td',
                    'mouseover').undelegate('td', 'mouseout')
                    .undelegate('td', 'mousemove')
            }
            if (data.tipDelayTime) {
                clearTimeout(data.tipDelayTime);
                data.tipDelayTime = null;
            }
        });
    }
});
//重写datagrid里 combogrid的getValue方法
$.extend($.fn.datagrid.defaults.editors, {
    combogrid: {
        getValue: function (target) {
            return $(target).combogrid('getText');
        },
        init: function (container, options) {
            var input = $('<input type="text" >').appendTo(container);
            input.combogrid(options);
            return input;
        },
        setValue: function (target, value) {
            $(target).combogrid('setValue', value);
        },
        resize: function (target, width) {
            var input = $(target);
            if ($.boxModel == true) {
                input.width(width - (input.outerWidth() - input.width()));
            } else {
                input.width(width);
            }
        }
    }
});
//treegrid扩展
$.extend($.fn.treegrid.methods, {
    /** 获取指定行的数据 从0开始 */
    getIndexRow: function (jq, rowIndex) {
        //获取所有列
        var rows = $(jq).treegrid('getRows');
        if (rowIndex >= rows.length)
            return null;
        return rows[rowIndex];
    },
    /** 编辑单元格 */
    editCell: function (jq, param) {
        return jq.each(function () {
            var dg = this;
            //判断当前列是否可编辑
            var editCol = $(dg).treegrid('getColumnOption', param.field);

            //获取所有列的编辑项的配置
            var opts = $(dg).treegrid('options');
            if (opts.editCellIndex > -1 && opts.editCellField) {
                //同一个单元格
                if (opts.editCellIndex == param.id && opts.editCellField == param.field)
                    return;

                // 验证是否符合结束条件
                if (!$(dg).treegrid('validateRow', param.id)) return;
                $(dg).treegrid('endCellEdit', { field: opts.editCellField, id: opts.editCellIndex });
            }

            opts.editCellIndex = param.id;
            opts.editCellField = param.field;

            //把除了指定列之外的列的编辑项置空
            var fields = $(this).treegrid('getColumnFields', true).concat($(this).treegrid('getColumnFields'));
            //var fields = $(dg).datagrid('getColumnFields');
            for (var i = 0; i < fields.length; i++) {
                var col = $(dg).treegrid('getColumnOption', fields[i]);
                col.editor1 = col.editor;
                if (fields[i] != param.field) {
                    col.editor = null;
                }
            }
            //编辑指定行
            $(dg).treegrid('beginEdit', param.id);

            //还原每列的编辑项
            for (var i = 0; i < fields.length; i++) {
                var col = $(dg).treegrid('getColumnOption', fields[i]);
                col.editor = col.editor1;
            }

            var ed = $(dg).treegrid('getEditor', param);//获取当前编辑器
            if (!ed) {
                $(dg).treegrid('endEdit', param.id);
                return;
            }

            //当前单元格编辑时触发
            if (param.OnCellEdit && typeof (param.OnCellEdit) == "function")
                param.OnCellEdit(ed, param);

            var edTarget;
            if (ed.target[0].tagName == "INPUT" && htmlInputType.indexOf(ed.type) > -1) {
                edTarget = ed.target;
                if (!ed.target[0].id)
                    ed.target[0].id = ed.type + "_" + (new Date().getTime());
            }
            else
                eval("edTarget = $(ed.target)." + ed.type + "('textbox')");

            if (!edTarget) return
            edTarget.focus()//获取焦点
            edTarget.select();//获取焦点
            //判断点击的元素的标位置 结束编辑
            //console.log("add click event %s", edTarget[0].id);
            var gridElement = $(this).parents(".datagrid");
            gridElement.on("mousedown." + edTarget[0].id, "", { cellInfo: param, targetId: edTarget[0].id, eventElement: gridElement }, function (e) {
                //return true 防止会阻止事件冒泡
                et = $(e.target);
                var data = e.data.cellInfo;
                var targetId = e.data.targetId;
                //是否是table内部的数据行
                var isIntable = et.is("table.datagrid-btable") || et.parents("table.datagrid-btable").is("table.datagrid-btable");
                //是否是同一列
                var isSameField = et.closest("td[field=" + data.field + "]").length > 0;
                if (isIntable) {
                    if (isSameField) {
                        //同一个单元格不会触发oncellclick 就不会结束编辑 所以要继续监听
                        //是否是同一个单元格
                        var index = et.parents("tr.datagrid-row-editing").attr("node-id");
                        if (index == data.id) return true;

                        //不是同一个单元格会触发oncellclick
                        //会在 oncellclick 中结束编辑 所以绑定的事件就没用了
                        // $("body").off("mousedown." + targetId);
                        // return true;
                    }
                }
                // 验证是否符合结束条件
                if (!$(dg).treegrid('validateRow', data.id)) return true;
                $(dg).treegrid('endCellEdit', data).treegrid('resetCellEditInfo');
                e.data.eventElement.off("mousedown." + targetId);
            });

            return $(dg);
        });
    },
    /* 结束单元格编辑
     * @param data {id = 行（索引），field=列名}
     */
    endCellEdit: function (jq, data) {
        var col = $(jq).treegrid('getEditor', data);
        if (col && col.target[0].classList.contains('combo-f')) {
            col.target.combo('hidePanel')
        }

        // 结束或取消编辑
        if ($(jq).treegrid('validateRow', data.id)) {
            $(jq).treegrid('endEdit', data.id);
            console.log("end edit %s", data.field);
        } else {
            $(jq).treegrid('cancelEdit', data.id);
            console.log("cancel edit %s", data.field);
        }

        return jq;
    },
    /* 结束单元格编辑
     */
    resetCellEditInfo: function (jq, param) {
        var opts = $(jq).treegrid('options');
        opts.editCellIndex = -1;
        opts.editCellField = undefined;
    }
});
//自定义验证
$.extend($.fn.validatebox.defaults.rules, {
    idCard: {// 验证身份证
        validator: function (value) {
            return /^\d{15}(\d{2}[A-Za-z0-9])?$/i.test(value);
        },
        message: '身份证号码格式不正确'
    },
    carNo: {// 验证车牌
        validator: function (vehicleNumber) {
            var result = false;
            if (vehicleNumber == "") {
                return true;
            }
            var xreg = /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}(([0-9]{5}[DF]$)|([DF][A-HJ-NP-Z0-9][0-9]{4}$))/;
            var creg = /^[京津沪渝冀豫云辽黑湘皖鲁新苏浙赣鄂桂甘晋蒙陕吉闽贵粤青藏川宁琼使领A-Z]{1}[A-Z]{1}[A-HJ-NP-Z0-9]{4}[A-HJ-NP-Z0-9挂学警港澳]{1}$/;

            if (vehicleNumber.length == 7) {
                result = creg.test(vehicleNumber);
            } else if (vehicleNumber.length == 8) {
                result = xreg.test(vehicleNumber);
            }
            return result;
        },
        message: '车牌号码格式不正确'
    },
    minLength: {//validType:"minLength[100]"
        validator: function (value, param) {
            return value.length >= param[0];
        },
        message: '请输入至少{0}个字符.'
    },
    maxLength: {//validType:"maxLength[100]"
        validator: function (value, param) {
            return value.length <= param[0];
        },
        message: '不能超过{0}个字符.'
    },
    length: { //validType="length[0,1000]"
        validator: function (value, param) {
            var len = $.trim(value).length;
            return len >= param[0] && len <= param[1];
        },
        message: "输入内容长度必须介于{0}和{1}之间."
    },
    phone: {// 验证电话号码
        validator: function (value) {
            return /^((\d2,3)|(\d{3}\-))?(0\d2,3|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '格式不正确,请使用下面格式:020-88888888'
    },
    mobile: {// 验证手机号码
        validator: function (value) {
            return /^[1][3-9][0-9]{9}$/i.test(value);
        },
        message: '手机号码格式不正确'
    },
    mobileOrPhone: {// 验证手机号码 后者电话号码
        validator: function (value) {
            //return /^1[3-8]+\d{9}$/i.test(value);
            return /^[1][3-9][0-9]{9}$/i.test(value) || /^((\d2,3)|(\d{3}\-))?(0\d2,3|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '手机或者座机号码格式不正确'
    },
    intOrFloat: {// 验证整数或小数
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '请输入数字，并确保格式正确'
    },
    currency: {// 验证货币
        validator: function (value) {
            return /^\d+(\.\d+)?$/i.test(value);
        },
        message: '货币格式不正确'
    },
    qq: {// 验证QQ,从10000开始
        validator: function (value) {
            return /^[1-9]\d{4,9}$/i.test(value);
        },
        message: 'QQ号码格式不正确'
    },
    integer: {// 验证整数 可正负数
        validator: function (value) {
            //return /^[+]?[1-9]+\d*$/i.test(value);
            return /^([+]?[0-9])|([-]?[0-9])+\d*$/i.test(value);
        },
        message: '请输入整数'
    },
    age: {// 验证年龄
        validator: function (value) {
            return /^(?:[1-9][0-9]?|1[01][0-9]|120)$/i.test(value);
        },
        message: '年龄必须是0到120之间的整数'
    },
    chinese: {// 验证中文
        validator: function (value) {
            return /^[\Α-\￥]+$/i.test(value);
        },
        message: '请输入中文'
    },
    english: {// 验证英语
        validator: function (value) {
            return /^[A-Za-z]+$/i.test(value);
        },
        message: '请输入英文'
    },
    unnormal: {// 验证是否包含空格和非法字符
        validator: function (value) {
            return /.+/i.test(value);
        },
        message: '输入值不能为空和包含其他非法字符'
    },
    faxno: {// 验证传真
        validator: function (value) {
            return /^((\d2,3)|(\d{3}\-))?(0\d2,3|0\d{2,3}-)?[1-9]\d{6,7}(\-\d{1,4})?$/i.test(value);
        },
        message: '传真号码不正确'
    },
    zip: {// 验证邮政编码
        validator: function (value) {
            return /^[1-9]\d{5}$/i.test(value);
        },
        message: '邮政编码格式不正确'
    },
    email: {// 验证邮箱
        validator: function (value) {
            return /^([0-9A-Za-z\-_\.]+)@([0-9a-z]+\.[a-z]{2,3}(\.[a-z]{2})?)/i.test(value);
        },
        message: '邮箱格式不正确'
    },
    ip: {// 验证IP地址
        validator: function (value) {
            return /((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))/i.test(value);
        },
        message: 'IP地址格式不正确'
    },
    name: {// 验证姓名，可以是中文或英文
        validator: function (value) {
            return /^[\Α-\￥]+$/i.test(value) | /^\w+[\w\s]+\w+$/i.test(value);
        },
        message: '请输入姓名'
    },
    date: {// 验证日期
        validator: function (value) {
            //格式yyyy-MM-dd或yyyy-M-d
            return /^(?:(?!0000)[0-9]{4}([-]?)(?:(?:0?[1-9]|1[0-2])\1(?:0?[1-9]|1[0-9]|2[0-8])|(?:0?[13-9]|1[0-2])\1(?:29|30)|(?:0?[13578]|1[02])\1(?:31))|(?:[0-9]{2}(?:0[48]|[2468][048]|[13579][26])|(?:0[48]|[2468][048]|[13579][26])00)([-]?)0?2\2(?:29))$/i.test(value);
        },
        message: '清输入合适的日期格式'
    },
    endTime: {
        validator: function (value, param) {
            return value >= $(param[0]).val();
        },
        message: '结束日期不能小于开始日期'
    },
    startTime: {
        validator: function (value, param) {
            return value <= $(param[0]).val();
        },
        message: '开始日期不能大于结束日期'
    },
    letterNum: {
        validator: function (value) {
            return /^[a-zA-Z][\d]+$/i.test(value);
        },
        message: '请输入字母加数字的组合，如：A10'
    },
    englishOrNum: {
        validator: function (value) {
            return /^[a-zA-Z\d]+$/i.test(value);
        },
        message: '请输入字母、数字'
    },
    categoryTitle: {
        validator: function (value) {
            return /^([a-z]|[0-9])-.+$/i.test(value);
        },
        message: '请输入正确的分类名称，如：A-成品'
    }
});

//-- easyui 控件增加扩展方法 ---
$.extend($.fn.tree.methods, {
    /**
     * 递归获取所有父节点
     * @param curNode 指定的结点(如果没有传入curNode，则取当前选中(getSelected)结点)
     */
    getParents: function (jq, curNode) {
        if (!curNode)
            curNode = jq.tree('getSelected');
        var arr = [];
        (function rec() {
            var pNode = jq.tree('getParent', curNode.target);
            if (pNode) {
                arr.push(curNode = pNode);
                rec();
            }
        })();
        return arr;
    },
    /**
     * 展示 tooltip 提示信息
     * @param option position=出现方向，content=消息内容, index=出现的位置（索引）
     */
    showTip: function (jq, option) {
        if (!option) option = {};
        var node = jq.find('.tree-node .tree-title').eq(option.index || 0);
        if (!option.content) {
            option.content = '请选择一个结点';
        }
        node.tooltip('error', option);
    },
    /**
     * 截取 tree 的 text 某一个字符并组成一个字符串
     * @param option lStart=起始层级，lEnd=截至层级，index=第几位字符，split=分隔符
     */
    getTextChar: function (jq, option) {
        var s = $(jq).tree('getSelected')
        if (!s) return [];
        var p = $(jq).tree('getParents', s)
        p.reverse();
        p.push(s);
        var p1 = p.map(i => i.text);
        var p2 = p1.slice(option.lStart || 0, option.lEnd || p1.length)
        var p3 = p2.map(i => i[option.index || 0])
        return p3.join(option.split || '')
    },
    /**
     * 获取树节点的层级（从 1 开始）
     * @param curNode 目标节点，如果不指定则默认为当前选中节点
     */
    getLevel: function (jq, curNode) {
        if (!curNode)
            curNode = $(jq).tree('getSelected')
        var parents = $(jq).tree('getParents', curNode);
        return parents.length + 1;
    },
    /** 判断是否是叶节点 */
    isLeafNode: function (jq, curNode) {
        if (!curNode)
            curNode = $(jq).tree('getSelected')
        let children = $(jq).tree('getChildren', curNode.target);
        return !(children.length > 0);
    },
    /** 判断是否是根节点 */
    isRootNode: function (jq, curNode) {
        if (!curNode)
            curNode = $(jq).tree('getSelected')
        let parent = $(jq).tree('getParent', curNode.target);
        return !parent;
    },
    unSelect: function (jq) {
        var selectedNode = $(jq).tree('getSelected');
        if (selectedNode) {
            $(jq).find('.tree-node-selected').removeClass('tree-node-selected');
        }
    }
});
$.extend($.fn.tooltip.methods, {
    /**
     * 验证错误时，提供手动提示功能（不适用于 EasyUI 控件）
     * @param option 基础设置参数
     */
    error: (jq, option) => {
        if (!option.onShow) {
            option.onShow = () => {
                jq.tooltip('tip')
                    .css({ backgroundColor: 'rgb(255,255,204)' })
                setTimeout(() => { jq.tooltip('hide') }, 1500);
            }
        }
        if (!option.onHide) {
            option.onHide = (e) => jq.tooltip('destroy')
        }
        jq.tooltip(option).tooltip('show');
    }
});
$.extend($.fn.combogrid.methods, {
    /** 获取选中项 */
    getSelected: function (jq) {
        return $(jq).combogrid('grid').datagrid('getSelected');
    },
    reload: function (jq) {
        return $(jq).combogrid('grid').datagrid('reload');
    }
})
$.extend($.fn.combotree.methods, {
    /** 获取选中项 */
    getSelected: function (jq) {
        return $(jq).combotree('tree').tree('getSelected');
    },
    /** 获取选中项的子节点 */
    getChildren: function (jq, curNode) {
        return $(jq).combotree('tree').tree('getChildren', curNode);
    },
    reload: function (jq) {
        return $(jq).combotree('tree').tree('reload');
    }
})
//--- easyui 控件增加扩展方法 ---

/*禁用表单内的控件*/
$.extend($.fn.form.methods, {
    disable: function (jq, isDisabled = true) {
        return jq.each(function () {
            var attr = "disable";
            if (!isDisabled) {
                attr = "enable";
            }
            //禁用jquery easyui中form控件
            var re = /(\w+)(?=-f)/;
            $(this).find("[class$='-f']").each((v, i) => {
                var tp = i.className.match(re)[0];
                if (tp && tp != "tooltip") {
                    var exps = `$("#${i.id}").${tp}("${attr}")`;
                    try {
                        eval(exps);
                    } catch (e) {
                        console.error(e);
                    }
                }
            })
        });
    },
    enable: function (jq, isEnable = true) {
        return jq.each(function () {
            $(this).form("disable", !isEnable);
        })
    },
    getJsonByFormData: function (jq) {
        var result = {};
        var fm = new FormData(jq[0]);
        var entriesObj = fm.entries()
        let loopEntrie = entriesObj.next();//={done:false, value:["k1", "v1"]}
        let loopValue = "";
        /** done 为 true 时 表示已经遍历完毕
         */
        while (!loopEntrie["done"]) {
            loopValue = loopEntrie["value"];
            loopEntrie = entriesObj.next();
            //console.log( + "=" + loopValue[1]);
            result[loopValue[0]] = loopValue[1];
        }
        return result;
    },
    getJson: function (jq) {
        var result = {};
        jq.find("input[name]").each(function (i) {
            result[this.name] = $(this).val();
        })
        return result;
    }
});