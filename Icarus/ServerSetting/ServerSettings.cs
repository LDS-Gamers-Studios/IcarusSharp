namespace Icarus.ServerSetting
{
    public enum ServerSettings
    {
        [ServerSetting("Moderation", "Logs", "The channel which will post non-flag moderation info, such as leaves/joins.", ServerSettingType.Channel)]
        Mod_ModLogs,
        [ServerSetting("Moderation", "Flags", "The channel which will post flags, such as filter catches or member reports.", ServerSettingType.Channel)]
        Mod_ModFlags,
        [ServerSetting("Moderation", "Role", "The role which is permitted to use mod commands/actions.", ServerSettingType.Role)]
        Mod_ModRole,
        [ServerSetting("Moderation", "Discussion", "The channel which moderators will discuss things. Used for flag linking, auto message embeding, etc.", ServerSettingType.Channel)]
        Mod_ModDiscussion,
        [ServerSetting("Moderation", "Mute Category", "The category where mute channels will go.", ServerSettingType.Channel)]
        Mod_MuteCategory,
        [ServerSetting("Moderation", "Muted", "The muted role.", ServerSettingType.Role)]
        Mod_MuteRole,
        [ServerSetting("Moderation", "User Updates", "The channel where avatar and name updates will go.", ServerSettingType.Channel)]
        Mod_UserUpdates,
        [ServerSetting("Moderation", "Watch List - New", "The watch list channel for members without trusted.", ServerSettingType.Channel)]
        Mod_WatchListNew,
        [ServerSetting("Moderation", "Watch List - Eyes", "The watch list channel for members who are watched.", ServerSettingType.Channel)]
        Mod_WatchListEyes,
        [ServerSetting("Moderation", "Trusted", "The role for those who are trusted with images/link embedding.", ServerSettingType.Role)]
        Mod_Trusted,
        [ServerSetting("Moderation", "Trusted Plus", "The role for those who are trusted with streaming in VCs.", ServerSettingType.Role)]
        Mod_TrustedPlus,

        [ServerSetting("Management", "Housekeeping", "The channel which will have server housekeeping notifications posted to it.", ServerSettingType.Channel)]
        Management_Housekeeping,
        [ServerSetting("Management", "Cake Day Member Count", "The member count for cake-day countdown. Leave blank to disable.", ServerSettingType.Integer)]
        Management_CakeDayMembers,

        [ServerSetting("Starboard", "Channel", "The channel for starboard posts.", ServerSettingType.Channel)]
        Starboard_Channel,
        [ServerSetting("Starboard", "Star Count", "The number of star emotes a message must get to be posted.", ServerSettingType.Integer)]
        Starboard_StarCount,
        [ServerSetting("Starboard", "Non-Star Count", "The number of non-star emotes a message must get to be posted.", ServerSettingType.Integer)]
        Starboard_NonStarCount,
        [ServerSetting("Starboard", "Ignored Channels", "A comma separated list of IDs of channels to be ignored for star board posts.", ServerSettingType.ChannelCollection)]
        Starboard_IgnoredChannels,

        [ServerSetting("Terra", "Heads of House Commons", "Channel for Terra-related notifications.", ServerSettingType.Channel)]
        Terra_HeadCommons,
        [ServerSetting("Terra", "RP Allowed", "Role for users who are allowed to use roleplay features on the site.", ServerSettingType.Role)]
        Terra_RPAllowedRole,
        [ServerSetting("Terra", "RP Allowed Channels", "A comma separated list of IDs of channels that can have roleplay characters.", ServerSettingType.ChannelCollection)]
        Terra_RPAllowedChannels,
        [ServerSetting("Terra", "House Starcamp", "Starcamp commons.", ServerSettingType.Channel)]
        Terra_StarcampCommons,
        [ServerSetting("Terra", "House Brighbeam", "Brighbeam commons.", ServerSettingType.Channel)]
        Terra_StarcampBrightbeam,
        [ServerSetting("Terra", "House Freshbeast", "Freshbeast commons.", ServerSettingType.Channel)]
        Terra_StarcampFreshbeast,

        [ServerSetting("Random", "Birthday Channel", "Where birthday posts will go.", ServerSettingType.Channel)]
        Random_Birthday,
        [ServerSetting("Random", "Join/Leave", "Where member join and leave posts will go.", ServerSettingType.Channel)]
        Random_JoinLeave,
    }
}
