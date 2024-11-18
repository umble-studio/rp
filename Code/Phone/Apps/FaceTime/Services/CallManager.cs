using System;
using Bindery;

namespace Rp.Phone.Apps.FaceTime.Services;

public sealed partial class CallManager : Singleton<CallManager>, Component.INetworkListener
{
	private readonly Dictionary<Guid, IncomingCallRequest> PendingIncomingCallsRequests = new();
	private readonly Dictionary<Guid, CallSession> Sessions = new();

	// private const int MaxPendingIncomingCallDuration = 11;

	// protected override void OnUpdate()
	// {
	// 	if ( Networking.IsHost )
	// 	{
	// 		CheckForOutdatedIncomingCallsRequests();
	// 	}
	// }
	//
	// private void CheckForOutdatedIncomingCallsRequests()
	// {
	// 	foreach ( var (callId, incomingCall) in PendingIncomingCallsRequests )
	// 	{
	// 		if ( DateTime.Now - incomingCall.CreatedAt > TimeSpan.FromSeconds( MaxPendingIncomingCallDuration ) )
	// 		{
	// 			Log.Info( "Removing outdated incoming call request: " + callId );
	//
	// 			// CancelPendingOutgoingCallRpcResponse( callId );
	// 			PendingIncomingCallsRequests.Remove( callId );
	// 		}
	// 	}
	// }

	public bool IsParticipantReadyToCall( PhoneNumber participantNumber )
	{
		var participantPhone = Scene.GetAllComponents<Phone>().FirstOrDefault( x =>
			x.SimCard is not null && x.SimCard.PhoneNumber == participantNumber );

		if ( participantPhone is null )
			return false;

		var callService = participantPhone.GameObject.GetComponent<CallService>();
		return !callService.IsOccupied;
	}

	[ConCmd("phone_reset_calls")]
	private static void ResetCmd()
	{
		Instance.PendingIncomingCallsRequests.Clear();
		Instance.Sessions.Clear();

		foreach ( var phone in Instance.Scene.GetAllComponents<Phone>() )
		{
			var callService = phone.GameObject.GetComponent<CallService>();
			callService.IsOutgoingCallCallPending = false;
			callService.IsIncomingCallPending = false;
			callService.IsOccupied = false;
			callService.CallInfo = default;
		}
	}
}
