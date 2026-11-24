using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using GameOff2026.Boot.Application;
using GameOff2026.Boot.Data.DataStore;
using GameOff2026.Boot.Domain.Ports;
using GameOff2026.Boot.Domain.Repository;
using GameOff2026.Boot.Domain.UseCase;
using GameOff2026.Common.Application;
using GameOff2026.Tests.EditMode.TestHelpers;
using NUnit.Framework;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameOff2026.Tests.EditMode.Boot.Domain.UseCase
{
    [TestFixture]
    public sealed class SplashUseCaseTests
    {
        private FakeAsyncSubscriber<SplashTransitionVO> _subscriber;
        private FakeAsyncPublisher<SplashTransitionVO> _publisher;
        private SplashTable _table;
        private SplashUseCase _useCase;

        [SetUp]
        public void SetUp()
        {
            _subscriber = new FakeAsyncSubscriber<SplashTransitionVO>();
            _publisher = new FakeAsyncPublisher<SplashTransitionVO>();
            _table = ScriptableObject.CreateInstance<SplashTable>();
            SetTableList(_table, new List<SplashData>());
            var ports = new SplashPorts(_subscriber, _publisher);
            var repository = new SplashRepository(_table);
            _useCase = new SplashUseCase(ports, repository);
        }

        [TearDown]
        public void TearDown()
        {
            if (_useCase != null)
            {
                ((IDisposable)_useCase).Dispose();
            }

            if (_table != null)
            {
                Object.DestroyImmediate(_table);
            }
        }

        [Test]
        public void Transition_AfterConstruction_ExposesPortsSubscriber()
        {
            Assert.AreSame(_subscriber, _useCase.transition);
        }

        [Test]
        public async Task InitAsync_Always_PublishesFadeOutWithoutSplash()
        {
            await _useCase.InitAsync(CancellationToken.None).AsTask();

            Assert.AreEqual(1, _publisher.PublishCount);
            var actual = _publisher.LastPublished;
            Assert.IsNull(actual.splash);
            Assert.AreEqual(Fade.Out, actual.transition.fade);
            Assert.AreEqual(0.0f, actual.transition.duration);
        }

        private static void SetTableList(SplashTable table, List<SplashData> items)
        {
            var field = typeof(GameOff2026.Common.Data.DataStore.BaseTable<SplashData>).GetField("list",
                BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(field);
            field.SetValue(table, items);
        }
    }
}