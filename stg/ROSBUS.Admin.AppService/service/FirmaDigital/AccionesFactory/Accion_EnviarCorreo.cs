using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.AppService.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_EnviarCorreo : AccionBuilder<AplicarAccioneResponseDto>, IAccion_EnviarCorreo
    {

        private readonly IDefaultEmailer defaultEmailer;
        private readonly ISysParametersAppService parameters;

        public Accion_EnviarCorreo(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {
            defaultEmailer = _serviceProvider.GetRequiredService<IDefaultEmailer>();
            parameters = _serviceProvider.GetRequiredService<ISysParametersAppService>();
        }

        protected override async Task<AplicarAccioneResponseDto> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            AplicarAccioneResponseDto responseDto = new AplicarAccioneResponseDto();

            var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento
            })).Items.ToList();

            var adjuntosAppService = this.serviceProvider.GetRequiredService<IAdjuntosAppService>();

            List<KeyValuePair<System.IO.Stream, string>> archivos = new List<KeyValuePair<System.IO.Stream, string>>();

            foreach (var doc in Documentos.OrderBy(e => e.Fecha))
            {

                DetalleResponse detalle = new DetalleResponse();
                detalle.IsValid = true;
                //fecha - desc tipo documento-apellido y nombre empleado
                detalle.Documento = String.Format("{0} - {1} - {2}",
                    doc.Fecha.ToString("yyyy-MM-dd"),
                    doc.TipoDocumento?.Descripcion,
                    doc.NombreEmpleado.Trim());

                try
                {
                    await this.logger.LogInformation($"Evaluando el Documento {doc.Id}");

                    var archivo = adjuntosAppService.GetById(doc.ArchivoId);

                    archivos.Add(new KeyValuePair<System.IO.Stream, string>(new System.IO.MemoryStream(archivo.Archivo), archivo.Nombre));

                }
                catch (Exception ex)
                {
                    detalle.IsValid = false;
                    detalle.Error += $"Error al procesar el archivo. ";
                    detalle.Error += $"{ex.Message}";

                    await this.logger.LogInformation($"Error: {ex.Message}");
                    await this.logger.LogInformation($"StackTrace: {ex.StackTrace}");
                }
                finally
                {
                    responseDto.Detalles.Add(detalle);
                }

            }

            decimal filesize = 0;
            foreach (var item in archivos)
            {
                filesize = filesize + item.Key.Length;
            }
            decimal sizeinmb = filesize / 1048576;

            var parameter = (await this.parameters.GetAllAsync(e => e.Token == "MBMaximoMail")).Items.FirstOrDefault();

            if (parameter == null)
            {
                throw new ValidationException("No se encuentra parámetro MBMaximoMail");
            }
            else
            {
                decimal output;
                if (decimal.TryParse(parameter.Value, out output))
                {
                    if (sizeinmb <= output)
                    {
                        StringBuilder CuerpoMail = new StringBuilder("Adjunto documentos empleado " + Documentos[0].Cuilempleado + "<br>");
                        foreach (var Documento in Documentos)
                        {
                            var archivo = adjuntosAppService.GetById(Documento.ArchivoId);
                            CuerpoMail.Append(archivo.Nombre + "<br>");
                        }

                        String CuerpoMailFinal = CuerpoMail.ToString();
                        await this.defaultEmailer.SendDefaultAsync(dto.Correo, "Documentos empleado " + Documentos[0].Cuilempleado, CuerpoMailFinal, archivos);
                    }
                    else
                    {
                        throw new ValidationException(String.Format("Los archivos adjuntos pesan más de {0} MB, no se puede enviar correo", output));
                    }
                }
                else
                {
                    throw new ValidationException("El valor del parámetro MBMaximoMail no se puede convertir a decimal");
                }
            }




            return responseDto;
        }

    }

    public interface IAccion_EnviarCorreo : IAccionBuilder
    {
    }
}