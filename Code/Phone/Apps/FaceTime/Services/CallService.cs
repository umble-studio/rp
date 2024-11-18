using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public sealed partial class CallService : Component, IPhoneService
{
	[HostSync] public Guid? CurrentCallId { get; private set; }
	[HostSync] public IncomingCallRequest? IncomingCall { get; private set; }
	[HostSync] public bool IsOutgoingCallCallPending { get; private set; }
	[HostSync] public bool IsIncomingCallPending { get; private set; }
	[HostSync] public bool IsOccupied { get; private set; }

	public bool CanCall { get; private set; } = true;
	public bool IsCalling { get; private set; }
	
	// public Info CallInfo { get; private set; }

	// protected override async void OnUpdate()
	// {
	// 	if ( Network.IsProxy ) return;
	//
	// 	var callService = Phone.Local.GetService<CallService>();
	//
	// 	if ( callService.IncomingCall is not null )
	// 	{
	// 		if ( _isCallInitialized ) return;
	// 		_isCallInitialized = true;
	//
	// 		Phone.Local.SwitchToApp<FaceTimeApp>();
	//
	// 		Log.Info( "Before" );
	// 		await GameTask.DelaySeconds( 1 );
	// 		Log.Info( "After" );
	//
	// 		var faceTimeApp = Phone.Local.GetApp<FaceTimeApp>();
	// 		var phoneContact = Phone.Local.Contacts.GetContactByNumber( callService.IncomingCall.Value.Caller );
	//
	// 		faceTimeApp.NavHost.Navigate<CallTab>( callService.IncomingCall.Value.Caller );
	// 	}
	// }

	protected override void OnUpdate()
	{
		if ( Networking.IsHost )
		{
			CheckForOutdatedIncomingCallsRequests();
		}
	}

	public readonly record struct IncomingCallRequest
	{
		public Guid CallId { get; init; }
		public PhoneNumber Caller { get; init; }
		public PhoneNumber Target { get; init; }
		public DateTime CreatedAt { get; init; }
	}

	public struct Participant
	{
		public PhoneContact Contact { get; set; }
		public bool IsVideoEnabled { get; set; }
		public bool IsMuted { get; set; }

		public bool IsSpeakerEnabled { get; set; }
		// public bool IsSpeaking { get; set; }
	}

	public record struct Info
	{
		public Guid CallId { get; init; }
		public List<Participant> Participants { get; init; }
		public DateTime StartedAt { get; init; }
	}

	public readonly struct End
	{
		/// <summary>
		/// The contact that ended the call
		/// </summary>
		public PhoneContact Contact { get; init; }

		/// <summary>
		/// The time the call started 
		/// </summary>
		public DateTime StartedAt { get; init; }

		/// <summary>
		/// The time the call ended
		/// </summary>
		public DateTime? EndedAt { get; init; }

		/// <summary>
		/// The reason the call ended
		/// </summary>
		public string Reason { get; init; }
	}
}
