using System;
using RoverDB.Attributes;

namespace Rp.Core;

[Collection( "players/data" )]
public record PlayerData
{
	[Id, Saved] public Guid Id { get; init; }
	[Saved] public SteamId Owner { get; init; }
	[Saved] public CharacterId CurrentCharacter { get; set; }
	[Saved] public List<CharacterId> Characters { get; init; } = new();
}
