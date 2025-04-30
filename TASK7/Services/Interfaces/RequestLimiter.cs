using System.Threading.Tasks;
using StringProcessor.API.Models.Requests;
using StringProcessor.API.Models.Responses;

namespace StringProcessor.API.Services.Interfaces
{
    public interface IRequestLimiter
    {
        bool TryAcquireSlot();
        void ReleaseSlot();
    }

    public class RequestLimiter : IRequestLimiter, IDisposable
    {
        private readonly SemaphoreSlim _semaphore;

        public RequestLimiter(int limit)
        {
            _semaphore = new SemaphoreSlim(limit, limit);
        }

        public bool TryAcquireSlot()
        {
            return _semaphore.Wait(0); // Не блокирует, если нет свободных слотов
        }

        public void ReleaseSlot()
        {
            _semaphore.Release();
        }

        public void Dispose()
        {
            _semaphore?.Dispose();
        }
    }
}