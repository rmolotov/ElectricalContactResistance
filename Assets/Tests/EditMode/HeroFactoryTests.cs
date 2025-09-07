using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using UnityEngine;
using ECR.Infrastructure.Factories.Interfaces;

namespace Tests.EditMode
{
    public class HeroFactoryTests
    {
        [Test]
        public async Task SpawnHeroAtEleven_ThenPositionShouldBeEleven()
        {
            // arrange
            var factory = Mock.Of<IHeroFactory>();
            var spawnPosition = Vector3.one * 11;

            // act
            var hero = await factory.Create(at: spawnPosition);

            // assert
            hero.transform.position.Should().Be(spawnPosition);
        }
    }
}