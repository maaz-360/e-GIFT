namespace eGift.Admin.Helpers;

public class WebClientHelper
{
    #region Fields
    private readonly HttpClient _httpClient;
    #endregion

    #region Constructors
    public WebClientHelper(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    #endregion

    #region Web Client Json Methods
    public async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(
        string url,
        TRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(url, request);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<bool> PutAsync<TRequest, TResponse>(
        string url,
        TRequest request)
    {
        var response = await _httpClient.PutAsJsonAsync(url, request);

        return response.IsSuccessStatusCode;
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);

        response.EnsureSuccessStatusCode();
    }
    #endregion

    #region Web Client Form Methods
    public async Task<TResponse?> PostFormAsync<TResponse>(
    string url,
    MultipartFormDataContent formData)
    {
        var response = await _httpClient.PostAsync(url, formData);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    public async Task<TResponse?> PutFormAsync<TResponse>(
    string url,
    MultipartFormDataContent formData)
    {
        var response = await _httpClient.PutAsync(url, formData);

        response.EnsureSuccessStatusCode();

        if (response.Content.Headers.ContentLength == 0)
        {
            return default;
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
    #endregion

    #region Web Client File Methods
    public async Task<byte[]?> GetFileAsync(string url)
    {
        var response = await _httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadAsByteArrayAsync();
    }
    #endregion
}