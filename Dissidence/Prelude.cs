using System;
using System.Linq;
using System.Collections.Generic;
using Metatron.Dissidence.Node;

namespace Metatron.Dissidence {
    public static class Prelude {
        // static void Initialize(Engine engine) {
        //     Engine.AddFunction("Meta.AST", "Replace Node", ReplaceNode);
        //     Engine.AddFunction("Meta.AST", "Remove Statement", RemoveStatement);
        //     Engine.AddFunction("Meta.AST", "Add Statement", AddStatement);
        // }
        // TODO: docs, ToString and shit
        // TODO: builtin methods

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

        public record Unit {};
    }
}
