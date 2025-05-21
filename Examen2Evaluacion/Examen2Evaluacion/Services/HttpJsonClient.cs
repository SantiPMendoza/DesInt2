using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Examen2Evaluacion.Services;

namespace Examen2Evaluacion.Services
{
    public class HttpJsonClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthService _authService;

        public HttpJsonClient(HttpClient httpClient, IAuthService authService)
        {
            _httpClient = httpClient;
            _authService = authService;
        }

        private void AddAuthorizationHeader()
        {
            var token = _authService.GetToken();

            // Limpia el header primero para evitar duplicaciones
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
                return await _httpClient.GetFromJsonAsync<List<T>>(url);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error al obtener la lista de {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<T> GetAsync<T>(string url)
        {
            try
            {
                AddAuthorizationHeader();
                return await _httpClient.GetFromJsonAsync<T>(url);
            }
            catch (HttpRequestException ex)
            {
                throw new ApplicationException($"Error al obtener el objeto de tipo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PutAsync<T>(string url, T data)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string url)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }

    }
}
