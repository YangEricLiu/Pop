namespace SE.DSP.Foundation.Infrastructure.BE.Structure
{
    public class AlarmNotificationPaging
    {
        public AlarmNotificationOrder[] Orders { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}