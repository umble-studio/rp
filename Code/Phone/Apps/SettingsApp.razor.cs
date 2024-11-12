namespace Rp.Phone.Apps;

public sealed partial class SettingsApp : PhoneApp, IPhoneEvent
{
	public override string AppName => "settings";
	public override string AppTitle => "Settings";
	public override string AppIcon => "textures/ui/phone/app_settings.png";
	public override string? AppNotificationIcon => "fluent:settings-48-filled";
}
