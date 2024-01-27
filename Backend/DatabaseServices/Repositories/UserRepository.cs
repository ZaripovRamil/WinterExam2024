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
    public Task SetRatingAsync(Guid userId, int rating);
    public Task<int> GetRatingAsync(Guid userId);
}

public class UserRepository : Repository, IUserRepository
{
    private readonly SignInManager<UserDbo> _signInManager;
    private readonly UserManager<UserDbo> _userManager;
    private readonly IMapper _mapper;
    private readonly IMongoDatabase _mongoDatabase;

    public UserRepository(
        AppDbContext dbContext, 
        UserManager<UserDbo> userManager, 
        IMapper mapper,
        SignInManager<UserDbo> signInManager, 
        IMongoDatabase mongoDatabase) : base(dbContext)
    {
        _userManager = userManager;
        _mapper = mapper;
        _signInManager = signInManager;
        _mongoDatabase = mongoDatabase;
    }

    public Task AddAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetAsync(Guid id)
    {
        var user = _mapper.Map<UserDbo?, User?>(await DbContext.Users.FindAsync(id));
        if (user is null) return user;
        user.Rating = await GetRatingAsync(id);
        return user;
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        var users = _mapper.Map<UserDbo[], User[]>(_userManager.Users.ToArray());
        foreach (var user in users)
            user.Rating =await GetRatingAsync(user.Id);
        return users;
    }

    public Task DeleteAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(User user)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> CreateAsync(User user, string password)
    {
        var userDbo = _mapper.Map<User, UserDbo>(user);
        var result = await _userManager.CreateAsync(userDbo, password);
        var userId = (await FindByNameAsync(user.UserName)).Id;
        await _mongoDatabase.SetRatingAsync(new UserRatingDbo() { Rating = 100, UserId = userId });
        return result;
    }

    public async Task<User?> FindByNameAsync(string username)
    {
        return _mapper.Map<UserDbo?, User?>(await _userManager.FindByNameAsync(username));
    }

    public async Task SetRatingAsync(Guid userId, int rating)
    {
        await _mongoDatabase.SetRatingAsync(new UserRatingDbo { Rating = rating, UserId = userId });
    }

    public async Task<int> GetRatingAsync(Guid userId)
    {
        return (await _mongoDatabase.GetRatingAsync(userId))?.Rating ?? 0;
    }

    public async Task<User?> FindByClaimAsync(ClaimsPrincipal claim)
    {
        var id = claim.Claims.First(c => c.Type == "Id").Value;
        return _mapper.Map<UserDbo?, User?>(await _userManager.FindByIdAsync(id));
    }

    public async Task<SignInResult> SignInAsync(string username, string password)
    {
        return await _signInManager.PasswordSignInAsync(username, password, true, false);
    }
}