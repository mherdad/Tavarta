using System.Net;
using System.Web.Mvc;
using Tavarta.Common.Constants;

namespace Tavarta.Controllers
{
    /// <summary>
    /// Provides methods that respond to HTTP requests with HTTP errors.
    /// </summary>
    [RoutePrefix("error")]
    public class ErrorController : Controller
    {

        #region Public Methods

        /// <summary>
        /// Returns a HTTP 400 Bad Request error view. Returns a partial view if the request is an AJAX call.
        /// </summary>
        /// <returns>The partial or full bad request view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.BadRequest)]
        [Route("badrequest")]
        public virtual ActionResult BadRequest()
        {
            return this.GetErrorView(HttpStatusCode.BadRequest, "BadRequest");
        }

        /// <summary>
        /// Returns a HTTP 403 Forbidden error view. Returns a partial view if the request is an AJAX call.
        /// Unlike a 401 Unauthorized response, authenticating will make no difference.
        /// </summary>
        /// <returns>The partial or full forbidden view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.Forbidden)]
        [Route("forbidden")]
        public virtual ActionResult Forbidden()
        {
            return this.GetErrorView(HttpStatusCode.Forbidden, "Forbidden");
        }

        /// <summary>
        /// Returns a HTTP 500 Internal Server Error error view. Returns a partial view if the request is an AJAX call.
        /// </summary>
        /// <returns>The partial or full internal server error view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.InternalServerError)]
        [Route("internalservererror")]
        public virtual ActionResult InternalServerError()
        {
            return this.GetErrorView(HttpStatusCode.InternalServerError, "InternalServerError");
        }

        /// <summary>
        /// Returns a HTTP 405 Method Not Allowed error view. Returns a partial view if the request is an AJAX call.
        /// </summary>
        /// <returns>The partial or full method not allowed view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.MethodNotAllowed)]
        [Route("methodnotallowed")]
        public virtual ActionResult MethodNotAllowed()
        {
            return this.GetErrorView(HttpStatusCode.MethodNotAllowed, "MethodNotAllowed");
        }

        /// <summary>
        /// Returns a HTTP 404 Not Found error view. Returns a partial view if the request is an AJAX call.
        /// </summary>
        /// <returns>The partial or full not found view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.NotFound)]
        [Route("notfound")]
        public virtual ActionResult NotFound()
        {
            return View();
            //return this.GetErrorView(HttpStatusCode.NotFound,"NotFound");
        }

        /// <summary>
        /// Returns a HTTP 401 Unauthorized error view. Returns a partial view if the request is an AJAX call.
        /// </summary>
        /// <returns>The partial or full unauthorized view.</returns>
        [OutputCache(CacheProfile = CacheProfileName.Unauthorized)]
        [Route("unauthorized")]
        public virtual ActionResult Unauthorized()
        {
            return this.GetErrorView(HttpStatusCode.Unauthorized, "Unauthorized");
        }
        [Route("lockout")]
        public virtual ActionResult LockOut()
        {
            return this.GetErrorView(HttpStatusCode.Forbidden, "LockOut");
        }
        #endregion

        #region Private Methods
        private ActionResult GetErrorView(HttpStatusCode statusCode, string viewName)
        {
            this.Response.StatusCode = (int)statusCode;

            // Don't show IIS custom errors.
            this.Response.TrySkipIisCustomErrors = true;

            ActionResult result;
            if (this.Request.IsAjaxRequest())
            {
                // This allows us to show errors even in partial views.
                result = this.PartialView(viewName);
            }
            else
            {
                result = this.View(viewName);
            }

            return result;
        }

        #endregion

    }
}