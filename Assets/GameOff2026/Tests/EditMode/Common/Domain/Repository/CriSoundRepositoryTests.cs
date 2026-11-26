using System;
using GameOff2026.Common.Application;
using GameOff2026.Common.Domain.Repository;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Domain.Repository
{
    [TestFixture]
    public sealed class CriSoundRepositoryTests
    {
        [Test]
        public void Find_BgmNone_ThrowsQuitException()
        {
            var repository = new CriSoundRepository();

            Assert.Throws<QuitExceptionVO>(() => repository.Find(BgmType.None));
        }

        [Test]
        public void Find_SeNone_ThrowsQuitException()
        {
            var repository = new CriSoundRepository();

            Assert.Throws<QuitExceptionVO>(() => repository.Find(SeType.None));
        }

        [Test]
        public void Find_UnsupportedGenericType_ThrowsQuitException()
        {
            var repository = new CriSoundRepository();

            Assert.Throws<QuitExceptionVO>(() => repository.Find<SceneName>(SceneName.Boot));
        }
    }
}

