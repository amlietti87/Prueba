using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace TECSO.FWK.DocumentsHelper.Excel
{
    public class ExcelHelper
    {
        internal const string defaulFormatDate = "dd/MM/yyyy";



        internal static void FillSheet<T>(ExcelWorksheet sheets, List<T> list, string headerText = null, List<ExcelFieldModel> fields = null)
        {
            int row = 1;

            T obj = default(T);

            if (list.Any())
            {
                obj = list.FirstOrDefault();
            }
            else
            {
                obj = Activator.CreateInstance<T>();
            }

            var cellheder = 1;

            var properties = obj.GetType().GetProperties().ToList();

            if (fields == null || !fields.Any())
            {
                fields = new List<ExcelFieldModel>();
                properties.ForEach(e => fields.Add(new ExcelFieldModel() { Name = e.Name, LabelText = e.Name }));
            }

            if (!string.IsNullOrEmpty(headerText))
            {
                sheets.Cells[row, cellheder].Value = headerText;
                sheets.Cells[row, cellheder].Style.Font.Bold = true;
                sheets.Cells[row, cellheder, row, fields.Count].Merge = true;
                row++;

            }

            sheets.Cells.AutoFitColumns();


            //whrite heder
            foreach (ExcelFieldModel field in fields)
            {
                PropertyInfo prop = properties.FirstOrDefault(e => e.Name == field.Name);

                if (prop != null && prop.CanRead)
                {
                    sheets.Cells[row, cellheder].Value = field.LabelText ?? field.Name;
                    sheets.Cells[row, cellheder].Style.Font.Bold = true;
                    cellheder++;
                }

            }

            row++;

            foreach (var item in list)
            {
                var cell = 1;
                //whrite heder
                foreach (ExcelFieldModel field in fields)
                {
                    PropertyInfo prop = properties.FirstOrDefault(e => e.Name == field.Name);

                    if (prop != null && prop.CanRead)
                    {
                        //var name = prop.Name;
                        sheets.Cells[row, cell].Value = GetValueFormated(prop.GetValue(item), field.FormatText, !String.IsNullOrEmpty(field.FormatCell));

                        if (!String.IsNullOrEmpty(field.FormatCell))
                        {
                            sheets.Cells[row, cell].Style.Numberformat.Format = field.FormatCell;
                        }
                        cell++;
                    }
                }




                row++;
            }



            cellheder = 1;
            var sumInitField = String.IsNullOrEmpty(headerText) ? 2 : 3;
            var sumEndField = row - 1;

            var anySum = fields.Any(e => e.AllowSum);

            if (anySum)
            {
                foreach (ExcelFieldModel field in fields)
                {
                    PropertyInfo prop = properties.FirstOrDefault(e => e.Name == field.Name);

                    if (prop != null && prop.CanRead)
                    {
                        if (field.AllowSum)
                        {
                            string columnName = char.ConvertFromUtf32(cellheder + 64);
                            sheets.Cells[row, cellheder].Formula = String.Format("=SUM({0}{1}:{2}{3})", columnName, sumInitField, columnName, sumEndField);
                        }

                        cellheder++;
                    }

                }
            }

            sheets.Cells.AutoFitColumns();


        }

        internal static object GetValueFormated(object value, string formatText, bool hasFormatCell)
        {

            if (!hasFormatCell && string.IsNullOrEmpty(formatText) && (value is DateTime || value is Nullable<DateTime>))
            {
                formatText = defaulFormatDate;
            }

            if (!String.IsNullOrEmpty(formatText))
            {
                var txt = "{0:" + formatText + "}";

                return string.Format(txt, value);
            }
            return value;
        }

        public static byte[] GenerateByteArray<T>(string nombre, List<T> list)
        {
            IDictionary<string, List<T>> col = new Dictionary<string, List<T>>();
            col.Add(nombre, list);
            return GenerateByteArray<T>(col);
        }

        public static byte[] GenerateByteArray<T>(IDictionary<string, List<T>> list)
        {
            List<IExcelParameters> s = list.Select(i => new ExcelParameters<T>() { SheetName = i.Key, Values = i.Value }).ToList<IExcelParameters>();

            return GenerateByteArray(s);
        }

        public static byte[] GenerateByteArray(List<IExcelParameters> sheets)
        {
            var excel = GetExcelPackage(sheets);
            return GenerateByteArray(excel);
        }

        public static byte[] GenerateByteArray(ExcelPackage paq)
        {
            using (var ms = new MemoryStream())
            {
                paq.SaveAs(ms);
                return ms.ToArray();
            }
        }

        public static byte[] GenerateByteArray(IExcelParameters sheets)
        {
            return GenerateByteArray(new List<IExcelParameters>() { sheets });
        }
        public static ExcelPackage GetExcelPackage<T>(IDictionary<string, List<T>> list)
        {
            List<IExcelParameters> s = list.Select(i => new ExcelParameters<T>() { SheetName = i.Key, Values = i.Value }).ToList<IExcelParameters>();

            return GetExcelPackage(s);

        }

        public static ExcelPackage GetExcelPackage(List<IExcelParameters> sheets)
        {
            ExcelPackage excel = new ExcelPackage();

            foreach (var item in sheets)
            {
                var hoja = excel.Workbook.Worksheets.Add(item.SheetName);
                ExcelHelper.FillSheet(hoja, item.GetValues(), headerText: item.HeaderText, fields: item.Fields);
            }

            return excel;
        }


    }

    public interface IExcelParameters
    {
        string SheetName { get; set; }
        List<ExcelFieldModel> Fields { get; set; }

        string HeaderText { get; set; }
        List<object> GetValues();
    }

    public class ExcelParameters<T> : IExcelParameters
    // where T: class, new()
    {
        public ExcelParameters()
        {
            this.Fields = new List<ExcelFieldModel>();
        }

        public string SheetName { get; set; }

        public List<T> Values { get; set; }



        public string HeaderText { get; set; }
        public List<ExcelFieldModel> Fields { get; set; }
        public ExcelParameters<T> AddField(string name, string labelText = null, string formatText = null, Boolean allowSum = false, string formatCell = null)
        {
            Fields.Add(new ExcelFieldModel { Name = name, LabelText = labelText ?? name, FormatText = formatText, AllowSum = allowSum, FormatCell = formatCell });
            return (ExcelParameters<T>)this;
        }

        public List<object> GetValues()
        {
            return Values.Select(e => e as object).ToList();
        }
    }

    public class ExcelFieldModel
    {
        public ExcelFieldModel()
        {

        }

        public String Name { get; set; }
        public String LabelText { get; set; }
        public String FormatText { get; set; }
        public String FormatCell { get; set; }
        public bool AllowSum { get; set; }
    }
}
