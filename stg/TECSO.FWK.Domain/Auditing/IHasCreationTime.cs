using System;

namespace TECSO.FWK.Domain.Auditing
{
  public interface IHasCreationTime
  {
    DateTime? CreatedDate { get; set; }
  }
}
