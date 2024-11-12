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

	private List<IPhoneApp> Apps => Phone.Current.Apps
		.Where( x => x.ShowAppInLauncher && !IsDocked( x ) )
		.ToList();

	protected override void OnAfterTreeRender( bool firstTime )
	{
		if ( !firstTime ) return;

		DockDefaultApps();
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

	public T? GetApp<T>() where T : IPhoneApp => Phone.Current.Apps.OfType<T>().FirstOrDefault();

	public void OnAppOpened( IPhoneApp app )
	{
		if ( app != this ) return;

		Phone.Current.StatusBar.TextPhoneTheme = PhoneTheme.Light;
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( base.BuildHash(), _dock, Phone.Current, Phone.Current.Apps );
	}
}
