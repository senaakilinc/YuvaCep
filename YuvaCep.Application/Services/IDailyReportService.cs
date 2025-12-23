using YuvaCep.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public interface IDailyReportService
    {
        Task<DailyReportResponseDto> CreateAsync(CreateDailyReportDto request, Guid teacherId);
        Task<DailyReportResponseDto> GetByIdAsync(Guid id);
        Task<List<DailyReportResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<List<DailyReportResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10);
        Task<List<DailyReportResponseDto>> GetByDateRangeAsync(Guid studentId, DateTime startDate, DateTime endDate);
        Task<List<DailyReportResponseDto>> GetTodayReportsAsync();
        Task<DailyReportResponseDto> UpdateAsync(Guid id, UpdateDailyReportDto request);
        Task<bool> DeleteAsync(Guid id);
    }
}