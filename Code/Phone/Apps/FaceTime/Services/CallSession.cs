using System;

namespace Rp.Phone.Apps.FaceTime.Services;

public record CallSession
{
	public Guid CallId { get; init; }
	public PhoneNumber Caller { get; init; }
	public PhoneNumber Callee { get; init; }
	public DateTime StartedAt { get; init; }
}
