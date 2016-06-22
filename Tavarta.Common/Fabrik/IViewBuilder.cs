
namespace Tavarta.Common.Fabrik
{
    public interface IViewBuilder<TView>
    {
        TView Build();
    }
    
    public interface IViewBuilder<TInput, TView>
    {
        TView Build(TInput input);
    }
}
