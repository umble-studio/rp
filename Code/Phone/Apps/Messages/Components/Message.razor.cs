using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.Apps.Messages.Components;

public sealed partial class Message : PhoneWidget
{
	public ConversationParticipant Participant { get; set; } = null!;
	public MessageData Data { get; set; } = null!;
	public bool ShowDate { get; set; }

	private PhoneNumber PhoneNumber { get; set; }
	
	private string Root => new CssBuilder()
		.AddClass( "me", Data.IsMe( PhoneNumber ) )
		.AddClass( "other", !Data.IsMe( PhoneNumber ) )
		.AddClass( "space", ShowDate )
		.Build();
	
	protected override void OnAfterRender( bool firstRender )
	{
		Log.Info("OnAfterRender");
		PhoneNumber = Phone.SimCard!.PhoneNumber;
	}

	protected override int ShouldRender() => HashCode.Combine( Data, ShowDate, DateTime.Now.Second );
}
