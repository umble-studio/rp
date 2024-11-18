using System;
using Rp.Phone.Apps.FaceTime.Components;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	[Broadcast( NetPermission.HostOnly )]
	private void StartCallRpcResponse( Info callInfo )
	{
		// IsCalling = true;
	}

	[Broadcast( NetPermission.HostOnly )]
	private void EndCallRpcResponse( Guid callId )
	{
		// IsCalling = false;
	}

	[Broadcast( NetPermission.HostOnly )]
	private void CancelPendingOutcomingCallRpcResponse( Guid callId )
	{
	}

	[Broadcast( NetPermission.HostOnly )]
	private async void ShowIncomingCallTabRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(ShowIncomingCallTabRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Target}" );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		app.NavHost.Navigate<IncomingCallTab>( incomingCallInfo );
	}

	// [Broadcast( NetPermission.HostOnly )]
	// private void AcceptIncomingCallRpcResponse( Guid callId )
	// {
	// }

	[Broadcast( NetPermission.HostOnly )]
	private async void CancelCallRpcResponse()
	{
		await Task.Delay( 2500 );

		_callingSound?.Stop();
		Sound.Play( "sounds/phone/phone_critical_error.sound" );

		CanCall = false;
	}
}
