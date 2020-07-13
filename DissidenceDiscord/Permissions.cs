using System;
using System.Collections.Generic;
using Metatron.Dissidence;
using static Metatron.Dissidence.Prelude;

namespace Metatron.DissidenceDiscord {
    public static class Permissions {
        // TODO: better name
        public static PermissionsMetadata DiscordPermissions = new PermissionsMetadata {
            Name = "Discord Permissions",
            Description = "A way to limit and grant certain abilities to users",
            Value = new List<PermissionMetadata> {
                new PermissionMetadata { Name = "create instant invite", Description = "Allows creation of instant invites" },
                new PermissionMetadata { Name = "kick members", Description = "Allows kicking members" },
                new PermissionMetadata { Name = "ban members", Description = "Allows banning members" },
                new PermissionMetadata { Name = "administrator", Description = "Allows all permissions and bypasses channel permission overwrites" },
                new PermissionMetadata { Name = "manage channels", Description = "Allows management and editing of channels" },
                new PermissionMetadata { Name = "manage guild", Description = "Allows management and editing of the guild" },
                new PermissionMetadata { Name = "add reactions", Description = "Allows for the addition of reactions to messages" },
                new PermissionMetadata { Name = "view audit log", Description = "Allows for viewing of audit logs" },
                new PermissionMetadata { Name = "allow priority speaker", Description = "Allows for using priority speaker in a voice channel" },
                new PermissionMetadata { Name = "stream", Description = "Allows the user to go live" },
                new PermissionMetadata { Name = "view channel", Description = "Allows guild members to view a channel, which includes reading messages in text channels" },
                new PermissionMetadata { Name = "send messages", Description = "Allows for sending messages in a channel" },
                new PermissionMetadata { Name = "send text-to-speech messages", Description = "Allows for sending of `/tts` messages" },
                new PermissionMetadata { Name = "manage messages", Description = "Allows for deletion of other users messages" },
                new PermissionMetadata { Name = "automatically create embeds for links", Description = "Links sent by users with this permission will be auto-embedded" },
                new PermissionMetadata { Name = "send files", Description = "Allows for uploading images and files" },
                new PermissionMetadata { Name = "read messages", Description = "Allows for reading of message history" },
                new PermissionMetadata { Name = "mention @everyone and @here", Description = "Allows for using the `@everyone` tag to notify all users in a channel, and the `@here` tag to notify all online users in a channel" },
                new PermissionMetadata { Name = "use external emojis", Description = "Allows the usage of custom emojis from other servers" },
                new PermissionMetadata { Name = "view guild insights", Description = "Allows for viewing guild insights" },
                new PermissionMetadata { Name = "connect to voice", Description = "Allows for joining of a voice channel" },
                new PermissionMetadata { Name = "speak", Description = "Allows for speaking in a voice channel" },
                new PermissionMetadata { Name = "mute members", Description = "Allows for muting members in a voice channel" },
                new PermissionMetadata { Name = "deafen members", Description = "Allows for deafening of members in a voice channel" },
                new PermissionMetadata { Name = "move members between voice channels", Description = "Allows for moving of members between voice channels" },
                new PermissionMetadata { Name = "use voice activity detection", Description = "Allows for using voice-activity-detection in a voice channel" },
                new PermissionMetadata { Name = "change nickname", Description = "Allows for modification of own nickname" },
                // NOTE: discord docs have a type and leave the apostrophe out
                new PermissionMetadata { Name = "manage nicknames", Description = "Allows for modification of other users' nicknames" },
                new PermissionMetadata { Name = "manage roles", Description = "Allows management and editing of roles" },
                new PermissionMetadata { Name = "manage webhooks", Description = "Allows management and editing of webhooks" },
                new PermissionMetadata { Name = "manage emojis", Description = "Allows management and editing of emojis" },
            },
        };
    
        public static PermissionsMetadata Oauth2Scopes = new PermissionsMetadata {
            Name = "OAuth2 Scopes",
            Description = "OAuth2 scopes that Discord supports. Scopes that are behind a whitelist cannot be requested unless your application is on said whitelist, and may cause undocumented/error behavior in the OAuth2 flow if you request them from a user.",
            Value = new List<PermissionMetadata> {
                new PermissionMetadata { Name = "view requester without email", Description = "Allows [/users/@me]() without `email`" },
                new PermissionMetadata { Name = "view email of requester", Description = "Enables [/users/@me]() to return an `email`" },
                new PermissionMetadata { Name = "view connections", Description = "Allows [/users/@me/connections]() to return linked third-party accounts" },
                new PermissionMetadata { Name = "view guilds of a user", Description = "Allows [/users/@me/guilds]() to return basic information about all of a user's guilds" },
                new PermissionMetadata { Name = "join guilds", Description = "Allows [/guilds/{guild.id}/members/{user.id}]() to be used for joining users to a guild" },
                // NOTE: modified from discord's description to match format of the other descriptions 
                // original: 
                new PermissionMetadata { Name = "join group DMs", Description = "Allows [/channels/{channel.id}/recipients/{user.id}]() to be used for joining users to a group DM" },
                new PermissionMetadata { Name = "RPC", Description = "For local rpc server access, this allows you to control a user's local Discord client - whitelist only" },
                new PermissionMetadata { Name = "API access over RPC", Description = "For local rpc server api access, this allows you to access the API as the local user - whitelist only" },
                new PermissionMetadata { Name = "read notifications over RPC", Description = "For local rpc server api access, this allows you to receive notifications pushed out to the user - whitelist only" },
                new PermissionMetadata { Name = "put bot in selected guild", Description = "For OAuth2 bots, this puts the bot in the user's selected guild by default" },
                // TODO: not 100% on the name
                new PermissionMetadata { Name = "read webhook", Description = "This generates a webhook that is returned in the OAuth token response for authorization code grants" },
                new PermissionMetadata { Name = "read messages", Description = "For local rpc server api access, this allows you to read messages from all client channels (otherwise restricted to channels/guilds your app creates)" },
                new PermissionMetadata { Name = "write user's applications' build data", Description = "Allows your app to upload/update builds for a user's applications - whitelist only" },
                new PermissionMetadata { Name = "read user's applications' build data", Description = "Allows your app to read build data for a user's applications" },
                new PermissionMetadata { Name = "read user's applications' store data", Description = "Allows your app to read and update store data (SKUs, store listings, achievements, etc.) for a user's applications" },
                new PermissionMetadata { Name = "read user's applications' entitlements", Description = "Allows your app to read entitlements for a user's applications" },
                new PermissionMetadata { Name = "read user activity", Description = "Allows your app to fetch data from a user's \"Now Playing/Recently Played\" list - whitelist only" },
                // NOTE: documentation also says "(NOT REQUIRED FOR GAMESDK ACTIVITIY MANAGER)" however bot has nothing to do with gamesdk (at least for now)
                new PermissionMetadata { Name = "write user activity", Description = "Allows your app to update a user's activity - whitelist only" },
                new PermissionMetadata { Name = "read user relationships", Description = "Allows your app to know a user's friends and implicit relationships - whitelist only" },
            },
        };
    }
}
