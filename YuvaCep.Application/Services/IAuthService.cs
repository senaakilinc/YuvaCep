using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuvaCep.Application.Dtos;

namespace YuvaCep.Application.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<LoginResponse> FirstRegisterAsync(RegisterRequest request);
    }
}
