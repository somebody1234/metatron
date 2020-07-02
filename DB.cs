using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Metatron.DB {
    public class Context : DbContext {
        public DbSet<Function> Functions;
        public DbSet<Module> Modules;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql($"Host={Environment.GetEnvironmentVariable("HOST")};Database={Environment.GetEnvironmentVariable("DBNAME")};Username={Environment.GetEnvironmentVariable("DBUSER")};Password={Environment.GetEnvironmentVariable("DBPASS")}");
    }

    // TODO: rename body
    public record Function { public UInt64 Id; public UInt64 ModuleId; public String Name; public String[] Arguments; public String NaturalFormat; public Boolean IsNative; public Byte[] Body; }
    public record Module { public UInt64 Id; public List<Function> Functions; }
}
