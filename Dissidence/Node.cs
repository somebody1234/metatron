using System;
using System.Collections.Generic;

namespace Metatron.Dissidence {
    namespace Node {
        public record Node { public Type Type; public Node? Parent; public Boolean IsHighlighted; }
        public record Literal : Node { public Object Value; }
        public record Variable : Node { public String Name; }
        public record Call : Node { public Node Function; public List<Node> Arguments; }
        public record If : Node { public Node Condition; public Node Body; }
        public record While : Node { public Node Condition; public Node Body; }
        // NOTE: if-else is match on bools. not sure if there is a need to subclass MatchNode for that alone
        public record Match : Node { public Node Value; public List<Node> Arms; }
        public record Mapping : Node { public Node Value; public Node Body; }
        public record Block : Node { public List<Node> Statements; }
        public record Hole : Node {}
    }
}
