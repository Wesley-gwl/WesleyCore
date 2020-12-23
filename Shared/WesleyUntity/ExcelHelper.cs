using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WesleyUntity
{
    public static class ExcelHelper
    {
        #region 导出excel三种模式

        /// <summary>
        /// 导出数据集合到CSV流
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="titleNames">列头</param>
        /// <param name="propertyNames">属性</param>
        /// <param name="list">数据集合</param>
        /// <returns></returns>
        public static MemoryStream ListToCSV<T>(string[] titleNames, string[] propertyNames, List<T> list)
        {
            List<string> rows = new List<string> {
                string.Join(",", titleNames)
            };
            var properties = typeof(T).GetProperties();
            list.ForEach(p =>
            {
                var row = new List<string>();
                foreach (var propName in propertyNames)
                {
                    var prop = properties.FirstOrDefault(_prop => _prop.Name.ToLower() == propName.ToLower());
                    if (!prop.IsNullEmpty())
                    {
                        var val = prop.GetValue(p, null);
                        row.Add(val.ToStringOrEmpty());
                    }
                }
                rows.Add(string.Join(",", row));
            });
            var result = string.Join("\n", rows);
            return new MemoryStream(Encoding.UTF8.GetBytes(result));
        }

        /// <summary>
        /// 导出数据集合到Excel流（HSSF）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="titleNames">列头</param>
        /// <param name="propertyNames">属性</param>
        /// <param name="list">数据集合</param>
        /// <returns></returns>
        public static MemoryStream ListToXLS<T>(string[] titleNames, string[] propertyNames, List<T> list)
        {
            var workbook = new HSSFWorkbook();//HSSF
            var sheet = workbook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            //设置日期单元格样式
            ICellStyle csDateTime = workbook.CreateCellStyle();
            ICellStyle csDateTime2 = workbook.CreateCellStyle();
            IDataFormat dfDateTime = workbook.CreateDataFormat();
            csDateTime.DataFormat = dfDateTime.GetFormat("yyyy-mm-dd hh:mm:ss");
            csDateTime2.DataFormat = dfDateTime.GetFormat("yyyy-mm-dd");

            for (int colNum = 0; colNum < titleNames.Length; colNum++)
            {
                headerRow.CreateCell(colNum).SetCellValue(titleNames[colNum]);
            }

            var type = list[0].GetType();
            var properties = type.GetProperties();
            for (int rowNum = 0; rowNum < list.Count; rowNum++)
            {
                IRow row = sheet.CreateRow(rowNum + 1);
                for (int colNum = 0; colNum < propertyNames.Length; colNum++)
                {
                    var propName = propertyNames[colNum];
                    var prop = properties.FirstOrDefault(p => p.Name.ToLower() == propName.ToLower());

                    ICell cell = row.CreateCell(colNum);
                    var colNumValue = prop?.GetValue(list[rowNum], null);
                    if (colNumValue == null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    else if (colNumValue.GetType() == typeof(DateTime))
                    {
                        var dateTemp = colNumValue as DateTime?;
                        if (dateTemp.HasValue && dateTemp.Value.Hour == 0 && dateTemp.Value.Minute == 0 && dateTemp.Value.Second == 0)
                        {
                            //设置单元格宽度
                            sheet.SetColumnWidth(colNum, 10 * 256);

                            cell.SetCellValue((DateTime)dateTemp);
                            cell.CellStyle = csDateTime2;
                        }
                        else
                        {
                            //设置单元格宽度
                            sheet.SetColumnWidth(colNum, 20 * 256);

                            cell.SetCellValue((DateTime)colNumValue);
                            cell.CellStyle = csDateTime;
                        }
                    }
                    else if (colNumValue.GetType() == typeof(decimal))
                    {
                        cell.SetCellValue(Convert.ToDouble(colNumValue.ToStringOrEmpty()));
                        cell.SetCellType(CellType.Numeric);
                    }
                    else
                    {
                        cell.SetCellValue(colNumValue.ToStringOrEmpty());
                    }
                }
            }
            var ms = new NPOIMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }

        /// <summary>
        /// 导出数据集合到Excel流（XSSF）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="titleNames">列头</param>
        /// <param name="propertyNames">属性</param>
        /// <param name="list">数据集合</param>
        /// <returns></returns>
        public static MemoryStream ListToXLSX<T>(string[] titleNames, string[] propertyNames, List<T> list)
        {
            var workbook = new XSSFWorkbook();//XSSF
            var sheet = workbook.CreateSheet();
            var headerRow = sheet.CreateRow(0);
            //设置日期单元格样式
            ICellStyle csDateTime = workbook.CreateCellStyle();
            ICellStyle csDateTime2 = workbook.CreateCellStyle();
            IDataFormat dfDateTime = workbook.CreateDataFormat();
            csDateTime.DataFormat = dfDateTime.GetFormat("yyyy-mm-dd hh:mm:ss");
            csDateTime2.DataFormat = dfDateTime.GetFormat("yyyy-mm-dd");

            for (int colNum = 0; colNum < titleNames.Length; colNum++)
            {
                headerRow.CreateCell(colNum).SetCellValue(titleNames[colNum]);
            }

            var type = list[0].GetType();
            var properties = type.GetProperties();
            for (int rowNum = 0; rowNum < list.Count; rowNum++)
            {
                IRow row = sheet.CreateRow(rowNum + 1);
                for (int colNum = 0; colNum < propertyNames.Length; colNum++)
                {
                    var propName = propertyNames[colNum];
                    var prop = properties.FirstOrDefault(p => p.Name.ToLower() == propName.ToLower());

                    ICell cell = row.CreateCell(colNum);
                    var colNumValue = prop?.GetValue(list[rowNum], null);
                    if (colNumValue == null)
                    {
                        cell.SetCellValue(string.Empty);
                    }
                    else if (colNumValue.GetType() == typeof(DateTime))
                    {
                        var dateTemp = colNumValue as DateTime?;
                        if (dateTemp.HasValue && dateTemp.Value.Hour == 0 && dateTemp.Value.Minute == 0 && dateTemp.Value.Second == 0)
                        {
                            //设置单元格宽度
                            sheet.SetColumnWidth(colNum, 10 * 256);

                            cell.SetCellValue((DateTime)dateTemp);
                            cell.CellStyle = csDateTime2;
                        }
                        else
                        {
                            //设置单元格宽度
                            sheet.SetColumnWidth(colNum, 20 * 256);

                            cell.SetCellValue((DateTime)colNumValue);
                            cell.CellStyle = csDateTime;
                        }
                    }
                    else if (colNumValue.GetType() == typeof(decimal))
                    {
                        cell.SetCellValue(Convert.ToDouble(colNumValue.ToStringOrEmpty()));
                        cell.SetCellType(CellType.Numeric);
                    }
                    else
                    {
                        cell.SetCellValue(colNumValue.ToStringOrEmpty());
                    }
                }
            }
            var ms = new NPOIMemoryStream
            {
                AllowClose = false
            };
            workbook.Write(ms);
            ms.Flush();
            ms.Seek(0, SeekOrigin.Begin);
            ms.AllowClose = true;
            return ms;
        }

        #endregion 导出excel三种模式

        #region 生成模板

        /// <summary>
        /// 添加表头和验证信息
        /// </summary>
        /// <param name="sheet"></param>
        /// <param name="rowlist"></param>
        public static void AddXSSFValidation(this ISheet sheet, List<ExcleTempRow> rowlist)
        {
            var newStyle = sheet.Workbook.CreateCellStyle();
            IFont font = sheet.Workbook.CreateFont();
            font.Color = HSSFColor.Red.Index;
            newStyle.SetFont(font);

            IRow row = sheet.CreateRow(0);
            var helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
            var rs = rowlist.OrderBy(r => r.Rank);
            var rowIndex = 0;
            foreach (var cell in rs)
            {
                if (cell.Require)
                    row.CreateCell(rowIndex, cell.Title, newStyle);
                else
                    row.CreateCell(rowIndex).SetCellValue(cell.Title);

                if (cell.ValidationData != null && cell.ValidationData.Length > 0)
                {
                    if (cell.ValidationData.Aggregate(0, (s, i) => s += i.Length + 1) < 255)
                    {
                        sheet.AddValidationData(helper.CreateDataValidation(rowIndex, rowIndex, cell.ValidationData));//添加进去
                    }
                    else
                    {
                        sheet.AddValidationData(helper.CreateDataValidationFormulaList(rowIndex, cell.ValidationData, sheet));//添加进去
                    }
                }
                rowIndex++;
            }

            sheet.ForceFormulaRecalculation = true;
        }

        public static IDataValidation CreateDataValidationFormulaList(this XSSFDataValidationHelper helper, int firstCol, string[] data, ISheet sheet)
        {
            //先创建一个Sheet专门用于存储下拉项的值，并将各下拉项的值写入其中：
            ISheet sheet2 = sheet.Workbook.CreateSheet("ShtDictionary");
            for (int i = 0; i < data.Length; i++)
            {
                sheet2.CreateRow(i).CreateCell(0).SetCellValue(data[i]);
            }

            //然后定义一个名称，指向刚才创建的下拉项的区域：
            IName range = sheet2.Workbook.CreateName();
            range.RefersToFormula = "ShtDictionary!$A$1:$A$" + data.Length;
            range.NameName = "dicRange";
            //最后，设置数据约束时指向这个名称而不是字符数组：

            var regions = new CellRangeAddressList(1, 65535, firstCol, firstCol);
            IDataValidation dataValidate = helper.CreateValidation(helper.CreateFormulaListConstraint("dicRange"), regions);
            return dataValidate;
        }

        public static IDataValidation CreateDataValidation(this XSSFDataValidationHelper helper, int firstCol, int lastCol, string[] data)
        {
            //下拉项总字符长度大于255时需要特殊处理

            CellRangeAddressList regions = new CellRangeAddressList(1, 65535, firstCol, lastCol);//约束范围：c2到c65535
                                                                                                 //XSSFDataValidationHelper helper = new XSSFDataValidationHelper((XSSFSheet)sheet);//获得一个数据验证Helper
            IDataValidation validation = helper.CreateValidation(helper.CreateExplicitListConstraint(data), regions);//创建约束
            validation.CreateErrorBox("错误", "请按右侧下拉箭头选择!");//不符合约束时的提示
            validation.ShowErrorBox = true;//显示上面提示 = True

            return validation;
        }

        public static ICell CreateCell(this IRow row, int colnum, string value, ICellStyle cellStyle)
        {
            var cell = row.CreateCell(colnum);
            cell.CellStyle = cellStyle;
            cell.SetCellValue(value);
            return cell;
        }

        /// <summary>
        /// 创建带验证信息的 xlsx workbook
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="sheetTitle"></param>
        /// <returns></returns>
        public static IWorkbook CreateXSSFWorkbookHasValidation(List<ExcleTempRow> rows, string sheetTitle)
        {
            IWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet(sheetTitle);
            sheet.AddXSSFValidation(rows);
            return workbook;
        }

        /// <summary>
        /// 创建带验证信息的xlsx 流
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="sheetTitle"></param>
        /// <returns></returns>
        public static NPOIMemoryStream CreateXSSFHasValidationStream(List<ExcleTempRow> rows, string sheetTitle)
        {
            var memory = new NPOIMemoryStream();
            var workbook = CreateXSSFWorkbookHasValidation(rows, sheetTitle);
            memory.AllowClose = false;
            workbook.Write(memory);
            memory.Flush();
            memory.Position = 0;    // 指定内存流起始值
            return memory;
        }

        #endregion 生成模板

        #region 读取excel

        /// <summary>
        ///  读取excel列头
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<string> ExcelHeaderToList(string filePath)
        {
            List<string> listStr = new List<string>
            {
                string.Empty
            };
            try
            {
                var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                IWorkbook workbook = null;
                string fileExtend = Path.GetExtension(filePath);
                if (fileExtend.ToLower() == ".xlsx")
                    workbook = new XSSFWorkbook(fs);
                else
                    workbook = new HSSFWorkbook(fs);
                ISheet sheet = workbook.GetSheetAt(0);
                fs.Close();
                if (sheet != null)
                {
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum;
                    for (int i = firstRow.FirstCellNum; i < cellCount; ++i)
                    {
                        listStr.Add(firstRow.GetCell(i).StringCellValue);
                    }
                }
            }
            catch
            {
                return listStr;
            }
            return listStr;
        }

        #endregion 读取excel

        #region 转换

        /// <summary>
        /// Excel文件转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">文件路径</param>
        /// <param name="_keyValues">Key为字段中文名称，Value为字段的数据库名称</param>
        /// <param name="menuStart">数据开始行数，默认为0</param>
        /// <returns></returns>
        public static List<T> ExcelToList<T>(string filePath, Dictionary<string, string> _keyValues, int menuStart = 0) where T : new()
        {
            List<T> list = new List<T>();
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                ISheet sheet = null;
                try
                {
                    XSSFWorkbook workbook = new XSSFWorkbook(fs);
                    sheet = workbook.GetSheetAt(0);
                }
                catch
                {
                    FileStream fs2 = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    HSSFWorkbook workbook = new HSSFWorkbook(fs2);
                    sheet = workbook.GetSheetAt(0);
                }

                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum;
                    IRow firstRow = sheet.GetRow(0);
                    int cellCount = firstRow.LastCellNum;

                    for (int i = 1; i <= rowCount; i++)
                    {
                        T item = new T();
                        //遍历每一行数据
                        for (int k = 0; k <= cellCount; k++)
                        {
                            //单元格数据输入item
                            ICell cell = sheet.GetRow(i).GetCell(k);
                            object cellValue = null;
                            if (cell == null) continue;

                            switch (cell.CellType)
                            {
                                case CellType.String: //文本
                                    cellValue = cell.StringCellValue;
                                    break;

                                case CellType.Numeric: //数值
                                    cellValue = Convert.ToInt32(cell.NumericCellValue);//Double转换为int
                                    break;

                                case CellType.Boolean: //bool
                                    cellValue = cell.BooleanCellValue;
                                    break;

                                case CellType.Blank: //空白
                                    cellValue = "";
                                    break;

                                case CellType.Unknown:
                                    break;

                                case CellType.Formula:
                                    break;

                                case CellType.Error:
                                    break;

                                default:
                                    cellValue = "ERROR";
                                    break;
                            }

                            try
                            {
                                var _key = _keyValues[sheet.GetRow(menuStart).GetCell(k).ToString()];
                                //时间格式处理
                                if (typeof(T).GetProperty(_key).ToString().IndexOf("String") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToString(cellValue), null);
                                else if (typeof(T).GetProperty(_key).ToString().IndexOf("DateTime") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToDateTime(cellValue), null);
                                else if (typeof(T).GetProperty(_key).ToString().IndexOf("Decimal") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToDecimal(cellValue), null);
                                else if (typeof(T).GetProperty(_key).ToString().IndexOf("Boolean") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToBoolean(cellValue), null);
                                else if (typeof(T).GetProperty(_key).ToString().IndexOf("Double") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToDouble(cellValue), null);
                                else if (typeof(T).GetProperty(_key).ToString().IndexOf("Int") > -1)
                                    typeof(T).GetProperty(_key).SetValue(item, Convert.ToInt32(cellValue), null);
                                else
                                    typeof(T).GetProperty(_key).SetValue(item, cellValue, null);
                            }
                            catch { }
                        }

                        list.Add(item);
                    }
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public static void CreateErrorExcel(string source, string target, List<int> lines)
        {
            IWorkbook workbook;
            var ext = Path.GetExtension(source);
            using (FileStream fs = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                if (ext == ".xlsx")
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else
                {
                    workbook = new HSSFWorkbook(fs);
                }
            }
            var sheet = workbook.GetSheetAt(0);
            if (sheet == null)
                throw new Exception(string.Format("地址为{0}的Excel文件不含有Sheet页", source));

            ICellStyle styleRed = workbook.CreateCellStyle();
            styleRed.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Red.Index;
            styleRed.FillPattern = FillPattern.SolidForeground;

            ICellStyle styleNormal = workbook.CreateCellStyle();
            styleNormal.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.COLOR_NORMAL;
            styleNormal.FillPattern = FillPattern.SolidForeground;

            var cellNum = sheet.GetRow(0).LastCellNum;
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                for (int j = 0; j < cellNum; j++)
                {
                    var cell = row.GetCell(j);
                    if (cell == null)//确保单元格存在
                        cell = row.CreateCell(j);
                    if (lines.Contains(i - 1))
                        cell.CellStyle = styleRed;
                    else //非错误行重置颜色
                        cell.CellStyle = styleNormal;
                }
            }

            using (FileStream fs = new FileStream(target, FileMode.OpenOrCreate, FileAccess.Write))
            {
                workbook.Write(fs);
            }
        }

        #endregion 转换
    }
}