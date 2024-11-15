using System;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class FavoriteTab : NavigationPage
{
	public override string PageName => "Favorites";
	private void OnSelectContact( PhoneContact contact )
	{
		Host.Navigate<CallTab>( contact );
	}

	protected override int BuildHash() => HashCode.Combine( Phone.Current.Contacts.All );
}
