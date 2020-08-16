using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Metatron.Dissidence.Attributes;
using static Metatron.Dissidence.Formats.Natural;
using Metatron.DissidenceDiscord.DiscordPermissions;
using Metatron.DissidenceDiscord.OAuth2Permissions;
using USize = System.UInt64;

namespace Metatron.DissidenceDiscord {
    public static class Discord {
#region Guild
        // TODO: maybe specific perms for view/modify text, voice, system channels.

        // TODO: figure out what's nullable and what's not
        // TODO: natural language way to use the result
        [Info(Module="Core", Name="File", Description="Pseudo-file object storing name and content")]
        public record File {
            [Info(Name="name", Description="Name")]
            public String Name;
            [Info(Name="content", Description="Binary content")]
            public Stream Content;
        }

        [Info(Module="Discord.Guild", Name="Delete", Description="Delete guild")]
        [NaturalFormat("Delete {0}")]
        public static async Task Delete<T>(T Context, IGuild Guild)
            where T: POwner
        {
            await Guild.DeleteAsync();
        }

        [Info(Module="Discord.Guild", Name="Leave", Description="Leave guild")]
        [NaturalFormat("Leave {0}")]
        public static async Task Leave<T>(T Context, IGuild Guild)
        {
            await Guild.LeaveAsync();
        }
        
        [Info(Module="Discord.Guild", Name="Guild Properties", Description="Guild properties")]
        public record GuildProperties2 {
            [Info(Name="name", Description="Name")]
            public Optional<String> Name;
            [Info(Name="region ID", Description="ID of voice region")]
            public Optional<String> RegionID;
            [Info(Name="verification level", Description="Verification level required for users to join guild")]
            public Optional<VerificationLevel> VerificationLevel;
            [Info(Name="default message notifications", Description="Default notification level for messages")]
            public Optional<DefaultMessageNotifications> DefaultMessageNotifications;
            [Info(Name="AFK timeout", Description="Delay before a user with no voice activity is moved to AFK channel")]
            public Optional<Int32> AfkTimeout;
            [Info(Name="icon", Description="Sidebar icon")]
            public Optional<Image?> Icon;
            [Info(Name="banner", Description="Banner image")]
            public Optional<Image?> Banner;
            [Info(Name="splash", Description="Splash image")] // TODO: whats this
            public Optional<Image?> Splash;
            [Info(Name="AFK channel ID", Description="ID of voice channel for AFK users")]
            public Optional<USize?> AfkChannelID;
            [Info(Name="system channel ID", Description="ID of channel for system messages")]
            public Optional<USize?> SystemChannelID;
            [Info(Name="owner ID", Description="ID of owner of guild")]
            public Optional<USize> OwnerID;
            [Info(Name="explicit content filter level", Description="Explicit content filter level")]
            public Optional<ExplicitContentFilterLevel> ExplicitContentFilter;
            [Info(Name="system channel flags", Description="System channel flags")]
            public Optional<SystemChannelMessageDeny> SystemChannelFlags;
            [Info(Name="preferred locale", Description="Preferred locale")]
            public Optional<String> PreferredLocale;
            [Info(Name="preferred culture", Description="Preferred culture")]
            public Optional<System.Globalization.CultureInfo> PreferredCulture;
        }

        [Info(Module="Discord.Guild", Name="Modify", Description="Modify guild")]
        [NaturalFormat("Modify {0}")]
        public static async Task Modify<T>(T Context, IGuild Guild, GuildProperties2 Properties)
        {
            // https://github.com/discord-net/Discord.Net/blob/2d80037f6b0f25e04e7ceeac9f3e6f04fb2fffa5/src/Discord.Net.Rest/Entities/Guilds/GuildHelper.cs#L28
            await Guild.ModifyAsync(props => {
                if (Properties.RegionID.IsSpecified) {
                    props.RegionId = Properties.RegionID;
                }
                if (Properties.AfkChannelID.IsSpecified) {
                    props.AfkChannelId = Properties.AfkChannelID;
                }
                if (Properties.AfkTimeout.IsSpecified) {
                    props.AfkTimeout = Properties.AfkTimeout;
                }
                if (Properties.SystemChannelID.IsSpecified) {
                    props.SystemChannelId = Properties.SystemChannelID;
                }
                if (Properties.DefaultMessageNotifications.IsSpecified) {
                    props.DefaultMessageNotifications = Properties.DefaultMessageNotifications;
                }
                if (Properties.Icon.IsSpecified) {
                    props.Icon = Properties.Icon;
                }
                if (Properties.Name.IsSpecified) {
                    props.Name = Properties.Name;
                }
                if (Properties.Splash.IsSpecified) {
                    props.Splash = Properties.Splash;
                }
                if (Properties.Banner.IsSpecified) {
                    props.Banner = Properties.Banner;
                }
                if (Properties.VerificationLevel.IsSpecified) {
                    props.VerificationLevel = Properties.VerificationLevel;
                }
                if (Properties.ExplicitContentFilter.IsSpecified) {
                    props.ExplicitContentFilter = Properties.ExplicitContentFilter;
                }
                if (Properties.SystemChannelFlags.IsSpecified) {
                    props.SystemChannelFlags = Properties.SystemChannelFlags;
                }
            });
        }

