using System;
using RoverDB.Attributes;

namespace Rp.Phone.Apps.Messages;

[Collection( "phone/conversations" )]
public record ConversationData
{
	[Id, Saved] public Guid Id { get; init; } = Guid.NewGuid();
	[Saved] public List<ConversationParticipant> Participants { get; init; } = new();
	[Saved] public List<MessageData> Messages { get; init; } = new();
	[Saved] public DateTime CreatedAt { get; init; } = DateTime.Now;
}
