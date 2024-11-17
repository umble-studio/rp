using Rp.UI;

namespace Rp.Phone.Apps.FaceTime.Components;

public abstract class PhoneNavigationPage : NavigationPage
{
	[CascadingProperty("Phone")] public Phone Phone { get; set; } = null!;
}
