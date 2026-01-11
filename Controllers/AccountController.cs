using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiDemo.Data;
using WebApiDemo.Dtos.Account;
using WebApiDemo.Interfaces;
using WebApiDemo.Models;

namespace WebApiDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly UserManager<AppUser> userManager;
    private readonly ITokenService tokenService;
    private readonly SignInManager<AppUser> signInManager;

    public AccountController(UserManager<AppUser> usermanager, ITokenService tokenService, SignInManager<AppUser> signInManager)
    {
        this.userManager = usermanager;
        this.tokenService = tokenService;
        this.signInManager = signInManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await userManager.Users.FirstOrDefaultAsync(x => x.UserName.ToUpper() == loginDto.UserName.ToUpper());
        if (user is null) return Unauthorized("Invalid UserName!");
        var valid = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if (!valid.Succeeded) return Unauthorized("Invalid UserName and password!");
        return Ok(
            new NewUserDto
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = tokenService.CreateToken(user)
            }
        );
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var appUser = new AppUser()
            {
                UserName = registerDto.UserName,
                Email = registerDto.EmailAddress
            };
            var createUser = await userManager.CreateAsync(appUser, registerDto.Password);
            if (createUser is null)
                return StatusCode(500, createUser.Errors);

            if (createUser.Succeeded)
            {
                var roleResult = await userManager.AddToRoleAsync(appUser, "User");
                if (roleResult is null)
                    return StatusCode(500, roleResult.Errors);
                if (roleResult.Succeeded)
                    return Ok(
                        new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                            Token = tokenService.CreateToken(appUser),
                        }
                    );
            }
            return (BadRequest("Failed to create user"));
        }
        catch (Exception ex)
        {
            return (BadRequest(ex.Message));
        }
    }
}
