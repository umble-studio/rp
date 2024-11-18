using System;
using Rp.Phone.Apps.FaceTime.Components;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	[Broadcast( NetPermission.HostOnly )]
	public void CancelPendingOutgoingCallRpcResponse( Guid callId )
	{
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void ShowIncomingCallTabRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(ShowIncomingCallTabRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		app.NavHost.Navigate<IncomingCallTab>( incomingCallInfo );
	}
	
	[Broadcast( NetPermission.HostOnly )]
	public async void EndingCallRpcRequest( CallResult callResult )
	{
		Log.Info("Ending call");
		
		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		app.NavHost.Navigate<FavoriteTab>();
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void CancelCallRpcResponse()
	{
		await Task.Delay( 2500 );

		_callingSound?.Stop();
		Sound.Play( "sounds/phone/phone_critical_error.sound" );
	}
}
