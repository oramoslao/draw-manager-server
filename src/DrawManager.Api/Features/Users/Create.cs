using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using DrawManager.Api.Infrastructure;
using DrawManager.Database.SqlServer;
using DrawManager.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DrawManager.Api.Features.Users
{
    public class Create
    {
        public class UserData
        {
            public string Login { get; set; }
            public string Password { get; set; }
        }

        public class Command : IRequest<UserEnvelope>
        {
            public UserData UserData { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(c => c.UserData).NotNull();
            }
        }

        public class Handler : IRequestHandler<Command, UserEnvelope>
        {
            private readonly IMapper _mapper;
            //private readonly DrawManagerDbContext _context;
            private readonly DrawManagerSqlServerDbContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;

            //public Handler(IMapper mapper, DrawManagerDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            public Handler(IMapper mapper, DrawManagerSqlServerDbContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator)
            {
                _mapper = mapper;
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
            }

            public async Task<UserEnvelope> Handle(Command request, CancellationToken cancellationToken)
            {
                // Getting if user exist
                var existUser = await _context
                    .Users
                    .Where(u => string.Equals(u.Login.ToLower(), request.UserData.Login.ToLower()))
                    .AnyAsync(cancellationToken);

                // Validations
                if (existUser)
                {
                    throw new RestException(HttpStatusCode.BadRequest, new { Error = $"El usuario {request.UserData.Login} ya se encuentra en uso." });
                }

                // Creating user 
                var salt = Guid.NewGuid().ToByteArray();
                var user = new User
                {
                    Login = request.UserData.Login,
                    Hash = _passwordHasher.Hash(request.UserData.Password, salt),
                    Salt = salt
                };

                // Saving
                _context.Users.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                // Mapping
                var userEnvelope = _mapper.Map<User, UserEnvelope>(user);
                userEnvelope.Token = await _jwtTokenGenerator.CreateToken(user.Login);
                return userEnvelope;
            }
        }
    }
}
