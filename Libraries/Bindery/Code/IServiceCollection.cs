namespace Bindery;

public interface IServiceCollection
{
	TService? GetService<TService>();
}
