using System;
using Rp.UI;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class KeyboardKey : Panel, IKeyboardEvent
{
	public new KeyStyle Style { get; set; } = KeyStyle.Light;
	public string Key { get; set; } = string.Empty;
	public string Icon { get; set; } = string.Empty;
	public int IconSize { get; set; } = 30;
	public bool Expand { get; set; } = false;

	private bool Pressed { get; set; }

	private string Root => new CssBuilder()
		.AddClass( "light", Style is KeyStyle.Light )
		.AddClass( "dark", Style is KeyStyle.Dark )
		.AddClass( "key", Key.Length is 1 )
		.AddClass( "word", Key.Length > 1 || !string.IsNullOrEmpty( Icon ) )
		.AddClass( "padding", Key.Length > 1 && string.IsNullOrEmpty( Icon ) )
		.AddClass( "expand", Expand )
		.AddClass( "pressed", Pressed )
		.Build();

	async void IKeyboardEvent.OnKeyboardKeyPressed( string key )
	{
		Sound.Play( "sounds/phone/keyboard_touch.sound" );
		
		if ( Key != key ) return;
		
		Pressed = true;
		await GameTask.Delay( 100 );
		Pressed = false;
	}

	protected override int BuildHash() => HashCode.Combine( Style, Key, Icon, IconSize, Expand, Pressed );

	public enum KeyStyle
	{
		Light,
		Dark,
	}
}
