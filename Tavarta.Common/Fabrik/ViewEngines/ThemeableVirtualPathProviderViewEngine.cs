﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tavarta.Common.Fabrik.ViewEngines {

    public abstract class ThemeableVirtualPathProviderViewEngine : IViewEngine {
        
        // format is ":ViewCacheEntry:{cacheType}:{theme}:{prefix}:{name}:{controllerName}:{areaName}:"
        private const string CacheKeyFormat = ":ViewCacheEntry:{0}:{1}:{2}:{3}:{4}:{5}:";
        private const string CacheKeyPrefixMaster = "Master";
        private const string CacheKeyPrefixPartial = "Partial";
        private const string CacheKeyPrefixView = "View";
        private static readonly string[] EmptyLocations = new string[0];

        private VirtualPathProvider _vpp;

        public Func<HttpContextBase, string> Theme { get; set; }

        public string[] AreaMasterLocationFormats { get; set; }
        public string[] AreaPartialViewLocationFormats { get; set; }
        public string[] AreaViewLocationFormats { get; set; }
        public string[] MasterLocationFormats { get; set; }
        public string[] PartialViewLocationFormats { get; set; }
        public string[] ViewLocationFormats { get; set; }
        public IViewLocationCache ViewLocationCache { get; set; }

        protected VirtualPathProvider VirtualPathProvider {
            get { return _vpp ?? (_vpp = HostingEnvironment.VirtualPathProvider); }
            set { _vpp = value; }
        }
        
        protected ThemeableVirtualPathProviderViewEngine() {
            if (HttpContext.Current == null || HttpContext.Current.IsDebuggingEnabled) {
                ViewLocationCache = DefaultViewLocationCache.Null;
            } else {
                ViewLocationCache = new DefaultViewLocationCache();
            }
        }

        private string CreateCacheKey(string theme, string prefix, string name, string controllerName, string areaName) {
            return string.Format(CultureInfo.InvariantCulture, CacheKeyFormat, GetType().AssemblyQualifiedName, theme, prefix, name, controllerName, areaName);
        }


        protected abstract IView CreatePartialView(ControllerContext controllerContext, string partialPath);
        protected abstract IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath);

        protected virtual bool FileExists(ControllerContext controllerContext, string virtualPath) {
            return VirtualPathProvider.FileExists(virtualPath);
        }

        public virtual ViewEngineResult FindPartialView(ControllerContext controllerContext, string partialViewName, bool useCache) {
            
            if (controllerContext == null)
                throw new ArgumentNullException("controllerContext");

            if (string.IsNullOrEmpty(partialViewName))
                throw new ArgumentException("Value cannot be null or empty.", "partialViewName");

            string[] searched;
            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            bool incompleteMatch = false;

            string partialPath = GetPath(controllerContext, PartialViewLocationFormats, AreaPartialViewLocationFormats,
                                         "PartialViewLocationFormats", partialViewName, controllerName,
                                         CacheKeyPrefixPartial, useCache, /* checkBaseType */ true,
                                         ref incompleteMatch, out searched);

            if (string.IsNullOrEmpty(partialPath)) 
                return new ViewEngineResult(searched);

            return new ViewEngineResult(CreatePartialView(controllerContext, partialPath), this);
        }


        public virtual ViewEngineResult FindView(ControllerContext controllerContext, string viewName, string masterName, bool useCache)
        {
            if (controllerContext == null)
            {
                throw new ArgumentNullException("controllerContext");
            }

            if (string.IsNullOrEmpty(viewName))
            {
                throw new ArgumentException("Value cannot be null or empty.", "viewName");
            }

            string[] viewLocationsSearched;
            string[] masterLocationsSearched;
            bool incompleteMatch = false;

            string controllerName = controllerContext.RouteData.GetRequiredString("controller");

            string viewPath = GetPath(controllerContext, ViewLocationFormats, AreaViewLocationFormats,
                                      "ViewLocationFormats", viewName, controllerName, CacheKeyPrefixView, useCache,
                                      /* checkPathValidity */ true, ref incompleteMatch, out viewLocationsSearched);

            string masterPath = GetPath(controllerContext, MasterLocationFormats, AreaMasterLocationFormats,
                                        "MasterLocationFormats", masterName, controllerName, CacheKeyPrefixMaster,
                                        useCache, /* checkPathValidity */ false, ref incompleteMatch,
                                        out masterLocationsSearched);

            if (string.IsNullOrEmpty(viewPath) || (string.IsNullOrEmpty(masterPath) && !string.IsNullOrEmpty(masterName)))
            {
                return new ViewEngineResult(viewLocationsSearched.Union(masterLocationsSearched));
            }

            return new ViewEngineResult(CreateView(controllerContext, viewPath, masterPath), this);
        }

        private string GetPath(ControllerContext controllerContext, IEnumerable<string> locations, IEnumerable<string> areaLocations, string locationsPropertyName, string name, string controllerName, string cacheKeyPrefix, bool useCache, bool checkPathValidity, ref bool incompleteMatch, out string[] searchedLocations)
        {
            searchedLocations = EmptyLocations;
            
            if (string.IsNullOrEmpty(name))
                return string.Empty;           

            string areaName = GetAreaName(controllerContext.RouteData);
            bool usingAreas = !string.IsNullOrEmpty(areaName);
            string theme = Theme(controllerContext.HttpContext) ?? "Default";

            List<ViewLocation> viewLocations = GetViewLocations(locations, (usingAreas) ? areaLocations : null);

            if (viewLocations.Count == 0) 
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "The property '{0}' cannot be null or empty.", locationsPropertyName));

            bool nameRepresentsPath = IsSpecificPath(name);
            string cacheKey = CreateCacheKey(theme, cacheKeyPrefix, name, (nameRepresentsPath) ? string.Empty : controllerName, areaName);

            if (useCache) 
                return ViewLocationCache.GetViewLocation(controllerContext.HttpContext, cacheKey);

            return nameRepresentsPath ?
                   GetPathFromSpecificName(controllerContext, name, cacheKey, checkPathValidity, ref searchedLocations, ref incompleteMatch) :
                   GetPathFromGeneralName(controllerContext, viewLocations, name, controllerName, areaName, theme, cacheKey, ref searchedLocations);
        }

        private static string GetAreaName(RouteData routeData) {
            object area;
            if (routeData.DataTokens.TryGetValue("area", out area))
                return area as string;

            return GetAreaName(routeData.Route);
        }

        private static string GetAreaName(RouteBase route) {
            
            var routeWithArea = route as IRouteWithArea;

            if (routeWithArea != null)
                return routeWithArea.Area;

            var castRoute = route as Route;

            if (castRoute != null && castRoute.DataTokens != null)
                return castRoute.DataTokens["area"] as string;

            return null;
        }

        private string GetPathFromGeneralName(ControllerContext controllerContext, IList<ViewLocation> locations, string name, string controllerName, string areaName, string theme, string cacheKey, ref string[] searchedLocations) {
            string result = string.Empty;
            searchedLocations = new string[locations.Count];

            for (int i = 0; i < locations.Count; i++) {
                ViewLocation location = locations[i];
                string virtualPath = location.Format(name, controllerName, areaName, theme);

                if (FileExists(controllerContext, virtualPath)) {
                    searchedLocations = EmptyLocations;
                    result = virtualPath;
                    ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
                    break;
                }

                searchedLocations[i] = virtualPath;
            }

            return result;
        }

        private string GetPathFromSpecificName(ControllerContext controllerContext, string name, string cacheKey, bool checkPathValidity, ref string[] searchedLocations, ref bool incompleteMatch) {
            string result = name;
            
            bool fileExists = FileExists(controllerContext, name);

            if (checkPathValidity && fileExists) {
                bool? validPath = IsValidPath(controllerContext, name);

                if (validPath == false) {
                    fileExists = false;
                } else if (validPath == null) {
                    incompleteMatch = true;
                }
            }

            if (!fileExists) {
                result = string.Empty;
                searchedLocations = new[] { name };
            }

            if (!incompleteMatch) {
                ViewLocationCache.InsertViewLocation(controllerContext.HttpContext, cacheKey, result);
            }

            return result;
        }

        private static List<ViewLocation> GetViewLocations(IEnumerable<string> viewLocationFormats, IEnumerable<string> areaViewLocationFormats) {
            var allLocations = new List<ViewLocation>();

            if (areaViewLocationFormats != null)
                allLocations.AddRange(areaViewLocationFormats.Select(areaViewLocationFormat => new AreaAwareViewLocation(areaViewLocationFormat)));

            if (viewLocationFormats != null)
                allLocations.AddRange(viewLocationFormats.Select(viewLocationFormat => new ViewLocation(viewLocationFormat)));

            return allLocations;
        }

        private static bool IsSpecificPath(string name) {
            char c = name[0];
            return (c == '~' || c == '/');
        }

        public virtual void ReleaseView(ControllerContext controllerContext, IView view) {
            var disposable = view as IDisposable;
            if (disposable != null) {
                disposable.Dispose();
            }
        }

        protected virtual bool? IsValidPath(ControllerContext controllerContext, string virtualPath) {
            return null;
        }

        private class ViewLocation {
            protected readonly string VirtualPathFormatString;

            public ViewLocation(string virtualPathFormatString) {
                this.VirtualPathFormatString = virtualPathFormatString;
            }

            public virtual string Format(string viewName, string controllerName, string areaName, string theme) {
                return string.Format(CultureInfo.InvariantCulture, VirtualPathFormatString, viewName, controllerName, theme);
            }
        }

        private class AreaAwareViewLocation : ViewLocation {
            public AreaAwareViewLocation(string virtualPathFormatString) : base(virtualPathFormatString)
            {
            }

            public override string Format(string viewName, string controllerName, string areaName, string theme) {
                return string.Format(CultureInfo.InvariantCulture, VirtualPathFormatString, viewName, controllerName, areaName, theme);
            }
        }
    }
}