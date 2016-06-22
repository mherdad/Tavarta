using System.Web.Mvc;
using Fabrik.Common.Web;
using Tavarta.Common.Fabrik.ActionResults;

namespace Tavarta.Common.Fabrik
{
    /// <summary>
    /// A <see cref="MediaTypeViewResultFormatter"/> to format the model as XML.
    /// </summary>
    public class XmlViewResultFormatter : MediaTypeViewResultFormatter
    {
        public XmlViewResultFormatter()
        {
            AddSupportedMediaType("text/xml");
        }

        public override ActionResult CreateResult(ControllerContext controllerContext, ActionResult currentResult)
        {
            var model = controllerContext.Controller.ViewData.Model;

            if (model == null)
                return null;

            return new XmlResult(model);
        }
    }
}