        // NOTE: ideally no RTTI required; so if you expect a text channel then use appropriate method
        [Info(Module="Discord.Guild", Name="Get Channel", Description="Get channel from guild if it exists")]
        [NaturalFormat("Get channel with ID {1} from {0}")]
        public static async Task<IGuildChannel?> GetChannel<T>(T Context, IGuild Guild, USize ChannelID)
            where T: PViewChannel
        {
            return await Guild.GetChannelAsync(ChannelID);
        }

        [Info(Module="Discord.Guild", Name="Get Channel", Description="Get all channels in guild")]
        [NaturalFormat("Get all channels in {0}")]
        public static async Task<IReadOnlyCollection<IGuildChannel>> GetChannels<T>(T Context, IGuild Guild)
            where T: PViewChannel
        {
            return await Guild.GetChannelsAsync();
        }

        [Info(Module="Discord.Guild", Name="Order Channel Properties", Description="Properties of channel order list item")]
        public record OrderChannelProperties2 {
            [Info(Name="channel ID", Description="ID of the channel this corresponds to")]
            public USize ID;
            [Info(Name="position", Description="0-based position of the channel from the top")]
            public Int32 Position;
        }

        [Info(Module="Discord.Guild", Name="Order Channels", Description="Rearrange channels in guild into the given order")]
        [NaturalFormat("Reorder all channels in {0}")]
        public static async Task OrderChannels<T>(T Context, IGuild Guild, IEnumerable<OrderChannelProperties2> Properties)
            where T: PManageChannels
        {
            await Guild.ReorderChannelsAsync(Properties.Select(prop => new ReorderChannelProperties(prop.ID, prop.Position)));
        }

        [Info(Module="Discord.Guild", Name="Get System Channel", Description="Get sustem channel from guild if it exists")]
        [NaturalFormat("Get system channel with ID {1} from {0}")]
        public static async Task<ITextChannel?> GetSystemChannel<T>(T Context, IGuild Guild)
            where T: PViewChannel // TODO: separate perms for view/edit system, text, voice channels? ugh
        {
            return await Guild.GetSystemChannelAsync();
        }

        [Info(Module="Discord.Guild", Name="Get Text Channel", Description="Get text channel from guild if it exists")]
        [NaturalFormat("Get text channel with ID {1} from {0}")]
        public static async Task<ITextChannel?> GetTextChannel<T>(T Context, IGuild Guild, USize ChannelID)
            where T: PViewChannel
        {
            return await Guild.GetTextChannelAsync(ChannelID);
        }

        [Info(Module="Discord.Guild", Name="Get Text Channels", Description="Get all text channels in guild")]
        [NaturalFormat("Get all text channels in {0}")]
        public static async Task<IReadOnlyCollection<ITextChannel>> GetTextChannels<T>(T Context, IGuild Guild)
            where T: PViewChannel
        {
            return await Guild.GetTextChannelsAsync();
        }

        [Info(Module="Discord.Guild", Name="Get Voice Channel", Description="Get voice channel from guild if it exists")]
        [NaturalFormat("Get voice channel with ID {1} from {0}")]
        public static async Task<IVoiceChannel?> GetVoiceChannel<T>(T Context, IGuild Guild, USize ChannelID)
            where T: PViewChannel
        {
            return await Guild.GetVoiceChannelAsync(ChannelID);
        }

