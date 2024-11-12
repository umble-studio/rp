using System;
using RoverDB.Attributes;

namespace Rp.Core;

[Collection( "players/characters" )]
public record CharacterData
{
	[Id, Saved] public Guid CharacterId { get; init; }
	[Saved] public string Firstname { get; init; } = null!;
	[Saved] public string Lastname { get; init; } = null!;
}
