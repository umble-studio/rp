using Rp.Phone.UI.Components;

namespace Rp.Phone;

public partial class Phone
{
	private Alert? _currentAlert;
	
	public void ShowAlert( Alert alert )
	{
		_currentAlert = alert;
		_phoneContent.AddChild( alert );
	}

	public void HideAlert()
	{
		_currentAlert?.Delete();
	}
}
