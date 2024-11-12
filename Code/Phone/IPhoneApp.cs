namespace Rp.Phone;

public interface IPhoneApp
{
	string AppName { get; }
	string AppTitle { get; }
	string? AppIcon { get; }
	string? AppNotificationIcon { get; }
	bool ShowAppInLauncher { get; }

	void OpenApp();
	void CloseApp();
	void FocusApp() { }
	void BlurApp() { }
}
