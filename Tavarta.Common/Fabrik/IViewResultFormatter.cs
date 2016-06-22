using System.Web.Mvc;

namespace Tavarta.Common.Fabrik
{
    public interface IViewResultFormatter
    {
        bool IsSatisfiedBy(ControllerContext controllerContext);
        ActionResult CreateResult(ControllerContext controllerContext, ActionResult currentResult);
    }
}
