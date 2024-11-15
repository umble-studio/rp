using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class Button : Panel
{
	public string Text { get; set; } = null!;
	public string Icon { get; set; } = null!;
	public string IconColor { get; set; } = "dodgerblue";
	public PhoneTheme Theme { get; set; } = PhoneTheme.Light;
	public Action? Action { get; set; }

	private string Root => new CssBuilder()
		.AddClass("light", Theme is PhoneTheme.Light)
		.AddClass("dark", Theme is PhoneTheme.Dark)
		.AddClass("transparent", Theme is PhoneTheme.Transparent)
		.Build();
	
	protected override void OnClick( MousePanelEvent e )
	{
		Action?.Invoke();
	}

	protected override int BuildHash() => HashCode.Combine( Text, Icon, IconColor, Theme );
}
