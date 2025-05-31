using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using AtecaWPF.Services;

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

        public async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<List<T>>();

                if (result == null)
                    throw new ApplicationException("La respuesta fue nula o no se pudo deserializar correctamente.");

                return result;
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
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<T>();

                if (result == null)
                    throw new ApplicationException($"La respuesta para {typeof(T).Name} fue nula o mal formateada.");

                return result;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al obtener el objeto de tipo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            try
            {
                AddAuthorizationHeader();

                var response = await _httpClient.PostAsJsonAsync(url, data);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadFromJsonAsync<TResponse>();

                if (result == null)
                    throw new ApplicationException("La respuesta al POST fue nula o no se pudo deserializar.");

                return result;
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
                response.EnsureSuccessStatusCode();
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
                response.EnsureSuccessStatusCode();
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
                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al realizar DELETE en {url}: {ex.Message}", ex);
            }
        }
    }
}
