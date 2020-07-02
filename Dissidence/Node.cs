using System;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    namespace Node {
        [Serializable] public record Node { public Type Type; public Node? Parent; public Boolean IsHighlighted; }
        [Serializable] public record Literal : Node { public Object Value; }
        [Serializable] public record Variable : Node { public String Name; }
        [Serializable] public record Call : Node { public Node Function; public List<Node> Arguments; }
        [Serializable] public record If : Node { public Node Condition; public Node Body; }
        [Serializable] public record While : Node { public Node Condition; public Node Body; }
        // NOTE: if-else is match on bools. not sure if there is a need to subclass MatchNode for that alone
        [Serializable] public record Match : Node { public Node Value; public List<Node> Arms; }
        [Serializable] public record Mapping : Node { public Node Value; public Node Body; }
        [Serializable] public record Block : Node { public List<Node> Statements; }
        [Serializable] public record Hole : Node {}
    }
}
