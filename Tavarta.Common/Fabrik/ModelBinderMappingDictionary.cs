using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tavarta.Common.Fabrik
{
    /// <summary>
    /// A dictionary for storing ASP.NET MVC ModelBinder mappings.
    /// </summary>
    public class ModelBinderMappingDictionary : Dictionary<Type, Type>
    {
        public void Add<TKey, TModelBinder>() where TModelBinder : IModelBinder
        {
            this.Add(typeof(TKey), typeof(TModelBinder));
        }
    }
}
