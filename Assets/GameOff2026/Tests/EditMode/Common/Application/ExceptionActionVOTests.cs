using GameOff2026.Common.Application;
using NUnit.Framework;

namespace GameOff2026.Tests.EditMode.Common.Application
{
    [TestFixture]
    public sealed class ExceptionActionVOTests
    {
        [Test]
        public void Constructor_GivenException_KeepsSameInstance()
        {
            var ex = new RebootExceptionVO("failed");

            var vo = new ExceptionActionVO(ex);

            Assert.AreSame(ex, vo.exception);
        }
    }
}