        [Info(Module="Discord.Guild", Name="Get Voice Channels", Description="Get all voice channels in guild")]
        [NaturalFormat("Get all voice channels in {0}")]
        public static async Task<IReadOnlyCollection<IVoiceChannel>> GetVoiceChannels<T>(T Context, IGuild Guild)
            where T: PViewChannel
        {
            return await Guild.GetVoiceChannelsAsync();
        }

        [Info(Module="Discord.Guild", Name="Get Voice Regions", Description="Get all voice regions in guild")]
        [NaturalFormat("Get voice regions accessible by {0}")]
        public static async Task<IReadOnlyCollection<IVoiceRegion>> GetVoiceRegions<T>(T Context, IGuild Guild)
            where T: PViewChannel
        {
            return await Guild.GetVoiceRegionsAsync();
        }

        // TODO
        // TODO
        // TODO: guild stuff almost done. move on to channel and message and vc

        [Info(Module="Discord.Guild", Name="GuildAddUserProperties", Description="Properties for user being added to a guild")]
        public record GuildAddUserProperties2 {
            [Info(Name="nickname", Description="Name shown in chat")]
            public Optional<String> Nickname;
            [Info(Name="is muted", Description="Is muted")]
            public Optional<Boolean> IsMuted;
            [Info(Name="is deafened", Description="Is deafened")]
            public Optional<Boolean> IsDeafened;
            [Info(Name="role IDs", Description="IDs of roles given to this user")]
            public Optional<IEnumerable<USize>> RoleIDs;
        }

        [Info(Module="Discord.Guild", Name="Add User", Description="Add user to guild")]
        [NaturalFormat("Add user with ID {1} to {0} with {2}")]
        public static async Task<IGuildUser> AddUser<T>(T Context, IGuild Guild, USize UserID, GuildAddUserProperties2 Properties)
            where T: PManageUsers, PGuildsJoin
        {
            return await Guild.AddGuildUserAsync(UserID, Metatron.Discord.OAuth2Token, props => {
                if (Properties.Nickname.IsSpecified) {
                    props.Nickname = Properties.Nickname;
                }
                if (Properties.IsDeafened.IsSpecified) {
                    props.Deaf = Properties.IsDeafened;
                }
                if (Properties.IsMuted.IsSpecified) {
                    props.Mute = Properties.IsMuted;
                }
                if (Properties.RoleIDs.IsSpecified) {
                    props.RoleIds = Properties.RoleIDs;
                }
            });
        }

        [Info(Module="Discord.Guild", Name="Add User", Description="Add user to guild")]
        [NaturalFormat("Add user with ID {1} to {0} with properties {2}")]
        public static async Task<IGuildUser> GetUser<T>(T Context, IGuild Guild, USize UserID)
            where T: PViewUsers
        {
            return await Guild.GetUserAsync(UserID);
        }

        [Info(Module="Discord.Guild", Name="Create Emote", Description="Create emote in guild")]
        [NaturalFormat("Add emote {2} to {0} as {1}, available to roles {2}")]
        public static async Task<GuildEmote> CreateEmote<T>(T Context, IGuild Guild, String Name, Image Image, Optional<IEnumerable<IRole>> Roles=default(Optional<IEnumerable<IRole>>))
            where T: PManageEmojis
        {
            return await Guild.CreateEmoteAsync(Name, Image, Roles);
        }

        [Info(Module="Discord.Guild", Name="Delete Emote", Description="Delete emote from guild")]
        [NaturalFormat("Remove emote with ID {1} from {0}")]
        public static async Task DeleteEmote<T>(T Context, IGuild Guild, USize EmoteID)
            where T: PManageEmojis
        {
            await Guild.DeleteEmoteAsync(await GetEmote(Context, Guild, EmoteID));
        }

        [Info(Module="Discord.Guild", Name="Emote Properties", Description="Properties for emote")]
        public record EmoteProperties2 {
            [Info(Name="name", Description="Name")]
            public Optional<String> Name;
            [Info(Name="role IDs", Description="IDs of the roles that can use this emote")]
            public Optional<IEnumerable<USize>> RoleIDs;
        }

