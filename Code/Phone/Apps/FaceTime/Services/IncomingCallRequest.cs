using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public record IncomingCallRequest
{
	public Guid CallId { get; init; }
	public PhoneNumber Caller { get; init; }
	public PhoneNumber Callee { get; init; }
	public DateTime CreatedAt { get; init; }
}
