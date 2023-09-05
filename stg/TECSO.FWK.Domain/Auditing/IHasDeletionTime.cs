using System;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
  public interface IHasDeletionTime : ISoftDelete
  {
    DateTime? DeletedDate { get; set; }
  }
}
