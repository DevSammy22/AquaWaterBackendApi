using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Common;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaWater.Data.Repository.Interfaces
{
    public interface IUserService
    {
        public Task<UserResponseDto> RegisterAsync(UserRegistrationRequestDTO registrationRequest);
        Task<Response<EditUserResponseDTO>> EditUser(string userId, EditUserDTO model);
    }
}
