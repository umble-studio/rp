using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Alert : Panel
{
	private Panel _footer = null!;
	
	public string Title { get; set; }
	public string Message { get; set; }
	public List<Button> Buttons { get; set; }

	public Alert( string title, string message, List<Button> buttons )
	{
		Title = title;
		Message = message;
		Buttons = buttons;
	}

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;
		
		foreach ( var button in Buttons )
			_footer.AddChild( button );
	}

	protected override int BuildHash() => HashCode.Combine( Title, Message, Buttons.Count );
}
