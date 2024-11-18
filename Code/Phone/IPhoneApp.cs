namespace Rp.Phone;

public interface IPhoneApp
{
	Phone Phone { get; internal set; }
	
	string AppName { get; }
	string AppTitle { get; }
	string? AppIcon { get; }
	string? AppNotificationIcon { get; }
	bool ShowAppInLauncher { get; }
	bool IsInitialized { get; }

	void OpenApp();
	void CloseApp();
	void FocusApp() { }
	void BlurApp() { }
}
