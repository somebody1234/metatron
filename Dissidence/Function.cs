using System;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    // TODO
    // TODO: also 
    [Serializable] public record Function { public AST Body; public List<String> Arguments; }

    public static class FunctionMethods {
        public static Object Call(this Function function, Object[] arguments) {
            var scope = new Scope();
            for (var i = 0; i < function.Arguments.Count; i++) {
                scope[function.Arguments[i]] = arguments[i];
            }
            return Interpret.Evaluate(function.Body.Root, scope);
        }
    }
}
