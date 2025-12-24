using Microsoft.EntityFrameworkCore;
using YuvaCep.Application.Dtos;
using YuvaCep.Domain.Entities;
using YuvaCep.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YuvaCep.Application.Services
{
    public class FeedbackService : IFeedbackService
    {
        private readonly YuvaCepDbContext _context;

        public FeedbackService(YuvaCepDbContext context)
        {
            _context = context;
        }

        public async Task<FeedbackResponseDto> CreateAsync(CreateFeedbackDto request, Guid parentId)
        {
            // Öğrenci kontrolü
            var studentExists = await _context.Students.AnyAsync(s => s.Id == request.StudentId);
            if (!studentExists)
            {
                throw new Exception("Öğrenci bulunamadı.");
            }

            // Veli kontrolü
            var parentExists = await _context.Parents.AnyAsync(p => p.Id == parentId);
            if (!parentExists)
            {
                throw new Exception("Veli bulunamadı.");
            }

            // Entity oluştur
            var feedback = new Feedback
            {
                Id = Guid.NewGuid(),
                ParentId = parentId,
                StudentId = request.StudentId,
                FeedBackType = request.FeedBackType,
                Subject = request.Subject,
                Content = request.Content,
                Priority = request.Priority ?? "Medium",
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return await GetByIdAsync(feedback.Id);
        }

        public async Task<FeedbackResponseDto> GetByIdAsync(Guid id)
        {
            var feedback = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feedback == null)
            {
                return null;
            }

            return MapToResponseDto(feedback);
        }

        public async Task<List<FeedbackResponseDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return feedbacks.Select(MapToResponseDto).ToList();
        }

        public async Task<List<FeedbackResponseDto>> GetByParentIdAsync(Guid parentId, int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .Where(f => f.ParentId == parentId)
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return feedbacks.Select(MapToResponseDto).ToList();
        }

        public async Task<List<FeedbackResponseDto>> GetByStudentIdAsync(Guid studentId, int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .Where(f => f.StudentId == studentId)
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return feedbacks.Select(MapToResponseDto).ToList();
        }

        public async Task<List<FeedbackResponseDto>> GetByPriorityAsync(string priority, int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .Where(f => f.Priority == priority)
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return feedbacks.Select(MapToResponseDto).ToList();
        }

        public async Task<List<FeedbackResponseDto>> GetUnansweredAsync(int pageNumber = 1, int pageSize = 10)
        {
            var feedbacks = await _context.Feedbacks
                .Include(f => f.Parent)
                .Include(f => f.Student)
                .Include(f => f.Teacher)
                .Where(f => string.IsNullOrEmpty(f.TeacherResponse))
                .OrderByDescending(f => f.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return feedbacks.Select(MapToResponseDto).ToList();
        }

        public async Task<FeedbackResponseDto> RespondAsync(Guid id, RespondFeedbackDto request, Guid teacherId)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return null;
            }

            // Öğretmen kontrolü
            var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teacherId);
            if (!teacherExists)
            {
                throw new Exception("Öğretmen bulunamadı.");
            }

            // Yanıt ekle
            feedback.TeacherResponse = request.TeacherResponse;
            feedback.RespondedByTeacherId = teacherId;
            feedback.RespondedAt = DateTime.UtcNow;
            feedback.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return false;
            }

            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();

            return true;
        }

        // Helper: Entity → Response DTO dönüşümü
        private FeedbackResponseDto MapToResponseDto(Feedback feedback)
        {
            return new FeedbackResponseDto
            {
                Id = feedback.Id,
                ParentId = feedback.ParentId,
                ParentName = feedback.Parent != null
                    ? $"{feedback.Parent.FirstName} {feedback.Parent.LastName}"
                    : "Bilinmiyor",
                StudentId = feedback.StudentId,
                StudentName = feedback.Student != null
                    ? $"{feedback.Student.Name} {feedback.Student.Surname}"
                    : "Bilinmiyor",
                CreatedAt = feedback.CreatedAt,
                UpdatedAt = feedback.UpdatedAt,

                FeedBackType = feedback.FeedBackType,
                Subject = feedback.Subject,
                Content = feedback.Content,
                Priority = feedback.Priority,

                // Öğretmen Yanıtı
                TeacherResponse = feedback.TeacherResponse,
                RespondedByTeacherId = feedback.RespondedByTeacherId,
                TeacherName = feedback.Teacher != null
                    ? $"{feedback.Teacher.FirstName}   {feedback.Teacher.LastName}"
                    : null,
                RespondedAt = feedback.RespondedAt
            };
        }
    }
}