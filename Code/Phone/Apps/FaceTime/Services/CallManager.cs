using System;
using Bindery;

namespace Rp.Phone.Apps.FaceTime.Services;

public sealed partial class CallManager : Singleton<CallManager>, Component.INetworkListener
{
	private readonly Dictionary<Guid, (IncomingCallRequest CallRequest, List<Connection> Connections)>
		PendingIncomingCallsRequests = new();

	private readonly Dictionary<Guid, (CallSession CallSession, List<Connection> Connections)> Sessions = new();

	private const int MaxPendingIncomingCallDuration = 10;

	protected override void OnUpdate()
	{
		if ( !Networking.IsHost ) return;
		CheckForOutdatedIncomingCallsRequests();
	}

	private void CheckForOutdatedIncomingCallsRequests()
	{
		foreach ( var (callId, (incomingCall, connections)) in PendingIncomingCallsRequests )
		{
			if ( DateTime.Now - incomingCall.CreatedAt <=
			     TimeSpan.FromSeconds( MaxPendingIncomingCallDuration ) ) continue;

			Log.Info( "Removing outdated incoming call request: " + callId );

			var callResult = new CallResult
			{
				CallId = callId,
				Caller = incomingCall.Caller,
				Callee = incomingCall.Callee,
				StartedAt = incomingCall.CreatedAt,
				EndedAt = DateTime.Now,
				Reason = CallResult.ReasonType.NoResponse
			};

			var phones = Scene.GetAllComponents<Phone>().Where( x => x.Network.Active ).ToList();

			foreach ( var connection in connections )
			{
				var phone = phones.FirstOrDefault( x => x.Network.Owner == connection );
				if ( phone is null ) continue;

				var callService = phone.GetComponent<CallService>();
				callService.EndingCallRpcRequest( callResult );
			}

			PendingIncomingCallsRequests.Remove( callId );
		}
	}

	private bool IsParticipantReadyToCall( PhoneNumber participantNumber )
	{
		var participantPhone = Scene.GetAllComponents<Phone>().FirstOrDefault( x =>
			x.SimCard is not null && x.SimCard.PhoneNumber == participantNumber );

		if ( participantPhone is null )
			return false;

		var callService = participantPhone.GameObject.GetComponent<CallService>();
		return !callService.IsOccupied;
	}
}
