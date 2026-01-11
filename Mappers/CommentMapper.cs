using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.Dtos.Comment;
using WebApiDemo.Models;

namespace WebApiDemo.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment commnetModel)
    {
        return new CommentDto
        {
            Id = commnetModel.Id,
            Title = commnetModel.Title,
            Content = commnetModel.Content,
            CreatedOn = commnetModel.CreatedOn,
            CreatedBy = commnetModel.AppUser != null ? commnetModel.AppUser.UserName : "Unknown",
            StockId = commnetModel.StockId
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentDto commnetDto, int StockId)
    {
        return new Comment
        {
            Title = commnetDto.Title,
            Content = commnetDto.Content,
           // CreatedOn = commnetDto.CreatedOn,
            StockId = StockId
        };
    }

     public static Comment ToCommentFromUpdate(this UpdateCommentDto commnetDto)
    {
        return new Comment
        {
            Title = commnetDto.Title,
            Content = commnetDto.Content,
        };
    }
}
