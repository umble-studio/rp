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
				CallService.CancelCallRpcResponse( incomingCallInfo.CallId );
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
		callService.TempCallId = incomingCallInfo.CallId;

		// We tell the other phone that we are waiting for a call response
		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = true;
		callService.TempCallId = incomingCallInfo.CallId;

		// Sending the incoming call request to the target phone
		var targetConnection = secondPhone.Network.Owner;

		using ( Rpc.FilterInclude( x => x == targetConnection ) )
			CallService.ShowIncomingCallTabRpcRequest( incomingCallInfo );
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

		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Callee );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Error( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		Log.Info( "Accept incoming call: " + request.CallRequest.CallId );

		// We set the current call state as occupied for both participants
		var callInfo = new CallSession()
		{
			CallId = callId,
			Caller = request.CallRequest.Caller,
			Callee = request.CallRequest.Callee,
			StartedAt = DateTime.Now
		};

		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = false;
		callService.IsOccupied = true;
		callService.CallInfo = callInfo;
		callService.TempCallId = null;

		CallService.AcceptingCallRpcRequest( request.CallRequest );

		// Just wait 1ms to be sure the mixer is created and synced on the other phone owner
		await GameTask.Delay( 1 );

		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = false;
		callService.IsOccupied = true;
		callService.CallInfo = callInfo;
		callService.TempCallId = null;

		Instance.Sessions.Add( callInfo.CallId, (callInfo, request.Connections) );
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

		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Caller );
		var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Callee );

		if ( firstPhone is null || secondPhone is null )
		{
			Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
			return;
		}

		Log.Info( "Reject incoming call from phone: " + request.CallRequest.Caller );

		// We set the current call state as not occupied for both participants
		var callService = firstPhone.GetService<CallService>();
		callService.IsOutgoingCallCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;
		callService.TempCallId = null;

		callService = secondPhone.GetService<CallService>();
		callService.IsIncomingCallPending = false;
		callService.IsOccupied = false;
		callService.CallInfo = null;
		callService.TempCallId = null;

		var callResult = new CallResult
		{
			CallId = callId,
			Caller = request.CallRequest.Caller,
			Callee = request.CallRequest.Callee,
			StartedAt = request.CallRequest.CreatedAt,
			EndedAt = DateTime.Now,
			Reason = CallResult.ReasonType.EndedByCallee
		};

		using ( Rpc.FilterInclude( x => request.Connections.Contains( x ) ) )
			CallService.EndingCallRpcRequest( callResult );
	}

	[Broadcast( NetPermission.Anyone )]
	public static void EndCallRpcRequest( Guid callId, PhoneNumber caller )
	{
		if ( !Networking.IsHost ) return;

		var phones = Instance.Scene.GetAllComponents<Phone>()
			.Where( x => x.Network.Active && x.SimCard is not null )
			.ToList();

		if ( Instance.PendingIncomingCallsRequests.Remove( callId, out var request ) )
		{
			var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Caller );
			var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == request.CallRequest.Callee );

			if ( firstPhone is null || secondPhone is null )
			{
				Log.Info( "One of the phones is null: " + (firstPhone is null) + ", " + (secondPhone is null) );
				return;
			}

			var reason = request.CallRequest.Caller == caller
				? CallResult.ReasonType.EndedByCaller
				: CallResult.ReasonType.EndedByCallee;

			var callResult = new CallResult
			{
				CallId = callId,
				Caller = request.CallRequest.Caller,
				Callee = request.CallRequest.Callee,
				StartedAt = request.CallRequest.CreatedAt,
				EndedAt = DateTime.Now,
				Reason = reason
			};

			using ( Rpc.FilterInclude( x => request.Connections.Contains( x ) ) )
				CallService.EndingCallRpcRequest( callResult );
		}
		else if ( Instance.Sessions.Remove( callId, out var session ) )
		{
			var firstPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == session.CallSession.Caller );
			var secondPhone = phones.FirstOrDefault( x => x.SimCard!.PhoneNumber == session.CallSession.Callee );

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
			callService.TempCallId = null;

			callService = secondPhone.GetService<CallService>();
			callService.IsIncomingCallPending = false;
			callService.IsOccupied = false;
			callService.CallInfo = null;
			callService.TempCallId = null;

			var reason = session.CallSession.Caller == caller
				? CallResult.ReasonType.EndedByCaller
				: CallResult.ReasonType.EndedByCallee;

			var callResult = new CallResult
			{
				CallId = callId,
				Caller = session.CallSession.Caller,
				Callee = session.CallSession.Callee,
				StartedAt = session.CallSession.StartedAt,
				EndedAt = DateTime.Now,
				Reason = reason
			};

			using ( Rpc.FilterInclude( x => session.Connections.Any( c => c.SteamId == x.SteamId ) ) )
				CallService.EndingCallRpcRequest( callResult );
		}
		else
		{
			Log.Info( $"{nameof(EndCallRpcRequest)}: Call not found: " + callId );
		}
	}
}
