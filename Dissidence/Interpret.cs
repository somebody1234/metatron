using System;
using System.Linq;
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
                    Call node => ((Func<object>) (() => {
                        var function = Evaluate(node.Function, scope);
                        if (!(function is Delegate)) {
                            throw new ArgumentException("Not a function; cannot be called");
                        }
                        return ((Delegate) function).DynamicInvoke(node.Arguments.Select(argument => Evaluate(argument, scope)).ToArray());
                    }))(),
                    While node => ((Func<object>) (() => {
                        // TODO: this is inconsistent (cannot be typechecked) if body may run 0 times.
                        var result = (object) new Prelude.Unit();
                        while ((bool) Evaluate(node.Condition, scope)) {
                            result = Evaluate(node.Body, scope);
                        }
                        return result;
                    }))(),
                    Match node => ((Func<object>) (() => {
                        var target2 = Evaluate(node.Value, scope);
                        foreach (var arm in node.Arms) {
                            if (!(arm is Mapping)) {
                                throw new ArgumentException("Expected mapping node in match arm");
                            }
                            if (target2 == Evaluate(((Mapping) arm).Value, scope)) {
                                return Evaluate(((Mapping) arm).Body, scope);
                            }
                        }
                        throw new ArgumentException("This match is not exhaustive");
                    }))(),
                    Mapping node => throw new ArgumentException("Mapping node cannot be used outside of match arm"),
                    Block node => ((Func<object>) (() => {
                        var result = (object) new Prelude.Unit();
                        var scope2 = new Scope(scope);
                        foreach (var statement in node.Statements) {
                            result = Evaluate(statement, scope2);
                        }
                        return result;
                    }))(),
                    _ => throw new ArgumentException("Unknown node type found when interpreting AST"),
            };
        }
    }
}
