using MediatR;
using Microsoft.AspNetCore.Identity;


namespace Application.Features.Auth.Commands.Register {
    public class RegisterHandler : IRequestHandler<RegisterCommand, bool> {
        private readonly UserManager<IdentityUser> userManager;

        public RegisterHandler(UserManager<IdentityUser> userManager) {
            this.userManager = userManager;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken cancellationToken) {

            var user = new IdentityUser {
                UserName = request.Register.Email.Trim(),
                Email = request.Register.Email.Trim(),
            };

            var identityResult = await userManager.CreateAsync(user, request.Register.Password);
            if (identityResult.Succeeded is false) {
                return false;
            }

            identityResult = await userManager.AddToRoleAsync(user, "Reader");
            if (identityResult.Succeeded is false) {
                return false;
            }

            return true;
        }
    }
}
