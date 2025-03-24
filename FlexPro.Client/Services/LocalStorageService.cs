using Microsoft.JSInterop;

namespace FlexPro.Client.Services
{
    public class LocalStorageService
    {
        private readonly IJSRuntime _JsRuntime;

        public LocalStorageService(IJSRuntime jSRuntime)
        {
            _JsRuntime = jSRuntime;
        }

        public async Task SetItemAsync(string key, string value)
        {
            await _JsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }

        public async Task<string> GetItemAsync(string key)
        {
            return await _JsRuntime.InvokeAsync<string>("localStorage.getItem", key);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _JsRuntime.InvokeVoidAsync("localStorage.removeItem", key);
        }
    }
}
