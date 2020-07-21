using System;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    // TODO
    // TODO: also 
    [Serializable] public record Function : Prelude.HasMetadata { public Node.Node Body; public List<(String Name, Type Type)> Arguments; }

    public static class FunctionMethods {
        public static Object Call(this Function function, Object[] arguments) {
            var scope = new Scope();
            for (var i = 0; i < function.Arguments.Count; i++) {
                scope[function.Arguments[i].Item1] = arguments[i];
            }
            return Interpret.Evaluate(function.Body, scope);
        }
    }
}
