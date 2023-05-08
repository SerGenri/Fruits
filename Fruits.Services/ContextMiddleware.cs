using System.Globalization;
using Microsoft.AspNetCore.Http;

namespace Fruits.Services;

public class ContextMiddleware
{
    private readonly RequestDelegate _next;

    public ContextMiddleware(RequestDelegate next)
    {
	    _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
	{
		var ru = new CultureInfo("ru-RU");
		ru.DateTimeFormat.LongTimePattern = "HH:mm:ss";
		ru.DateTimeFormat.DateSeparator = ".";
		CultureInfo.DefaultThreadCurrentCulture = ru;
		CultureInfo.DefaultThreadCurrentUICulture = ru;

		await _next.Invoke(httpContext);
    }

}