using System;
using Rp.Phone.Apps.FaceTime.Components;
using Rp.UI.Extensions;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	[Broadcast( NetPermission.HostOnly )]
	public async void ShowIncomingCallTabRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(ShowIncomingCallTabRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		_incomingSound?.Stop();
		_incomingSound = Sound.Play( "sounds/phone/facetime_calling.sound" );

		app.NavHost.Navigate<IncomingCallTab>( incomingCallInfo );
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void AcceptingCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(AcceptingCallRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		var app = Phone.Local.GetApp<FaceTimeApp>();
		var tab = app.NavHost.Navigate<CallTab>();

		var callService = Phone.Local.GetService<CallService>();

		while ( callService.CallInfo is null )
			await GameTask.Delay( 100 );

		tab.ShowCallView( callService.CallInfo );

		_incomingSound?.Stop();

		var voice = GetComponent<Voice>();
		voice.CreateVoiceCallMixer( incomingCallInfo.CallId );

		Sound.Play( "sounds/phone/facetime_accept.sound" );
		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallAccepted( incomingCallInfo ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void EndingCallRpcRequest( CallResult callResult )
	{
		Log.Info( "Ending call" );

		await StopCall( callResult );
		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallEnded( callResult ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void RejectingCallRpcRequest( CallResult callResult )
	{
		Log.Info( "Rejecting call" );

		await StopCall( callResult );
		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallRejected( callResult ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void CancelCallRpcResponse( Guid callId )
	{
		await Task.Delay( 2500 );

		_outgoingSound?.Stop();
		Sound.Play( "sounds/phone/phone_critical_error.sound" );

		await Task.Delay( 2000 );

		var app = Phone.Local.SwitchToApp<FaceTimeApp>();
		app.NavHost.Navigate<FavoriteTab>();

		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallFailed( callId ), true );
	}
}
