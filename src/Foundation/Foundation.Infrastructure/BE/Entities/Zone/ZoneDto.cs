

using SE.DSP.Foundation.Infrastructure.BaseClass;

namespace SE.DSP.Foundation.Infrastructure.BE.Entities
{
    public class ZoneDto : DtoBase
    {
        public string Code { get; set; }

        public long? ParentId { get; set; }

        public string Comment { get; set; }
    }
}