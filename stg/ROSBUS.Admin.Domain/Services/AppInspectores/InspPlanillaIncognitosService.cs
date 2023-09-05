using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspPlanillaIncognitosService : ServiceBase<InspPlanillaIncognitos,int, IInspPlanillaIncognitosRepository>, IInspPlanillaIncognitosService
    {
        private readonly IInspPreguntasIncognitosRepository _preguntasRepository;
        private readonly IInspPreguntasIncognitosRespuestasRepository _preguntasRespuestaRepository;
        public InspPlanillaIncognitosService(IInspPlanillaIncognitosRepository produtoRepository, IInspPreguntasIncognitosRepository preguntasRepository, IInspPreguntasIncognitosRespuestasRepository preguntasRespuestasRepository)
            : base(produtoRepository)
        {
            _preguntasRepository = preguntasRepository;
            _preguntasRespuestaRepository = preguntasRespuestasRepository;
        }
        protected override async Task ValidateEntity(InspPlanillaIncognitos entity, SaveMode mode)
        {
            Expression<Func<InspPreguntasIncognitos, bool>> expPreguntas = e => true;
            Expression<Func<InspPreguntasIncognitosRespuestas, bool>> expPreguntasRespuestas = e => true;
            
            var preguntas_repuestas = await _preguntasRespuestaRepository.GetAllAsync(expPreguntasRespuestas);
            var preguntas = await _preguntasRepository.GetAllAsync(expPreguntas);
            foreach (var item in entity.InspPlanillaIncognitosDetalle)
            {

                var pregunta = preguntas.Items.FirstOrDefault(e => e.Id == item.PreguntaIncognitoId);
                if (pregunta.RespuestaRequerida)
                {
                    if(item.RespuestaIncognitoId == null)
                    {
                        throw new TECSO.FWK.Domain.DomainValidationException("No todas las preguntas requeridas fueron contestadas");
                    }

                    var respuestasPosiblesParaLaPregunta = preguntas_repuestas.Items.Where(e => e.PreguntaIncognitoId == pregunta.Id);
                    if (!respuestasPosiblesParaLaPregunta.Any(e => e.RespuestaId == item.RespuestaIncognitoId))
                    {
                        throw new TECSO.FWK.Domain.DomainValidationException($"La respuesta seleccionada para la pregunta {pregunta.Descripcion} no está dentro de las respuestas posibles" );
                    }


                }
            }

        }
    }
    
}
