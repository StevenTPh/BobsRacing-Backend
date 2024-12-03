using Xunit;
using Bobs_Racing.Models;

namespace Bobs_Racing.Tests.Models
{
    public class AnimalTests
    {
        [Fact]
        public void Animal_ShouldInitializeCorrectly()
        {
            var animal = new Animal
            {
                Name = "Cheetah",
                MinSpeed = 60,
                MaxSpeed = 100
            };

            Assert.Equal("Cheetah", animal.Name);
            Assert.Equal(60, animal.MinSpeed);
            Assert.Equal(100, animal.MaxSpeed);
        }
    }
}
