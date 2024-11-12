namespace Rp.Phone.Apps;

public sealed partial class CameraApp : PhoneApp, IPhoneEvent
{
	public override string AppName => "camera";
	public override string AppTitle => "Camera";
	public override string AppIcon => "textures/ui/phone/app_camera.png";
	public override string AppNotificationIcon => "fluent:camera-28-filled";
}
