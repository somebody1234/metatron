using System;
using Metatron.Dissidence;

namespace Metatron.DissidenceDiscord {
    [DissidenceRecordInfo(Name="Discord Permissions", Description="A way to limit and grant certain abilities to users")]
    public record Permissions {
        [DissidenceMemberInfo(Description="Allows creation of instant invites")]
        public Boolean CreateInstantInvite;
        [DissidenceMemberInfo(Description="Allows kicking members")]
        public Boolean KickMembers;
        [DissidenceMemberInfo(Description="Allows banning members")]
        public Boolean BanMembers;
        [DissidenceMemberInfo(Description="Allows all permissions and bypasses channel permission overwrites")]
        public Boolean Administrator;
        [DissidenceMemberInfo(Description="Allows management and editing of channels")]
        public Boolean ManageChannels;
        [DissidenceMemberInfo(Description="Allows management and editing of the guild")]
        public Boolean ManageGuild;
        [DissidenceMemberInfo(Description="Allows for the addition of reactions to messages")]
        public Boolean AddReactions;
        [DissidenceMemberInfo(Description="Allows for viewing of audit logs")]
        public Boolean ViewAuditLog;
        [DissidenceMemberInfo(Description="Allows for using priority speaker in a voice channel")]
        public Boolean PrioritySpeaker;
        [DissidenceMemberInfo(Description="Allows the user to go live")]
        public Boolean Stream;
        [DissidenceMemberInfo(Description="Allows guild members to view a channel, which includes reading messages in text channels")]
        public Boolean ViewChannel;
        [DissidenceMemberInfo(Description="Allows for sending messages in a channel")]
        public Boolean SendMessages;
        [DissidenceMemberInfo(Description="Allows for sending of `/tts` messages")]
        public Boolean SendTtsMessages;
        [DissidenceMemberInfo(Description="Allows for deletion of other users messages")]
        public Boolean ManageMessages;
        [DissidenceMemberInfo(Description="Links sent by users with this permission will be auto-embedded")]
        public Boolean EmbedLinks;
        [DissidenceMemberInfo(Description="Allows for uploading images and files")]
        public Boolean AttachFiles;
        [DissidenceMemberInfo(Description="Allows for reading of message history")]
        public Boolean ReadMessageHistory;
        [DissidenceMemberInfo(Description="Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel")]
        public Boolean MentionEveryone;
        [DissidenceMemberInfo(Description="Allows the usage of custom emojis from other servers")]
        public Boolean UseExternalEmojis;
        [DissidenceMemberInfo(Description="Allows for viewing guild insights")]
        public Boolean ViewGuildInsights;
        [DissidenceMemberInfo(Description="Allows for joining of a voice channel")]
        public Boolean Connect;
        [DissidenceMemberInfo(Description="Allows for speaking in a voice channel")]
        public Boolean Speak;
        [DissidenceMemberInfo(Description="Allows for muting members in a voice channel")]
        public Boolean MuteMembers;
        [DissidenceMemberInfo(Description="Allows for deafening of members in a voice channel")]
        public Boolean DeafenMembers;
        [DissidenceMemberInfo(Description="Allows for moving of members between voice channels")]
        public Boolean MoveMembers;
        [DissidenceMemberInfo(Description="Allows for using voice-activity-detection in a voice channel")]
        public Boolean UseVad;
        [DissidenceMemberInfo(Description="Allows for modification of own nickname")]
        public Boolean ChangeNickname;
        // NOTE: discord docs have a type and leave the apostrophe out
        [DissidenceMemberInfo(Description="Allows for modification of other users' nicknames")]
        public Boolean ManageNicknames;
        [DissidenceMemberInfo(Description="Allows management and editing of roles")]
        public Boolean ManageRoles;
        [DissidenceMemberInfo(Description="Allows management and editing of webhooks")]
        public Boolean ManageWebhooks;
        [DissidenceMemberInfo(Description="Allows management and editing of emojis")]
        public Boolean ManageEmojis;
    }

    [DissidenceRecordInfo(Name="OAuth2 Scopes", Description="OAuth2 scopes that Discord supports. Scopes that are behind a whitelist cannot be requested unless your application is on said whitelist, and may cause undocumented/error behavior in the OAuth2 flow if you request them from a user.")]
    public record Oauth2Scopes {
        [DissidenceMemberInfo(Name="view requester without email", Description="Allows [/users/@me]() without `email`")]
        public Boolean Identify;
        [DissidenceMemberInfo(Name="view email of requester", Description="Enables [/users/@me]() to return an `email`")]
        public Boolean Email;
        [DissidenceMemberInfo(Description="Allows [/users/@me/connections]() to return linked third-party accounts")]
        public Boolean Connections;
        [DissidenceMemberInfo(Name="view guilds of a user", Description="Allows [/users/@me/guilds]() to return basic information about all of a user's guilds")]
        public Boolean Guilds;
        [DissidenceMemberInfo(Name="join guilds", Description="Allows [/guilds/{guild.id}/members/{user.id}]() to be used for joining users to a guild")]
        public Boolean GuildsJoin;
        // NOTE: modified from discord's description to match format of the other descriptions 
        // original: 
        [DissidenceMemberInfo(Name="join group DMs", Description="Allows [/channels/{channel.id}/recipients/{user.id}]() to be used for joining users to a group DM")]
        public Boolean GdmJoin;
        [DissidenceMemberInfo(Description="For local rpc server access, this allows you to control a user's local Discord client - whitelist only")]
        public Boolean Rpc;
        [DissidenceMemberInfo(Description="For local rpc server api access, this allows you to access the API as the local user - whitelist only")]
        public Boolean RpcApi;
        [DissidenceMemberInfo(Description="For local rpc server api access, this allows you to receive notifications pushed out to the user - whitelist only")]
        public Boolean RpcNotificationsRead;
        [DissidenceMemberInfo(Description="For OAuth2 bots, this puts the bot in the user's selected guild by default")]
        public Boolean Bot;
        [DissidenceMemberInfo(Description="This generates a webhook that is returned in the OAuth token response for authorization code grants")]
        public Boolean WebhookIncoming;
        [DissidenceMemberInfo(Name="read messages", Description="For local rpc server api access, this allows you to read messages from all client channels (otherwise restricted to channels/guilds your app creates)")]
        public Boolean MessagesRead;
        [DissidenceMemberInfo(Description="Allows your app to upload/update builds for a user's applications - whitelist only")]
        public Boolean ApplicationsBuildsUpload;
        [DissidenceMemberInfo(Description="Allows your app to read build data for a user's applications")]
        public Boolean ApplicationsBuildsRead;
        [DissidenceMemberInfo(Description="Allows your app to read and update store data (SKUs, store listings, achievements, etc.) for a user's applications")]
        public Boolean ApplicationsStoreUpdate;
        [DissidenceMemberInfo(Description="Allows your app to read entitlements for a user's applications")]
        public Boolean ApplicationsEntitlements;
        [DissidenceMemberInfo(Description="Allows your app to fetch data from a user's \"Now Playing/Recently Played\" list - whitelist only")]
        public Boolean ActivitiesRead;
        // NOTE: documentation also says "(NOT REQUIRED FOR GAMESDK ACTIVITIY MANAGER)" however bot has nothing to do with gamesdk (at least for now)
        [DissidenceMemberInfo(Description="Allows your app to update a user's activity - whitelist only")]
        public Boolean ActivitiesWrite;
        [DissidenceMemberInfo(Description="Allows your app to know a user's friends and implicit relationships - whitelist only")]
        public Boolean RelationshipsRead;
    }
}
