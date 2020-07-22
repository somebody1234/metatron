using System;
using System.Linq;
using System.Collections.Generic;
using Metatron.Dissidence.Node;
using Metatron.Dissidence.Attributes;
using static Metatron.Dissidence.Formats.Natural;
using UInt8 = System.Byte;

namespace Metatron.Dissidence {
    public static class Prelude {
        // TODO: builtin methods (like string shit)

#region Miscellaneous
        public record Unit {};

        // TODO: idk what im doing. this is for function and module i guess
        public record HasMetadata { public String Name; public String Description; }
#endregion

#region Meta.Function
        // TODO: not sure if this works ._. am i really going to need generics this soon
        [FunctionInfo(Module="Meta.Function", Name="Change Name", Description="Change name")]
        [NaturalFormat("Change name of {1} to {2}")]
        public static HasMetadata ChangeName(HasMetadata Object, String Name) {
            return Object with { Name = Name };
        }

        [FunctionInfo(Module="Meta.Function", Name="Change Description", Description="Change description")]
        [NaturalFormat("Change description of {1} to {2}")]
        public static HasMetadata ChangeDescription(HasMetadata Object, String Description) {
            return Object with { Description = Description };
        }

        [FunctionInfo(Module="Meta.Function", Name="Remove Input", Description="Remove input at specified position")]
        [NaturalFormat("Remove argument at position {2} from {1}")]
        public static Function RemoveArgument(Function Function, UInt8 Position) {
            var arguments = Function.Arguments;
            arguments.RemoveAt(Position);
            return Function with { Arguments = arguments };
        }

        [FunctionInfo(Module="Meta.Function", Name="Add Input", Description="Add input to specified position")]
        [NaturalFormat("Insert argument into {1} at position {2}")]
        public static Function AddArgument(Function Function, UInt8 Position, (String Name, Type Type) Argument) {
            var arguments = Function.Arguments;
            arguments.Insert(Position, Argument);
            return Function with { Arguments = arguments };
        }
#endregion

#region Meta.AST
        // TODO: maybe make this an extension method
        // which would signify the Node.Node is pulled in from locals if possible
        [FunctionInfo(Module="Meta.AST", Name="Replace Expression", Description="Replace selected expression with another expression")]
        [NaturalFormat("Replace {2} with {3} in {1}")]
        public static Node.Node ReplaceNode(Node.Node ast, Node.Node target, Node.Node replacement) {
            while (target.Parent != null) {
                replacement = target.Parent switch {
                    // NOTE: literals, variables and holes don't contain node children.
                    If node => replacement == node.Condition ? node with { Condition = replacement } : node with { Body = replacement },
                    Call node => replacement == node.Function ? node with { Function = replacement } : node with { Arguments = node.Arguments.Select(item => item == target ? replacement : item).ToList() },
                    While node => replacement == node.Condition ? node with { Condition = replacement } : node with { Body = replacement },
                    Match node => replacement == node.Value ? node with { Value = replacement } : node with { Arms = node.Arms.Select(item => item == target ? replacement : item).ToList() },
                    Mapping node => replacement == node.Value ? node with { Value = replacement } : node with { Body = replacement },
                    Block node => node with { Statements = node.Statements.Select(item => item == target ? replacement : item).ToList() },
                    // TODO: silently do nothing i guess...
                    _ => throw new ArgumentException("Unknown node type found when editing Node.Node"),
                };
                target = target.Parent;
            }
            return replacement;
        }

        private static List<T> Removed<T>(this List<T> list, T item) {
            list.Removed(item);
            return list;
        }

        private static List<T> Inserted<T>(this List<T> list, int index, T item) {
            list.Insert(index, item);
            return list;
        }

        [FunctionInfo(Module="Meta.AST", Name="Remove Statement", Description="Remove selected statement")]
        [NaturalFormat("Remove {2} from {1}")]
        public static Node.Node RemoveStatement(Node.Node ast, Node.Node target) {
            return target.Parent switch {
                Match node => target == node.Value ? throw new ArgumentException("Not a statement; cannot remove statement") : ReplaceNode(ast, node, node with { Arms = node.Arms.Removed(target) }),
                Block node => ReplaceNode(ast, node, node with { Statements = node.Statements.Removed(target) }),
                _ => throw new ArgumentException("Not a statement; cannot remove statement"),
            };
        }

