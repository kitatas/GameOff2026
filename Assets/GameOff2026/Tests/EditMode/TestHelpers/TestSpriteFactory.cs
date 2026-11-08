using NUnit.Framework;
using UnityEngine;

namespace GameOff2026.Tests.EditMode.TestHelpers
{
    internal static class TestSpriteFactory
    {
        private static Sprite _valid;

        public static Sprite Valid
        {
            get
            {
                if (_valid == null)
                {
                    _valid = Sprite.Create(Texture2D.whiteTexture, new Rect(0, 0, 1, 1), Vector2.zero);
                }

                return _valid;
            }
        }

        public static void Release()
        {
            if (_valid != null)
            {
                Object.DestroyImmediate(_valid);
                _valid = null;
            }
        }
    }
}

namespace GameOff2026.Tests.EditMode
{
    [SetUpFixture]
    public sealed class TestSpriteLifetime
    {
        [OneTimeTearDown]
        public void TearDown()
        {
            // テスト間で共有する Sprite をテスト実行終了時に破棄する。
            TestHelpers.TestSpriteFactory.Release();
        }
    }
}
