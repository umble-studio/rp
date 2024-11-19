using System;
using Rp.Phone.Apps.FaceTime.Components;
using Sandbox.Audio;

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

		app.NavHost.Navigate<IncomingCallTab>( incomingCallInfo );
	}

	[Broadcast( NetPermission.HostOnly )]
	public void AcceptingCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		Log.Info( $"{nameof(AcceptingCallRpcRequest)}: {incomingCallInfo.Caller}, {incomingCallInfo.Callee}" );

		_callingSound?.Stop();

		var mixerName = $"phone-{incomingCallInfo.CallId}";
		var mixer = Mixer.FindMixerByName( mixerName );

		if ( mixer is not null )
		{
			Log.Info( "Skipping mixer creation: " + mixerName );
			return;
		}

		mixer = Mixer.Master.AddChild();
		mixer.Name = mixerName;
		mixer.Occlusion = 0;
		mixer.Spacializing = 0;
		mixer.AirAbsorption = 0;
		mixer.DistanceAttenuation = 0;
		mixer.Solo = false;
		mixer.Mute = false;

		var voice = GetComponent<Voice>();
		voice.TargetMixer = mixer;
		voice.IsListening = true;
	}

	[Broadcast( NetPermission.HostOnly )]
	public async void EndingCallRpcRequest( CallResult callResult )
	{
		Log.Info( "Ending call" );
		
		var voice = GetComponent<Voice>();
		voice.IsListening = false;

		// Don't forget to destroy the mixer when the call ends
		var mixerName = $"phone-{callResult.CallId}";
		var mixer = Mixer.FindMixerByName( mixerName );
		mixer?.Destroy();

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
