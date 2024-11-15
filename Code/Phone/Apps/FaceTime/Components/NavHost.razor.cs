using System.ComponentModel;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class NavHost : NavigationHost
{
	protected override Panel Container { get; set; } = null!;
	
	private bool IsFavoriteTab => CurrentPage is FavoriteTab;
	private bool IsRecentTab => CurrentPage is RecentTab;
	private bool IsContactsTab => CurrentPage is ContactsTab;
	private bool IsKeypadTab => CurrentPage is KeypadTab;
	
	public NavHost()
	{
		DefaultPage = typeof(FavoriteTab);

		RegisterPage<FavoriteTab>();
		RegisterPage<RecentTab>();
		RegisterPage<ContactsTab>();
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
		Navigate<RecentTab>();
	}

	public void GoToContacts()
	{
		Navigate<ContactsTab>();
	}

	public void GoToKeypad()
	{
		Navigate<KeypadTab>();
	}
}
