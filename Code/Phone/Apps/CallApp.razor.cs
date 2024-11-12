using System;
using Rp.Phone.UI.Components;

namespace Rp.Phone.Apps;

public sealed partial class CallApp : PhoneApp, IPhoneEvent, IAppNotifiable
{
	private DateTime _date = DateTime.Now;

	public override string AppName => "call";
	public override string AppTitle => "Call";
	public override string AppIcon => "textures/ui/phone/app_call.png";
	public override string AppNotificationIcon => "fluent:call-48-filled";
}