        [Info(Module="Discord.Guild", Name="Modify Emote", Description="Modify emote in guild")]
        [NaturalFormat("Modify emote with ID {1} from {0} with {2}")]
        public static async Task<GuildEmote> ModifyEmote<T>(T Context, IGuild Guild, USize EmoteID, EmoteProperties2 Properties)
            where T: PManageEmojis
        {
            return await Guild.ModifyEmoteAsync(await GetEmote(Context, Guild, EmoteID), props => {
                if (Properties.Name.IsSpecified) {
                    props.Name = Properties.Name;
                }
                if (Properties.RoleIDs.IsSpecified) {
                    props.Roles = new Optional<IEnumerable<IRole>>(Properties.RoleIDs.Value.Select(Guild.GetRole));
                }
            });
        }

        [Info(Module="Discord.Guild", Name="Get Emote", Description="Get emote from guild")]
        [NaturalFormat("Get emote {1} from {0}")]
        public static async Task<GuildEmote> GetEmote<T>(T Context, IGuild Guild, USize EmoteID)
            where T: PManageEmojis
        {
            return await Guild.GetEmoteAsync(EmoteID);
        }

        [Info(Module="Discord.Guild", Name="Create Integration", Description="Create integration in guild")]
        [NaturalFormat("Create integration with ID {1} of type {2} to {0}")]
        public static async Task<IGuildIntegration> CreateIntegration<T>(T Context, IGuild Guild, USize IntegrationID, String Type)
            where T: PManageIntegrations
        {
            return await Guild.CreateIntegrationAsync(IntegrationID, Type);
        }

        [Info(Module="Discord.Guild", Name="Get Integrations", Description="Get all integrations in guild")]
        [NaturalFormat("Get all integrations in {0}")]
        public static async Task<IReadOnlyCollection<IGuildIntegration>> GetIntegrations<T>(T Context, IGuild Guild)
            where T: PViewIntegrations
        {
            return await Guild.GetIntegrationsAsync();
        }

        [Info(Module="Discord.Guild", Name="Get Webhook", Description="Get webhook from guild if it exists")]
        [NaturalFormat("Get webhook with ID {1} in {0}")]
        public static async Task<IWebhook> GetWebhook<T>(T Context, IGuild Guild, USize WebhookID)
            where T: PViewWebhooks
        {
            return await Guild.GetWebhookAsync(WebhookID);
        }

        [Info(Module="Discord.Guild", Name="Get Webhooks", Description="Get all webhooks in guild")]
        [NaturalFormat("Get all webhooks in {0}")]
        public static async Task<IReadOnlyCollection<IWebhook>> GetWebhooks<T>(T Context, IGuild Guild)
            where T: PViewWebhooks
        {
            return await Guild.GetWebhooksAsync();
        }

        [Info(Module="Discord.Guild", Name="Add Role", Description="Add role to guild")]
        [NaturalFormat("Add role {2} to {0} as {1}, available to roles {2}")]
        public static async Task<IRole> AddRole<T>(T Context, IGuild Guild, String Name, GuildPermissions? Permissions=default(GuildPermissions?), Color? Color=default(Color?), Boolean DoShowSeparately=false, Boolean CanBeMentioned=false)
            where T: PManageRoles
        {
            return await Guild.CreateRoleAsync(Name, Permissions, Color, DoShowSeparately, CanBeMentioned);
        }

        [Info(Module="Discord.Guild", Name="Get Role", Description="Get role with specified ID from guild")]
        [NaturalFormat("Add role {2} to {0} as {1}, available to roles {2}")]
        public static IRole GetRole<T>(T Context, IGuild Guild, USize RoleID)
            where T: PManageRoles
        {
            return Guild.GetRole(RoleID);
        }

        [Info(Module="Discord.Guild", Name="Order Role Properties", Description="Properties of role order list item")]
        public record OrderRoleProperties2 {
            [Info(Name="role ID", Description="ID of the role this corresponds to")]
            public USize ID;
            [Info(Name="position", Description="0-based position of the role from the top")]
            public Int32 Position;
        }

        [Info(Module="Discord.Guild", Name="Order Channels", Description="Rearrange channels in guild into the given order")]
        [NaturalFormat("Reorder all channels in {0}")]
        public static async Task OrderRoles<T>(T Context, IGuild Guild, IEnumerable<OrderRoleProperties2> Properties)
            where T: PManageRoles
        {
            await Guild.ReorderRolesAsync(Properties.Select(prop => new ReorderRoleProperties(prop.ID, prop.Position)));
        }
#region Moderation
        [Info(Module="Discord.Guild", Name="Kick", Description="Kick user from guild")]
        [NaturalFormat("Kick user with ID {1} from {0} with reason {2}")]
        public static async Task Kick<T>(T Context, IGuild Guild, USize UserID, String? Reason=null)
            where T: PKickMembers
        {
            var user = await Guild.GetUserAsync(UserID);
            await user.KickAsync(Reason);
        }

