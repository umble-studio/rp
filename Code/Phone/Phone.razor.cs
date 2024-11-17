using System;
using Rp.Core.Managers;
using Rp.Phone.Apps;
using Rp.Phone.Extensions;
using Rp.Phone.UI.Components;
using Sandbox.UI;
using ControlCenter = Rp.Phone.UI.Components.ControlCenter;
using SteamId = Rp.Core.SteamId;

namespace Rp.Phone;

public sealed partial class Phone : PanelComponent, IPhoneEvent, Component.INetworkListener
{
	private Panel _phoneContent = null!;
	private Panel _appContainer = null!;
	private Keyboard _keyboard = null!;
	private ControlCenter _controlCenter = null!;
	private StatusBar _statusBar = null!;
	private IPhoneApp? _currentApp;
	private bool _isInitialized;

	public List<IPhoneApp> Apps { get; } = new();

	public StatusBar StatusBar => _statusBar;
	public Keyboard Keyboard => _keyboard;

	public static Phone Local => Game.ActiveScene.GetAllComponents<Phone>()
		.FirstOrDefault( x => x.Network.Owner == Connection.Local )!;

	public Phone()
	{
		Notification = new NotificationCenter( this );
	}

	// public void OnActive( Connection channel )
	// {
	// 	// We need to register all services when we join the server
	// 	// Without this, phone services are not synced on other clients, so the service doesn't exist
	// 	if ( Connection.Host == channel )
	// 		RegisterAllServices();
	// }

	// Temporary code to give the ownership of the phone to the client (only for testing purposes)
	// TODO - Need to be refactored later
	// public void OnActive( Connection channel )
	// {
	// 	Network.AssignOwnership( channel );
	// }

	protected override async void OnUpdate()
	{
		if ( Network.IsProxy ) return;
		
		if ( Input.Pressed( "phone" ) )
			IsOpen = !_isOpen;

		if ( _isOpen && !_isInitialized )
		{
			_isInitialized = true;

			await CharacterManager.Instance.WaitForCharacterInitialization();

			Log.Info( "is character loaded: " + CharacterManager.Instance.Current );

			LoadSimCard( SteamId.Local, CharacterManager.Instance.Current.CharacterId );

			// TODO - Refactor this code to be cleaner
			while ( !_isSimCardLoaded )
				await GameTask.Delay( 100 );

			Log.Info( "Sim card loaded" );

			LoadContacts();
			CreateAppInstances();

			// Wait 1ms to be sure the app container is valid before switching to any app
			while ( _appContainer is null )
				await GameTask.Delay( 1 );

			// RegisterAllServices();
			// ConversationService.Instance.LoadConversations();
			SwitchToApp<LockScreen>();
		}
	}

	private void CreateAppInstances()
	{
		var apps = PhoneExtensions.GetApps();

		foreach ( var app in apps )
		{
			var instance = PhoneExtensions.CreateAppInstance( app.TargetType );
			instance.Phone = this;

			Apps.Add( instance );
			Log.Info( "Registered app: " + app.Name );
		}
	}

	private void OnMainMenu()
	{
		if ( _isLocked )
		{
			Unlock();
		}
		else
		{
			if ( _currentApp is Launcher )
			{
				Lock();
				return;
			}

			SwitchToApp<Launcher>();
		}
	}

	public void SwitchToApp<T>() where T : IPhoneApp
	{
		_currentApp?.CloseApp();
		_currentApp = GetApp<T>();

		var panel = _currentApp as Panel;

		if ( !panel.IsValid() )
			RefreshAppInstance( _currentApp!, out panel );

		History.Push( _currentApp! );

		_appContainer.AddChild( panel );
		_currentApp?.OpenApp();
	}

	public void SwitchToApp( IPhoneApp app )
	{
		_currentApp?.CloseApp();
		_currentApp = app;

		var panel = _currentApp as Panel;

		if ( !panel.IsValid() )
			RefreshAppInstance( _currentApp, out panel );

		History.Push( _currentApp! );

		_appContainer.AddChild( panel );
		_currentApp?.OpenApp();
	}

	public void RefreshAppInstance( IPhoneApp app, out Panel panel )
	{
		Apps.RemoveAll( x => x.AppName == app.AppName );

		_currentApp = PhoneExtensions.CreateAppInstance( app.GetType() );
		_currentApp.Phone = this;

		panel = (Panel)_currentApp;
		Apps.Add( _currentApp );
	}

	public T GetApp<T>() where T : IPhoneApp => Apps.OfType<T>().First();

	protected override int BuildHash() => HashCode.Combine( _isLocked, _isOpen, _currentApp );
}
