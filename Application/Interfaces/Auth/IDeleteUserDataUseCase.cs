using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;

namespace InputWeb.Application.Interfaces;

public interface IDeleteUserDataUseCase
{
    Task ExecuteAsync(Guid userId);
}