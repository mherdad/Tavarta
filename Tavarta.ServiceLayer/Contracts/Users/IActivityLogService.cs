using System.Collections.Generic;
using Tavarta.DomainClasses.Entities.Users;

namespace Tavarta.ServiceLayer.Contracts.Users
{
    /// <summary>
    /// مشخص کننده الزاماتی که ارائه دهنده سرویس باید رعایت کند
    /// </summary>
    public interface IActivityLogService
    {
        void Create(ActivityLog log);
        //IList<LastActivityViewModel> GetLastActivities();
    }
}
