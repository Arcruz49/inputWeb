using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IAuthenticateUseCase
{
    Task<UserDto> ExecuteAsync(LoginRequest request);
}