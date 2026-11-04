using System.Threading;
using Cysharp.Threading.Tasks;
using GameOff2026.Game.Application;
using MessagePipe;

namespace GameOff2026.Game.Domain.Ports
{
    public sealed class FieldPorts
    {
        public readonly IAsyncSubscriber<RenderFieldSquareVO> renderFieldSquareSubscriber;
        private readonly IAsyncPublisher<RenderFieldSquareVO> _renderFieldSquarePublisher;

        public FieldPorts(IAsyncSubscriber<RenderFieldSquareVO> renderFieldSquareSubscriber,
            IAsyncPublisher<RenderFieldSquareVO> renderFieldSquarePublisher)
        {
            this.renderFieldSquareSubscriber = renderFieldSquareSubscriber;
            _renderFieldSquarePublisher = renderFieldSquarePublisher;
        }

        public UniTask PublishRenderFieldSquareAsync(RenderFieldSquareVO renderFieldSquare, CancellationToken token)
        {
            return _renderFieldSquarePublisher.PublishAsync(renderFieldSquare, token);
        }
    }
}