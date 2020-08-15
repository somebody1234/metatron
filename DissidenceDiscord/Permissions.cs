using System;
using System.Collections.Generic;
using Metatron.Dissidence.Attributes;

namespace Metatron.DissidenceDiscord {
    namespace DiscordPermissions {
        // TODO: maybe change descriptions. don't have to be the same as discord.
        // TODO: note that would restrict interfaces that consist of multiple others
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view invites", Description="Allows for viewing of invites")]
        public interface PViewInvites {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit invites", Description="Allows management and editing of invites")]
        public interface PManageInvites {}
        [NativeOnly]
        [Info(Module="Discord", Name="can create instant invite", Description="Allows creation of instant invites")]
        public interface PCreateInstantInvite : PManageInvites {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view HTML widget", Description="Allows for viewing of guild HTML widget")]
        public interface PViewWidget {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit HTML widget", Description="Allows management and editing of guild HTML widget")]
        public interface PManageWidget {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view members", Description="Allows for viewing of members")]
        public interface PViewUsers {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit members", Description="Allows management and editing of members")]
        public interface PManageUsers {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can kick members", Description="Allows kicking members")]
        public interface PKickUsers {}
        [NativeOnly]
        [Info(Module="Discord", Name="can kick members", Description="Allows kicking members")]
        public interface PKickMembers : PKickUsers {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can prune members", Description="Allows pruning members")]
        public interface PPruneUsers {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view bans", Description="Allows for viewing of bans")]
        public interface PViewBans {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit bans", Description="Allows management and editing of bans")]
        public interface PManageBans {}
        [Info(Module="Discord", Name="can ban members", Description="Allows banning members")]
        public interface PBanMembers : PManageBans, PViewBans {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can prune members", Description="Allows pruning members")]
        public interface PPruneMembers {}
        [NativeOnly]
        [Info(Module="Discord", Name="can administrate", Description="Allows all permissions and bypasses channel permission overwrites")]
        public interface PAdministrator {}
        [Info(Module="Discord", Name="can edit channels", Description="Allows management and editing of channels")]
        public interface PManageChannels {}
        [Info(Module="Discord", Name="can edit guild", Description="Allows management and editing of the guild")]
        public interface PManageGuild {}

#region Reactions
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view reactions", Description="Allows for viewing of reactions")]
        public interface PViewReactions {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit reactions", Description="Allows management and editing of reactions")]
        public interface PManageReactions {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can send reactions", Description="Allows adding and removing reactions")]
        public interface PSendReactions {}
        // NOTE: inherits since this discord permission corresponds to this dissidence permission
        // not that they should both be in one place at the same time.
        [Info(Module="Discord", Name="can add new reactions", Description="Allows for the addition of reactions to messages")]
        public interface PAddReactions : PSendReactions {}
        [Info(Module="Discord", Name="can clear reactions", Description="Allows clearing of reactions from a single message")]
        public interface PClearReactions {}
#endregion Reactions

        [Info(Module="Discord", Name="can view audit log", Description="Allows for viewing of audit logs")]
        public interface PViewAuditLog {}
        [Info(Module="Discord", Name="can allow priority speaker", Description="Allows for using priority speaker in a voice channel")]
        public interface PPrioritySpeaker {}
        [Info(Module="Discord", Name="can stream", Description="Allows the user to go live")]
        public interface PStream {}
        [Info(Module="Discord", Name="can view channel", Description="Allows guild members to view a channel, which includes reading messages in text channels by default")]
        public interface PViewChannel {}
        [Info(Module="Discord", Name="can send messages", Description="Allows for sending messages in a channel")]
        public interface PSendMessages {}
        [Info(Module="Discord", Name="can send text-to-speech messages", Description="Allows for sending of `/tts` messages")]
        public interface PSendTTSMessages {}
        [Info(Module="Discord", Name="can manage messages", Description="Allows for deletion of other users' messages")]
        public interface PManageMessages {}
        [Info(Module="Discord", Name="can automatically create embeds for links", Description="Links sent by users with this permission will be auto-embedded")]
        public interface PEmbedLinks {}
        [Info(Module="Discord", Name="can send files", Description="Allows for uploading images and files")]
        public interface PAttachFiles {}
        [Info(Module="Discord", Name="can read messages", Description="Allows for reading of message history")]
        public interface PReadMessageHistory {}
        [Info(Module="Discord", Name="can mention @everyone and @here", Description="Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel")]
        public interface PMentionEveryone {}
        [Info(Module="Discord", Name="can use external emojis", Description="Allows the usage of custom emojis from other servers")]
        public interface PUseExternalEmojis {}
        [Info(Module="Discord", Name="can view guild insights", Description="Allows for viewing guild insights")]
        public interface PViewGuildInsights {}
        [Info(Module="Discord", Name="can connect to voice", Description="Allows for joining of a voice channel")]
        public interface PConnect {}
        [Info(Module="Discord", Name="can speak", Description="Allows for speaking in a voice channel")]
        public interface PSpeak {}
        [Info(Module="Discord", Name="can mute members", Description="Allows for muting members in a voice channel")]
        public interface PMuteMembers {}
        [Info(Module="Discord", Name="can deafen members", Description="Allows for deafening of members in a voice channel")]
        public interface PDeafenMembers {}
        [Info(Module="Discord", Name="can move members between voice channels", Description="Allows for moving of members between voice channels")]
        public interface PMoveMembers {}
        [Info(Module="Discord", Name="can use voice activity detection", Description="Allows for using voice-activity-detection in a voice channel")]
        public interface PUseVAD {}
        [Info(Module="Discord", Name="can change nickname", Description="Allows for modification of own nickname")]
        public interface PChangeNickname {}
        // NOTE: discord docs have a type and leave the apostrophe out
        [Info(Module="Discord", Name="can edit nicknames", Description="Allows for modification of other users' nicknames")]
        public interface PManageNicknames {}
        [Info(Module="Discord", Name="can edit roles", Description="Allows management and editing of roles")]
        public interface PManageRoles {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view integrations", Description="Allows for viewing of integrations")]
        public interface PViewIntegrations {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can edit integrations", Description="Allows management and editing of integrations")]
        public interface PManageIntegrations {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="can view webhooks", Description="Allows for viewing of webhooks")]
        public interface PViewWebhooks {}
        [Info(Module="Discord", Name="can edit webhooks", Description="Allows management and editing of webhooks")]
        public interface PManageWebhooks {}
        [Info(Module="Discord", Name="can edit emojis", Description="Allows management and editing of emojis")]
        public interface PManageEmojis {}
        [DissidenceOnly]
        [Info(Module="Discord", Name="is owner", Description="Allows deletion of guild and ownership transfer")]
        public interface POwner {}
    }