        [Info(Module="Discord.Guild", Name="Prune", Description="Prune users based on inactivity")]
        [NaturalFormat("Prune users from {0} with no role that have been inactive for the past {1} days")]
        public static async Task<Int32> Prune<T>(T Context, IGuild Guild, Int32 Days=30, Boolean Simulate=false)
            where T: PPruneMembers // TODO: rename to pmanageprunes?
        {
            return await Guild.PruneUsersAsync(Days, Simulate);
        }

        [Info(Module="Discord.Guild", Name="Create Ban", Description="Ban user")]
        [NaturalFormat("Ban user with ID {1} from {0} with reason {3}, pruning last {2} days of messages")]
        public static async Task CreateBan<T>(T Context, IGuild Guild, USize UserID, Int32 PruneDays=0, String? Reason=null)
            where T: PManageBans
        {
            await Guild.AddBanAsync(UserID, PruneDays, Reason);
        }

        [Info(Module="Discord.Guild", Name="Delete Ban", Description="Unban user")]
        [NaturalFormat("Unban user with ID {1} from {0}")]
        public static async Task DeleteBan<T>(T Context, IGuild Guild, USize UserID)
            where T: PManageBans
        {
            await Guild.RemoveBanAsync(UserID);
        }

        [Info(Module="Discord.Guild", Name="Read Audit Log", Description="Read audit log")]
        [NaturalFormat("Read the most recent {1} pages from audit log of {0} with {2}")]
        public static async Task<IReadOnlyCollection<IAuditLogEntry>> ReadAuditLog<T>(T Context, IGuild Guild, Int32 Limit=32)
            where T: PViewAuditLog
        {
            return await Guild.GetAuditLogsAsync(Limit);
        }

        [Info(Module="Discord.Guild", Name="Get Ban", Description="Get ban")]
        [NaturalFormat("Get ban for user {2} in {0}")]
        public static async Task<IBan> GetBan<T>(T Context, IGuild Guild, USize UserID)
            where T: PViewBans
        {
            return await Guild.GetBanAsync(UserID);
        }

        [Info(Module="Discord.Guild", Name="Get Bans", Description="Get all bans")]
        [NaturalFormat("Get all bans in {0}")]
        public static async Task<IReadOnlyCollection<IBan>> GetBans<T>(T Context, IGuild Guild)
            where T: PViewBans
        {
            return await Guild.GetBansAsync();
        }
#endregion Moderation

#region Channels

        [Info(Module="Discord.GuildChannel", Name="", Description="")]
        public record GuildChannelProperties2 {
            [Info(Name="name", Description="Name")]
            public Optional<string> Name;
            [Info(Name="position", Description="Position from top")]
            public Optional<int> Position;
            [Info(Name="category ID", Description="ID of parent category")]
            public Optional<ulong?> CategoryID;
            [Info(Name="permission overwrites", Description="Permissions overridden from the category defaults")]
            public Optional<IEnumerable<Overwrite>> PermissionOverwrites;
        }

        [Info(Module="Discord.GuildTextChannel", Name="Text Channel Properties", Description="Text channel properties")]
        public record TextChannelProperties2 : GuildChannelProperties2 {
            [Info(Name="topic", Description="Topic")]
            public Optional<String> Topic;
            [Info(Name="is NSFW", Description="Is not safe for work")]
            public Optional<Boolean> IsNSFW;
            [Info(Name="slow mode interval", Description="Interval between messages if slow mode is enabled")]
            public Optional<Int32> SlowModeInterval;
        }

        [Info(Module="Discord.GuildVoiceChannel", Name="", Description="")]
        public record VoiceChannelProperties2 : GuildChannelProperties2 {
            [Info(Name="bitrate", Description="Audio bits per second")]
            public Optional<Int32> Bitrate;
            [Info(Name="maximum users", Description="Maximum number of users in channel")]
            public Optional<Int32?> MaxUsers;
        }

