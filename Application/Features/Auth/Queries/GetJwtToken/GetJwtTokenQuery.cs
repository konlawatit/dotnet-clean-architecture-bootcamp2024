using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Queries.GetJwtToken {
    public class GetJwtTokenQuery: IRequest<string> {
        public IdentityUser IdentityUser { get; set; }
        public IList<string> Roles { get; set; }
    }
}
