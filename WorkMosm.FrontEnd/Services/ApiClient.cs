namespace WorkMosm.FrontEnd.Services
{
    public class ApiClient
    {
        private readonly HttpClient _http;

        public ApiClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<bool> IsBackendAliveAsync()
        {
            var response = await _http.GetAsync("/health");
            return response.IsSuccessStatusCode;
        }
    }
}
