using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IResetPasswordUseCase
{
    Task ExecuteAsync(string token, string password);
}