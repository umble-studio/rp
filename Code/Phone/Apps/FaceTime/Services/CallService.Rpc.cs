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
	public void AcceptingCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(AcceptingCallRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		_incomingSound?.Stop();

		var voice = GetComponent<Voice>();
		voice.CreateVoiceCallMixer( incomingCallInfo.CallId );

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
