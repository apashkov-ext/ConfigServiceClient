using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    internal class HttpMessageHandler : DelegatingHandler
    {
        private readonly Random _jitterer = new Random();
        private readonly TimeSpan _defaultTimeout = TimeSpan.FromSeconds(4);
        private readonly int _retryAttempts;
        private readonly TimeSpan? _attemptTimeout;

        public HttpMessageHandler(System.Net.Http.HttpMessageHandler handler, int retryAttempts, TimeSpan? attemptTimeout = null) : base(handler)
        {
            _retryAttempts = retryAttempts;
            _attemptTimeout = attemptTimeout;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Policy
                .Handle<HttpRequestException>()
                .Or<TaskCanceledException>()
                .Or<TimeoutException>()
                .OrResult<HttpResponseMessage>(x => !x.IsSuccessStatusCode)
                // Exponential back-off + some jitter.
                .WaitAndRetryAsync(_retryAttempts, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(_jitterer.Next(0, 100)))
                .ExecuteAsync(() => SendRequest(request, cancellationToken));
        }

        private async Task<HttpResponseMessage> SendRequest(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            using (var cts = GetCancellationTokenSource(cancellationToken))
            {
                try
                {
                    return await base.SendAsync(request, cts?.Token ?? cancellationToken);
                }
                catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
                {
                    throw new TimeoutException();
                }
            }
        }

        private CancellationTokenSource GetCancellationTokenSource(CancellationToken cancellationToken)
        {
            var timeout = _attemptTimeout ?? _defaultTimeout;
            if (timeout == Timeout.InfiniteTimeSpan)
            {
                // No need to create a CTS if there's no timeout.
                return null;
            } 
            
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(timeout);

            return cts;
        }
    }
}
