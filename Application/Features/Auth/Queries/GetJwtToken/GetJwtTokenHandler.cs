using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Features.Auth.Queries.GetJwtToken {
    public class GetJwtTokenHandler : IRequestHandler<GetJwtTokenQuery, string> {
        private readonly IConfiguration configuration;

        public GetJwtTokenHandler(IConfiguration configuration) {
            this.configuration = configuration;
        }

        public async Task<string> Handle(GetJwtTokenQuery request, CancellationToken cancellationToken) {
            // Create Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, request.IdentityUser.Email)
            };

            claims.AddRange(request.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

            // JWT Security Token Parameters
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
