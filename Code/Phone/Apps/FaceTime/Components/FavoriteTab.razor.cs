using System;
using Rp.Phone.Apps.FaceTime.Services;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class FavoriteTab : NavigationPage
{
	private MessageBar _navigationBar = null!;

	public override string PageName => "Favorites";

	private List<PhoneContact> Contacts => Phone.Local.Contacts.All;

	private void OnSelectContact( PhoneContact contact )
	{
		var callService = Phone.Local.GetService<CallService>();

		var canCall = callService.StartOutgoingCall( contact.ContactNumber );
		if ( !canCall ) return;

		var tab = Host.Navigate<CallTab>();
		tab.ShowPendingCallView( contact );
	}

	protected override int ShouldRender() => HashCode.Combine( base.ShouldRender(), Contacts, Contacts.Count );
}
