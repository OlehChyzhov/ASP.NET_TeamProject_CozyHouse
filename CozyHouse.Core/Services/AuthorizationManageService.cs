using CozyHouse.Core.Domain.IdentityEntities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.ServiceContracts;
using Microsoft.AspNetCore.Identity;

namespace CozyHouse.Core.Services
{
    public class AuthorizationManageService : IAuthorizationManageService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInmanager;
        public AuthorizationManageService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInmanager = signInManager;
        }
        public async Task<ExtendedSignInResult> LoginAsync(string username, string password)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if (user == null) return new ExtendedSignInResult(SignInResult.Failed, "Account does not exist");

            await _signInmanager.PasswordSignInAsync(user, password, false, false);
            return new ExtendedSignInResult(SignInResult.Success, "Signed in successfully");
        }

        public async Task LogoutAsync()
        {
            await _signInmanager.SignOutAsync();
        }

        public async Task<IdentityResult> DeleteAsync(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return IdentityResult.Failed(new IdentityError() { Description = "Account does not exist" });

            await _userManager.DeleteAsync(user);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> RegisterWithRoleAsync(ApplicationUser user, string password, string role)
        {
            if (role != "User" && role != "Manager") return IdentityResult.Failed(new IdentityError() { Description = $"Wrong role {role}" });
            ApplicationUser? userFromDb = await _userManager.FindByIdAsync(user.Id.ToString());
            if (userFromDb != null) return IdentityResult.Failed(new IdentityError() { Description = "Account already exists" });

            user.SecurityStamp = Guid.NewGuid().ToString();
            user.Id = Guid.NewGuid();

            IdentityResult registrationResult = await _userManager.CreateAsync(user, password);
            if (registrationResult.Succeeded == false) return IdentityResult.Failed(registrationResult.Errors.ToArray());
            await _userManager.AddToRoleAsync(user, role);
            
            return IdentityResult.Success;
        }
        
        public async Task<ApplicationUser> GetUserAsync(Guid userId)
        {
            ApplicationUser? user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new ArgumentNullException();

            return user;
        }
    }
}
