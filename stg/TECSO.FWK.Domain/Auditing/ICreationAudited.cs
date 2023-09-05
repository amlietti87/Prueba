

using TECSO.FWK.Domain.Interfaces.Entities;
namespace TECSO.FWK.Domain.Auditing
{
    public interface ICreationAudited : IHasCreationTime
    {
        int? GetCreatorUserId();
        void SetCreatorUserId(int? value);
    }
    public interface ICreationAudited<TUser> : ICreationAudited, IHasCreationTime 
        where TUser : IEntity<int>
    {
        TUser CreatedUser { get; set; }
    }
}
