namespace Icarus.ServerSetting
{
    public enum ServerSettings
    {
        [ServerSetting("Moderation", "Mod Logs", "The channel which will post non-flag moderation info, such as leaves/joins.", ServerSettingType.Channel)]
        Mod_ModLogs,
        [ServerSetting("Moderation", "Mod Flags", "The channel which will post flags, such as filter catches or member reports.", ServerSettingType.Channel)]
        Mod_ModFlags,
        [ServerSetting("Moderation", "Mod Role", "The role which is permitted to use mod commands/actions.", ServerSettingType.Role)]
        Mod_ModRole,
        [ServerSetting("Moderation", "Mod Discussion", "The channel which moderators will discuss things. Used for flag linking, auto message embeding, etc.", ServerSettingType.Channel)]
        Mod_ModDiscussion,

        [ServerSetting("Management", "Housekeeping Channel", "The channel which will have housekeeping updates/notifications posted to it.", ServerSettingType.Channel)]
        Management_Housekeeping,
        [ServerSetting("Management", "Cake Day Member Count", "The member count for cake-day countdown. Leave blank to disable.", ServerSettingType.Integer)]
        Management_CakeDayMembers
    }
}
