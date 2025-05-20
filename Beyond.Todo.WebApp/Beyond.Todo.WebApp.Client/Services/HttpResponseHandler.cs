using Beyond.Todo.WebApp.Client.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Net.Http.Json;

namespace Beyond.Todo.WebApp.Client.Services;

public interface IHttpResponseHandler
{
    Task<bool> HandleResponseAsync(HttpResponseMessage response, string successMessage);
}

public class HttpResponseHandler : IHttpResponseHandler
{
    private readonly IToastService _toastService;

    public HttpResponseHandler(IToastService toastService)
    {
        _toastService = toastService;
    }

    public async Task<bool> HandleResponseAsync(HttpResponseMessage response, string successMessage)
    {
        if (response.IsSuccessStatusCode)
        {
            _toastService.ShowSuccess(successMessage);
            return true;
        }

        var errorResponse = await response.Content.ReadFromJsonAsync<ApiErrorResponse>();
        _toastService.ShowError(errorResponse?.Message ?? "An unexpected error occurred");
        return false;
    }
}
