using System;
using System.Linq;
using System.Collections.Generic;
using Metatron.Dissidence.Node;
using Metatron.Dissidence.Attributes;
using static Metatron.Dissidence.Formats.Natural;
using USize = System.UInt64;

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
        [Info(Module="Meta.Function", Name="Change Name", Description="Change name")]
        [NaturalFormat("Change name of {1} to {2}")]
        public static HasMetadata ChangeName<T>(T Context, HasMetadata Object, String Name) {
            return Object with { Name = Name };
        }

        [Info(Module="Meta.Function", Name="Change Description", Description="Change description")]
        [NaturalFormat("Change description of {1} to {2}")]
        public static HasMetadata ChangeDescription<T>(T Context, HasMetadata Object, String Description) {
            return Object with { Description = Description };
        }

        [Info(Module="Meta.Function", Name="Remove Input", Description="Remove input at specified position")]
        [NaturalFormat("Remove argument at position {2} from {1}")]
        public static Function RemoveArgument<T>(T Context, Function Function, USize Position) {
            var arguments = Function.Arguments;
            arguments.RemoveAt((Int32) Position);
            return Function with { Arguments = arguments };
        }

        [Info(Module="Meta.Function", Name="Add Input", Description="Add input to specified position")]
        [NaturalFormat("Insert argument into {1} at position {2}")]
        public static Function AddArgument<T>(T Context, Function Function, USize Position, (String Name, Type Type) Argument) {
            var arguments = Function.Arguments;
            arguments.Insert((Int32) Position, Argument);
            return Function with { Arguments = arguments };
        }
#endregion

