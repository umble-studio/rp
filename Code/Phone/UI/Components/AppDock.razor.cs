using System;
using Sandbox.UI;

namespace Rp.Phone.UI.Components;

public sealed partial class AppDock : Panel
{
	private readonly List<IPhoneApp> _apps = new();

	public void DockApp( IPhoneApp app )
	{
		_apps.Add( app );
	}

	public void UndockApp( IPhoneApp app )
	{
		_apps.Remove( app );
	}

	public bool IsDocked( IPhoneApp app )
	{
		return _apps.Exists( x => x.AppName == app.AppName );
	}

	protected override int BuildHash() => HashCode.Combine( _apps );
}
