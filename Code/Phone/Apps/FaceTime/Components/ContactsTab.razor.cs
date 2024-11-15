using System;
using Rp.Phone.UI.Components;
using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public sealed partial class ContactsTab : NavigationPage
{
	private MessageBar _navigationBar = null!;
	
	public override string PageName => "Contacts";
}
