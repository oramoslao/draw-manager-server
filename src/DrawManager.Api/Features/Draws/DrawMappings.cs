using AutoMapper;
using DrawManager.Domain.Entities;

namespace DrawManager.Api.Features.Draws
{
    public class DrawMappings : Profile
    {
        public DrawMappings()
        {
            CreateMap<Draw, DrawEnvelope>(MemberList.None);
        }
    }
}
