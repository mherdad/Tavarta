using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Tavarta.Common.Fabrik
{
    /// <summary>
    /// Provides a base class for formatters that use the media type to determine if they should format the result.
    /// </summary>
    public abstract class MediaTypeViewResultFormatter : IViewResultFormatter
    {
        private readonly List<string> _supportedMediaTypes = new List<string>();
        public IEnumerable<string> SupportedMediaTypes { get { return _supportedMediaTypes; } }

        public void AddSupportedMediaType(string mediaType)
        {
            if (mediaType == null) throw new ArgumentNullException(nameof(mediaType));
            
            _supportedMediaTypes.Add(mediaType);
        }

        public abstract ActionResult CreateResult(ControllerContext controllerContext, ActionResult current);

        public virtual bool IsSatisfiedBy(ControllerContext controllerContext)
        {
            if (controllerContext == null) throw new ArgumentNullException(nameof(controllerContext));

            var acceptTypes = controllerContext.HttpContext.Request.AcceptTypes;
            return acceptTypes != null && acceptTypes.Intersect(_supportedMediaTypes).Any();
        }
    }
}
