namespace Rp.Phone;

public interface IPhoneEvent : ISceneEvent<IPhoneEvent>
{
	void OnAppOpened( IPhoneApp app ) { }
	void OnAppClosed( IPhoneApp app ) { }
	void OnAppFocused( IPhoneApp app ) { }
	void OnAppBlurred( IPhoneApp app ) { }
	void OnAppDock( IPhoneApp app ) { }
	void OnAppUndock( IPhoneApp app ) { }

	void OnPhoneShow() { }
	void OnPhoneHide() { }
	void OnPhoneLocked() { }
	void OnPhoneUnlocked() { }
	void OnPhoneShown() { }
	void OnPhoneHidden() { }
	void OnPhoneSimCardChanged(SimCardData simCard) { }
}
