using InputWeb.Application.Interfaces;
using InputWeb.Application.Security;

namespace InputWeb.Application.UseCases;
public class RegisterUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtTokenGenerator _tokenGenerator;
}