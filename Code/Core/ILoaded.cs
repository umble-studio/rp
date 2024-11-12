using System.Threading.Tasks;

namespace Rp.Core;

public interface ILoaded
{
	Task WaitForCharacterInitialization();
}
