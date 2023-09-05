using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace ROSBUS.Admin.Domain.Report
{
    public class ReportModel
    {
        public ReportModel()
        {
            this.DataSources = new List<ReportDataSourceType>();
            this.Parameters = new List<ReportParameterModel>();
        }


        public List<ReportDataSourceType> DataSources { get; set; }
         
        //public ReadOnlyCollection<ReportDataSourceType> DataSources { get { return _DataSources.AsReadOnly(); } }

        public List<ReportParameterModel> Parameters { get; set; }
        public string ReportName { get; set; }

        public string ReportPath { get; set; }

        


        public void AddDataSources<T>(string DataSetName,  List<T> list) {
            if (string.IsNullOrEmpty(DataSetName))
            {
                throw new ArgumentException("DataSetName no tiene valor");
            }
             

            var dst = new ReportDataSourceType();

            dst.Type = Activator.CreateInstance<T>().GetType().ToString(); 
            dst.TypeList = list.GetType().ToString();
            dst.ListJSON = JsonConvert.SerializeObject(list);

            dst.DataSetName = DataSetName;

            this.DataSources.Add(dst);
        }
    }


    public class ReportDataSourceType
    {
        public string DataSetName { get; set; }
        public string ListJSON { get; set; } 
        public string Type { get; set; } 
        public string TypeList { get; set; }
    }

    public class ReportParameterModel
    {
        public string Key { get; set; }
        public string Value { get; set; }

    }

}
