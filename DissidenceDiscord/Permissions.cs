using System;
using System.Collections.Generic;
using Metatron.Dissidence.Attributes;

namespace Metatron.DissidenceDiscord {
    namespace DiscordPermissions {
        // TODO: note that would restrict interfaces that consist of multiple others
        [Info(Module="Discord", Name="create instant invite", Description="Allows creation of instant invites")]
        public interface PCreateInstantInvite {}
        [Info(Module="Discord", Name="kick members", Description="Allows kicking members")]
        public interface PKickMembers {}
        [Info(Module="Discord", Name="ban members", Description="Allows banning members")]
        public interface PBanMembers {}
        [Info(Module="Discord", Name="administrator", Description="Allows all permissions and bypasses channel permission overwrites")]
        public interface PAdministrator {}
        [Info(Module="Discord", Name="manage channels", Description="Allows management and editing of channels")]
        public interface PManageChannels {}
        [Info(Module="Discord", Name="manage guild", Description="Allows management and editing of the guild")]
        public interface PManageGuild {}
        [Info(Module="Discord", Name="add reactions", Description="Allows for the addition of reactions to messages")]
        public interface PAddReactions {}
        [Info(Module="Discord", Name="view audit log", Description="Allows for viewing of audit logs")]
        public interface PViewAuditLog {}
        [Info(Module="Discord", Name="allow priority speaker", Description="Allows for using priority speaker in a voice channel")]
        public interface PPrioritySpeaker {}
        [Info(Module="Discord", Name="stream", Description="Allows the user to go live")]
        public interface PStream {}
        [Info(Module="Discord", Name="view channel", Description="Allows guild members to view a channel, which includes reading messages in text channels")]
        public interface PViewChannel {}
        [Info(Module="Discord", Name="send messages", Description="Allows for sending messages in a channel")]
        public interface PSendMessages {}
        [Info(Module="Discord", Name="send text-to-speech messages", Description="Allows for sending of `/tts` messages")]
        public interface PSendTTSMessages {}
        [Info(Module="Discord", Name="manage messages", Description="Allows for deletion of other users messages")]
        public interface PManageMessages {}
        [Info(Module="Discord", Name="automatically create embeds for links", Description="Links sent by users with this permission will be auto-embedded")]
        public interface PEmbedLinks {}
        [Info(Module="Discord", Name="send files", Description="Allows for uploading images and files")]
        public interface PAttachFiles {}
        [Info(Module="Discord", Name="read messages", Description="Allows for reading of message history")]
        public interface PReadMessageHistory {}
        [Info(Module="Discord", Name="mention @everyone and @here", Description="Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel")]
        public interface PMentionEveryone {}
        [Info(Module="Discord", Name="use external emojis", Description="Allows the usage of custom emojis from other servers")]
        public interface PUseExternalEmojis {}
        [Info(Module="Discord", Name="view guild insights", Description="Allows for viewing guild insights")]
        public interface PViewGuildInsights {}
        [Info(Module="Discord", Name="connect to voice", Description="Allows for joining of a voice channel")]
        public interface PConnect {}
        [Info(Module="Discord", Name="speak", Description="Allows for speaking in a voice channel")]
        public interface PSpeak {}
        [Info(Module="Discord", Name="mute members", Description="Allows for muting members in a voice channel")]
        public interface PMuteMembers {}
        [Info(Module="Discord", Name="deafen members", Description="Allows for deafening of members in a voice channel")]
        public interface PDeafenMembers {}
        [Info(Module="Discord", Name="move members between voice channels", Description="Allows for moving of members between voice channels")]
        public interface PMoveMembers {}
        [Info(Module="Discord", Name="use voice activity detection", Description="Allows for using voice-activity-detection in a voice channel")]
        public interface PUseVAD {}
        [Info(Module="Discord", Name="change nickname", Description="Allows for modification of own nickname")]
        public interface PChangeNickname {}
        // NOTE: discord docs have a type and leave the apostrophe out
        [Info(Module="Discord", Name="manage nicknames", Description="Allows for modification of other users' nicknames")]
        public interface PManageNicknames {}
        [Info(Module="Discord", Name="manage roles", Description="Allows management and editing of roles")]
        public interface PManageRoles {}
        [Info(Module="Discord", Name="manage webhooks", Description="Allows management and editing of webhooks")]
        public interface PManageWebhooks {}
        [Info(Module="Discord", Name="manage emojis", Description="Allows management and editing of emojis")]
        public interface PManageEmojis {}
    }

