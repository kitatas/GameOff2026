using GameOff2026.Game.Application;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.UseCase;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class PlayerUseCaseTests
    {
        private DateEntity _dateEntity;
        private EnemyEntity _enemyEntity;
        private PlayerEntity _playerEntity;
        private PlayerUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _dateEntity = new DateEntity();
            _enemyEntity = new EnemyEntity();
            _playerEntity = new PlayerEntity();
            _useCase = new PlayerUseCase(_dateEntity, _enemyEntity, _playerEntity);
        }

        [Test]
        public void IsEncounterEnemy_PlayerSharesSquareWithCurrentDateEnemy_ReturnsTrue()
        {
            _dateEntity.Reset();
            var playerSquare = new SquareVO(2, 3);
            _playerEntity.SetSquare(playerSquare);
            SetEnemySquares(DateType.Day, new SquareVO(1, 1), playerSquare, new SquareVO(3, 3), new SquareVO(4, 4));

            var actual = _useCase.IsEncounterEnemy();

            Assert.IsTrue(actual);
        }

        [Test]
        public void IsEncounterEnemy_PlayerDoesNotShareSquareWithCurrentDateEnemy_ReturnsFalse()
        {
            _dateEntity.Reset();
            _playerEntity.SetSquare(new SquareVO(2, 3));
            SetEnemySquares(DateType.Day, new SquareVO(1, 1), new SquareVO(1, 2), new SquareVO(3, 3),
                new SquareVO(4, 4));

            var actual = _useCase.IsEncounterEnemy();

            Assert.IsFalse(actual);
        }

        [Test]
        public void IsEncounterEnemy_EnemyIsOnOtherDate_ReturnsFalse()
        {
            _dateEntity.Reset();
            var playerSquare = new SquareVO(2, 3);
            _playerEntity.SetSquare(playerSquare);
            SetEnemySquares(DateType.Day, new SquareVO(1, 1), new SquareVO(1, 2), new SquareVO(3, 3),
                new SquareVO(4, 4));
            SetEnemySquares(DateType.Night, playerSquare, new SquareVO(1, 2), new SquareVO(3, 3), new SquareVO(4, 4));

            var actual = _useCase.IsEncounterEnemy();

            Assert.IsFalse(actual);
        }

        private void SetEnemySquares(DateType date, params SquareVO[] squares)
        {
            for (var i = 0; i < squares.Length; i++)
            {
                _enemyEntity.Set(date, i, squares[i]);
            }
        }
    }
}