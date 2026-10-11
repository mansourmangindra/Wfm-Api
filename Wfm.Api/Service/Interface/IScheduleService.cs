using Common.DataTransferObjects;
using DataAccess.DBContexts.Wfm.Models;

namespace Wfm.Api.Service.Interface
{
    public interface IScheduleService
    {
        Task<IEnumerable<Schedule>> GetAllSchedulesAsync();
        Task<ScheduleDetails> GetByScheduleId(int id);
        Task<(bool Success, string Message, Schedule? Schedule)> CreateScheduleAsync(ScheduleDetails dto);
        Task<bool> DeleteScheduleAsync(int id);

        Task<(bool Success, string Message, Schedule? Schedule)> UpdateScheduleAsync(int id, ScheduleDetails dto);
    }
}
