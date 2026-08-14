using InputWeb.Domain.Exceptions;
using InputWeb.Application.DTOs.Request;
using InputWeb.Application.DTOs.Responses;
using InputWeb.Application.Interfaces;
using InputWeb.Application.Security;
using InputWeb.Domain.Entities;
using InputWeb.Domain.Interfaces;
using InputWeb.Domain.ValueObjects;
using Microsoft.AspNetCore.Identity;

namespace InputWeb.Application.UseCases;
public class RegisterUseCase(IUnitOfWork unitOfWork, JwtTokenGenerator tokenGenerator, PasswordHasher<User> passwordHasher,
IUserRepository userRepository) : IRegisterUserUseCase
{
    public async Task<UserDto> ExecuteAsync(RegisterUserRequest request)
    {
        var email = new Email(request.Email);

        if(await userRepository.GetUserByEmail(email.Value) != null) throw new ValidationException("Email já cadastrado");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            BirthDate = request.BirthDate,
            CreationDate = DateTime.UtcNow
        };

        var password = new Password(request.Password);

        user.Password = passwordHasher.HashPassword(user, password.Value);

        user = userRepository.CreateUser(user);

        await unitOfWork.SaveChangesAsync();

        var token = tokenGenerator.GenerateToken(user.Id, user.Name);

        return new UserDto(user.Name, user.Email, token); 

    }
}