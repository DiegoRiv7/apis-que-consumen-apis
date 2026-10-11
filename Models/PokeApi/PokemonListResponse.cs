namespace backend_II.Models.PokeApi
{
    // Forma del JSON que devuelve GET https://pokeapi.co/api/v2/pokemon?limit=..&offset=..
    public class PokemonListResponse
    {
        public int Count { get; set; }
        public string? Next { get; set; }
        public string? Previous { get; set; }
        public List<PokemonListItem> Results { get; set; } = new();
    }

    public class PokemonListItem
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
