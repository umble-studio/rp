using System;
using Sandbox;
using Sandbox.UI;

namespace Iconify;

[Alias( "iconify" )]
public partial class IconifyPanel : Panel
{
	public static readonly BaseFileSystem? DefaultCache;

	private Texture? _svgTexture;

	public string Icon { get; set; } = null!;
	public string? Color { get; set; } = "white";
	public int Size { get; set; } = 32;
	public LengthUnit Unit { get; set; } = LengthUnit.Pixels;
	
	private string SizeStyle => $"{Size}{GetUnit()}";

	static IconifyPanel()
	{
		if ( FileSystem.Data is null )
			return;

		FileSystem.Data.CreateDirectory( "iconify" );
		DefaultCache = FileSystem.Data.CreateSubSystem( "iconify" );
	}

	private string GetUnit() => Unit switch
	{
		LengthUnit.Pixels => "px",
		LengthUnit.Percentage => "%",
		LengthUnit.Em => "em",
		LengthUnit.RootEm => "rem",
		LengthUnit.ViewWidth => "vw",
		LengthUnit.ViewHeight => "vh",
		LengthUnit.ViewMin => "vmin",
		LengthUnit.ViewMax => "vmax",
		_ => "px"
	};

	protected override void OnAfterTreeRender( bool firstTime ) => SetIcon();

	protected override int BuildHash() => HashCode.Combine( _svgTexture, Icon, Color, Size );

	private void SetIcon()
	{
		_svgTexture = Texture.White;

		var icon = new IconifyIcon( Icon );
		var rect = Box.Rect;
		var tintColor = Color ?? (ComputedStyle ?? Style)?.FontColor;

		icon.LoadTextureAsync( rect, tintColor ).ContinueWith( ( task ) =>
		{
			Log.Trace( $"Loaded icon {task.Result?.ResourcePath}" );
			_svgTexture = task.Result;
		} );
	}
}
