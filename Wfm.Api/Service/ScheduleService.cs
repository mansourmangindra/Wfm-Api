using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.DataTransferObjects;
using DataAccess.DBContexts.Wfm.Models;
using DataAccess.UnitOfWorks.Wfm;
using Wfm.Api.Service.Interface;

namespace Wfm.Api.Service
{
    public class ScheduleService : IScheduleService
    {
        private readonly IWfmUnitOfWork _unitOfWork;

        public ScheduleService(IWfmUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Schedule>> GetAllSchedulesAsync()
        {
            return await _unitOfWork.SchedulesRepository.GetAllAsync();
        }

        public async Task<(bool Success, string Message, Schedule? Schedule)> CreateScheduleAsync(ScheduleDetails dto)
        {
            if (dto.StartTime >= dto.EndTime)
            {
                return (false, "Start time must be strictly before end time.", null);
            }

            var existingSchedules = await _unitOfWork.SchedulesRepository.FindAsync(s =>
                s.AgentId == dto.AgentId &&
                s.Date == dto.Date &&
                s.StartTime < dto.EndTime &&
                s.EndTime > dto.StartTime &&
                s.Status != "Cancelled"
            );

            if (existingSchedules.Any())
            {
                return (false, "Validation Error: Agent already has an overlapping schedule during this time slot.", null);
            }

            var schedule = new Schedule
            {
                AgentId = dto.AgentId,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Status = dto.Status ?? "Scheduled"
            };

            await _unitOfWork.SchedulesRepository.AddAsync(schedule);

            string timePeriodKey = dto.StartTime.ToString("HH:mm");
            var staffingReq = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == dto.Date && s.TimePeriod == timePeriodKey
            );

            if (staffingReq != null)
            {
                staffingReq.ScheduledAgents += 1;
                _unitOfWork.StaffingRequirementsRepository.Update(staffingReq);
            }

            await _unitOfWork.SaveChangesAsync();

            return (true, "Schedule created successfully.", schedule);
        }
        public async Task<bool> DeleteScheduleAsync(int id)
        {
            var schedule = await _unitOfWork.SchedulesRepository.GetByIdAsync(id);
            if (schedule == null) return false;

            string timePeriodKey = schedule.StartTime.ToString("HH:mm");
            var staffingReq = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == schedule.Date && s.TimePeriod == timePeriodKey
            );

            if (staffingReq != null && staffingReq.ScheduledAgents > 0)
            {
                staffingReq.ScheduledAgents -= 1;
                _unitOfWork.StaffingRequirementsRepository.Update(staffingReq);
            }

            _unitOfWork.SchedulesRepository.Remove(schedule);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<ScheduleDetails> GetByScheduleId(int id)
        {
            var schedule = await _unitOfWork.SchedulesRepository.FirstOrDefaultAsync(
                selector: e => new ScheduleDetails
                {
                    Id = e.Id,
                    AgentId = e.AgentId,
                    Date = e.Date,
                    StartTime = e.StartTime,
                    EndTime = e.EndTime,
                    Status = e.Status,
                    UserDetail = new Users()
                    {
                        Id = e.AgentId,
                        Name = e.Agent.Name,
                        Role = e.Agent.Role
                    }
                },
                predicate: e => e.Id == id);

            return schedule;
        }
    }
}