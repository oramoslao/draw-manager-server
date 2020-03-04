using AutoMapper;
using DrawManager.Api.Infrastructure;
using DrawManager.Database.SqlServer;
using DrawManager.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Prizes
{
    public class List
    {
        public class Query : IRequest<PrizesEnvelope>
        {
            public int DrawId { get; private set; }

            public Query(int drawId)
            {
                DrawId = drawId;
            }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(q => q.DrawId)
                    .NotNull()
                    .GreaterThan(0);
            }
        }

        public class QueryHandler : IRequestHandler<Query, PrizesEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerSqlServerDbContext _context;

            public QueryHandler(IMapper mapper, DrawManagerSqlServerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<PrizesEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                // Getting the parent draw & your prizes
                var draw = await _context
                    .Draws
                    .Include(d => d.Prizes)
                        .ThenInclude(p => p.SelectionSteps)
                    .FirstOrDefaultAsync(d => d.Id == request.DrawId, cancellationToken);

                // Validations
                if (draw == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { Error = $"El sorteo con id '{ request.DrawId }' no existe." });
                }

                // Loading entrant info 
                var allPrizeSelectionSteps = draw
                    .Prizes
                    .SelectMany(p => p.SelectionSteps);
                foreach (var pss in allPrizeSelectionSteps)
                {
                    await _context
                        .Entry(pss)
                        .Reference(p => p.Entrant)
                        .LoadAsync();
                }

                // Mapping
                return new PrizesEnvelope
                {
                    Prizes = _mapper.Map<ICollection<Prize>, ICollection<PrizeEnvelope>>(draw.Prizes),
                    PrizesQty = draw.PrizesQty
                };
            }
        }
    }
}
