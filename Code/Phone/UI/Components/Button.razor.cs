using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Button : Panel
{
	public string Text { get; set; } = null!;
	public string Icon { get; set; } = null!;
	public Action? Action { get; set; }

	protected override void OnClick( MousePanelEvent e )
	{
		Action?.Invoke();
	}

	protected override int BuildHash() => HashCode.Combine( Text, Icon );
}
