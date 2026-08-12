using System.ComponentModel.DataAnnotations;
using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Application.Security;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;
using InputWeb.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace InputWeb.Application.UseCases;
public class AuthenticateUseCase(JwtTokenGenerator tokenGenerator, PasswordHasher<User> passwordHasher,
IUserRepository userRepository) : IAuthenticateUseCase
{
    public async Task<UserDto> ExecuteAsync(LoginRequest request)
    {
        var email = new Email(request.Email);

        var user = await userRepository.GetUserByEmail(email.Value) ?? throw new ValidationException("Email ou senha incorretos");

        var result = passwordHasher.VerifyHashedPassword(user, user.Password, request.Password);

        if (result == PasswordVerificationResult.Failed) throw new ValidationException("Email ou senha incorretos");

        var token = tokenGenerator.GenerateToken(user.Id, user.Name);

        return new UserDto(user.Name, user.Email, token); 
    }
}