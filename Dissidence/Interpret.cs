using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Metatron.Dissidence.Node;
using USize = System.UInt64;

namespace Metatron.Dissidence {
    public static class Interpret {
        private static Dictionary<UInt64, Object> Functions = new Dictionary<USize, Object>();

        public static async Task<Object> Evaluate(Node.Node node) {
            // TODO: surely scope should have *something*? message at least
            // not that message can't be passed in; dissidence shouldn't know about discord functions
            return await Evaluate(node, new Scope());
        }

        // TODO: this (cascading async) is very slow
        // TODO: lambdas here are also disgustingly slow.
        public static async Task<Object> Evaluate(Node.Node target, Scope scope) {
            return target switch {
                    Literal node => node.Value,
                    Variable node => scope[node.Name],
                    // TODO: make sure that body returns unit when typechecking
                    Call node => await ((Func<Task<Object>>) (async () => {
                        var function = await Evaluate(node.Function, scope);
                        var arguments = (await Task.WhenAll(node.Arguments.Select(async argument => await Evaluate(argument, scope)))).ToArray();
                        if (function is Function) {
                            return ((Function) function).Call(arguments);
                        } else if (!(function is Delegate)) {
                            throw new ArgumentException("Not a function; cannot be called");
                        }
                        // TODO: null warning here. not sure how to solve this.
                        var result = ((Delegate) function).DynamicInvoke(arguments);
                        if (result.GetType().GetGenericTypeDefinition() == typeof(Task<>)) {
                            Task task = (Task) result;
                            await task.ConfigureAwait(false);
                            return (object) ((dynamic) task).Result;
                        } else {
                            return result;
                        }
                    }))(),
                    While node => await ((Func<Task<Object>>) (async () => {
                        // TODO: this is inconsistent (cannot be typechecked) if body may run 0 times.
                        var result = (Object) new Prelude.Unit();
                        while ((bool) await Evaluate(node.Condition, scope)) {
                            result = await Evaluate(node.Body, scope);
                        }
                        return result;
                    }))(),
                    Match node => await ((Func<Task<Object>>) (async () => {
                        var target2 = await Evaluate(node.Value, scope);
                        foreach (var arm in node.Arms) {
                            if (!(arm is Mapping)) {
                                throw new ArgumentException("Expected Mapping node in Match node");
                            }
                            if (target2 == await Evaluate(((Mapping) arm).Value, scope)) {
                                return await Evaluate(((Mapping) arm).Body, scope);
                            }
                        }
                        // NOTE: exhaustivity check (TODO) should ensure this does not happen.
                        throw new ArgumentException("This Match is not exhaustive");
                    }))(),
                    Mapping node => throw new ArgumentException("Mapping node cannot be used outside of Match node"),
                    Effect node => await ((Func<Task<Object>>) (async () => {
                        var target2 = await Evaluate(node.Body, scope);
                        return target2;
                        // foreach (var arm in node.Arms) {
                        //     if (!(arm is Mapping)) {
                        //         throw new ArgumentException("Expected Handler node in Effect node");
                        //     }
                        //     if (target2 == await Evaluate(((Mapping) arm).Value, scope)) {
                        //         return await Evaluate(((Mapping) arm).Body, scope);
                        //     }
                        // }
                        // // NOTE: typecheck (TODO) should ensure this does not happen.
                        // throw new ArgumentException("This Effect is not exhaustive");
                    }))(),
                    Handler node => throw new ArgumentException("Handler node cannot be used outside of Effect node"),
                    Block node => await ((Func<Task<Object>>) (async () => {
                        var result = (Object) new Prelude.Unit();
                        var scope2 = new Scope(scope);
                        foreach (var statement in node.Statements) {
                            result = await Evaluate(statement, scope2);
                        }
                        return result;
                    }))(),
                    _ => throw new ArgumentException("Unknown node type found when interpreting AST"),
            };
        }
    }
}
