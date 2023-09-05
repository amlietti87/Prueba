using System;

namespace TECSO.FWK.Domain.Auditing
{
  public interface IHasModificationTime
  {
    DateTime? LastUpdatedDate { get; set; }
  }
}
