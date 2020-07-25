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
    public static class Guild {
        public record File { public String Name; public Stream Content; }

        [Info(Module="Discord.Guild", Name="Send Message", Description="Send message")]
        [NaturalFormat("Get channel with ID {1} from {0}")]
        public static async Task<IGuildChannel> GetChannel<T>(T Context, IGuild Guild, USize ChannelID)
            where T: PViewChannel
        {
            return await Guild.GetChannelAsync(ChannelID, CacheMode.AllowDownload, null);
        }

#region SendMessage overloads
        [Info(Module="Discord.Guild", Name="Send Message", Description="Send message")]
        [NaturalFormat("Send message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
        // TODO: GetGenericParameterConstraints 
        // TODO: does *this* need readmessages.
            where T: PSendMessages
        {
            return await Channel.SendMessageAsync(Content, false, Embed);
        }

        [Info(Module="Discord.Guild", Name="Send TTS Message", Description="Send TTS message")]
        [NaturalFormat("Send TTS message {1} to {0} with {2}")]
        public static async Task<IUserMessage> SendTTSMessage<T>(T Context, ITextChannel Channel, String Content, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages
        {
            return await Channel.SendMessageAsync(Content, true, Embed);
        }

        [Info(Module="Discord.Guild", Name="Send TTS Message With File", Description="Send message")]
        [NaturalFormat("Send message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Name, Content, false, Embed);
        }

        [Info(Module="Discord.Guild", Name="Send TTS Message With File", Description="Send message")]
        [NaturalFormat("Send message {1} to {0} with {2} and {3}")]
        public static async Task<IUserMessage> SendTTSMessageWithFile<T>(T Context, ITextChannel Channel, String Content, File File, Embed? Embed=null)
            where T: PSendMessages, PSendTTSMessages, PAttachFiles
        {
            return await Channel.SendFileAsync(File.Name, Content, true, Embed);
        }
#endregion SendMessage overloads

        [Info(Module="Discord.Guild", Name="Send TTS Message With File", Description="Send message")]
        [NaturalFormat("Delete message with ID {1} from {0}")]
        public static async Task DeleteMessage<T>(T Context, ITextChannel Channel, USize MessageID)
            where T : PManageMessages // TODO: does this need say, readmessages
        {
            await Channel.DeleteMessageAsync(MessageID);
        }
    }
}