#region Meta.AST
        // TODO: maybe make this an extension method
        // which would signify the Node.Node is pulled in from locals if possible
        [Info(Module="Meta.AST", Name="Replace Expression", Description="Replace selected expression with another expression")]
        [NaturalFormat("Replace {2} with {3}")]
        public static Node.Node ReplaceNode<T>(T Context, Node.Node Target, Node.Node Replacement) {
            while (Target.Parent != null) {
                Replacement = Target.Parent switch {
                    // NOTE: literals, variables and holes don't contain node children.
                    // TODO: we shouldn't need this many runtime checks, do we???
                    Call node => Target == node.Function ? node with { Function = Replacement } : node with { Arguments = node.Arguments.Select(item => item == Target ? Replacement : item).ToList() },
                    While node => Target == node.Condition ?
                        node with { Condition = Replacement } : Replacement is Block ? node with { Body = (Block) Replacement } :
                        throw new ArgumentException($"While body must be Block, found {Replacement.GetType().Name}"),
                    Match node => Target == node.Value ?
                        node with { Value = Replacement } : Replacement is Mapping ? node with { Arms = node.Arms.Select(item => item == Target ? (Mapping) Replacement : item).ToList() } :
                        throw new ArgumentException($"Match arm must be Mapping, found {Replacement.GetType().Name}"),
                    Mapping node => Target == node.Value ? Replacement is Literal ?
                        node with { Value = (Literal) Replacement } :
                        throw new ArgumentException($"Mapping key must be Literal, found {Replacement.GetType().Name}")
                    : Replacement is Block ?
                        node with { Body = (Block) Replacement } :
                        throw new ArgumentException($"Mapping value must be Block, found {Replacement.GetType().Name}"),
                    Effect node => Target == node.Body ? Replacement is Block ?
                        node with { Body = (Block) Replacement } :
                        throw new ArgumentException($"Effect body must be Block, found {Replacement.GetType().Name}")
                    : Replacement is Handler ? node with { Handlers = node.Handlers.Select(item => item == Target ? (Handler) Replacement : item).ToList() } :
                        throw new ArgumentException($"Effect handler must be Handler, found {Replacement.GetType().Name}"),
                    Handler node => Target == node.Type ? Replacement is Literal && ((Literal) Replacement).Value is Type ?
                        node with { Type = (Literal) Replacement } :
                        throw new ArgumentException($"Handler type mustbe Type, found {Replacement.GetType().Name}")
                    : Target == node.Name ? Replacement is Literal && ((Literal) Replacement).Value is String ?
                        node with { Name = (Literal) Replacement } :
                        throw new ArgumentException($"Handler name must be String, found {Replacement.GetType().Name}")
                    : Replacement is Block ?
                        node with { Body = (Block) Replacement } :
                        throw new ArgumentException($"Handler value must be Block, found {Replacement.GetType().Name}"),
                    Block node => node with { Statements = node.Statements.Select(item => item == Target ? Replacement : item).ToList() },
                    // TODO: silently do nothing i guess...
                    _ => throw new ArgumentException("Unknown node type found when editing Node.Node"),
                };
                Target = Target.Parent;
            }
            return Replacement;
        }

        private static List<T> Removed<T>(this List<T> list, T item) {
            list.Removed(item);
            return list;
        }

        private static List<T> Inserted<T>(this List<T> list, int index, T item) {
            list.Insert(index, item);
            return list;
        }

        [Info(Module="Meta.AST", Name="Remove Statement", Description="Remove selected statement")]
        [NaturalFormat("Remove {2} from its parent")]
        public static Node.Node RemoveStatement<T>(T Context, Node.Node Statement) {
            return Statement.Parent switch {
                Match node => Statement == node.Value ? throw new ArgumentException("Not a statement; cannot remove statement") :
                    Statement is Mapping ? ReplaceNode(Context, node, node with { Arms = node.Arms.Removed((Mapping) Statement) }) :
                    throw new ArgumentException($"Match statement must be Mapping, found {Statement.GetType().Name}"),
                // TODO: assert 
                Effect node => Statement == node.Body ? throw new ArgumentException("Not a statement; cannot remove statement") :
                    Statement is Handler ? ReplaceNode(Context, node, node with { Handlers = node.Handlers.Removed((Handler) Statement) }) :
                    throw new ArgumentException($"Effect statement must be Handler, found {Statement.GetType().Name}"),
                Block node => ReplaceNode(Context, node, node with { Statements = node.Statements.Removed(Statement) }),
                _ => throw new ArgumentException("Not a statement; cannot remove statement"),
            };
        }

        [Info(Module="Meta.AST", Name="Add Statement Before", Description="Add empty statement before selected statement")]
        [NaturalFormat("Add empty statement before {2} in its parent")]
        public static Node.Node AddStatementBefore<T>(T Context, Node.Node Statement) {
            return Statement.Parent switch {
                Match node => Statement == node.Value ? throw new ArgumentException("Cannot add statement here") :
                    // TODO: figure out how to represent _.
                    // TODO: the Mapping and Handler reprs are a stopgap measure. normally they should be Hole but obviously that can't be used in all places
                    ReplaceNode(Context, node, node with { Arms = node.Arms.Inserted(node.Arms.IndexOf((Mapping) Statement), new Mapping {
                        Value = new Literal { Value = (Object) null },
                        Body = new Block { Statements = new List<Node.Node>() }
                    }) }),
                Effect node => Statement == node.Body ? throw new ArgumentException("Cannot add statement here") :
                    ReplaceNode(Context, node, node with { Handlers = node.Handlers.Inserted(node.Handlers.IndexOf((Handler) Statement), new Handler {
                        Type = new Literal { Value = typeof(Object) },
                        Name = new Literal { Value = "_" },
                        Body = new Block { Statements = new List<Node.Node>() }
                    }) }),
                Block node => ReplaceNode(Context, node, node with { Statements = node.Statements.Inserted(node.Statements.IndexOf(Statement), new Hole {}) }),
                _ => throw new ArgumentException("Cannot add statement here"),
            };
        }

        // TODO: DRY
        [Info(Module="Meta.AST", Name="Add Statement After", Description="Add empty statement after selected statement")]
        [NaturalFormat("Add empty statement after {2} in its parent")]
        public static Node.Node AddStatementAfter<T>(T Context, Node.Node Statement) {
            return Statement.Parent switch {
                Match node => Statement == node.Value ? throw new ArgumentException("Cannot add statement here") :
                    ReplaceNode(Context, node, node with { Arms = node.Arms.Inserted(node.Arms.IndexOf((Mapping) Statement), new Mapping {
                        Value = new Literal { Value = (Object) null },
                        Body = new Block { Statements = new List<Node.Node>() }
                    }) }),
                Effect node => Statement == node.Body ? throw new ArgumentException("Cannot add statement here") :
                    ReplaceNode(Context, node, node with { Handlers = node.Handlers.Inserted(node.Handlers.IndexOf((Handler) Statement), new Handler {
                        Type = new Literal { Value = typeof(Object) },
                        Name = new Literal { Value = "_" },
                        Body = new Block { Statements = new List<Node.Node>() }
                    }) }),
                Block node => ReplaceNode(Context, node, node with { Statements = node.Statements.Inserted(node.Statements.IndexOf(Statement) + 1, new Hole {}) }),
                _ => throw new ArgumentException("Cannot add statement here"),
            };
        }
