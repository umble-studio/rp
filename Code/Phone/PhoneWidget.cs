using Rp.UI;

namespace Rp.Phone;

public abstract class PhoneWidget : CascadingPanel
{
	[CascadingProperty("Phone")] public Phone Phone { get; set; } = null!;
}