    namespace OAuth2Permissions {
        [Info(Module="Discord.OAuth2", Name="view requester without email", Description="Allows [/users/@me]() without `email`")]
        public interface PBot {}
        [Info(Module="Discord.OAuth2", Name="view email of requester", Description="Enables [/users/@me]() to return an `email`")]
        public interface PConnections {}
        [Info(Module="Discord.OAuth2", Name="view connections", Description="Allows [/users/@me/connections]() to return linked third-party accounts")]
        public interface PEmail {}
        [Info(Module="Discord.OAuth2", Name="view guilds of a user", Description="Allows [/users/@me/guilds]() to return basic information about all of a user's guilds")]
        public interface PIdentify {}
        [Info(Module="Discord.OAuth2", Name="join guilds", Description="Allows [/guilds/{guild.id}/members/{user.id}]() to be used for joining users to a guild")]
        public interface PGuilds {}
        // NOTE: modified from discord's description to match format of the other descriptions 
        // original: wait wot where it go???
        [Info(Module="Discord.OAuth2", Name="join group DMs", Description="Allows [/channels/{channel.id}/recipients/{user.id}]() to be used for joining users to a group DM")]
        public interface PGuildsJoin {}
        [Info(Module="Discord.OAuth2", Name="RPC", Description="For local rpc server access, this allows you to control a user's local Discord client - whitelist only")]
        public interface PGDMJoin {}
        [Info(Module="Discord.OAuth2", Name="API access over RPC", Description="For local rpc server api access, this allows you to access the API as the local user - whitelist only")]
        public interface PMessagesRead {}
        [Info(Module="Discord.OAuth2", Name="read notifications over RPC", Description="For local rpc server api access, this allows you to receive notifications pushed out to the user - whitelist only")]
        public interface PRPC {}
        [Info(Module="Discord.OAuth2", Name="put bot in selected guild", Description="For OAuth2 bots, this puts the bot in the user's selected guild by default")]
        public interface PRPCAPI {}
        // TODO: not 100% on the name
        [Info(Module="Discord.OAuth2", Name="read webhook", Description="This generates a webhook that is returned in the OAuth token response for authorization code grants")]
        public interface PRPCNotificationsRead {}
        [Info(Module="Discord.OAuth2", Name="read messages", Description="For local rpc server api access, this allows you to read messages from all client channels (otherwise restricted to channels/guilds your app creates)")]
        public interface PWebhookIncoming {}
        [Info(Module="Discord.OAuth2", Name="write user's applications' build data", Description="Allows your app to upload/update builds for a user's applications - whitelist only")]
        public interface PApplicationsBuildsUpload {}
        [Info(Module="Discord.OAuth2", Name="read user's applications' build data", Description="Allows your app to read build data for a user's applications")]
        public interface PApplicationsBuildsRead {}
        [Info(Module="Discord.OAuth2", Name="read user's applications' store data", Description="Allows your app to read and update store data (SKUs, store listings, achievements, etc.) for a user's applications")]
        public interface PApplicationsStoreUpdate {}
        [Info(Module="Discord.OAuth2", Name="read user's applications' entitlements", Description="Allows your app to read entitlements for a user's applications")]
        public interface PApplicationsEntitlements {}
        [Info(Module="Discord.OAuth2", Name="read user activity", Description="Allows your app to fetch data from a user's \"Now Playing/Recently Played\" list - whitelist only")]
        public interface PActivitiesRead {}
        // NOTE: documentation also says "(NOT REQUIRED FOR GAMESDK ACTIVITIY MANAGER)" however bot has nothing to do with gamesdk (at least for now)
        [Info(Module="Discord.OAuth2", Name="write user activity", Description="Allows your app to update a user's activity - whitelist only")]
        public interface PActivitiesWrite {}
        [Info(Module="Discord.OAuth2", Name="read user relationships", Description="Allows your app to know a user's friends and implicit relationships - whitelist only")]
        public interface PRelationshipsRead {}
    }
}
