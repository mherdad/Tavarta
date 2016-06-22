using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Tavarta.Common.Fabrik
{
    /// <summary>
    /// An interface for metadata plugins.
    /// </summary>
    public interface IMetadataPlugin
    {
        void AssignMetadata(IEnumerable<Attribute> attributes, ModelMetadata metadata);
    }
}
