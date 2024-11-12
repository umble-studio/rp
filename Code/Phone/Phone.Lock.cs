using Rp.Phone.Apps;
using Rp.UI.Extensions;

namespace Rp.Phone;

public partial class Phone
{
	private bool _isLocked = true;

	private void Lock()
	{
		_isLocked = true;
		
		SwitchToApp<LockScreen>();
		Scene.RunEvent<IPhoneEvent>( x => x.OnPhoneLocked(), true );
	}

	private void Unlock()
	{
		_isLocked = false;
		
		SwitchToApp<Launcher>();
		Scene.RunEvent<IPhoneEvent>( x => x.OnPhoneUnlocked(), true );
	}
}
