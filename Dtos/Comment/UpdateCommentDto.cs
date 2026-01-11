using System.ComponentModel.DataAnnotations;

namespace WebApiDemo.Dtos.Comment;

public class UpdateCommentDto
{
    [Required]
    [MinLength(5, ErrorMessage = "Title must be 5 charater")]
    [MaxLength(250, ErrorMessage ="Title can nto be over 250 charager")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MinLength(5, ErrorMessage = "Content must be 5 charater")]
    [MaxLength(250, ErrorMessage ="Content can nto be over 250 charager")]
    public string Content { get; set; } = string.Empty;
}