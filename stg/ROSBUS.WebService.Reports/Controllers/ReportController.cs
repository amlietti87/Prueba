using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.WebForms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ROSBUS.Admin.Domain.Report;
using Svg;

namespace ROSBUS.WebService.Reports.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ILogger _logger;

        public ReportController(ILogger<ReportController> loger)
        {
            this._logger = loger;
        }

        [HttpGet]
        public string Test()
        {
            this._logger.LogError("Test");

            return "ok";
        }

        private IList createList(Type myType)
        {
            Type genericListType = typeof(List<>).MakeGenericType(myType);
            return (IList)Activator.CreateInstance(genericListType);
        }

        private IList createListByType(Type mytype)
        {
            Type listGenericType = typeof(List<>);
            Type list = listGenericType.MakeGenericType(mytype);
            ConstructorInfo ci = list.GetConstructor(new Type[] { });
            return (IList)ci.Invoke(new object[] { });
        }

        [HttpPost]
        public ResponseModel<byte[]> GenerarReporteGenerico(ReportModel model)
        {
            try
            {
                ReportViewer reportViewer = new ReportViewer();
                reportViewer.ProcessingMode = ProcessingMode.Local;
                reportViewer.LocalReport.EnableExternalImages = true;
                
                if (!string.IsNullOrEmpty(model.ReportName))
                {
                    Assembly assembly = typeof(ROSBUS.Admin.Domain.Report.ReportName).Assembly;
                    Stream stream = assembly.GetManifestResourceStream(model.ReportName);
                    if (stream != null)
                    {
                        reportViewer.LocalReport.LoadReportDefinition(stream);
                    }
                    else {
                        throw new ValidationException("No se encontro el reporte :" + model.ReportName);
                    }
                    
                }
                else if (!string.IsNullOrEmpty(model.ReportPath))
                {
                    reportViewer.LocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, model.ReportPath);
                }
                else {
                    throw new ValidationException("No se encontro el Nombre del reporte");
                }

             



                Assembly inic = model.GetType().Assembly;
                foreach (var ds in model.DataSources)
                {
                    var type = inic.GetType(ds.Type);
                    //var TypeList = inic.GetType(ds.TypeList);
                    //var dst = JsonConvert.DeserializeObject(ds.ListJSON, TypeList);

                    //ejemplo 2
                    var l = createList(type);
                    var dst2 = (IList)JsonConvert.DeserializeObject(ds.ListJSON, l.GetType());

                    //ejemplo 3
                    //var l = createListByType(type);
                    //var dst2 = (IList)JsonConvert.DeserializeObject(ds.ListJSON, l.GetType());

                    reportViewer.LocalReport.DataSources.Add(new ReportDataSource(ds.DataSetName, dst2));
                }

                foreach (var p in model.Parameters)
                {
                    reportViewer.LocalReport.SetParameters(new ReportParameter(p.Key, p.Value));
                }

                return RenderPdf(reportViewer);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "GenerarReporteGenerico");
                throw ex;
            }        
        }

        [HttpPost]
        public ResponseModel<string> GenerarBase64DesdeSvg([FromBody]string Svg)
        {
            string base64String = String.Empty;
            var byteArray = Encoding.ASCII.GetBytes(Svg);
            using (var stream = new MemoryStream(byteArray))
            {
                var svgDocument = SvgDocument.Open<SvgDocument>(stream);
                var bitmap = svgDocument.Draw();
                // Convert Image to byte[]
                MemoryStream image = new MemoryStream();
                bitmap.Save(image, ImageFormat.Png);
                byte[] imageBytes = image.ToArray();

                // Convert byte[] to Base64 String
                base64String = Convert.ToBase64String(imageBytes);
                
            }

            var objectReturn = new ResponseModel<string>()
            {
                DataObject = base64String,
                Status = ActionStatus.Ok.ToString()
            };
            return objectReturn;
        }

        private static ResponseModel<byte[]> RenderPdf(ReportViewer reportViewer)
        {
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            byte[] bytes = reportViewer.LocalReport.Render(
                "PDF", null, out mimeType, out encoding, out filenameExtension,
                out streamids, out warnings); 

            var objectReturn = new ResponseModel<byte[]>()
            {
                DataObject = bytes,
                Status = ActionStatus.Ok.ToString()
            };
            return objectReturn;
        }



    }

    public class ResponseModel<T> : AbstractModel
    {
        public ResponseModel()
        {
            this.Messages = new List<string>();
        }

        public T DataObject { get; set; }
    }

    public abstract class AbstractModel
    {
        public String Status { get; set; }

        public List<String> Messages { get; set; }
    }

    public enum ActionStatus
    {
        Ok,
        Error,
        Warning,
        ValidationError,
        OkAndConfirm
    }


}
