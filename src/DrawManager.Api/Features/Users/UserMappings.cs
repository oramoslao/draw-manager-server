using AutoMapper;
using DrawManager.Domain.Entities;

namespace DrawManager.Api.Features.Users
{
    public class UserMappings : Profile
    {
        public UserMappings()
        {
            CreateMap<User, UserEnvelope>(MemberList.None);
        }
    }
}
