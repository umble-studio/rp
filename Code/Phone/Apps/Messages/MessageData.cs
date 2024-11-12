using System;

namespace Rp.Phone.Apps.Messages;

public record MessageData
{
	public MessageAuthor Author { get; init; }
	public string Content { get; init; } = null!;
	public DateTime Date { get; init; } = DateTime.Now;
}
