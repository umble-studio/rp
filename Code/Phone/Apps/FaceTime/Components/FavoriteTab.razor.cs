using System;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class FavoriteTab : PhoneNavigationPage
{
	private MessageBar _navigationBar = null!;
	
	public override string PageName => "Favorites";

	private List<PhoneContact> Contacts { get; set; }
	
	protected override void OnAfterRender( bool firstRender )
	{
		Contacts = Phone.Contacts.All;
	}

	private void OnSelectContact( PhoneContact contact )
	{
		Host.Navigate<CallTab>( contact );
	}

	protected override int ShouldRender() => HashCode.Combine( Phone.Contacts.All );
}
