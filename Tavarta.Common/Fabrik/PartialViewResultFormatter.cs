using System;
using System.Web.Mvc;
using Tavarta.Common.Fabrik;
using Tavarta.Common.Fabrik.Others;

namespace Fabrik.Common.Web
{
    public class PartialViewResultFormatter : MediaTypeViewResultFormatter
    {       
        private readonly string partialViewPrefix;
        private readonly string viewOverrideParameter;

        public PartialViewResultFormatter(string partialViewPrefix = "_", string viewOverrideParameter = null)
        {
            if (partialViewPrefix == null) throw new ArgumentNullException(nameof(partialViewPrefix));
            
            this.partialViewPrefix = partialViewPrefix;
            this.viewOverrideParameter = viewOverrideParameter;

            AddSupportedMediaType("text/html");
        }
       
        public override bool IsSatisfiedBy(ControllerContext controllerContext)
        {
            return base.IsSatisfiedBy(controllerContext)
                && (controllerContext.HttpContext.Request.IsAjaxRequest() || controllerContext.IsChildAction);               
        }

        public override ActionResult CreateResult(ControllerContext controllerContext, ActionResult currentResult)
        {
            var viewResult = currentResult as ViewResult;

            if (viewResult == null)
            {
                return null;
            }
   
            var partialViewResult = new PartialViewResult
            {
                ViewData = viewResult.ViewData,
                TempData = viewResult.TempData,
                ViewName = GetPartialViewName(viewResult, controllerContext),
                ViewEngineCollection = viewResult.ViewEngineCollection,
            };

            return partialViewResult;
        }

        protected string GetPartialViewName(ViewResult viewResult, ControllerContext controllerContext)
        {           
            var routeData = controllerContext.RequestContext.RouteData;
            var viewName = viewResult.ViewName.NullIfEmpty() ?? routeData.GetRequiredString("action");

            // Check for view name override (child actions only)
            if (StringExtensions.IsNotNullOrEmpty(viewOverrideParameter) && controllerContext.IsChildAction)
            {
                var overrideView = routeData.Values.GetOrDefault(viewOverrideParameter) as string;
                if (StringExtensions.IsNotNullOrEmpty(overrideView))
                {
                    return overrideView;
                }
            }

            // Otherwise use partial view prefix
            
            if (viewName.IsNullOrEmpty())
            {
                throw new InvalidOperationException("View name cannot be null.");
            }
                    
            var partialViewName = string.Concat(partialViewPrefix, viewName);
            // check if partial exists, otherwise we'll use the same view but with no layout page
            var partialExists = viewResult.ViewEngineCollection.FindPartialView(controllerContext, partialViewName).View != null;

            return partialExists ? partialViewName : viewName;
        }
    }
}
