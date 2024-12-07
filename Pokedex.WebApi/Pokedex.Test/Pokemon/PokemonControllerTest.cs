using JewelArchitecture.Examples.SmartCharging.WebApiTest.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Pokedex.Interface.Pokemon;
using PokeDex.Domain.Pokemon;
using Shouldly;

namespace Pokedex.Test.Pokemon
{
    public class PokemonControllerTest : PokedexTestBase
    {
        [Theory]
        [InlineData("mewtwo", 150, "rare", true,
            "It was created by\na scientist after\nyears of horrific\fgene splicing and\nDNA engineering\nexperiments.")]
        [InlineData("clefairy", 35, "mountain", false,
            "Its magical and\ncute appeal has\nmany admirers.\fIt is rare and\nfound only in\ncertain areas.")]
        public async Task GetPokemonAsync_Success(string name, int expectedId, string expectedHabitat,
            bool expectedIsLegendary, string expectedDescription)
        {
            // Arrange
            var controller = ServiceProvider!.GetRequiredService<PokemonController>();

            // Act
            var response = await controller.GetPokemonAsync(name);

            // Assert
            response.ShouldBeOfType<OkObjectResult>();
            var okResult = response as OkObjectResult;
            var pokemon = okResult!.Value as PokemonAggregate;
            pokemon!.Id.ShouldBe(expectedId);
            pokemon!.Name.ShouldBe(name);
            pokemon!.Description.ShouldBe(expectedDescription);
            pokemon!.Habitat.ShouldBe(expectedHabitat);
            pokemon!.IsLegendary.ShouldBe(expectedIsLegendary);
        }

        [Theory]
        [InlineData("Pikachu", 25)]
        [InlineData("pikachu", 25)]
        [InlineData("pikAchu", 25)]
        public async Task GetPokemonAsync_CaseInsensitive(string name, int expectedId)
        {
            // Arrange
            var controller = ServiceProvider!.GetRequiredService<PokemonController>();

            // Act
            var response = await controller.GetPokemonAsync(name);

            // Assert
            response.ShouldBeOfType<OkObjectResult>();
            var okResult = response as OkObjectResult;
            var pokemon = okResult!.Value as PokemonAggregate;
            pokemon!.Id.ShouldBe(expectedId);           
        }

        [Fact]
        public async Task GetPokemonAsync_NotFound()
        {
            // Arrange
            var controller = ServiceProvider!.GetRequiredService<PokemonController>();

            // Act
            var response = await controller.GetPokemonAsync("pincopallino");

            // Assert
            response.ShouldBeOfType<NotFoundResult>();            
        }
    }
}