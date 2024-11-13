using System;
using RoverDB.Attributes;

namespace Rp.Phone;

[Collection( "phone/data" )]
public record PhoneData
{
	[Id, Saved] public Guid Id { get; init; }
}
