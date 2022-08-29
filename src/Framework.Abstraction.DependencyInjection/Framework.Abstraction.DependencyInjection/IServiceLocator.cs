using Microsoft.Extensions.DependencyInjection;

namespace Framework.Abstraction.DependencyInjection;

public interface IServiceLocator : IDisposable
{
    IServiceScope CreateScope();
    T Get<T>();
}