        [Info(Module="Discord.Guild", Name="Add Text Channel", Description="Add text channel")]
        [NaturalFormat("Add new text channel in {0} with name {1} and properties {2}")]
        public static async Task<ITextChannel> AddTextChannel<T>(T Context, IGuild Guild, String Name, TextChannelProperties2 Properties)
            where T: PViewAuditLog
        {
            return await Guild.CreateTextChannelAsync(Name, props => {
                if (Properties.CategoryID.IsSpecified) {
                    props.CategoryId = Properties.CategoryID;
                }
                if (Properties.Topic.IsSpecified) {
                    props.Topic = Properties.Topic;
                }
                if (Properties.IsNSFW.IsSpecified) {
                    props.IsNsfw = Properties.IsNSFW;
                }
                if (Properties.Position.IsSpecified) {
                    props.Position = Properties.Position;
                }
            });
        }

        [Info(Module="Discord.Guild", Name="Add Voice Channel", Description="Add voice channel")]
        [NaturalFormat("Add new voice channel in {0} with name {1} and properties {2}")]
        public static async Task<IVoiceChannel> AddVoiceChannel<T>(T Context, IGuild Guild, String Name, VoiceChannelProperties2 Properties)
            where T: PViewAuditLog
        {
            return await Guild.CreateVoiceChannelAsync(Name, props => {
                if (Properties.CategoryID.IsSpecified) {
                    props.CategoryId = Properties.CategoryID;
                }
                if (Properties.Bitrate.IsSpecified) {
                    props.Bitrate = Properties.Bitrate;
                }
                if (Properties.MaxUsers.IsSpecified) {
                    props.UserLimit = Properties.MaxUsers;
                }
                if (Properties.Position.IsSpecified) {
                    props.Position = Properties.Position;
                }
            });
        }

        [Info(Module="Discord.Guild", Name="Add Category", Description="Add category")]
        [NaturalFormat("Add new category in {0} with name {1} and properties {2}")]
        public static async Task<ICategoryChannel> CreateCategory<T>(T Context, IGuild Guild, String Name, GuildChannelProperties Properties)
            where T: PViewAuditLog
        {
            return await Guild.CreateCategoryAsync(Name, props => {
                if (Properties.Position.IsSpecified) {
                    props.Position = Properties.Position;
                }
            });
        }

        // TODO: this has no counterpart create since invites are specific to channels
        [Info(Module="Discord.Guild", Name="Get Invites", Description="Get invites")]
        [NaturalFormat("Get invites in {0}")]
        public static async Task<IReadOnlyCollection<IInviteMetadata>> GetInvites<T>(T Context, IGuild Guild)
            where T: PViewInvites
        {
            return await Guild.GetInvitesAsync();
        }

        // TODO: this has no counterpart create since invites are specific to channels
        [Info(Module="Discord.Guild", Name="Get Vanity Invite", Description="Get vanity invite if it exists")]
        [NaturalFormat("Get invites in {0}")]
        public static async Task<IInviteMetadata?> GetVanityInvite<T>(T Context, IGuild Guild)
            where T: PViewInvites
        {
            return await Guild.GetVanityInviteAsync();
        }

        [Info(Module="Discord.Guild", Name="Widget Properties", Description="Widget properties")]
        public record GuildWidgetProperties2 {
            [Info(Name="channel ID", Description="ID of the channel the widget is linked to")]
            public Optional<USize?> ChannelID;
            [Info(Name="enabled", Description="whether the widged is enabled")]
            public Optional<Boolean> Enabled;
        }

