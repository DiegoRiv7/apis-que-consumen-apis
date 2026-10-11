using Microsoft.AspNetCore.Mvc;
using backend_II.Models;
using backend_II.Services;

namespace backend_II.Controllers
{
    [ApiController]
    [Route("consult/pokemons")]
    public class PokemonsController : ControllerBase
    {
        private readonly PokeApiService _pokeApi;

        public PokemonsController(PokeApiService pokeApi)
        {
            _pokeApi = pokeApi;
        }

        // GET: consult/pokemons?limit=20&offset=0
        [HttpGet]
        public async Task<GeneralResponse<PokemonPage>> GetAll([FromQuery] int limit = 20, [FromQuery] int offset = 0)
        {
            if (limit < 1 || limit > 100)
            {
                return new GeneralResponse<PokemonPage>
                {
                    Success = false,
                    Message = "El parámetro 'limit' debe estar entre 1 y 100.",
                    Data = null
                };
            }

            if (offset < 0)
            {
                return new GeneralResponse<PokemonPage>
                {
                    Success = false,
                    Message = "El parámetro 'offset' no puede ser negativo.",
                    Data = null
                };
            }

            try
            {
                var page = await _pokeApi.GetPokemonsAsync(limit, offset);

                return new GeneralResponse<PokemonPage>
                {
                    Success = true,
                    Message = $"Se obtuvieron {page.Pokemons.Count} de {page.Total} pokémon.",
                    Data = page
                };
            }
            catch (HttpRequestException ex)
            {
                return new GeneralResponse<PokemonPage>
                {
                    Success = false,
                    Message = $"No se pudo consultar PokeAPI: {ex.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<PokemonPage>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // GET: consult/pokemons/{nombre o id}
        [HttpGet("{nameOrId}")]
        public async Task<GeneralResponse<Pokemon>> GetByNameOrId([FromRoute] string nameOrId)
        {
            try
            {
                var pokemon = await _pokeApi.GetPokemonAsync(nameOrId);

                if (pokemon == null)
                {
                    return new GeneralResponse<Pokemon>
                    {
                        Success = false,
                        Message = $"No existe el pokémon '{nameOrId}'.",
                        Data = null
                    };
                }

                return new GeneralResponse<Pokemon>
                {
                    Success = true,
                    Message = "Pokémon encontrado.",
                    Data = pokemon
                };
            }
            catch (HttpRequestException ex)
            {
                return new GeneralResponse<Pokemon>
                {
                    Success = false,
                    Message = $"No se pudo consultar PokeAPI: {ex.Message}",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new GeneralResponse<Pokemon>
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }
    }
}
