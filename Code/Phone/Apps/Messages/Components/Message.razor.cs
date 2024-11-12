using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class Message : Panel
{
	public ConversationParticipant Participant { get; set; } = null!;
	public MessageData Data { get; set; } = null!;

	private string Root => new CssBuilder()
		.AddClass( "me", Data.IsMe() )
		.AddClass( "other", !Data.IsMe() )
		.Build();

	protected override int BuildHash() => HashCode.Combine( Data, DateTime.Now.Second );
}
