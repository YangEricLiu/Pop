namespace SE.DSP.Foundation.Infrastructure.Enumerations
{
    public enum AlarmErrorCode
    {
        TagNotExist = 001, //ui
        AlarmSettingNotExist = 002, //ui
        AlarmSettingChanged = 003, //ui
        AlartSettingVersionIsNull = 004,
        TriggerCycleIsNull = 005,
        TriggerCycleIsNotNull = 006,
        AlarmSubscribersIsNull = 007,
        AlarmCalendarIsNull = 008,
        AlarmThresholdIsNull = 009,
        AlarmThresholdValueIllegal = 010,
        AlarmCalendarNotExist = 011, //ui
        AlarmDateCalendarIsNull = 012,
        AlarmDateCalendarIllegal = 013,
        AlarmTimeCalendarIllegal = 014,
        AlarmSettingExist = 015, //ui
        AlarmSubscribersNotExist = 016, //ui

        AlarmNotificationNotExist = 050,
        AlarmNotificationChanged = 051
    }
}