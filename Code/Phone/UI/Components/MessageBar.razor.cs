using System;
using Rp.UI;
using Sandbox.Razor;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class MessageBar : Panel
{
	public RenderFragment Content { get; set; } = null!;
	public PhoneTheme Theme { get; set; } = PhoneTheme.Light;
	public new Action? OnBack { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "light", Theme is PhoneTheme.Light )
		.AddClass( "transparent", Theme is PhoneTheme.Transparent )
		.Build();

	private void Back()
	{
		OnBack?.Invoke();
	}

	protected override int BuildHash() => HashCode.Combine( Content, Theme );
}
