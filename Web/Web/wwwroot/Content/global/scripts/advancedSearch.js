// 2017.11.9    add by YZ
var advancedSearch = {
    //打开对话框  arr = 属性为id,text的字段数组; funcName = 回调函数的名称字符串''
    openDialog: function (arr, funcName) {
        var dlg = $('#dlg_advsearch');
        if (!dlg[0]) //初始化对话框
        {
            var html = '<div id="dlg_advsearch" style="width:600px;height:400px;">' +
                '<form id= "form_advsearch" style= "width:100%; height:100%;" >' +
                '<table id="dg_advsearch" style="width:100%; height:100%;"></table>' +
                '<div id="btn_advsearch">' +
                //'<font size="2" color="red">提示:时间区间查询格式为 2000.1.1/2000.12.31</font>  '+
                '<a href="javascript:;" class="btn btn-primary" onclick="advancedSearch.addprop()"><i class="fa fa-plus-circle"></i>添加</a> ' +
                '<a href="javascript:;" class="btn btn-primary" onclick="advancedSearch.clearprop()"><i class="fa fa-trash"></i>清空</a> ' +
                '<a href="javascript:;" class="btn btn-primary" onclick="advancedSearch.doSearch(' + funcName + ')"><i class="fa fa-check"></i>确定</a> ' +
                '<a href="javascript:;" class="btn btn-primary" onclick="advancedSearch.closeDialog()"><i class="fa fa-ban"></i>取消</a> ' +
                '</div>' +
                '</form>' +
                '</div>';
            $('body>div:last').after(html);
            $('#dlg_advsearch').dialog({
                title: '高级检索',
                closed: true,
                closable: false,
                cache: false,
                modal: true,
                buttons: '#btn_advsearch'
            });
            var conditionDic = [
                { id: 'contains', text: '包含' },
                { id: '==', text: '等于' },
                { id: '!=', text: '不等于' },
                { id: '>', text: '大于' },
                { id: '>=', text: '大于等于' },
                { id: '<', text: '小于' },
                { id: '<=', text: '小于等于' }
            ];
            $('#dg_advsearch').datagrid({
                noheader: true,
                rownumbers: true,
                fit: true,
                fitColumns: true,
                nowrap: true,
                striped: true,
                singleSelect: true,
                autoRowHeight: false,
                //toolbar:'#tb_advsearch',
                columns: [[
                    {
                        field: 'relation', title: '关系', width: 150, align: 'center',
                        editor: {
                            type: 'combobox', options:
                            {
                                data: [{ 'id': 'AND', 'text': '并且' }, { 'id': 'OR', 'text': '或者' }],
                                valueField: 'id',
                                textField: 'text',
                                editable: false
                            }
                        }
                    },
                    {
                        field: 'property', title: '字段名', width: 280, align: 'center',
                        editor: {
                            type: 'combobox', options:
                            {
                                data: arr,
                                valueField: 'id',
                                textField: 'text',
                                editable: false,
                                required: true
                            }
                        }
                    },
                    {
                        field: 'condition', title: '条件', width: 220, align: 'center',
                        editor: {
                            type: 'combobox', options:
                            {
                                data: conditionDic,
                                valueField: 'id',
                                textField: 'text',
                                editable: false,
                                required: true
                            }
                        }
                    },
                    {
                        field: 'searchvalue', title: '值', width: 400, align: 'center',
                        editor: { type: 'textbox', options: { required: true } }
                    },
                    
                    {
                        field: 'rmv', title: '移除', width: 150, align: 'center'
                    }
                ]]
            });
        }
        //if ($('#dg_advsearch').datagrid('getRows').length == 0)
        //    advancedSearch.addprop();
        $('#dlg_advsearch').dialog('open');
    },

    addprop: function () {
        $('#dg_advsearch').datagrid('appendRow', {
            property: '', condition: '', searchvalue: '', relation: '',
            rmv: '<a href="javascript:;"  onclick="advancedSearch.delprop(this)"><i class="fa fa-minus-circle" style="color:#e7505a"></i></a>'
        });
        var editIndex = $('#dg_advsearch').datagrid('getRows').length - 1;
        $('#dg_advsearch').datagrid('beginEdit', editIndex);
        var edt_c = $('#dg_advsearch').datagrid('getEditor', { index: editIndex, field: 'condition' });
        edt_c.target.combobox('setValue', 'contains');
        //var edt_r = $('#dg_advsearch').datagrid('getEditor', { index: editIndex, field: 'relation' });
        //edt_r.target.combobox('setValue', 'AND');
    },

    delprop: function (el) {
        $(el.parentNode).click();  //10000匹草泥马
        var row = $('#dg_advsearch').datagrid('getSelected');
        var rowIndex = $('#dg_advsearch').datagrid('getRowIndex', row);
        $('#dg_advsearch').datagrid('deleteRow', rowIndex);
    },

    clearprop: function () {
        $('#dg_advsearch').datagrid('loadData', []);
    },

    doSearch: function (func) {
        if ($('#form_advsearch').form('validate')) {
            var rows = $('#dg_advsearch').datagrid('getRows');
            var json = [];
            for (var i = 0; i < rows.length; i++) {
                var rowIndex = $('#dg_advsearch').datagrid('getRowIndex', rows[i]);
                var edt0 = $('#dg_advsearch').datagrid('getEditor', { index: rowIndex, field: 'relation' });
                var value0 = edt0.target.combobox('getValue');
                var edt1 = $('#dg_advsearch').datagrid('getEditor', { index: rowIndex, field: 'property' });
                var value1 = edt1.target.combobox('getValue');
                var edt2 = $('#dg_advsearch').datagrid('getEditor', { index: rowIndex, field: 'condition' });
                var value2 = edt2.target.combobox('getValue');
                var edt3 = $('#dg_advsearch').datagrid('getEditor', { index: rowIndex, field: 'searchvalue' });
                var value3 = edt3.target.textbox('getText');

                var obj = {};
                obj.Relation = value0;
                obj.Property = value1;
                obj.Condition = value2;
                obj.SearchValue = value3;

                json.push(obj);
            }
            var jsonstr = JSON.stringify(json);
            $('#dlg_advsearch').dialog('close');
            func(jsonstr);
            //eval('window.' + func).call(this, JSON.stringify(json));  
        }
    },

    closeDialog: function () {
        $('#dlg_advsearch').dialog('close');
    }
}