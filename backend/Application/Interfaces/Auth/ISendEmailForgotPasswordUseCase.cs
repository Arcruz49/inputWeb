using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface ISendEmailForgotPasswordUseCase
{
    Task ExecuteAsync(string email);
}