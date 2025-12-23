using YuvaCep.Application.Dtos;

namespace YuvaCep.Application.Services
{
    public interface IMonthlyPlanService
    {
        Task<MonthlyPlanResponseDto> CreateAsync(CreateMonthlyPlanDto request, Guid teacherId);
        Task<MonthlyPlanResponseDto> GetByIdAsync(Guid id);
        Task<List<MonthlyPlanResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<List<MonthlyPlanResponseDto>> GetByClassIdAsync(Guid classId, int year, int month);
        Task<List<MonthlyPlanResponseDto>> GetCurrentMonthByClassIdAsync(Guid classId);
        Task<List<MonthlyPlanResponseDto>> GetByPlanTypeAsync(Guid classId, string planType);
        Task<MonthlyPlanResponseDto> UpdateAsync(Guid id, UpdateMonthlyPlanDto request);
        Task<MonthlyPlanResponseDto> UpdateImageAsync(Guid id, string newImageUrl);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> SetActiveStatusAsync(Guid id, bool isActive);
    }
}