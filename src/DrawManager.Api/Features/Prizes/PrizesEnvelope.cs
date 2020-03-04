using System.Collections.Generic;

namespace DrawManager.Api.Features.Prizes
{
    public class PrizesEnvelope
    {
        public ICollection<PrizeEnvelope> Prizes { get; set; }
        public int PrizesQty { get; set; }
    }
}
