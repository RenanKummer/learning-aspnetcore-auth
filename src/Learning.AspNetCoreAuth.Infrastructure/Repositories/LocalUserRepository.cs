using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Learning.AspNetCoreAuth.Core.Models;
using Learning.AspNetCoreAuth.Core.Repositories;

namespace Learning.AspNetCoreAuth.Infrastructure.Repositories;

public class LocalUserRepository : IUserRepository
{
    private readonly Dictionary<string, User> _users = new()
    {
        ["john.doe"] = new User
        {
            Id = "john.doe", 
            Name = "John Doe",
            Password = "A6xnQhbz4Vx2HuGl4lXwZ5U2I8iziLRFnhP5eNfIRvQ=", // 1234
            Claims = 
            [
                new UserClaim(ClaimTypes.NameIdentifier, "john.doe"),
                new UserClaim(ClaimTypes.Name, "John Doe"),
                new UserClaim(ClaimTypes.Role, "admin"),
                new UserClaim("CustomKey", "any")
            ]
        } 
    };
    
    public Task<User?> FindByCredentialsAsync(string id, string password)
    {
        var hashedPassword = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var base64Password = Convert.ToBase64String(hashedPassword);

        return _users.TryGetValue(id, out var user) && user.Password == base64Password
            ? Task.FromResult<User?>(user)
            : Task.FromResult<User?>(null);
    }
}