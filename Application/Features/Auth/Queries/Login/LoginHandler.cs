using Application.Features.Auth.Queries.GetJwtToken;
using Application.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Queries.Login {
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto> {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IMediator mediator;
        public LoginHandler(UserManager<IdentityUser> userManager, IMediator mediator) {
            this.userManager = userManager;
            this.mediator = mediator;
        }

        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken) {
            var identityUser = await userManager.FindByEmailAsync(request.LoginRequest.Email);
            if (identityUser is null) {
                return null;
            }

            var checkPasswordResult = await userManager.CheckPasswordAsync(identityUser, request.LoginRequest.Password);

            if (checkPasswordResult is false) {
                return null;
            }

            var roles = await userManager.GetRolesAsync(identityUser);

            //Create a token and response
            var jwtToken = await mediator.Send(new GetJwtTokenQuery { IdentityUser = identityUser, Roles = roles });

            var response = new LoginResponseDto {
                Email = request.LoginRequest.Email,
                Roles = roles.ToList(),
                Token = jwtToken
            };

            return response;
        }
    }
}
