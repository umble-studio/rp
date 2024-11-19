using System;
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
	
	public void StartCall( PhoneNumber target )
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
		CallManager.StartCallRpcRequest( incomingCallInfo );
	}

	public void EndCall()
	{
		if ( CallInfo == null || CallInfo.CallId == Guid.Empty )
		{
			Log.Error( "CurrentCallId is null or empty" );
			return;
		}

		using var _ = Rpc.FilterInclude( x => x.IsHost );
		CallManager.EndCallRpcRequest( CallInfo.CallId, Phone.Local.SimCard!.PhoneNumber );
	}
}
