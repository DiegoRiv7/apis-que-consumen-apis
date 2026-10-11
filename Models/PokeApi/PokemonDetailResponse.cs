using System.Text.Json.Serialization;

namespace backend_II.Models.PokeApi
{
    // Forma del JSON que devuelve GET https://pokeapi.co/api/v2/pokemon/{nombre o id}
    // Solo se modelan los campos que la API nuestra necesita; el resto se ignora al deserializar.
    public class PokemonDetailResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Height { get; set; }   // en decímetros
        public int Weight { get; set; }   // en hectogramos
        public PokemonSprites Sprites { get; set; } = new();
        public List<PokemonTypeSlot> Types { get; set; } = new();
        public List<PokemonAbilitySlot> Abilities { get; set; } = new();
    }

    public class PokemonSprites
    {
        [JsonPropertyName("front_default")]
        public string? FrontDefault { get; set; }
    }

    public class PokemonTypeSlot
    {
        public NamedResource Type { get; set; } = new();
    }

    public class PokemonAbilitySlot
    {
        public NamedResource Ability { get; set; } = new();
    }

    public class NamedResource
    {
        public string Name { get; set; } = string.Empty;
    }
}
