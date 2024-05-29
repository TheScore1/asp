using Core.HttpLogic.Types;

namespace Core.HttpLogic.Services.Interfaces;

internal interface IHttpConnectionService
{
    HttpClient CreateHttpClient(HttpConnectionData httpConnectionData);

    Task<HttpResponseMessage> SendRequestAsync(
        HttpRequestMessage httpRequestMessage, 
        HttpClient httpClient, 
        CancellationToken cancellationToken, 
        HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead);
}