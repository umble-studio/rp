using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class StatusBar : Panel
{
	public PhoneTheme BackgroundPhoneTheme { get; set; } = PhoneTheme.Light;
	public PhoneTheme TextPhoneTheme { get; set; } = PhoneTheme.Light;

	private DateTime Date => DateTime.Now;

	private string TextCss => new CssBuilder()
		.AddClass( "light", () => TextPhoneTheme == PhoneTheme.Light )
		.AddClass( "dark", () => TextPhoneTheme == PhoneTheme.Dark )
		.Build();

	private string BackgroundCss => new CssBuilder()
		.AddClass( "light", () => BackgroundPhoneTheme == PhoneTheme.Light )
		.AddClass( "dark", () => BackgroundPhoneTheme == PhoneTheme.Dark )
		.Build();

	public void SetDefault()
	{
		BackgroundPhoneTheme = PhoneTheme.Light;
		TextPhoneTheme = PhoneTheme.Dark;
	}

	protected override int BuildHash() => HashCode.Combine( Date, BackgroundPhoneTheme, TextPhoneTheme );
}
