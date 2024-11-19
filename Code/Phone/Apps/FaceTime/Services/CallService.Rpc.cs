using System;
using Rp.Phone.Apps.FaceTime.Components;
using Rp.UI.Extensions;

namespace Rp.Phone.Apps.FaceTime.Services;

public partial class CallService
{
	[Broadcast( NetPermission.HostOnly )]
	public static async void ShowIncomingCallTabRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(ShowIncomingCallTabRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		Local._incomingSound?.Stop();
		Local._incomingSound = Sound.Play( "sounds/phone/facetime_calling.sound" );

		app.NavHost.Navigate<IncomingCallTab>( incomingCallInfo );
	}

	[Broadcast( NetPermission.HostOnly )]
	public static async void AcceptingCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(AcceptingCallRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		var app = Phone.Local.GetApp<FaceTimeApp>();
		var tab = app.NavHost.Navigate<CallTab>();

		var callService = Phone.Local.GetService<CallService>();

		while ( callService.CallInfo is null )
			await GameTask.Delay( 100 );

		tab.ShowCallView( callService.CallInfo );

		Local._incomingSound?.Stop();

		var voice = Local.GetComponent<Voice>();
		voice.CreateVoiceCallMixer( incomingCallInfo.CallId );

		Sound.Play( "sounds/phone/facetime_accept.sound" );
		Local.Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallAccepted( incomingCallInfo ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public static async void EndingCallRpcRequest( CallResult callResult )
	{
		Log.Info( "Ending call" );

		await Local.StopCall( callResult );
		Local.Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallEnded( callResult ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public static async void RejectingCallRpcRequest( CallResult callResult )
	{
		Log.Info( "Rejecting call" );

		await Local.StopCall( callResult );
		Local.Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallRejected( callResult ), true );
	}

	[Broadcast( NetPermission.HostOnly )]
	public static async void CancelCallRpcResponse( Guid callId )
	{
		await Local.Task.Delay( 2500 );

		Local._outgoingSound?.Stop();
		Sound.Play( "sounds/phone/phone_critical_error.sound" );

		await Local.Task.Delay( 2000 );

		var app = Phone.Local.SwitchToApp<FaceTimeApp>();
		app.NavHost.Navigate<FavoriteTab>();
		
		Local.Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallFailed( callId ), true );
	}
}
