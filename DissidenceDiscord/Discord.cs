using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Discord;
using Metatron.Dissidence.Attributes;
using static Metatron.Dissidence.Formats.Natural;
using Metatron.DissidenceDiscord.DiscordPermissions;
using USize = System.UInt64;

namespace Metatron.DissidenceDiscord {
    public static class Discord {
#region Guild
        // TODO: natural language way to use the result
        public record File { public String Name; public Stream Content; }

        [Info(Module="Discord.Guild", Name="Send Message", Description="Send message")]
        [NaturalFormat("Get channel with ID {1} from {0}")]
        public static async Task<IGuildChannel> GetChannel<T>(T Context, IGuild Guild, USize ChannelID)
            where T: PViewChannel
        {
            return await Guild.GetChannelAsync(ChannelID, CacheMode.AllowDownload, null);
        }

        [Info(Module="Discord.Guild", Name="Kick", Description="Kick user")]
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
            where T: PPruneMembers
        {
            return await Guild.PruneUsersAsync(Days, Simulate);
        }

        [Info(Module="Discord.Guild", Name="Ban", Description="Ban user")]
        [NaturalFormat("Ban user with ID {1} from {0} with reason {3}, pruning last {2} days of messages")]
        public static async Task Ban<T>(T Context, IGuild Guild, USize UserID, Int32 PruneDays=0, String? Reason=null)
            where T: PBanMembers
        {
            await Guild.AddBanAsync(UserID, PruneDays, Reason);
        }

        // TODO:
        // manage channels
        // manage guild
        // finish reaction functions
#endregion Guild

#region GuildTextChannel
        [Info(Module="Discord.GuildTextChannel", Name="Create Invite", Description="Create invite")]
        // TODO: correct formatting. esp. null handling
        [NaturalFormat("Create invite for {0} with maximum age {1}")]
        public static async Task<IInviteMetadata> CreateInvite<T>(T Context, ITextChannel Channel, Int32? MaximumAge, Int32? MaximumUses=default(Int32?), Boolean IsTemporary=false, Boolean IsUnique=false)
            where T: PCreateInstantInvite
        {
            return await Channel.CreateInviteAsync(MaximumAge, MaximumUses, IsTemporary, IsUnique);
        }

#region SendMessage overloads
        // TODO: allowed_mentions
        [Info(Module="Discord.GuildTextChannel", Name="Send Message", Description="Send message")]
        [NaturalFormat("Send message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
        // TODO: GetGenericParameterConstraints for metadata (permissions needed.)
            where T: PSendMessages
        {
            return await Channel.SendMessageAsync(Content, false, Embed);
        }

        [Info(Module="Discord.GuildTextChannel", Name="Send TTS Message", Description="Send TTS message")]
        [NaturalFormat("Send TTS message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendTTSMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages
        {
            return await Channel.SendMessageAsync(Content, true, Embed);
        }

        [Info(Module="Discord.GuildTextChannel", Name="Send Message With File", Description="Send message with file")]
        [NaturalFormat("Send message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Name, Content, false, Embed);
        }

        [Info(Module="Discord.GuildTextChannel", Name="Send TTS Message With File", Description="Send TTS message with file")]
        [NaturalFormat("Send TTS message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendTTSMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Name, Content, true, Embed);
        }
#endregion SendMessage overloads

        [Info(Module="Discord.GuildTextChannel", Name="Delete Message", Description="Delete message")]
        [NaturalFormat("Delete message with ID {1} from {0}")]
        public static async Task DeleteMessage<T>(T Context, ITextChannel Channel, USize MessageID)
            where T : PManageMessages
        {
            await Channel.DeleteMessageAsync(MessageID);
        }
#endregion GuildTextChannel

        // TODO: string -> emote
        [Info(Module="Discord.UserMessage", Name="Add Reaction", Description="Add reaction")]
        [NaturalFormat("Add {1} to {0}")]
        public static async Task AddReaction<T>(T Context, IUserMessage Message, IEmote Emote)
            where T : PManageReactions
        {
            await Message.AddReactionAsync(Emote);
        }

        [Info(Module="Discord.UserMessage", Name="Remove Reaction", Description="Remove reaction")]
        [NaturalFormat("Remove {1} from {0}")]
        public static async Task RemoveReaction<T>(T Context, IUserMessage Message, IEmote Emote)
            where T : PManageReactions
        {
            await Message.RemoveReactionAsync(Emote, Metatron.Discord.Client.CurrentUser);
        }

        // TODO: what permission is this/should this be?
        [Info(Module="Discord.UserMessage", Name="Remove Reaction From User", Description="Remove reaction by a certain user")]
        [NaturalFormat("Remove {1} from {0}")]
        public static async Task RemoveReactionFromUser<T>(T Context, IUserMessage Message, IEmote Emote, IUser User)
            where T : PManageReactions
        {
            await Message.RemoveReactionAsync(Emote, User);
        }

        // TODO: what permission is this/should this be?
        // TODO: clear reactions
    }
}
