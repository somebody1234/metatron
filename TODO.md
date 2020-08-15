# todos
- [ ] remove unneccessary public modifiers on struct fields; look like the feature is not completely in quite yet
- [ ] incremental stringification. because well generating 2k long messages every time hurts at least a bit
- [ ] figure out what to do with overlong messages. likely should just discourage large functions
- [ ] if `List` properties in records are mutable then use `IReadOnlyCollection`