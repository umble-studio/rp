using System;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class FavoriteTab : NavigationPage
{
	private MessageBar _navigationBar = null!;
	
	public override string PageName => "Favorites";
	
	[CascadingProperty("Phone")] public Phone Phone { get; set; } = null!;

	private void OnSelectContact( PhoneContact contact )
	{
		Host.Navigate<CallTab>( contact );
	}

	protected override int ShouldRender() => HashCode.Combine( Phone.Contacts.All );
}
