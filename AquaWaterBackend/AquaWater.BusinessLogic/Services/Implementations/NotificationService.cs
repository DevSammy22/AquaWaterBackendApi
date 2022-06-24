using AquaWater.BusinessLogic.Services.Interfaces;
using AquaWater.BusinessLogic.Utilities;
using AquaWater.Data.Repository;
using AquaWater.Domain.Commons;
using AquaWater.Domain.Entities;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly IGenericRepo<Notification> _notificationRepo;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IGenericRepo<Notification> _notification;
        public NotificationService(IGenericRepo<Notification> genericRepo, IMapper mapper, UserManager<User> userManager, IGenericRepo<Notification> notification)
        {
            _notificationRepo = genericRepo;
            _mapper = mapper;
            _userManager = userManager;
            _notification = notification;
        }

        public async Task CreateNotification(string userId, NotificationRequestDTO notificationDTO)
        {
            User user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                if (notificationDTO != null)
                {
                    var notification = _mapper.Map<Notification>(notificationDTO);
                    notification.UserId = userId;
                    await _notificationRepo.InsertAsync(notification);
                    return;
                }

                throw new ArgumentException(nameof(notificationDTO));
            }
            throw new ArgumentException("User not found");
        }

        public async Task MarkNotificationAsRead(List<string> notificationId, string userId)
        {
            foreach (var item in notificationId)
            {
                var notification = await _notificationRepo.GetByIdAysnc(Guid.Parse(item));
                if (notification == null)
                {
                    continue;
                }

                if (notification.UserId == userId)
                {
                    notification.IsRead = true;
                    await _notificationRepo.UpdateAsync(notification);
                }
            }
        }
        public SearchResponse<IEnumerable<NotificationResponseDTO>> GetPaginatedUserNotification(SearchRequest<int> search, string userId)
        {
            var response = _notification.TableNoTracking.Where(x => x.UserId == userId);

            var paginatedResponse = Paginator.Pagination<Notification, NotificationResponseDTO>(response, _mapper, search.PageSize, search.Page);
            return paginatedResponse;
        }
    }
}