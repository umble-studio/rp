using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public record CallResult
{
	/// <summary>
	/// The participant that ended the call
	/// </summary>
	public PhoneNumber Caller { get; init; }
	
	/// <summary>
	/// The participant that started the call
	/// </summary>
	public PhoneNumber Callee { get; init; }

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
	public ReasonType Reason { get; init; }

	public enum ReasonType
	{
		EndedByCaller,
		EndedByCallee,
		NetworkError,
		Unknown
	}
}
