using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Common.Data.DataStore;
using GameOff2026.Game.Application;
using GameOff2026.Game.Data.DataStore;
using GameOff2026.Game.Data.Entity;
using GameOff2026.Game.Domain.Ports;
using GameOff2026.Game.Domain.Repository;
using GameOff2026.Game.Domain.UseCase;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.Game.Domain.UseCase
{
    [TestFixture]
    public sealed class FieldUseCaseTests
    {
        private FieldEntity _fieldEntity;
        private FieldTable _fieldTable;
        private FieldData _fieldData;
        private FakeAsyncSubscriber<RenderFieldSquareVO> _subscriber;
        private FakeAsyncPublisher<RenderFieldSquareVO> _publisher;
        private FieldUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _fieldEntity = new FieldEntity();
            _fieldTable = ScriptableObject.CreateInstance<FieldTable>();
            _fieldData = ScriptableObject.CreateInstance<FieldData>();
            SetField(_fieldData, "fieldType", FieldType.Ground);
            SetField(_fieldData, "weight", 1.0f);
            SetField(typeof(BaseTable<FieldData>), _fieldTable, "list", new List<FieldData> { _fieldData });
            _subscriber = new FakeAsyncSubscriber<RenderFieldSquareVO>();
            _publisher = new FakeAsyncPublisher<RenderFieldSquareVO>();
            var ports = new FieldPorts(_subscriber, _publisher);
            var repository = new FieldRepository(_fieldTable);
            _useCase = new FieldUseCase(_fieldEntity, ports, repository);
        }

        [TearDown]
        public void TearDown()
        {
            UnityEngine.Object.DestroyImmediate(_fieldData);
            UnityEngine.Object.DestroyImmediate(_fieldTable);
        }

        [Test]
        public async Task BuildFieldAsync_ExistingSquares_RebuildsAndPublishesWholeField()
        {
            _fieldEntity.Add(FieldSquareVO.Create(99, 99, new FieldVO(FieldType.Ground, null, 1.0f)));
            using var source = new CancellationTokenSource();

            await _useCase.BuildFieldAsync(source.Token).AsTask();

            var renderMessage = _publisher.LastPublished;
            Assert.AreEqual(1, _publisher.PublishCount);
            Assert.AreEqual(source.Token, _publisher.tokens[0]);
            Assert.AreEqual(RenderType.Refresh, renderMessage.type);
            Assert.AreEqual(FieldConfig.MAX_FILE * FieldConfig.MAX_RANK, renderMessage.squares.Length);
            Assert.AreEqual(renderMessage.squares.Length, _fieldEntity.fieldSquares.Length);

            for (int i = 0; i < renderMessage.squares.Length; i++)
            {
                Assert.AreEqual(i % FieldConfig.MAX_FILE + 1, renderMessage.squares[i].square.file);
                Assert.AreEqual(i / FieldConfig.MAX_FILE + 1, renderMessage.squares[i].square.rank);
                Assert.AreEqual(FieldType.Ground, renderMessage.squares[i].field.type);
            }
        }

        [Test]
        public void RenderField_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.renderField);
        }

        private static void SetField(object target, string fieldName, object value)
        {
            SetField(target.GetType(), target, fieldName, value);
        }

        private static void SetField(Type declaringType, object target, string fieldName, object value)
        {
            var field = declaringType.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field, $"Expected serialized field '{fieldName}' to exist.");
            field.SetValue(target, value);
        }
    }
}
