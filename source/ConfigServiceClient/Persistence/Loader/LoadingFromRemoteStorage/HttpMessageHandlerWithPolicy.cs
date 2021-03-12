using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ConfigServiceClient.Options;
using Polly;

namespace ConfigServiceClient.Persistence.Loader.LoadingFromRemoteStorage
{
    internal class HttpMessageHandlerWithPolicy : DelegatingHandler
    {
        private readonly Random _jitterer = new Random();
        private readonly int _retryAttempts;
        private readonly TimeSpan _attemptTimeout;

        public HttpMessageHandlerWithPolicy(ConfigClientOptions options) : base(new HttpClientHandler())
        {
            _retryAttempts = options.RemoteConfigRequestingAttemptsCount;
            _attemptTimeout = options.RemoteConfigRequestingTimeout;
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
            if (_attemptTimeout == Timeout.InfiniteTimeSpan || _attemptTimeout == TimeSpan.Zero)
            {
                return null;
            } 
            
            var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(_attemptTimeout);

            return cts;
        }
    }
}
