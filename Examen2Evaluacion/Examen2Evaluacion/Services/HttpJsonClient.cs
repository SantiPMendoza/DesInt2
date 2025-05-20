using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace Examen2Evaluacion.Services
{
    public class HttpJsonClient
    {
        private readonly HttpClient _httpClient;

        public HttpJsonClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<T>> GetListAsync<T>(string url)
        {
            try
            {
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
                return await _httpClient.GetFromJsonAsync<T>(url);
            }
            catch (HttpRequestException ex)
            {
                // Manejo de errores HTTP
                throw new ApplicationException($"Error al obtener el objeto de tipo {typeof(T).Name}: {ex.Message}", ex);
            }
        }

        public async Task<TResponse> PostAsync<TRequest, TResponse>(string url, TRequest data)
        {
            var response = await _httpClient.PostAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TResponse>();
        }

        public async Task PutAsync<T>(string url, T data)
        {
            var response = await _httpClient.PutAsJsonAsync(url, data);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteAsync(string url)
        {
            var response = await _httpClient.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
