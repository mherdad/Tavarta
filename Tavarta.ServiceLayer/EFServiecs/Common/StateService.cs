﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Tavarta.Common.Extentions;
using Tavarta.ServiceLayer.Contracts.Common;
using Tavarta.ServiceLayer.Settings;

namespace Tavarta.ServiceLayer.EFServiecs.Common
{
    /// <summary>
    /// نشان دهنده سرویس دهنده عملیات مرتبط با استان 
    /// </summary>
    public class StateService : IStateService
    {
        #region Fields

        private readonly HttpContextBase _httpContextBase;

        #endregion

        #region Ctor

        public StateService(HttpContextBase httpContextBase)
        {
            _httpContextBase = httpContextBase;
        }
        #endregion
        public IList<SelectListItem> GetAsSelectListItemAsync(string selected, string path)
        {
            var lst =
                XDocument.Load(_httpContextBase.Server.MapPath(path)).Root?.Elements(OwnConstants.State).Select(e => e);
            var states = lst.Select(a => a.Attribute(OwnConstants.Name).Value);
            return
                states.OrderBy(a => a).Select(a => new SelectListItem { Value = a, Text = a, Selected = selected.HasValue() && a == selected })
                    .ToList();

        }
    }
}
