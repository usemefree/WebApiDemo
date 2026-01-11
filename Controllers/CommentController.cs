using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using WebApiDemo.Data;
using WebApiDemo.Dtos.Comment;
using WebApiDemo.Extension;
using WebApiDemo.Interfaces;
using WebApiDemo.Mappers;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CommentController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly ICommentRepository repository;
    private readonly IStockRepository stockRepository;
    private readonly UserManager<AppUser> userManager;

    public CommentController(ApplicationDbContext context, ICommentRepository repository, IStockRepository stockRepository, UserManager<AppUser> userManager)
    {
        this.context = context;
        this.repository = repository;
        this.stockRepository = stockRepository;
        this.userManager = userManager;
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> AllComments()
    {
        var comments = await repository.GetAllAsync();
        var commentDto = comments.Select(x => x.ToCommentDto());
        return Ok(commentDto);
    }

    [HttpGet]
    [Route("{Id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var comment = await repository.GetByIdAsync(Id);
        if (comment is null)
            return NotFound();
        return Ok(comment);
    }

    [HttpPost("{StockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int StockId, [FromBody] CreateCommentDto createCommentDto)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);

        if (!await stockRepository.StockExists(StockId))
        {
            return BadRequest("Stock does not exits");
        }

        var userName = User.GetUserName();
        var user = await userManager.FindByNameAsync(userName);

        var commentModel = createCommentDto.ToCommentFromCreate(StockId);
        commentModel.AppUserId = user.Id;   
        
        await repository.CreateAsync(commentModel);

        return CreatedAtAction(nameof(GetById), new { Id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut]
    [Route("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateCommentDto)
    {
         if(!ModelState.IsValid) return BadRequest(ModelState);

        var commentModel = await repository.UpdateAsync(id, updateCommentDto.ToCommentFromUpdate());
        if (commentModel is null)
            return NotFound("Commetn not found");
        return Ok(commentModel);
    }

    [HttpDelete("{Id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int Id)
    {
        var commentModel = await repository.DeleteAsync(Id);
        if (commentModel is null)
            return NotFound($"Comment does not exists : {Id}");
        return NoContent();
    }
}