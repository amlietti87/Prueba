﻿using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.ART
{
    public partial class ArtPrestadoresMedicosDto : EntityDto<int>
    {
        public ArtPrestadoresMedicosDto()
        {
            this.ArtDenunciasDto = new List<ArtDenunciasDto>();
        }

        public string Descripcion { get; set; }
        public bool Anulado { get; set; }

        public List<ArtDenunciasDto> ArtDenunciasDto { get; set; }

        public override string Description => Descripcion;
    }
}
