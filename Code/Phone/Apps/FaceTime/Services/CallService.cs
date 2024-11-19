using System;
using System.Threading.Tasks;
using Rp.Phone.Apps.FaceTime.Components;
using Rp.UI.Extensions;
using Sandbox.Audio;

namespace Rp.Phone.Apps.FaceTime.Services;

public sealed partial class CallService : Component, IPhoneService
{
	private SoundHandle? _outgoingSound;
	private SoundHandle? _incomingSound;

	/// <summary>
	/// The current call info, if the phone is in a call.
	/// </summary>
	[HostSync]
	public CallSession? CallInfo { get; set; }

	/// <summary>
	/// Whether the phone is waiting for an outgoing call response.
	/// </summary>
	[HostSync]
	public bool IsOutgoingCallCallPending { get; set; }

	/// <summary>
	/// Whether the phone is waiting for an incoming call response.
	/// </summary>
	[HostSync]
	public bool IsIncomingCallPending { get; set; }

	/// <summary>
	/// Is the phone currently occupied with a call ?
	/// </summary>
	[HostSync]
	public bool IsOccupied { get; set; }

	public bool IsCalling => CallInfo is not null;

	/// <summary>
	/// Starts an outgoing call
	/// </summary>
	/// <param name="target">The target to call.</param>
	public void StartOutgoingCall( PhoneNumber target )
	{
		Log.Info( "StartCall: " + IsOccupied );
		if ( Phone.Local.SimCard?.PhoneNumber == target || IsOccupied ) return;

		var me = Phone.Local.SimCard?.PhoneNumber;
		if ( me is null ) return;

		using var _ = Rpc.FilterInclude( x => x.IsHost );

		var incomingCallInfo = new IncomingCallRequest
		{
			CallId = Guid.NewGuid(), Caller = me.Value, Callee = target, CreatedAt = DateTime.Now
		};

		_outgoingSound?.Stop();
		_outgoingSound = Sound.Play( "sounds/phone/facetime_calling.sound" );

		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallStarted( incomingCallInfo.CallId ), true );
		CallManager.StartCallRpcRequest( incomingCallInfo );
	}

	/// <summary>
	/// Ends the outgoing call.
	/// </summary>
	public void EndOutgoingCall()
	{
		if ( CallInfo == null || CallInfo.CallId == Guid.Empty )
		{
			Log.Error( "CurrentCallId is null or empty" );
			return;
		}

		using var _ = Rpc.FilterInclude( x => x.IsHost );
		CallManager.EndCallRpcRequest( CallInfo.CallId, Phone.Local.SimCard!.PhoneNumber );
	}


	/// <summary>
	/// Stops the call.
	/// </summary>
	/// <param name="callResult">The result of the call.</param>
	/// <returns></returns>
	private async Task StopCall( CallResult callResult )
	{
		_incomingSound?.Stop();

		var voice = GetComponent<Voice>();
		voice.DestroyVoiceCallMixer( callResult.CallId );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		Sound.Play( "sounds/phone/facetime_call_end.sound" );
		app.NavHost.Navigate<FavoriteTab>();
	}
}
