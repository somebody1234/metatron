using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Metatron.Dissidence.Node;

namespace Metatron.Dissidence {
    public static class Prelude {
        public static Dictionary<UInt64, Object> Functions = new Dictionary<UInt64, Object>();

        public static List<(String ModuleName, String FunctionName, Object Function, String[] Arguments, String NaturalFormat)> FunctionInfos = new List<(String ModuleName, String FunctionName, Object Function, String[] Arguments, String NaturalFormat)> {
            ("Meta.AST", "Replace Node", (Object) (Func<AST, Node.Node, Node.Node, AST>) ReplaceNode, new[] { "AST", "Target", "Replacement" }, "Replace {2} with {3} in {1}"),
            ("Meta.AST", "Remove Statement", (Object) (Func<AST, Node.Node, AST>) RemoveStatement, new[] { "AST", "Target" }, "Remove {2} from {1}"),
            ("Meta.AST", "Add Statement", (Object) (Func<AST, Node.Node, AST>) AddStatement, new[] { "AST", "Target" }, "Add empty statement after {2} in {1}"),
            // TODO: take ienumerables rather than lists?
            ("Core.Session", "Create User Session", (Object) (Func<UInt64, List<UInt64>, List<UInt64>, UserSession>) CreateUserSession, new[] { "Guild", "Users", "Roles" }, "Create session associated with {0} which includes {1} and {2}"),
            ("Core.Session", "Create Channel Session", (Object) (Func<UInt64, List<UInt64>, List<UInt64>, ChannelSession>) CreateChannelSession, new[] { "Guild", "Channels", "Categories" }, "Create session associated with {0} which includes {1} and {2}"),
            ("Core.Session", "Create Guild Session", (Object) (Func<UInt64, GuildSession>) CreateGuildSession, new[] { "Guild" }, "Create session associated with {0}"),
        };
        // TODO: builtin methods (like string shit)

#region Miscellaneous
        public record Unit {};

        private static String NaturalFormatPlural(String name) {
            return name + "s"; // TODO
        }

        private static System.Text.RegularExpressions.Regex rNewWord = new System.Text.RegularExpressions.Regex(@"(?=[A-Z])", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static String NaturalFormatTypeName<T>() {
            // TODO: if type has DissidenceRecordInfo attribute use name from that
            return string.Join(" ", rNewWord.Split(typeof(T).Name).Select(word => word.ToLower()));
        }

        public interface INaturalFormat {
            public String NaturalFormat();
        }

        public static String NaturalFormat<T>(T item) {
            if (item is INaturalFormat) {
                return ((INaturalFormat) item).NaturalFormat();
            } else {
                return $"{NaturalFormatTypeName<T>()}";
            }
        }

        public static String NaturalFormat<T>(IEnumerable<T> items) {
            var count = items.Count();
            switch (count) {
                case 0:
                    return $"no {NaturalFormatPlural(NaturalFormatTypeName<T>())}";
                case 1:
                    foreach (var item in items) {
                        return NaturalFormat(item);
                    }
                    return ""; // NOTE: should never happen
                default:
                    var result = new StringBuilder();
                    foreach (var item in items) {
                        if (count-- == 0) {
                            // TODO: i18n
                            result.Append(" and ");
                        } else {
                            result.Append(", ");
                        }
                        result.Append(NaturalFormat(item));
                    }
                    return result.ToString();
            }
        }
#endregion

#region Meta.AST
        // TODO: maybe make this an extension method
        // which would signify the AST is pulled in from locals if possible
        public static AST ReplaceNode(AST ast, Node.Node target, Node.Node replacement) {
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
                    _ => throw new ArgumentException("Unknown node type found when editing AST"),
                };
                target = target.Parent;
            }
            return ast with { Root = replacement };
        }

        private static List<T> Removed<T>(this List<T> list, T item) {
            list.Removed(item);
            return list;
        }

        private static List<T> Inserted<T>(this List<T> list, int index, T item) {
            list.Insert(index, item);
            return list;
        }

        public static AST RemoveStatement(AST ast, Node.Node target) {
            return target.Parent switch {
                Match node => target == node.Value ? throw new ArgumentException("Not a statement; cannot remove statement") : ReplaceNode(ast, node, node with { Arms = node.Arms.Removed(target) }),
                Block node => ReplaceNode(ast, node, node with { Statements = node.Statements.Removed(target) }),
                _ => throw new ArgumentException("Not a statement; cannot remove statement"),
            };
        }

        public static AST AddStatement(AST ast, Node.Node target) {
            return target.Parent switch {
                Match node => target == node.Value ? throw new ArgumentException("Cannot add statement here") : ReplaceNode(ast, node, node with { Arms = node.Arms.Inserted(node.Arms.IndexOf(target), new Mapping { Value = new Hole {}, Body = new Hole {} }) }),
                Block node => ReplaceNode(ast, node, node with { Statements = node.Statements.Inserted(node.Statements.IndexOf(target), new Hole {}) }),
                _ => throw new ArgumentException("Cannot add statement here"),
            };
        }
#endregion

#region Core.Session
        // TODO
        public record SessionData {}
        public record Session {
            [DissidenceMemberInfo(Name="guild ID", Description="ID of the guild this session is active in")]
            public UInt64 GuildId;
            [DissidenceMemberInfo(Description="ID of the module this session is associated with")]
            public UInt64 Module;
            [DissidenceMemberInfo(Description="Data of the session that is persisted across function invocations")]
            public SessionData Data;
        }
        [DissidenceRecordInfo(Description="Session that includes certain users and roles in a guild")]
        public record UserSession : Session {
            [DissidenceMemberInfo(Description="List of users included in this session")]
            public UInt64[] UserIds;
            [DissidenceMemberInfo(Description="List of roles included in this session")]
            public UInt64[] RoleIds;
        }
        [DissidenceRecordInfo(Description="Session that includes certain channels and categories in a guild")]
        public record ChannelSession : Session {
            [DissidenceMemberInfo(Description="List of channels included in this session")]
            public UInt64[] ChannelIds;
            [DissidenceMemberInfo(Description="List of categories included in this session")]
            public UInt64[] CategoryIds;
        }
        [DissidenceRecordInfo(Description="Session that includes an entire guild")]
        public record GuildSession : Session {}


        public static UserSession CreateUserSession(UInt64 guild, List<UInt64> users, List<UInt64> roles) {
            return new UserSession { GuildId = guild, UserIds = users.ToArray(), RoleIds = roles.ToArray(), Data = new SessionData {} };
        }

        public static ChannelSession CreateChannelSession(UInt64 guild, List<UInt64> channels, List<UInt64> categories) {
            return new ChannelSession { GuildId = guild, ChannelIds = channels.ToArray(), CategoryIds = categories.ToArray(), Data = new SessionData {} };
        }

        public static GuildSession CreateGuildSession(UInt64 guild) {
            return new GuildSession { GuildId = guild, Data = new SessionData {} };
        }
#endregion
    }
}
