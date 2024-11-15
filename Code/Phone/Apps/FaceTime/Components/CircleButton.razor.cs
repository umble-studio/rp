using System;
using Rp.UI;
using Sandbox.Diagnostics;
using Sandbox.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class CircleButton : Panel
{
	public string Icon { get; set; } = string.Empty;
	public string Text { get; set; } = string.Empty;
	public bool CanToggled { get; set; }
	public bool Toggled { get; set; }
	public Action? Clicked { get; set; }
	public Action<bool>? ToggleChanged { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "toggled", CanToggled && Toggled )
		.Build();

	protected override void OnClick( MousePanelEvent e )
	{
		if ( CanToggled )
		{
			Toggled = !Toggled;
			ToggleChanged?.Invoke( Toggled );
			return;
		}

		Clicked?.Invoke();
	}

	protected override int BuildHash() => HashCode.Combine( Icon, Text, CanToggled, Toggled );
}
