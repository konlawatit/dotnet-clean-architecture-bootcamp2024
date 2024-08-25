using Application.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Queries.Login {
    public class LoginCommand : IRequest<LoginResponseDto> {
        public LoginRequestDto LoginRequest { get; set; }
    }
}