        [Info(Module="Discord.Guild", Name="Modify Widget", Description="Modify guild's HTML widget")]
        [NaturalFormat("Send TTS message {1} to {0} with {2} and {3}")]
        public static async Task ModifyWidget<T>(T Context, IGuild Guild, GuildWidgetProperties2 Properties)
            where T: PManageWidget
        {
            await Guild.ModifyEmbedAsync(props => {
                if (Properties.ChannelID.IsSpecified) {
                    props.ChannelId = Properties.ChannelID;
                }
                if (Properties.Enabled.IsSpecified) {
                    props.Enabled = Properties.Enabled;
                }
            });
        }
#endregion Channels
        // TODO:
        // manage channels
        // manage guild
#endregion Guild

#region GuildTextChannel
        [Info(Module="Discord.TextChannel", Name="Create Invite", Description="Create invite")]
        // TODO: correct formatting. esp. null handling
        [NaturalFormat("Create invite for {0} with maximum age {1}")]
        public static async Task<IInviteMetadata> CreateInvite<T>(T Context, ITextChannel Channel, Int32? MaximumAge, Int32? MaximumUses=default(Int32?), Boolean IsTemporary=false, Boolean IsUnique=false)
            where T: PManageInvites
        {
            return await Channel.CreateInviteAsync(MaximumAge, MaximumUses, IsTemporary, IsUnique);
        }

#region SendMessage overloads
        // TODO: allowed_mentions
        [Info(Module="Discord.TextChannel", Name="Send Message", Description="Send message")]
        [NaturalFormat("Send message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
        // TODO: GetGenericParameterConstraints for metadata (permissions needed.)
            where T: PSendMessages
        {
            return await Channel.SendMessageAsync(Content, false, Embed);
        }

        [Info(Module="Discord.TextChannel", Name="Send TTS Message", Description="Send TTS message")]
        [NaturalFormat("Send TTS message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendTTSMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages
        {
            return await Channel.SendMessageAsync(Content, true, Embed);
        }

        [Info(Module="Discord.TextChannel", Name="Send Message With File", Description="Send message with file")]
        [NaturalFormat("Send message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Content, File.Name, Content, false, Embed);
        }

        [Info(Module="Discord.TextChannel", Name="Send TTS Message With File", Description="Send TTS message with file")]
        [NaturalFormat("Send TTS message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendTTSMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Content, File.Name, Content, true, Embed);
        }
#endregion SendMessage overloads

        [Info(Module="Discord.TextChannel", Name="Delete Message", Description="Delete message")]
        [NaturalFormat("Delete message with ID {1} from {0}")]
        public static async Task DeleteMessage<T>(T Context, ITextChannel Channel, USize MessageID)
            where T : PManageMessages
        {
            await Channel.DeleteMessageAsync(MessageID);
        }
#endregion GuildTextChannel

#region Reactions
        [Info(Module="Discord.UserMessage", Name="Get Reaction", Description="Get users who reacted with a specific emoji")]
        [NaturalFormat("Add {1} to {0}")]
        public static IReadOnlyDictionary<IEmote, ReactionMetadata> GetReaction<T>(T Context, IUserMessage Message)
            where T : PViewReactions
        {
            return Message.Reactions;
        }

        // [Info(Module="Discord.UserMessage", Name="Get Reactions", Description="Get users who reacted with a specific emoji")]
        // [NaturalFormat("Add {1} to {0}")]
        // public static async Task<IReadOnlyDictionary<IEmote, ReactionMetadata>> GetReactions<T>(T Context, IUserMessage Message, IEmote Emote, Int32 Limit=Int32.MaxValue)
        //     where T : PViewReactions
        // {
        //     return Message.GetReactionUsersAsync(Emote, Limit);
        // }

        // TODO: string -> emote
        [Info(Module="Discord.UserMessage", Name="Add Reaction", Description="Add reaction")]
        [NaturalFormat("Add {1} to {0}")]
        public static async Task AddReaction<T>(T Context, IUserMessage Message, IEmote Emote)
            where T : PSendReactions
        {
            await Message.AddReactionAsync(Emote);
        }

        [Info(Module="Discord.UserMessage", Name="Remove Reaction", Description="Remove reaction")]
        [NaturalFormat("Remove {1} from {0}")]
        public static async Task RemoveReaction<T>(T Context, IUserMessage Message, IEmote Emote)
            where T : PSendReactions
        {
            await Message.RemoveReactionAsync(Emote, Metatron.Discord.Client.CurrentUser);
        }

        // TODO: what permission is this/should this be?
        [Info(Module="Discord.UserMessage", Name="Remove Reaction From User", Description="Remove reaction by a certain user")]
        [NaturalFormat("Remove {1} of {2} from {0}")]
        public static async Task RemoveReactionFromUser<T>(T Context, IUserMessage Message, IEmote Emote, USize UserID)
            where T : PManageReactions
        {
            var user = await Message.Channel.GetUserAsync(UserID);
            await Message.RemoveReactionAsync(Emote, user);
        }

        [Info(Module="Discord.UserMessage", Name="Remove Reaction From User", Description="Remove reaction by a certain user")]
        [NaturalFormat("Remove all reactions from {0}")]
        public static async Task ClearReactions<T>(T Context, IUserMessage Message)
            where T : PClearReactions
        {
            await Message.RemoveAllReactionsAsync();
        }
#endregion Reactions
    }
}
