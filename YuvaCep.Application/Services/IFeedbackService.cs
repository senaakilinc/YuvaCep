using YuvaCep.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public interface IFeedbackService
    {
        Task<FeedbackResponseDto> CreateAsync(CreateFeedbackDto request, Guid parentId);
        Task<FeedbackResponseDto> GetByIdAsync(Guid id);
        Task<List<FeedbackResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<List<FeedbackResponseDto>> GetByParentIdAsync(Guid parentId, int pageNumber = 1, int pageSize = 10);
        Task<List<FeedbackResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10);
        Task<List<FeedbackResponseDto>> GetByPriorityAsync(string priority, int pageNumber = 1, int pageSize = 10);
        Task<List<FeedbackResponseDto>> GetUnansweredAsync(int pageNumber = 1, int pageSize = 10);
        Task<FeedbackResponseDto> RespondAsync(Guid id, RespondFeedbackDto request, Guid teacherId);
        Task<bool> DeleteAsync(Guid id);
    }
}