#endregion

#region Core.Session
        // TODO
        [Info(Module="Core.Session", Name="Session Data", Description="Data associated with a session")]
        public record SessionData {}
        [Info(Module="Core.Session", Name="Session", Description="Session that includes certain users and roles in a guild")]
        public record Session {
            [Info(Module="Core.Session", Name="Guild ID", Description="ID of the guild this session is active in")]
            public USize GuildID;
            [Info(Module="Core.Session", Name="Module ID", Description="ID of the module this session is associated with")]
            public USize ModuleID;
            [Info(Module="Core.Session", Name="Session Data", Description="Data of the session that is persisted across function invocations")]
            public SessionData Data;
        }
        [Info(Module="Core.Session", Name="User Session", Description="Session that includes certain users and roles in a guild")]
        public record UserSession : Session {
            [Info(Module="Core.Session", Name="User IDs", Description="IDs of users included in this session")]
            public USize[] UserIDs;
            [Info(Module="Core.Session", Name="Role IDs", Description="IDs of roles included in this session")]
            public USize[] RoleIDs;
        }
        [Info(Module="Core.Session", Name="Channel Session", Description="Session that includes certain channels and categories in a guild")]
        public record ChannelSession : Session {
            [Info(Module="Core.Session", Name="Channel IDs", Description="IDs of channels included in this session")]
            public USize[] ChannelIDs;
            [Info(Module="Core.Session", Name="Category IDs", Description="IDs of categories included in this session")]
            public USize[] CategoryIDs;
        }
        [Info(Description="Session that includes an entire guild")]
        public record GuildSession : Session {}

        // TODO: decide whether to extract IDs from passed object or to pass IDs directly.
        [Info(Module="Core.Session", Name="Create User Session", Description="Create session that includes certain users and roles in a guild")]
        [NaturalFormat("Create session associated with {0} which includes {1} and {2}")]
        public static UserSession CreateUserSession<T>(T Context, USize GuildID, List<USize> UserIDs, List<USize> RoleIDs) {
            return new UserSession { GuildID = GuildID, UserIDs = UserIDs.ToArray(), RoleIDs = RoleIDs.ToArray(), Data = new SessionData {} };
        }

        [Info(Module="Core.Session", Name="Create Channel Session", Description="Create session that includes certain channels and categories in a guild")]
        [NaturalFormat("Create session associated with {0} which includes {1} and {2}")]
        public static ChannelSession CreateChannelSession<T>(T Context, USize GuildID, List<USize> ChannelIDs, List<USize> CategoryIDs) {
            return new ChannelSession { GuildID = GuildID, ChannelIDs = ChannelIDs.ToArray(), CategoryIDs = CategoryIDs.ToArray(), Data = new SessionData {} };
        }

        [Info(Module="Core.Session", Name="Create Guild Session", Description="Create session that includes entire guild")]
        [NaturalFormat("Create session associated with {0}")]
        public static GuildSession CreateGuildSession<T>(T Context, USize GuildID) {
            return new GuildSession { GuildID = GuildID, Data = new SessionData {} };
        }
#endregion
    }
}
