namespace Tavarta.ServiceLayer.Contracts.Schedulers
{
    public interface IIndexService
    {
        void ReBuildUserIndex();
        void ReBuildUserRoleIndex();
        void ReBuildUserLoginsIndex();
        void RebuildUserClaimIndex();
        void ReBuildRolesIndex();
    }
}