    namespace OAuth2Permissions {
        [Info(Module="Discord.OAuth2", Name="can view requester without email", Description="Allows [/users/@me]() without `email`")]
        public interface PIdentify {}
        [Info(Module="Discord.OAuth2", Name="can view email of requester", Description="Enables [/users/@me]() to return an `email`")]
        public interface PEmail {}
        // TODO: connections of what. only the dude who added to guild?
        [Info(Module="Discord.OAuth2", Name="can view connections", Description="Allows [/users/@me/connections]() to return linked third-party accounts")]
        public interface PConnections {}
        [Info(Module="Discord.OAuth2", Name="can view guilds of a user", Description="Allows [/users/@me/guilds]() to return basic information about all of a user's guilds")]
        public interface PGuilds {}
        [Info(Module="Discord.OAuth2", Name="can join guilds", Description="Allows [/guilds/{guild.id}/members/{user.id}]() to be used for joining users to a guild")]
        public interface PGuildsJoin {}
        // NOTE: modified from discord's description to match format of the other descriptions 
        // original: wait wot where it go???
        [Info(Module="Discord.OAuth2", Name="can join group DMs", Description="Allows [/channels/{channel.id}/recipients/{user.id}]() to be used for joining users to a group DM")]
        public interface PGDMJoin {}
        [Info(Module="Discord.OAuth2", Name="can use RPC", Description="For local rpc server access, this allows you to control a user's local Discord client - whitelist only")]
        public interface PRPC {}
        [Info(Module="Discord.OAuth2", Name="can access API over RPC", Description="For local rpc server api access, this allows you to access the API as the local user - whitelist only")]
        public interface PRPCAPI {}
        [Info(Module="Discord.OAuth2", Name="can read notifications over RPC", Description="For local rpc server api access, this allows you to receive notifications pushed out to the user - whitelist only")]
        public interface PRPCNotificationsRead {}
        [Info(Module="Discord.OAuth2", Name="can put bot in selected guild", Description="For OAuth2 bots, this puts the bot in the user's selected guild by default")]
        public interface PBot {}
        // TODO: not 100% on the name
        [Info(Module="Discord.OAuth2", Name="can read webhook", Description="This generates a webhook that is returned in the OAuth token response for authorization code grants")]
        public interface PWebhookIncoming {}
        [Info(Module="Discord.OAuth2", Name="can read messages", Description="For local rpc server api access, this allows you to read messages from all client channels (otherwise restricted to channels/guilds your app creates)")]
        public interface PMessagesRead {}
        [Info(Module="Discord.OAuth2", Name="can write user's applications' build data", Description="Allows your app to upload/update builds for a user's applications - whitelist only")]
        public interface PApplicationsBuildsUpload {}
        [Info(Module="Discord.OAuth2", Name="can read user's applications' build data", Description="Allows your app to read build data for a user's applications")]
        public interface PApplicationsBuildsRead {}
        [Info(Module="Discord.OAuth2", Name="can read user's applications' store data", Description="Allows your app to read and update store data (SKUs, store listings, achievements, etc.) for a user's applications")]
        public interface PApplicationsStoreUpdate {}
        [Info(Module="Discord.OAuth2", Name="can read user's applications' entitlements", Description="Allows your app to read entitlements for a user's applications")]
        public interface PApplicationsEntitlements {}
        [Info(Module="Discord.OAuth2", Name="can read user activity", Description="Allows your app to fetch data from a user's \"Now Playing/Recently Played\" list - whitelist only")]
        public interface PActivitiesRead {}
        // NOTE: documentation also says "(NOT REQUIRED FOR GAMESDK ACTIVITIY MANAGER)" however bot has nothing to do with gamesdk (at least for now)
        [Info(Module="Discord.OAuth2", Name="can write user activity", Description="Allows your app to update a user's activity - whitelist only")]
        public interface PActivitiesWrite {}
        [Info(Module="Discord.OAuth2", Name="can read user relationships", Description="Allows your app to know a user's friends and implicit relationships - whitelist only")]
        public interface PRelationshipsRead {}
    }
}
