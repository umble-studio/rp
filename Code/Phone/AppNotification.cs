using System;

namespace Rp.Phone;

public record struct AppNotification
{
	public IPhoneApp App { get; set; }
	public string Title { get; init; }
	public string? Message { get; init; }
	public DateTime Date { get; init; }
}
