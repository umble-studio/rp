using System;

namespace Rp.Phone.Apps;

public sealed partial class LockScreen : PhoneApp, IPhoneEvent
{
	private DateTime _date = DateTime.Now;

	public override string AppName => "lock";
	public override string AppTitle => "Lock Screen";
	public override string? AppIcon => null;
	public override bool ShowAppInLauncher => false;
	
	public override void Tick()
	{
		_date = DateTime.Now;
	}

	protected override int BuildHash()
	{
		return HashCode.Combine( base.BuildHash(), _date );
	}
}
