using Core.HttpLogic.Types;

namespace Core.HttpLogic.Services.Interfaces;

/// <summary>
/// Функционал для HTTP-соединения
/// </summary>
internal interface IHttpConnectionService
{
    /// <summary>
    /// Создание клиента для HTTP-подключения
    /// </summary>
    HttpClient CreateHttpClient(HttpConnectionData httpConnectionData);

    /// <summary>
    /// Отправить HTTP-запрос
    /// </summary>
    Task<HttpResponseMessage> SendRequestAsync(
        HttpRequestMessage httpRequestMessage, 
        HttpClient httpClient, 
        CancellationToken cancellationToken, 
        HttpCompletionOption httpCompletionOption = HttpCompletionOption.ResponseContentRead);
}