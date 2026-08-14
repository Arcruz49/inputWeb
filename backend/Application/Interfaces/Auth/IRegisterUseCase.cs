using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IRegisterUserUseCase
{
    Task<UserDto> ExecuteAsync(RegisterUserRequest request);
}