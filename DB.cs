using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Metatron.Dissidence;

namespace Metatron.DB {
    public class Context : DbContext {
        public DbSet<Function> Functions;
        public DbSet<Module> Modules;
        public DbSet<UserSession> UserSessions;
        public DbSet<ChannelSession> ChannelSessions;
        public DbSet<GuildSession> GuildSessions;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("HOST")};Database={Environment.GetEnvironmentVariable("DBNAME")};Username={Environment.GetEnvironmentVariable("DBUSER")};Password={Environment.GetEnvironmentVariable("DBPASS")}");
    }

    // TODO: rename body
    public record Function { public UInt64 Id; public UInt64 ModuleId; public String Name; public String[] Arguments; public Boolean IsNative; }
    public record Module { public UInt64 Id; public List<Function> Functions; }
    // primary key should be channel + user + userSpecific.
    public record Session { public UInt64 GuildId; public UInt64 Module; public Byte[] Data; }
    public record UserSession : Session { public UInt64[] UserIds; public UInt64[] RoleIds; }
    public record ChannelSession : Session { public UInt64[] ChannelIds; public UInt64[] CategoryIds; }
    public record GuildSession : Session {}
}
