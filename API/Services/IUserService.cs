

using API.Dtos;

namespace API.Services;

public interface IUserService
{
   Task<string> RegisterAsync(RegisterDto model);
   Task<DatosUserDto> GetTokenAsync(LoginDto model);
   Task<AutoTokenResponseDto> GetTokenRefreshAsync(RefreshTokenUserDto model, int idUsuaerio);

   Task<string> AddRoleAsync(AddRoleDto model);
}
