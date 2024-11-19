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

	/// <summary>
	/// A temporary call id, used to identify the call until the call info is set.
	/// </summary>
	[HostSync]
	public Guid? TempCallId { get; set; }
	
	/// <summary>
	/// Whether the phone is currently calling someone.
	/// </summary>
	public bool IsCalling => CallInfo is not null;

	/// <summary>
	/// Gets the local instance of the <see cref="CallService"/>.
	/// </summary>
	/// <remarks>
	/// This is a shortcut for <see cref="Phone.Local.GetService{CallService}"/>.
	/// </remarks>
	public static CallService Local => Phone.Local.GetService<CallService>();
	
	/// <summary>
	/// Starts an outgoing call
	/// </summary>
	/// <param name="target">The target to call.</param>
	public bool StartOutgoingCall( PhoneNumber target )
	{
		Log.Info( "StartCall: " + IsOccupied );

		if ( Phone.Local.SimCard?.PhoneNumber == target || IsOccupied )
			return false;

		var me = Phone.Local.SimCard?.PhoneNumber;

		if ( me is null )
			return false;

		using var _ = Rpc.FilterInclude( x => x.IsHost );

		var incomingCallInfo = new IncomingCallRequest
		{
			CallId = Guid.NewGuid(), Caller = me.Value, Callee = target, CreatedAt = DateTime.Now
		};

		TempCallId = incomingCallInfo.CallId;

		_outgoingSound?.Stop();
		_outgoingSound = Sound.Play( "sounds/phone/facetime_calling.sound" );

		Scene.RunEvent<IFaceTimeEvent>( x => x.OnCallStarted( incomingCallInfo ), true );
		CallManager.StartCallRpcRequest( incomingCallInfo );

		return true;
	}

	/// <summary>
	/// Ends the outgoing call.
	/// </summary>
	public void EndOutgoingCall()
	{
		using var _ = Rpc.FilterInclude( x => x.IsHost );

		if ( TempCallId is not null )
		{
			CallManager.EndCallRpcRequest( TempCallId.Value, Phone.Local.SimCard!.PhoneNumber );
			return;
		}

		if ( CallInfo is null || CallInfo.CallId == Guid.Empty ) return;
		CallManager.EndCallRpcRequest( CallInfo.CallId, Phone.Local.SimCard!.PhoneNumber );
	}

	/// <summary>
	/// Stops the call.
	/// </summary>
	/// <param name="callResult">The result of the call.</param>
	/// <returns></returns>
	private async Task StopCall( CallResult callResult )
	{
		var voice = GetComponent<Voice>();
		voice.DestroyVoiceCallMixer( callResult.CallId );

		// Switch to FaceTime app and wait a second to make sure the app is loaded
		var app = Phone.Local.SwitchToApp<FaceTimeApp>();

		while ( !app.IsInitialized )
			await GameTask.Delay( 1 );

		_outgoingSound?.Stop();
		_incomingSound?.Stop();
		
		Sound.Play( "sounds/phone/facetime_call_end.sound" );
		app.NavHost.Navigate<FavoriteTab>();
	}
}
