using Rp.UI.Extensions;

namespace Rp.Phone;

public partial class Phone
{
	private bool _isOpen;

	public bool IsOpen
	{
		get => _isOpen;
		private set
		{
			if ( _isOpen == value ) return;
			_isOpen = value;

			if ( _isOpen ) RunShowEvent();
			else RunHideEvent();
		}
	}

	public void Show() => IsOpen = true;
	public void Hide() => IsOpen = false;

	private void RunShowEvent()
	{
		Scene.RunEvent<IPhoneEvent>( x => x.OnPhoneShown(), true );
	}

	private void RunHideEvent()
	{
		Scene.RunEvent<IPhoneEvent>( x => x.OnPhoneHidden(), true );
	}
}
