/** 上传 */
const UploadUtil = {
    /**
     * 注入 HTML 并渲染对话框及内容
     * @param url 处理地址
     * @param fn 返回值处理函数
     * @param supTypeArr 支持的文件类型数组,暂支持常见的
     * @param mul 是否支持多选
     */
    openDialog: (url, fn, supTypeArr, mul) => {
        const html = `
        <div id="dlg-upload">
            <div class="easyui-layout" data-options="fit: true">
                <div data-options="region:'north', border: false, height: 30">
                    <input id="file-upload">
                </div>
                <div data-options="region:'center', border: false">
                    <table id="pg-upload"></table>
                </div>
            </div>
        </div>
        `;

        console.log('UploadUtil-html-注入')
        // 注入 HTML
        $('body').append(html);
        // 重渲染（easyui-layout）
        $.parser.parse('#dlg-upload');

        $('#dlg-upload').dialog({
            title: '上传',
            width: 640,
            height: 480,
            modal: true,
            closable: false,
            maximizable: true,
            buttons: [
                {
                    text: '确定', width: 60, handler: () => {
                        UploadUtil.upload(url, fn);
                    }
                },
                { text: '取消', width: 60, handler: UploadUtil.destroyDialog }
            ]
        })

        let pgValueFormatter = function (value, row, index) {
            switch (row.name) {
                case '最后修改时间':
                    return new Date(value).toLocaleString();
                case '文件大小':
                    return `${(value / 1024).toFixed(2)}KB`;
                case '预览':
                    {
                        let reader = new FileReader();
                        if (/image\/\w+/.test(value.type)) {
                            // image 图片
                            reader.readAsDataURL(value)
                            reader.onload = (e) => {
                                $(`#imgPre-${index}`).prop('src', reader.result);
                                // 会造成一定程度的DOM崩坏，所以重新渲染
                                setTimeout(function () {
                                    $.parser.parse('#dlg-upload');
                                }, 100)
                            }
                            // 由于 FileReader 读取是异步的，所以先使用一个空白占位图（也可以改成加载中什么的？）
                            return `<img id="imgPre-${index}" alt="${value.name}" style="max-width:100%">`;
                        } else if (/text\/plain/.test(value.type)) {
                            // txt 文本
                            reader.readAsText(value);
                            reader.onload = (e) => {
                                $(`#txtPre-${index}`).html(reader.result);
                                // 会造成一定程度的DOM崩坏，所以重新渲染
                                setTimeout(function () {
                                    $.parser.parse('#dlg-upload');
                                }, 100)
                            }
                            let style = 'display:inline-block;width:100%;text-overflow:ellipsis;overflow:hidden;white-space:pre;max-height:100px;';
                            return `<span id="txtPre-${index}" title="${value.name}" style="${style}">`;
                        }
                    }
                default:
                    return value;
            }
        };
        $('#pg-upload').propertygrid({
            fit: true,
            showGroup: true,
            columns: [[
                { field: 'name', title: '属性', width: 100, sortable: true },
                { field: 'value', title: '值', width: 200, formatter: pgValueFormatter }
            ]]
        })//.propertygrid('getPanel').find('.datagrid-body,.datagrid-group').css({ 'background-color': '#fff' })//将背景色设置为白色

        let fbOnChange = function (nVal, oVal) {
            let files = $(this).filebox('files');
            if (files.length == 0) return;

            let rows = [];
            for (let index = 0; index < files.length; index++) {
                const file = files[index];
                // 将文件信息按照文件名分组
                rows.push({ name: '最后修改时间', value: file.lastModified, group: file.name })
                rows.push({ name: '文件大小', value: file.size, group: file.name })
                rows.push({ name: '文件类型', value: file.type, group: file.name })
                if (/image\/\w+|text\/plain/.test(file.type)) {
                    // 如果是图片或文本，则加上预览
                    rows.push({ name: '预览', value: file, group: file.name })
                }
                // rows.push({ name: 'webkitRelativePath', value: file.webkitRelativePath, group: file.name })
            }
            $('#pg-upload').propertygrid('loadData', rows);
        }
        function setAccept(arrs) {
            if (arrs) {
                var arr = [];
                for (var i = 0; i < arrs.length; i++) {
                    switch (arrs[i]) { //.toLowerCase()
                        case "csv":
                            arr.push("text/csv");
                            break;
                        case "doc":
                            arr.push("application/msword");
                            break;
                        case "wps":
                            arr.push("application/vnd.ms-works");
                            break;
                        case "xls":
                            arr.push("application/vnd.ms-excel");
                            break;
                        case "xlsx":
                            arr.push("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                            break;
                        case "zip":
                            arr.push("aplication/zip");
                        default:
                            break;
                    }
                }
                return arr.join(",");
            }
            else
                return undefined;
        }
        $('#file-upload').filebox({
            fit: true,
            multiple: mul | false,
            buttonText: '选择文件',
            prompt: '请选择要上传的文件',
            onChange: fbOnChange,
            accept: setAccept(supTypeArr),
        }).filebox('textbox').parent().css({ border: 'none' })//去除父容器的边框
    },
    /**
     * 上传文件
     * @param url 后台接口地址
     * @param fn 成功回调函数
     */
    upload: (url, fn) => {
        let files = $('#file-upload').filebox('files'),
            formData = new FormData();
        // 将（多个）文件压入 FormData
        for (let index = 0; index < files.length; index++) {
            const file = files[index];
            formData.append(`file${index}`, file);
        }

        $.ajax({
            url: url,
            type: 'post',
            data: formData,
            processData: false,
            contentType: false,
            cache: false,
            beforeSend: () => {
                $.messager.progress();
            }
        }).done(function (data, textStatus, jqXHR) {
            fn(data);
            UploadUtil.destroyDialog();
        }).always(() => {
            $.messager.progress('close');
        })
    },
    /** 销毁 */
    destroyDialog: () => {
        $('#dlg-upload').dialog('destroy');
    }
}
/** 映射 */
const MappedUtil = {
    /** 当前编辑行 */
    editRowIndex: NaN,
    /**
     * 注入 HTML 并渲染对话框及内容
     * @param obj 解析 Excel 后得到的映射数据列表及相关信息
     * @param url 后台接口地址
     * @param 成功回调函数
     */
    openDialog: (obj, url, fn) => {
        const html = `<div id="dlg-mapped"><table id="dg-mapped"></table></div>`;

        console.log('MappedUtil-html-注入')
        $('body').append(html);

        $('#dlg-mapped').dialog({
            title: '导入 Excel',
            width: 640,
            height: 480,
            modal: true,
            closable: false,
            buttons: [
                {
                    text: '确定', width: 60, handler: () => {
                        MappedUtil.mapped(obj.path, url, fn);
                    }
                },
                { text: '取消', width: 60, handler: MappedUtil.destroyDialog }
            ]
        })

        let dgOnSelect = function (index, row) {
            // 第一次编辑，或者是从无到有
            if (MappedUtil.editRowIndex == NaN) {
                $(this).datagrid('beginEdit', index)
            }
            // 在已经编辑的情况下编辑另一行
            if (MappedUtil.editRowIndex != index) {
                $(this).datagrid('endEdit', MappedUtil.editRowIndex)
                $(this).datagrid('beginEdit', index)
            }
            MappedUtil.editRowIndex = index;
        }

        $('#dg-mapped').datagrid({
            fit: true,
            border: false,
            fitColumns: true,
            data: obj.maps,
            singleSelect: true,
            rownumbers: true,
            striped: true,
            remoteSort: false,
            onSelect: dgOnSelect,
            onLoadSuccess: function (data) {
                console.log('MappedUtil-datagrid-onLoadSuccess-开始')
                let rows = $(this).datagrid('getRows');
                for (let index = 0; index < rows.length; index++) {
                    const row = rows[index];
                    let source = obj.header.find(i => i == row.target);
                    if (source) {
                        $(this).datagrid('updateRow', {
                            index: index,
                            row: { source: source }
                        });
                    }
                }
                console.log('MappedUtil-datagrid-onLoadSuccess-结束')
            },
            columns: [[
                {
                    field: 'source', title: '源字段', width: 150, align: 'center', sortable: true,
                    editor: {
                        type: 'combobox',
                        options: {
                            editable: false,
                            data: obj.header.map(d => { return { value: d, text: d } })
                        }
                    }
                },
                { field: 'target', title: '目标字段', width: 150, align: 'center' },
                {
                    field: 'isRequired', title: '必须', align: 'center', sortable: true,
                    formatter: function (value, row, index) {
                        return value
                            ? '<input type="checkbox" disabled checked>'
                            : '<input type="checkbox" disabled>'
                    }
                },
                { field: 'defaultValues', hidden: true },// 隐藏的数据源
                {
                    field: 'defaultValue', title: '默认值', width: 100, align: 'center',
                    editor: {
                        type: 'combobox',
                        options: {
                            editable: false,
                            onShowPanel: function () {
                                let s = $('#dg-mapped').datagrid('getSelected')
                                let source = s.defaultValues.map(d => { return { value: d, text: d } })
                                $(this).combobox({ data: source })
                            }
                        }
                    }
                },
            ]]
        }).datagrid('getPanel').click((e) => {
            var hasEditor = $(e.target).hasClass('datagrid-editable')
                || $(e.target).children('div.datagrid-editable').length > 0;

            if (!hasEditor && MappedUtil.editRowIndex != NaN) {
                $('#dg-mapped').datagrid('endEdit', MappedUtil.editRowIndex);
                MappedUtil.editRowIndex = NaN;
            }
        })
    },
    /**
     * 提交映射信息（并完成导入）
     * @param url 后台接口地址
     * @param fn 成功回调函数
     * @param path 之前上传的临时文件地址
     */
    mapped: (path, url, fn) => {
        let $dgMapped = $('#dg-mapped');
        // 结束编辑
        if (MappedUtil.editRowIndex != NaN)
            $dgMapped.datagrid('endEdit', MappedUtil.editRowIndex);
        // 获取数据
        let rows = $dgMapped.datagrid('getRows');
        // 交互
        $.messager.progress();
        $.postbody(url, { path: path, maps: rows }).done(function (data, textStatus, jqXHR) {
            fn(data);
        }).always(() => {
            $.messager.progress('close');
        });
    },
    /** 销毁 */
    destroyDialog: () => {
        $('#dg-mapped').datagrid('getPanel').unbind('click');
        $('#dlg-mapped').dialog('destroy')
    }
}
/** 导出 */
const ExportUtil = {
    /** 请求的地址(action) */
    _url: undefined,
    /** 需要从哪个控件获取列信息 */
    _selector: undefined,
    /** 额外的查询参数 */
    _queryParams: undefined,
    /** 使用何种方法请求 */
    _method: undefined,
    openDialog: function (selector, url, method, queryParams) {
        this._selector = selector;
        this._url = url;
        this._queryParams = queryParams;
        this._method = method || 'post';

        var html = `
            <div id="dlgExport">
                <div class="easyui-layout" data-options="fit:true">
                    <div data-options="region:'center',border:false">
                        <table id="dgColSetting"></table>
                    </div>
                    <div data-options="region:'south',border:false" style="height:170px;">
                        <form id="formExport" style="padding:5px" method="POST">
                            <ul class="list-unstyled">
                                <li style="margin-top:5px">
                                    <span>文&ensp;件&ensp;名:</span>
                                    <input name="fileName" class="easyui-textbox" required data-options="prompt:'请输入文件名'" style="width:310px">
                                </li>
                                <li style="margin-top:5px">
                                    <span>保存类型:</span>
                                    <select name="fileType" class="easyui-combobox" required data-options="editable:false" style="width:310px">
                                        <option value="xls">Microsoft Excel 97-2003 文件(*.xls)</option>
                                        <option value="xlsx">Microsoft Excel 文件(*.xlsx)</option>
                                        <option value="csv">CSV (逗号分隔)(*.csv)</option>
                                    </select>
                                </li>
<li><div><input type="radio" name="datatype" id="all" class="ct" checked="checked">全部数据（最多导出40000条数据）</div>
    <li><input type="radio" name="datatype" id="part"class="ct" >选择数据量导出（最多导出40000条数据）</li>
    <li>从<input type="number" style="width:100px" id="start" disabled="disabled" class="easyui-number"  name="StartRows">条起，共导出<input type="number" class="easyui-number"  style="width:100px" id="end" disabled="disabled" name="EndRows" >条数据 </li>

</div>
</li>
                            </ul>
                        </form>
                    </div>
                </div>
                <div class="dlg-export-btns">
                    <button class="btn btn-success" onclick="ExportUtil.export()"><i class="fa fa-check"></i>导出</button>
                    <button class="btn btn-primary" onclick="ExportUtil.destroyDialog()"><i class="fa fa-close"></i>关闭</button>
                </div>
            </div>
        `;
        $(html).appendTo('body');
        $('#start').numberbox({
            min: 0,
            max: 400000
        });
        $('#end').numberbox({
            min: 0,
            max: 400000
        });
        $.parser.parse('#dlgExport');
        $('#dlgExport').dialog({
            title: '导出文件',
            width: 400,
            height: 600,
            buttons: '.dlg-export-btns',
            modal: true
        })
        // 获取 selector 上的列信息
        var opt = $(selector).datagrid('options');
        var arr = [];
        if (opt.frozenColumns.length)
            arr = arr.concat(opt.frozenColumns[0]);
        if (opt.columns.length)
            arr = arr.concat(opt.columns[0]);
        var data = arr.filter(function (item) {
            return !item.hidden
        }).map(function (item) {
            return { field: item.field, title: item.title }
        });
        $('.ct').click(function () {
            if ($('#all').prop('checked') == true) {
                $('#start').numberbox("disable");
                $('#end').numberbox("disable");
                $('#start').numberbox("setValue", "")
                $('#end').numberbox("setValue", "")
            }
            else {
                $('#start').numberbox("enable");
                $('#end').numberbox("enable");
            }
        })

        // 初始化导出列信息
        $('#dgColSetting').datagrid({
            fit: true,
            fitColumns: true,
            columns: [[
                { field: 'check', checkbox: true },
                { field: 'field', title: '字段', align: 'center', width: 200, hidden: true },
                { field: 'title', title: '列名称（拖动以排序）', align: 'center', width: 200 }
            ]],
            onLoadSuccess: function () {
                $('#dgColSetting').datagrid('enableDnd');
            }
        }).datagrid('loadData', data).datagrid('checkAll');
        // 设定文档名称+当前时间为默认文件名
        $('#formExport').form('load', {
            fileName: `[${document.title}]${new Date().format('yyyyMMddhhmmss')}`
        })
    },
    export: function () {
        var isValid = $('#formExport').form('validate');
        if (!isValid) return;
        var formData = $.serializeObject('#formExport');
        formData.columns = $('#dgColSetting').datagrid('getChecked');
        // 合并额外的查询参数
        debugger;
        if (typeof this._queryParams == 'function') {
            this._queryParams = this._queryParams();
        }
        var data = $.extend(formData, this._queryParams);
        $.downloadAsync(this._method, this._url, data, 'application/json', data.fileName + '.' + data.fileType, function () {
            $('#dlgExport').dialog('destroy')
        });
    },
    destroyDialog: function () {
        $('#dlgExport').dialog('destroy')
    }
}
/** 常规导出 */
const ExportUtilNormal = {
    /** 请求的地址(action) */
    _url: undefined,
    /** 需要从哪个控件获取列信息 */
    _selector: undefined,
    /** 额外的查询参数 */
    _queryParams: undefined,
    /** 使用何种方法请求 */
    _method: undefined,
    _columns: undefined,//表的列数据
    openDialog: function (selector, url, method, queryParams) {
        this._selector = selector;
        this._url = url;
        this._queryParams = queryParams;
        this._method = method || 'post';

        var html = `
            <div id="dlgExport">
                <div class="easyui-layout" data-options="fit:true">
                    <div data-options="region:'center',border:false" style="height:170px;">
                        <form id="formExport" style="padding:5px" method="POST">
                            <ul class="list-unstyled">
                                <li style="margin-top:5px">
                                    <span>文&ensp;件&ensp;名:</span>
                                    <input name="fileName" class="easyui-textbox" required data-options="prompt:'请输入文件名'" style="width:310px">
                                </li>
                                <li style="margin-top:5px">
                                    <span>保存类型:</span>
                                    <select name="fileType" class="easyui-combobox" required data-options="editable:false" style="width:310px">
                                        <option value="xlsx">Microsoft Excel 文件(*.xlsx)</option>
                                        <option value="xls">Microsoft Excel 97-2003 文件(*.xls)</option>
                                    </select>
                                </li>
                            </ul>
                        </form>
                    </div>
                </div>
                <div class="dlg-export-btns">
                    <button class ="btn btn-success" onclick="ExportUtilNormal.export()"><i class ="fa fa-check"></i>导出</button>
                    <button class ="btn btn-primary" onclick="ExportUtilNormal.destroyDialog()"><i class ="fa fa-close"></i>关闭</button>
                </div>
            </div>
        `;
        $(html).appendTo('body');
        $.parser.parse('#dlgExport');
        $('#dlgExport').dialog({
            title: '导出文件',
            width: 400,
            height: 180,
            buttons: '.dlg-export-btns',
            modal: true
        })
        // 设定文档名称+当前时间为默认文件名
        $('#formExport').form('load', {
            fileName: `[${document.title}]${new Date().format('yyyyMMddhhmmss')}`
        })
        // 获取 selector 上的列信息 
        var opt = $(selector).datagrid('options');
        var arr = [];
        if (opt.frozenColumns.length)
            arr = arr.concat(opt.frozenColumns[0]);
        if (opt.columns.length)
            arr = arr.concat(opt.columns[0]);
        _columns = arr.filter(function (item) {
            return !item.hidden
        }).map(function (item) {
            return { field: item.field, title: item.title }
        });
    },
    export: function () {
        var isValid = $('#formExport').form('validate');
        if (!isValid) return;
        var formData = $.serializeObject('#formExport');
        formData.columns = _columns;
        // 合并额外的查询参数
        if (typeof this._queryParams == 'function') {
            this._queryParams = this._queryParams();
        }
        var data = $.extend(formData, this._queryParams);
        $.downloadAsync(this._method, this._url, data, 'application/json', data.fileName + '.' + data.fileType, function () {
            $('#dlgExport').dialog('destroy')
        });
    },
    destroyDialog: function () {
        $('#dlgExport').dialog('destroy')
    }
}