using System;
using Rp.Phone.UI.Components;

namespace Rp.Phone.Apps;

public sealed partial class Launcher : PhoneApp, IPhoneEvent
{
	private AppDock _dock = null!;

	public override string AppName => "launcher";
	public override string AppTitle => "Launcher";
	public override string? AppIcon => null;
	public override bool ShowAppInLauncher => false;

	private List<IPhoneApp> Apps { get; set; } = new();

	protected override void OnAfterRender( bool firstRender )
	{
		if ( !firstRender ) return;

		DockDefaultApps();
		Apps = Phone.Apps.Where( x => x.ShowAppInLauncher && !IsDocked( x ) ).ToList();
	}

	public void DockApp( IPhoneApp app )
	{
		_dock.DockApp( app );
	}

	public void UndockApp( IPhoneApp app )
	{
		_dock.UndockApp( app );
	}

	public bool IsDocked( IPhoneApp app ) => _dock?.IsDocked( app ) ?? false;

	private void DockDefaultApps()
	{
		DockApp( GetApp<MessagesApp>()! );
		DockApp( GetApp<CallApp>()! );
	}

	public T? GetApp<T>() where T : IPhoneApp => Phone.Apps.OfType<T>().FirstOrDefault();

	public void OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.StatusBar.TextPhoneTheme = PhoneTheme.Light;
	}

	protected override int ShouldRender() => HashCode.Combine( base.ShouldRender(), _dock, Apps, Apps.Count );
}
