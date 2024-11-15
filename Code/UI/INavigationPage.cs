namespace Rp.UI;

public interface INavigationPage
{
	string PageName { get; }
	NavigationHost Host { get; }
}
