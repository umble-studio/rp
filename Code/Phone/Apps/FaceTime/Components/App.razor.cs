using System;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class App : PhoneApp, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent, INavigationEvent
{
	private MessageBar _navigationBar = null!;
	// private Call _callTab = null!;

	private string _pageName = string.Empty;
	
	public override string AppName => "facetime";
	public override string AppTitle => "FaceTime";
	public override string AppIcon => "textures/ui/phone/app_facetime.png";
	public override string? AppNotificationIcon => "fluent:comment-48-filled";
	
	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;

		// SwitchToContacts();
	}

	// public void SwitchToCall( PhoneContact contact )
	// {
	// 	_callTab.Show( contact );
	// }
	//
	// public void SwitchToContacts()
	// {
	// 	_callTab.Hide();
	// }

	void IPhoneEvent.OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Dark;
		Phone.Current.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	void INavigationEvent.OnNavigationOpen( INavigationPage page, params object[] args )
	{
		_pageName = page.PageName;
	}

	// void IPhoneEvent.OnAppClosed( IPhoneApp app )
	// {
	// 	if ( app != this ) return;
	//
	// 	Phone.Current.Keyboard.Hide();
	// }

	protected override int BuildHash() => HashCode.Combine( _pageName, Phone.Current.Keyboard.IsOpen );
}
