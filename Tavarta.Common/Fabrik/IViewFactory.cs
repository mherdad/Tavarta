
namespace Tavarta.Common.Fabrik
{
    public interface IViewFactory
    {
        TView CreateView<TView>();
        TView CreateView<TInput, TView>(TInput input);
    }
}
