using System;
using Rp.Phone.UI.Components;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class App : PhoneApp, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent
{
	private Call _callTab = null!;
	private Contacts _contactsTab = null!;

	public override string AppName => "facetime";
	public override string AppTitle => "FaceTime";
	public override string AppIcon => "textures/ui/phone/app_facetime.png";
	public override string? AppNotificationIcon => "fluent:comment-48-filled";

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;

		SwitchToContacts();
	}

	public void SwitchToCall( PhoneContact contact )
	{
		_contactsTab.Hide();
		_callTab.Show( contact );
	}

	public void SwitchToContacts()
	{
		_callTab.Hide();
		_contactsTab.Show();
	}

	void IPhoneEvent.OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		Phone.Current.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	// void IPhoneEvent.OnAppClosed( IPhoneApp app )
	// {
	// 	if ( app != this ) return;
	//
	// 	Phone.Current.Keyboard.Hide();
	// }

	protected override int BuildHash() => HashCode.Combine( Phone.Current.Keyboard.IsOpen );
}
