namespace Rp.Phone.Apps.Messages.Services;

public sealed partial class ConversationService : Component, IPhoneService
{
	private Phone Phone { get; set; } = null!;

	protected override void OnStart()
	{
		Phone = GameObject.GetComponent<Phone>();
	}
}
