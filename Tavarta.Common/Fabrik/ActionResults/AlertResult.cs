﻿using System;
using System.Web.Mvc;
using Fabrik.Common.Web;

namespace Tavarta.Common.Fabrik.ActionResults
{
    /// <summary>
    /// An ActionResult for returning notifications to the user
    /// </summary>
    public class AlertResult<TResult> : ActionResult where TResult : ActionResult
    {
        public TResult Result { get; private set; }
        public Alert Message { get; private set; }

        public AlertResult(TResult result, AlertType alertType, string title, string description = null)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));
            if (title == null) throw new ArgumentNullException(nameof(title));
           

            Result = result;
            Message = new Alert(alertType, title, description);
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.Controller.TempData[typeof(Alert).FullName] = Message;
            Result.ExecuteResult(context);
        }
    }
}
