using System;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class App : PhoneApp, IPhoneEvent, IAppNotifiable, IAppNotifiable<MessagesApp>, IKeyboardEvent, INavigationEvent
{
	private NavHost _navHost = null!;
	private string _pageName = string.Empty;
	
	public override string AppName => "facetime";
	public override string AppTitle => "FaceTime";
	public override string AppIcon => "textures/ui/phone/app_facetime.png";
	public override string? AppNotificationIcon => "fluent:comment-48-filled";
	
	public NavHost NavHost => _navHost;
	
	void IPhoneEvent.OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.StatusBar.TextPhoneTheme = PhoneTheme.Dark; 
		Phone.StatusBar.BackgroundPhoneTheme = PhoneTheme.Light;
	}

	void INavigationEvent.OnNavigationOpen( INavigationPage page, params object[] args )
	{
		_pageName = page.PageName;
	}

	protected override int ShouldRender() => HashCode.Combine( base.ShouldRender(), _pageName, Phone.Keyboard?.IsOpen );
}
