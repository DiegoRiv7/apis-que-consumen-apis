using System.Net;
using System.Net.Http.Json;
using backend_II.Models;
using backend_II.Models.PokeApi;

namespace backend_II.Services
{
    // Consume PokeAPI con un HttpClient administrado por IHttpClientFactory.
    // No se crea "new HttpClient()" en cada llamada: la fábrica reutiliza las conexiones
    // y evita el agotamiento de sockets (ver artículo de NetMentor sobre HttpClient).
    public class PokeApiService
    {
        public const string ClientName = "PokeApi";

        private readonly IHttpClientFactory _httpClientFactory;

        public PokeApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET pokemon?limit=..&offset=..  ->  listado paginado
        public async Task<PokemonPage> GetPokemonsAsync(int limit, int offset)
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            // GetFromJsonAsync hace la petición, valida el status y deserializa en un solo paso.
            var response = await client.GetFromJsonAsync<PokemonListResponse>($"pokemon?limit={limit}&offset={offset}")
                           ?? throw new InvalidOperationException("PokeAPI devolvió una respuesta vacía.");

            return new PokemonPage
            {
                Total = response.Count,
                Limit = limit,
                Offset = offset,
                Pokemons = response.Results.Select(item => new PokemonSummary
                {
                    Id = ExtractIdFromUrl(item.Url),
                    Name = item.Name,
                    Url = item.Url
                }).ToList()
            };
        }

        // GET pokemon/{nombre o id}  ->  detalle, o null si no existe
        public async Task<Pokemon?> GetPokemonAsync(string nameOrId)
        {
            var client = _httpClientFactory.CreateClient(ClientName);

            // Aquí se usa GetAsync para poder distinguir el 404 (no existe) de otros errores.
            using var response = await client.GetAsync($"pokemon/{nameOrId.Trim().ToLowerInvariant()}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var detail = await response.Content.ReadFromJsonAsync<PokemonDetailResponse>()
                         ?? throw new InvalidOperationException("PokeAPI devolvió una respuesta vacía.");

            return new Pokemon
            {
                Id = detail.Id,
                Name = detail.Name,
                HeightInMeters = detail.Height / 10.0,   // PokeAPI entrega decímetros
                WeightInKg = detail.Weight / 10.0,       // PokeAPI entrega hectogramos
                Types = detail.Types.Select(t => t.Type.Name).ToList(),
                Abilities = detail.Abilities.Select(a => a.Ability.Name).ToList(),
                ImageUrl = detail.Sprites.FrontDefault
            };
        }

        // La URL del listado termina en ".../pokemon/25/"; el 25 es el id.
        private static int ExtractIdFromUrl(string url)
        {
            var segments = url.TrimEnd('/').Split('/');
            return int.TryParse(segments[^1], out var id) ? id : 0;
        }
    }
}
