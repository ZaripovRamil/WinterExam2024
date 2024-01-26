using System.Security.Claims;
using AutoMapper;
using Contracts.Dbo;
using Database;
using Microsoft.AspNetCore.Identity;
using Models;

namespace DatabaseServices.Repositories;

public interface IUserRepository : IRepository<User>
{
    public Task<User?> FindByClaimAsync(ClaimsPrincipal user);
    public Task<SignInResult> SignInAsync(string username, string password);
    public Task<IdentityResult> CreateAsync(User user, string password);
    public Task<User?> FindByNameAsync(string username);
}

public class UserRepository : Repository, IUserRepository
{
    private readonly SignInManager<UserDbo> _signInManager;
    private readonly UserManager<UserDbo> _userManager;
    private readonly IMapper _mapper;

    public UserRepository(AppDbContext dbContext, UserManager<UserDbo> userManager, IMapper mapper, SignInManager<UserDbo> signInManager) : base(dbContext)
    {
        
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
    }

    public async Task AddAsync(User room)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return _mapper.Map<UserDbo?, User?>(await DbContext.Users.FindAsync(id));
    }

    public IEnumerable<User> GetAll()
    {
        return _mapper.Map<UserDbo[], IEnumerable<User>>(_userManager.Users.ToArray());
    }

    public Task DeleteAsync(User room)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User room)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var userDbo = _mapper.Map<User, UserDbo>(user);
        return await _userManager.CreateAsync(userDbo, password);
    }

    public async Task<User?> FindByNameAsync(string username)
    {
        return _mapper.Map<UserDbo?, User?>(await _userManager.FindByNameAsync(username));
    }

    public async Task<User?> FindByClaimAsync(ClaimsPrincipal claim)
    {
        return _mapper.Map<UserDbo?, User?>(await _userManager.GetUserAsync(claim));
    }

    public async Task<SignInResult> SignInAsync(string username, string password)
    {
        return await _signInManager.PasswordSignInAsync(username, password, true, false);
    }
}