using AutoMapper;
using DrawManager.Domain.Entities;

namespace DrawManager.Api.Features.PrizeSelectionSteps
{
    public class PrizeSelectionStepMappings : Profile
    {
        public PrizeSelectionStepMappings()
        {
            CreateMap<PrizeSelectionStep, PrizeSelectionStepEnvelope>(MemberList.None);
                //.ForMember(psse => psse.PrizeSelectionStepType, o => o.MapFrom(pss => pss.PrizeSelectionStepType));
        }
    }
}
