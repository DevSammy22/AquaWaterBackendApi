using AquaWater.Data.Repository.Interfaces;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Threading.Tasks;
using AquaWater.Dto.Common;
using AquaWater.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace AquaWater.Data.Repository.Implementations
{
    #region Constructor
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepo<User> _userRepo;
        private readonly AppDbContext _appDbContext;

        public UserService(IMapper mapper, UserManager<User> userManager, IGenericRepo<User> genericRepoUser, AppDbContext appDbContext)
        {
            _mapper = mapper;
            _userManager = userManager;
            _userRepo = genericRepoUser;
            _appDbContext = appDbContext;

        }
        #endregion

        #region Implimentation
        public async Task<UserResponseDto> RegisterAsync(UserRegistrationRequestDTO registrationRequest)
        {
            User user = _mapper.Map<User>(registrationRequest);
            user.UserName = registrationRequest.Email;
            IdentityResult result = await _userManager.CreateAsync(user, registrationRequest.Password);
            if (result.Succeeded)
            {
                var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var response = _mapper.Map<UserRegistrationRequestDTO>(user);
                var answer = new UserResponseDto
                {
                    Id = user.Id,
                    Token = emailToken,
                    FullName = $"{user.FirstName + " " + user.LastName}",
                    Email = user.Email,
                };
                return answer;
            }
            string errors = result.Errors.Aggregate(string.Empty, (current, error) => current + (error.Description + Environment.NewLine));
            throw new ArgumentException(errors);
        }
        public async Task<Response<EditUserResponseDTO>> EditUser(string userId, EditUserDTO model)
        {
            var user = _userRepo.Table.Where(y => y.Id == userId).Include(x => x.Location).FirstOrDefault();
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.ProfilePictureUrl = model.ImageUrl ?? user.ProfilePictureUrl;
            user.Location.Country = model.Location.Country ?? user.Location.Country;
            user.Location.State = model.Location.State ?? user.Location.State;
            user.Location.City = model.Location.City ?? user.Location.City;
            user.Location.Street = model.Location.Street ?? user.Location.Street;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var userResponse = _mapper.Map<EditUserResponseDTO>(user);

                return new Response<EditUserResponseDTO>()
                {

                    Success = true,
                    Message = "User updated Successfully ",
                    Data = userResponse,
                    Errors = null
                };
            }

            return new Response<EditUserResponseDTO>()
            {
                Success = false,
                Message = "User update fails",
                Data = null,
                Errors = null
            };
        }
    }
    #endregion
}