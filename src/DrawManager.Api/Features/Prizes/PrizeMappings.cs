using AutoMapper;
using DrawManager.Domain.Entities;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizeMappings : Profile
    {
        public PrizeMappings()
        {
            CreateMap<Prize, PrizeEnvelope>(MemberList.None);
        }
    }
}
