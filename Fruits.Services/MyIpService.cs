using Microsoft.AspNetCore.Http;
using Fruits.Interfaces;

namespace Fruits.Services;

public class MyIpService : IMyIpService
{
	private readonly IHttpContextAccessor _contextAccessor;

	public MyIpService(IHttpContextAccessor contextAccessor)
	{
		_contextAccessor = contextAccessor;
	}

	public string? GetIp()
	{
		return _contextAccessor.HttpContext != null
		            && _contextAccessor.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For")
			? _contextAccessor.HttpContext.Request.Headers["X-Forwarded-For"].ToString()
			: null;
	}
}