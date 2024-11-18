using System;
using Cascade;
using Rp.UI;
using Rp.UI.Extensions;
using Sandbox.UI;

namespace Rp.Phone;

public abstract class PhoneApp : CascadingPanel, IPhoneApp
{
	private bool _isOpen;
	private bool _isFocused;

	public abstract string AppName { get; }
	public abstract string AppTitle { get; }
	public abstract string? AppIcon { get; }
	public virtual string? AppNotificationIcon { get; } = null;
	public virtual bool ShowAppInLauncher { get; } = true;
	public bool IsInitialized { get; private set; }

	[CascadingProperty("Phone")] public Phone Phone { get; set; } = null!;

	protected override void OnAfterRender( bool firstRender )
	{
		if ( firstRender )
			IsInitialized = true;
	}

	public virtual void OpenApp()
	{
		_isOpen = true;
		FocusApp();

		if ( !Game.ActiveScene.IsValid() ) return;
		Game.ActiveScene.RunEvent<IPhoneEvent>( x => x.OnAppOpened( this ), true );
	}

	public virtual void CloseApp()
	{
		_isOpen = false;
		BlurApp();
		Delete();

		if ( !Game.ActiveScene.IsValid() ) return;
		Game.ActiveScene.RunEvent<IPhoneEvent>( x => x.OnAppClosed( this ), true );
	}

	public virtual void FocusApp()
	{
		_isFocused = true;

		if ( !Game.ActiveScene.IsValid() ) return;
		Game.ActiveScene.RunEvent<IPhoneEvent>( x => x.OnAppFocused( this ), true );
	}

	public virtual void BlurApp()
	{
		_isFocused = false;

		if ( !Game.ActiveScene.IsValid() ) return;
		Game.ActiveScene.RunEvent<IPhoneEvent>( x => x.OnAppBlurred( this ), true );
	}

	protected override int ShouldRender() => HashCode.Combine( _isOpen, _isFocused );
}
