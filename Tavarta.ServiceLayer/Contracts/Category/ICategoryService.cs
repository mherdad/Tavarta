using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Tavarta.ServiceLayer.Contracts.Category
{
    public interface ICategoryService
    {
        Task<IEnumerable<SelectListItem>> GetAllAsSelectList();
    }
}