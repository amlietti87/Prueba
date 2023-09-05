


using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
    public interface IDeletionAudited<TUser> : IDeletionAudited, IHasDeletionTime, ISoftDelete 
        where TUser : IEntity<int>
    {
        TUser DeletedUser { get; set; }
    }

    public interface IDeletionAudited : IHasDeletionTime, ISoftDelete
    {
        int? DeletedUserId { get; set; }
    }
}
