using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuvaCep.Application.Dtos;
using YuvaCep.Application.DTOs;

namespace YuvaCep.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<LoginResponse> RegisterParentAsync(ParentRegisterRequest request);
        Task<LoginResponse> RegisterTeacherAsync(TeacherRegisterRequest request);
        Task<RegisterResponse> RegisterAsync(RegisterRequest request);
        Task<LinkChildResponse> LinkChildAsync(Guid parentId, LinkChildRequest request);
    }
}