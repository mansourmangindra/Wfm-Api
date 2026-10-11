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
            // Safely parse strings from frontend into native DateOnly and TimeOnly types
            if (!DateOnly.TryParse(dto.Date, out var parsedDate) ||
                !TimeOnly.TryParse(dto.StartTime, out var parsedStartTime) ||
                !TimeOnly.TryParse(dto.EndTime, out var parsedEndTime))
            {
                return (false, "Invalid date or time format provided.", null);
            }

            if (parsedStartTime >= parsedEndTime)
            {
                return (false, "Start time must be strictly before end time.", null);
            }

            var existingSchedules = await _unitOfWork.SchedulesRepository.FindAsync(s =>
                s.AgentId == dto.AgentId &&
                s.Date == parsedDate &&
                s.StartTime < parsedEndTime &&
                s.EndTime > parsedStartTime &&
                s.Status != "Cancelled"
            );

            if (existingSchedules.Any())
            {
                return (false, "Validation Error: Agent already has an overlapping schedule during this time slot.", null);
            }

            var schedule = new Schedule
            {
                AgentId = dto.AgentId,
                Date = parsedDate,
                StartTime = parsedStartTime,
                EndTime = parsedEndTime,
                Status = dto.Status ?? "Scheduled"
            };

            await _unitOfWork.SchedulesRepository.AddAsync(schedule);

            string timePeriodKey = parsedStartTime.ToString("HH:mm");
            var staffingReq = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == parsedDate && s.TimePeriod == timePeriodKey
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
                    Date = e.Date.ToString(),
                    StartTime = e.StartTime.ToString(),
                    EndTime = e.EndTime.ToString(),
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

        public async Task<(bool Success, string Message, Schedule? Schedule)> UpdateScheduleAsync(int id, ScheduleDetails dto)
        {
            var existingSchedule = await _unitOfWork.SchedulesRepository.GetByIdAsync(id);
            if (existingSchedule == null)
            {
                return (false, "Schedule not found.", null);
            }

            if (!DateOnly.TryParse(dto.Date, out var parsedDate) ||
                !TimeOnly.TryParse(dto.StartTime, out var parsedStartTime) ||
                !TimeOnly.TryParse(dto.EndTime, out var parsedEndTime))
            {
                return (false, "Invalid date or time format provided.", null);
            }

            if (parsedStartTime >= parsedEndTime)
            {
                return (false, "Start time must be strictly before end time.", null);
            }

            string oldTimePeriodKey = existingSchedule.StartTime.ToString("HH:mm");
            var oldStaffingReq = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == existingSchedule.Date && s.TimePeriod == oldTimePeriodKey
            );
            if (oldStaffingReq != null && oldStaffingReq.ScheduledAgents > 0)
            {
                oldStaffingReq.ScheduledAgents -= 1;
                _unitOfWork.StaffingRequirementsRepository.Update(oldStaffingReq);
            }

            existingSchedule.AgentId = dto.AgentId;
            existingSchedule.Date = parsedDate;
            existingSchedule.StartTime = parsedStartTime;
            existingSchedule.EndTime = parsedEndTime;
            existingSchedule.Status = dto.Status ?? "Scheduled";

            _unitOfWork.SchedulesRepository.Update(existingSchedule);

            string newTimePeriodKey = parsedStartTime.ToString("HH:mm");
            var newStaffingReq = await _unitOfWork.StaffingRequirementsRepository.FirstOrDefaultAsync(s =>
                s.Date == parsedDate && s.TimePeriod == newTimePeriodKey
            );
            if (newStaffingReq != null)
            {
                newStaffingReq.ScheduledAgents += 1;
                _unitOfWork.StaffingRequirementsRepository.Update(newStaffingReq);
            }

            await _unitOfWork.SaveChangesAsync();
            return (true, "Schedule updated successfully.", existingSchedule);
        }
    }
}