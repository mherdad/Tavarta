using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Tavarta.Common.Fabrik;
using Tavarta.Common.Fabrik.Others;

namespace Fabrik.Common.Web
{
    /// <summary>
    /// A pluggable version of <see cref="DataAnnotationsModelMetadataProvider"/>
    /// </summary>
    public class PluggableModelMetaDataProvider : DataAnnotationsModelMetadataProvider
    {
        private readonly IEnumerable<IMetadataPlugin> _plugins;

        public PluggableModelMetaDataProvider(IEnumerable<IMetadataPlugin> plugins)
        {
            if (plugins == null) throw new ArgumentNullException(nameof(plugins));
            
            this._plugins = plugins;
        }
        
        protected override ModelMetadata CreateMetadata(IEnumerable<Attribute> attributes, Type containerType, Func<object> modelAccessor, Type modelType, string propertyName)
        {
            var metadata = base.CreateMetadata(attributes, containerType, modelAccessor, modelType, propertyName);
            _plugins.ForEach(p => p.AssignMetadata(attributes, metadata));
            return metadata;
        }
    }
}
