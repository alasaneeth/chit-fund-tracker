using HLChitFund.Application.Common.Interfaces;
using HLChitFund.Application.DTOs.Auth;
using HLChitFund.Domain.Entities;
using HLChitFund.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace HLChitFund.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly PasswordHasher<User> _passwordHasher;

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<LoginResponseDto> LoginAsync(
        LoginRequestDto request)
    {
        var users = await _unitOfWork.Users.FindAsync(
            u => u.Email == request.Email && u.IsActive);

        var user = users.FirstOrDefault()
            ?? throw new UnauthorizedAccessException(
                "Invalid email or password!");

        var result = _passwordHasher.VerifyHashedPassword(
            user, user.PasswordHash, request.Password);

        if (result == PasswordVerificationResult.Failed)
            throw new UnauthorizedAccessException(
                "Invalid email or password!");

        return new LoginResponseDto
        {
            Token = _jwtService.GenerateToken(user),
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role.ToString(),
            ExpiresAt = _jwtService.GetExpiryDate()
        };
    }

    public async Task<bool> RegisterAsync(
        RegisterRequestDto request)
    {
        var existing = await _unitOfWork.Users.FindAsync(
            u => u.Email == request.Email);

        if (existing.Any())
            throw new InvalidOperationException(
                "Email already exists!");

        var user = new User
        {
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            Role = Enum.Parse<UserRole>(request.Role),
            IsActive = true
        };

        user.PasswordHash = _passwordHasher
            .HashPassword(user, request.Password);

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}