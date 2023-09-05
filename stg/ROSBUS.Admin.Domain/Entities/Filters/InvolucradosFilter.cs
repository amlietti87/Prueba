using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class InvolucradosFilter : FilterPagedListFullAudited<SinInvolucrados, int>
    {
        public string ApellidoNombre { get; set; }

        public int? SiniestroID { get; set; }
        public int? InvolucradoId { get; set; }
        public bool? Reclamo { get; set; }

        public int? TipoInvolucradoId { get; set; }

        public string Dominio { get; set; }

        public int? TipoDocumentoId { get; set; }

        public string Documento { get; set; }
        public string Apellido { get; set; }

        public string Domicilio { get; set; }
        public override Expression<Func<SinInvolucrados, bool>> GetFilterExpression()
        {
            Expression<Func<SinInvolucrados, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.ApellidoNombre))
            {
                Expression<Func<SinInvolucrados, bool>> filterTextExp = e => true;
                filterTextExp = e => e.ApellidoNombre == ApellidoNombre;
                Exp = Exp.And(filterTextExp);
            }

            if (SiniestroID.HasValue)
            {
                Exp = Exp.And(e => e.SiniestroId == this.SiniestroID);
            }
            if (Reclamo.HasValue && Reclamo.Value)
            {
                Exp = Exp.And(e => e.TipoInvolucrado.Reclamo == this.Reclamo.Value);
            }

            if (TipoInvolucradoId.HasValue)
            {
                Exp = Exp.And(e => e.TipoInvolucradoId == this.TipoInvolucradoId);
            }

            if (!String.IsNullOrWhiteSpace(Dominio))
            {
                Exp = Exp.And(e => e.Vehiculo.Dominio.Contains(Dominio));
            }

            if (TipoDocumentoId.HasValue)
            {
                Exp = Exp.And(e => e.TipoDocId == this.TipoDocumentoId);
            }

            if (!String.IsNullOrWhiteSpace(Documento))
            {
                Exp = Exp.And(e => e.NroDoc.Contains(Documento));
            }

            if (!String.IsNullOrWhiteSpace(Apellido))
            {
                Exp = Exp.And(e => e.ApellidoNombre.Contains(Apellido));
            }

            if (!String.IsNullOrWhiteSpace(Domicilio))
            {
                Exp = Exp.And(e => e.Domicilio.Contains(Domicilio));
            }
            if (InvolucradoId.HasValue)
            {
                Exp = Exp.Or(e => e.Id == this.InvolucradoId);
            }

            return Exp;
        }

        public override List<Expression<Func<SinInvolucrados, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinInvolucrados, object>>>() {
                e => e.Lesionado,
                e => e.MuebleInmueble,
                e => e.Lesionado.TipoLesionado,
                e => e.MuebleInmueble.TipoInmueble,
                e => e.TipoDoc,
                e => e.TipoInvolucrado,
                e => e.Vehiculo,
                e => e.Conductor,
                e => e.SinDetalleLesion,
                e => e.Siniestro,
                e => e.Conductor.TipoDoc
            };
        }

        public override Func<SinInvolucrados, ItemDto<int>> GetItmDTO()
        {

            return e => new ItemDto<int>(e.Id, e.getDescription());
        }
    }
}
