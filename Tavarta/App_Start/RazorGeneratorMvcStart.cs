using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using RazorGenerator.Mvc;
using Tavarta;
using WebActivatorEx;

[assembly: PostApplicationStartMethod(typeof(RazorGeneratorMvcStart), "Start")]

namespace Tavarta
{
    public static class RazorGeneratorMvcStart
    {
        public static void Start()
        {
            //var engine = new PrecompiledMvcEngine(typeof(RazorGeneratorMvcStart).Assembly)
            //{
            //    UsePhysicalViewsIfNewer = HttpContext.Current.Request.IsLocal
            //};

            //ViewEngines.Engines.Clear();
            //ViewEngines.Engines.Add(engine);
            //VirtualPathFactoryManager.RegisterVirtualPathFactory(engine);
        }
    }
}
