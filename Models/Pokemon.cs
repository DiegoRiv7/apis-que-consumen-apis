namespace backend_II.Models
{
    // Lo que nuestra API devuelve al cliente: una versión limpia de lo que da PokeAPI.

    public class PokemonSummary
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }

    public class PokemonPage
    {
        public int Total { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public List<PokemonSummary> Pokemons { get; set; } = new();
    }

    public class Pokemon
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public double HeightInMeters { get; set; }
        public double WeightInKg { get; set; }
        public List<string> Types { get; set; } = new();
        public List<string> Abilities { get; set; } = new();
        public string? ImageUrl { get; set; }
    }
}
