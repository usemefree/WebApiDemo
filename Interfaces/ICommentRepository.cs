using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Dtos.Comment;
using WebApiDemo.Models;

namespace WebApiDemo.Interfaces;

public interface ICommentRepository
{
    Task<List<Comment>> GetAllAsync();
    Task<CommentDto?> GetByIdAsync(int Id);
    Task<Comment?> CreateAsync(Comment commentModel);
    Task<Comment?> UpdateAsync(int Id, Comment commentModel);
    Task<Comment?> DeleteAsync(int Id);
}