        [FunctionInfo(Module="Meta.AST", Name="Add Statement Before", Description="Add empty statement before selected statement")]
        [NaturalFormat("Add empty statement before {2} in {1}")]
        public static Node.Node AddStatementBefore(Node.Node ast, Node.Node target) {
            return target.Parent switch {
                Match node => target == node.Value ? throw new ArgumentException("Cannot add statement here") : ReplaceNode(ast, node, node with { Arms = node.Arms.Inserted(node.Arms.IndexOf(target), new Mapping { Value = new Hole {}, Body = new Hole {} }) }),
                Block node => ReplaceNode(ast, node, node with { Statements = node.Statements.Inserted(node.Statements.IndexOf(target), new Hole {}) }),
                _ => throw new ArgumentException("Cannot add statement here"),
            };
        }

        // TODO: DRY
        [FunctionInfo(Module="Meta.AST", Name="Add Statement After", Description="Add empty statement after selected statement")]
        [NaturalFormat("Add empty statement after {2} in {1}")]
        public static Node.Node AddStatementAfter(Node.Node ast, Node.Node target) {
            return target.Parent switch {
                Match node => target == node.Value ? throw new ArgumentException("Cannot add statement here") : ReplaceNode(ast, node, node with { Arms = node.Arms.Inserted(node.Arms.IndexOf(target), new Mapping { Value = new Hole {}, Body = new Hole {} }) }),
                Block node => ReplaceNode(ast, node, node with { Statements = node.Statements.Inserted(node.Statements.IndexOf(target) + 1, new Hole {}) }),
                _ => throw new ArgumentException("Cannot add statement here"),
            };
        }
#endregion

#region Core.Session
        // TODO
        public record SessionData {}
        public record Session {
            [MemberInfo(Name="guild ID", Description="ID of the guild this session is active in")]
            public UInt64 GuildId;
            [MemberInfo(Description="ID of the module this session is associated with")]
            public UInt64 Module;
            [MemberInfo(Description="Data of the session that is persisted across function invocations")]
            public SessionData Data;
        }
        [RecordInfo(Description="Session that includes certain users and roles in a guild")]
        public record UserSession : Session {
            [MemberInfo(Description="List of users included in this session")]
            public UInt64[] UserIds;
            [MemberInfo(Description="List of roles included in this session")]
            public UInt64[] RoleIds;
        }
        [RecordInfo(Description="Session that includes certain channels and categories in a guild")]
        public record ChannelSession : Session {
            [MemberInfo(Description="List of channels included in this session")]
            public UInt64[] ChannelIds;
            [MemberInfo(Description="List of categories included in this session")]
            public UInt64[] CategoryIds;
        }
        [RecordInfo(Description="Session that includes an entire guild")]
        public record GuildSession : Session {}

        [FunctionInfo(Module="Core.Session", Name="Create User Session", Description="Create session that includes certain users and roles in a guild")]
        [NaturalFormat("Create session associated with {0} which includes {1} and {2}")]
        public static UserSession CreateUserSession(UInt64 guild, List<UInt64> users, List<UInt64> roles) {
            return new UserSession { GuildId = guild, UserIds = users.ToArray(), RoleIds = roles.ToArray(), Data = new SessionData {} };
        }

        [FunctionInfo(Module="Core.Session", Name="Create Channel Session", Description="Create session that includes certain channels and categories in a guild")]
        [NaturalFormat("Create session associated with {0} which includes {1} and {2}")]
        public static ChannelSession CreateChannelSession(UInt64 guild, List<UInt64> channels, List<UInt64> categories) {
            return new ChannelSession { GuildId = guild, ChannelIds = channels.ToArray(), CategoryIds = categories.ToArray(), Data = new SessionData {} };
        }

        [FunctionInfo(Module="Core.Session", Name="Create Guild Session", Description="Create session that includes entire guild")]
        [NaturalFormat("Create session associated with {0}")]
        public static GuildSession CreateGuildSession(UInt64 guild) {
            return new GuildSession { GuildId = guild, Data = new SessionData {} };
        }
#endregion
    }
}
