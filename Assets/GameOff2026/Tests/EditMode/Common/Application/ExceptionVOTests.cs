using System;
using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    // NOTE: RebootExceptionVO / RetryExceptionVO / QuitExceptionVO は振る舞いが同形のため本ファイルでまとめて検証する
    [TestFixture]
    public sealed class ExceptionVOTests
    {
        [Test]
        public void RebootExceptionVO_Message_AppendsRebootMessage()
        {
            var ex = new RebootExceptionVO("failed");

            Assert.AreEqual("failed\n" + ExceptionConfig.REBOOT_MESSAGE, ex.message);
        }

        [Test]
        public void RetryExceptionVO_Message_AppendsRetryMessage()
        {
            var ex = new RetryExceptionVO("failed");

            Assert.AreEqual("failed\n" + ExceptionConfig.RETRY_MESSAGE, ex.message);
        }

        [Test]
        public void QuitExceptionVO_Message_AppendsQuitMessage()
        {
            var ex = new QuitExceptionVO("failed");

            Assert.AreEqual("failed\n" + ExceptionConfig.QUIT_MESSAGE, ex.message);
        }

        [Test]
        public void QuitExceptionVO_ExceptionMessage_UsesBaseDefault()
        {
            ExceptionVO ex = new QuitExceptionVO("failed");

            Assert.AreEqual(ExceptionConfig.QUIT_MESSAGE, ex.exceptionMessage);
        }

        [Test]
        public void ExceptionMessage_ThroughBaseReference_DispatchesToOverride()
        {
            ExceptionVO reboot = new RebootExceptionVO("a");
            ExceptionVO retry = new RetryExceptionVO("a");

            Assert.AreEqual(ExceptionConfig.REBOOT_MESSAGE, reboot.exceptionMessage);
            Assert.AreEqual(ExceptionConfig.RETRY_MESSAGE, retry.exceptionMessage);
        }

        [Test]
        public void Constructor_GivenMessage_KeepsOriginalInBaseMessage()
        {
            var ex = new RetryExceptionVO("failed");

            Assert.AreEqual("failed", ((Exception)ex).Message);
        }

        [Test]
        public void Message_EmptyString_StillAppendsActionMessage()
        {
            var ex = new RetryExceptionVO(string.Empty);

            Assert.AreEqual("\n" + ExceptionConfig.RETRY_MESSAGE, ex.message);
        }

        [Test]
        public void Throw_Caught_IsCatchableAsExceptionVO()
        {
            Assert.Throws<RebootExceptionVO>(() => throw new RebootExceptionVO("failed"));
        }
    }
}
