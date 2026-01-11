using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using WebApiDemo.Data;
using WebApiDemo.Dtos.Stock;
using WebApiDemo.HelperFilter;
using WebApiDemo.Interfaces;
using WebApiDemo.Mappers;
using WebApiDemo.Repositorys;

namespace WebApiDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class StockController : ControllerBase
{
    private readonly ApplicationDbContext Context;
    private readonly IStockRepository stockRepository;
    public StockController(ApplicationDbContext context, IStockRepository stockRepository)
    {
        this.Context = context;
        this.stockRepository = stockRepository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stockModel = await stockRepository.GetAllAsync();
        var stock = stockModel.Select(x => x.ToStockDto()).ToList() ;
        return Ok(stock);
    }

    [HttpGet("{Id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int Id)
    {
        var stockModel = await stockRepository.GetByIdAsync(Id);
        if (stockModel == null)
            return NotFound();
        return Ok(stockModel.ToStockDto());
    }

    [HttpGet("Search")]
    public async Task<IActionResult> GetFilter([FromQuery] StockQueryObject query)
    {
        var stockModel = await stockRepository.GetFilter(query);
        if (stockModel == null)
            return NotFound();
        return Ok(stockModel);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockdto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stock = stockdto.ToStockFromCreateDto();
        await stockRepository.CreateAsync(stock);
        return CreatedAtAction(nameof(GetById), new { Id = stock.Id }, stock.ToStockDto());
    }

    [HttpPut]
    [Route("{Id:int}")]
    public async Task<IActionResult> Update([FromRoute] int Id, [FromBody] UpdateStockRequestDto updateStockRequestDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var stockModel = await stockRepository.UpdateAsync(Id, updateStockRequestDto);
        if (stockModel is null)
            return NotFound();
        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{Id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int Id)
    {

        var stockModel = await stockRepository.DeleteAsync(Id);
        if (stockModel is null)
            return NotFound();
        return NoContent();
    }
}
