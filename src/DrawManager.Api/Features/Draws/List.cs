using AutoMapper;
using DrawManager.Api.Infrastructure;
using DrawManager.Database.SqlServer;
using DrawManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DrawManager.Api.Features.Draws
{
    public class List
    {
        public class Query : IRequest<DrawsEnvelope>
        {
            public int Limit { get; set; }
            public int Offset { get; set; }

            public Query(int? limit, int? offset)
            {
                Limit = limit ?? 10;
                Offset = offset ?? 0;
            }
        }

        public class QueryHandler : IRequestHandler<Query, DrawsEnvelope>
        {
            private readonly IMapper _mapper;
            private readonly DrawManagerSqlServerDbContext _context;

            public QueryHandler(IMapper mapper, DrawManagerSqlServerDbContext context)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<DrawsEnvelope> Handle(Query request, CancellationToken cancellationToken)
            {
                // Getting draws that have not been executed yet.
                var draws = await _context
                    .Draws
                    .Include(d => d.Prizes)
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                // Mapping
                var drawsEnvelope = new DrawsEnvelope
                {
                    Draws = _mapper.Map<List<Draw>, List<DrawEnvelope>>(draws),
                    DrawsCount = await _context.Draws.CountAsync() //d => !d.ExecutedOn.HasValue, cancellationToken)
                };

                // Getting quantity of entries by draw.
                foreach (var drawEnvelope in drawsEnvelope.Draws)
                {
                    drawEnvelope.EntriesQty = await _context.DrawEntries.CountAsync(de => de.DrawId == drawEnvelope.Id, cancellationToken);
                }

                return drawsEnvelope;
            }
        }
    }
}
