using System;
using Metatron.Dissidence.Node;

namespace Metatron.Dissidence {
    // TODO: better name. no syntax so not AST
    [Serializable] public record AST { public Node.Node Root; public short HoleCount; }
}
