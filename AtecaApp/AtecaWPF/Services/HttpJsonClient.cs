using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace AtecaWPF.Services
{
    public class HttpJsonClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public HttpJsonClient(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        private void AddAuthorizationHeader()
        {
            var token = _authService.GetToken();
            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
        }

        private async Task<string> ExtractErrorMessage(HttpResponseMessage response)
        {
            try
            {
                var json = await response.Content.ReadAsStringAsync();
                var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(json);
                return parsed != null && parsed.TryGetValue("mensaje", out var mensaje)
                    ? mensaje
                    : json; // fallback: return raw JSON
            }
            catch
            {
                return response.ReasonPhrase ?? "Error desconocido";
            }
        }

        public async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<List<T>>();
                return result ?? throw new ApplicationException("La respuesta fue nula o no se pudo deserializar correctamente.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener la lista de tipo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<T?> GetAsync<T>(string url)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<T>();
                return result ?? throw new ApplicationException($"La respuesta fue nula para tipo {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener objeto de tipo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.PostAsJsonAsync(url, data);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);

                var result = await response.Content.ReadFromJsonAsync<TResponse>();
                return result ?? throw new ApplicationException("La respuesta fue nula tras el POST.");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al realizar POST a {url}: {ex.Message}", ex);
            }
        }

        public async Task PutAsync(string url)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.PutAsync(url, null);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al realizar PUT en {url}: {ex.Message}", ex);
            }
        }

        public async Task PutAsync<T>(string url, T data)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.PutAsJsonAsync(url, data);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al realizar PUT con datos en {url}: {ex.Message}", ex);
            }
        }

        public async Task DeleteAsync(string url)
        {
            try
            {
                AddAuthorizationHeader();
                var response = await _httpClient.DeleteAsync(url);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException(await ExtractErrorMessage(response), response.StatusCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al realizar DELETE en {url}: {ex.Message}", ex);
            }
        }
    }
}
