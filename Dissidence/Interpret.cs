using System;
using Metatron.Dissidence.Node;

namespace Metatron.Dissidence {
    public static class Interpret {
        public static object Evaluate(AST ast) {
            if (ast.HoleCount > 0) {
                throw new ArgumentException("AST has holes; cannot interpret");
            }
            // TODO: surely scope should have *something*? message at least
            // not that message can't be passed in; dissidence shouldn't know about discrod functions
            return Evaluate(ast.Root, new Scope());
        }

        public static object Evaluate(Node.Node target, Scope scope) {
            return target switch {
                    Literal node => node.Value,
                    Variable node => scope[node.Name],
                    // TODO: make sure that body returns unit when typechecking
                    If node => ((bool) Evaluate(node.Condition, scope)) ? Evaluate(node.Body, scope) : new Prelude.Unit(),
                    // Call node => replacement == node.Function ? node with { Function = replacement } : node with { Arguments = node.Arguments.Select(item => item == target ? replacement : item).ToList() },
                    // While node => replacement == node.Condition ? node with { Condition = replacement } : node with { Body = replacement },
                    // Match node => replacement == node.Value ? node with { Value = replacement } : node with { Arms = node.Arms.Select(item => item == target ? replacement : item).ToList() },
                    // Mapping node => replacement == node.Value ? node with { Value = replacement } : node with { Body = replacement },
                    // Block node => node with { Statements = node.Statements.Select(item => item == target ? replacement : item).ToList() },
                    _ => throw new ArgumentException("Unknown node type found when interpreting AST"),
            };
        }
    }
}
