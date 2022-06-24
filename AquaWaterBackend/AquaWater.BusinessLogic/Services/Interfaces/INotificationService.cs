using AquaWater.Domain.Commons;
using AquaWater.Dto.Request;
using AquaWater.Dto.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AquaWater.BusinessLogic.Services.Interfaces
{
    public interface INotificationService
    {
        public Task CreateNotification(string userId, NotificationRequestDTO notificationDTO);
        public  Task MarkNotificationAsRead(List<string> notificationId, string userId);
        SearchResponse<IEnumerable<NotificationResponseDTO>> GetPaginatedUserNotification(SearchRequest<int> search, string userId);
    }
}
