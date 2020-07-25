using System;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    namespace Node {
        // TODO: does IsSelected belong here?
        // TODO: now with node specializations Hole is a lot less useful.
        [Serializable] public record Node { public Type Type; public Node? Parent; public Boolean IsSelected; }
        [Serializable] public record Literal : Node { public Object Value; }
        [Serializable] public record Variable : Node { public String Name; }
        [Serializable] public record Call : Node { public Node Function; public List<Node> Arguments; }
        [Serializable] public record While : Node { public Node Condition; public Block Body; }
        // NOTE: if-else is match on bools. not sure if there is a need to subclass MatchNode for that alone
        [Serializable] public record Match : Node { public Node Value; public List<Mapping> Arms; }
        [Serializable] public record Mapping : Node { public Literal Value; public Block Body; }
        [Serializable] public record Effect : Node { public Node Body; public List<Handler> Handlers; }
        [Serializable] public record Handler : Node { public Literal Type; public Literal Name; public Block Body; }
        [Serializable] public record Block : Node { public List<Node> Statements; }
        // NOTE: mapping where the value is a type (of handler)
        // therefore if a type is not in any handler it should not typecheck
        [Serializable] public record Hole : Node {}
    }
}
