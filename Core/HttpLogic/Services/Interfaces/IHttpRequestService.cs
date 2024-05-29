using Core.HttpLogic.Types;

namespace Core.HttpLogic.Services.Interfaces;

public interface IHttpRequestService
{
    Task<HttpResponse<TResponse>> SendRequestAsync<TResponse>(HttpRequestData requestData, HttpConnectionData connectionData = default);
}