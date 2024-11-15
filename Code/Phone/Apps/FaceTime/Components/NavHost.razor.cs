using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class NavHost : NavigationHost
{
	protected override Panel Container { get; set; } = null!;

	public NavHost()
	{
		DefaultPage = typeof(FavoriteTab);

		RegisterPage<FavoriteTab>();
		RegisterPage<KeypadTab>();
		RegisterPage<CallTab>();

		Navigate<FavoriteTab>();
	}

	public void GoToFavorite()
	{
		Navigate<FavoriteTab>();
	}

	public void GoToRecent()
	{
		// Navigate("/recent" );
	}

	public void GoToContacts()
	{
		// Navigate("/contacts" );
	}

	public void GoToKeypad()
	{
		Navigate<KeypadTab>();
	}
}
