using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public sealed partial class CallManager
{
	[Broadcast( NetPermission.Anyone )]
	public static void StartCallRpcRequest( IncomingCallRequest incomingCallInfo )
	{
		if ( !Networking.IsHost ) return;

		var isReadyToCall = Instance.IsParticipantReadyToCall( incomingCallInfo.Caller );
		var isTargetReadyToCall = Instance.IsParticipantReadyToCall( incomingCallInfo.Callee );

		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallInfo.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallInfo.Callee );

		if ( isReadyToCall && !isTargetReadyToCall )
		{
			Log.Warning( "Cancel call: " + incomingCallInfo.Caller + ", " + incomingCallInfo.Callee );

			using ( Rpc.FilterInclude( x => x == firstPhone!.Network.Owner ) )
				firstPhone!.GetService<CallService>().CancelCallRpcResponse( incomingCallInfo.CallId );
		}

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Warning( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		Log.Info( "Add pending incoming call request: " + incomingCallInfo.CallId );

		var connections = new List<Connection> { firstPhone.Network.Owner, secondPhone.Network.Owner };
		Instance.PendingIncomingCallsRequests[incomingCallInfo.CallId] = (incomingCallInfo, connections);

		// The other phone is now waiting for the call to be accepted
		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = true;

		// We tell the other phone that we are waiting for a call response
		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = true;

		// Sending the incoming call request to the target phone
		var targetConnection = secondPhone.Network.Owner;

		using ( Rpc.FilterInclude( x => x == targetConnection ) )
			callService.ShowIncomingCallTabRpcRequest( incomingCallInfo );
	}

	[Broadcast( NetPermission.Anyone )]
	public static async void AcceptIncomingCallRpcRequest( Guid callId )
	{
		if ( !Networking.IsHost ) return;

		if ( !Instance.PendingIncomingCallsRequests.Remove( callId, out var request ) )
		{
			Log.Error( $"{nameof(AcceptIncomingCallRpcRequest)}: Call not found: " + callId );
			return;
		}
		
		var incomingCallRequest = request.CallRequest;
		
		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Callee );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Error( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		Log.Info( "Accept incoming call: " + incomingCallRequest.CallId );

		// We set the current call state as occupied for both participants
		var callInfo = new CallSession()
		{
			CallId = callId,
			Caller = incomingCallRequest.Caller,
			Callee = incomingCallRequest.Callee,
			StartedAt = DateTime.Now
		};

		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = false;
		callService.IsOccupied = true;
		callService.CallInfo = callInfo;
		callService.AcceptingCallRpcRequest( incomingCallRequest );

		// Just wait 1ms to be sure the mixer is created and synced on the other phone owner
		await GameTask.Delay( 1 );

		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = false;
		callService.IsOccupied = true;
		callService.CallInfo = callInfo;
		callService.AcceptingCallRpcRequest( incomingCallRequest );

		Instance.Sessions.Add( callInfo.CallId, callInfo );
	}

	[Broadcast( NetPermission.Anyone )]
	public static void RejectIncomingCallRpcRequest( Guid callId )
	{
		if ( !Networking.IsHost ) return;

		if ( !Instance.PendingIncomingCallsRequests.Remove( callId, out var request ) )
		{
			Log.Info( $"{nameof(RejectIncomingCallRpcRequest)}: Call not found: " + callId );
			return;
		}

		var incomingCallRequest = request.CallRequest;
		
		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == incomingCallRequest.Callee );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		Log.Info( "Reject incoming call from phone: " + incomingCallRequest.Caller );

		// We set the current call state as not occupied for both participants
		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;

		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;

		var targets = new List<Connection> { firstPhone.Network.Owner, secondPhone.Network.Owner };

		var callResult = new CallResult
		{
			CallId = callId,
			Caller = incomingCallRequest.Caller,
			Callee = incomingCallRequest.Callee,
			StartedAt = incomingCallRequest.CreatedAt,
			EndedAt = DateTime.Now,
			Reason = CallResult.ReasonType.EndedByCallee
		};

		using ( Rpc.FilterInclude( x => targets.Contains( x ) ) )
			callService.EndingCallRpcRequest( callResult );
	}

	[Broadcast( NetPermission.Anyone )]
	public static void EndCallRpcRequest( Guid callId, PhoneNumber caller )
	{
		if ( !Networking.IsHost ) return;

		if ( !Instance.Sessions.Remove( callId, out var session ) )
		{
			Log.Info( $"{nameof(EndCallRpcRequest)}: Call not found: " + callId );
			return;
		}

		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == session.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == session.Callee );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		// We set the current call state as not occupied for both participants
		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;

		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;

		var reason = session.Caller == caller
			? CallResult.ReasonType.EndedByCaller
			: CallResult.ReasonType.EndedByCallee;

		var callResult = new CallResult
		{
			CallId = callId,
			Caller = session.Caller,
			Callee = session.Callee,
			StartedAt = session.StartedAt,
			EndedAt = DateTime.Now,
			Reason = reason
		};

		// Send the call result to both participants
		var connections = new List<Connection> { firstPhone.Network.Owner, secondPhone.Network.Owner };

		using ( Rpc.FilterInclude( x => connections.Any( c => c.SteamId == x.SteamId ) ) )
			callService.EndingCallRpcRequest( callResult );
	}
}
