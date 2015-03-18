

using SE.DSP.Foundation.Infrastructure.BE.Enumeration;
using SE.DSP.Foundation.Infrastructure.Constant;
using SE.DSP.Foundation.Infrastructure.Enumerations;
namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public struct AlarmNotificationOrder
    {
        public AlarmNotificationColumn Column;
        public OrderType OrderType;

        public override string ToString()
        {
            return this.Column + ASCII.SPACE + this.OrderType.ToString();
        }
    }
}