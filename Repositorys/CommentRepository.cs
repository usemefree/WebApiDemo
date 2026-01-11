using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Dtos.Comment;
using WebApiDemo.Interfaces;
using WebApiDemo.Mappers;
using WebApiDemo.Models;

namespace WebApiDemo.Repositorys;

public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext context;

    public CommentRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
        return await context.Comments.Include(x => x.AppUser).ToListAsync();
    }

    public async Task<CommentDto?> GetByIdAsync(int Id)
    {
        var comment = await context.Comments.Include(x => x.AppUser).FirstOrDefaultAsync(x => x.Id == Id);
        if (comment is null)
            return null;
        return comment.ToCommentDto();
    }
    public async Task<Comment?> CreateAsync(Comment commentModel)
    {
        await context.Comments.AddAsync(commentModel);
        await context.SaveChangesAsync();
        return commentModel;
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
        var existingComment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        if (existingComment is null)
            return null;
        existingComment.Title = commentModel.Title;
        existingComment.Content = commentModel.Content;
        await context.SaveChangesAsync();
        return existingComment;
    }

    public async Task<Comment?> DeleteAsync(int Id)
    {
        var existingComment = await context.Comments.FirstOrDefaultAsync(x => x.Id == Id);
        if (existingComment is null)
            return null;
        context.Comments.Remove(existingComment);
        await context.SaveChangesAsync();
        return existingComment;
    }
}