using System;
using System.Collections.Generic;

namespace Metatron {
    public static class Utils {
        public static void Setup() {
            // TODO: create tables and shit
            
            var modules = new Dictionary<String, (UInt64 Id, Dictionary<String, UInt64> Functions)>();
            foreach (var function in Dissidence.Prelude.FunctionInfos) {
                var moduleId = (UInt64) 0;
                var functionId = (UInt64) 0;
                if (!modules.ContainsKey(function.ModuleName)) {
                    moduleId = (UInt64) modules.Count;
                    modules[function.ModuleName] = (moduleId, new Dictionary<String, UInt64>());
                    using (var db = new DB.Context()) {
                        db.Modules.Add(new DB.Module() {
                            Id = moduleId,
                            Functions = null,
                        });
                    }
                }
                var functions = modules[function.ModuleName].Functions;
                if (functions.ContainsKey(function.FunctionName)) {
                    throw new Exception($"Duplicate function {function.FunctionName} in module {function.ModuleName}");
                }
                functionId = (UInt64) functions.Count;
                functions[function.FunctionName] = functionId;
                // 0x1fffff00000000ul, 2 bytes for module, 2 bytes for function (meaning 65536 of each max)
                // TODO:
                using (var db = new DB.Context()) {
                    db.Functions.Add(new DB.Function() {
                        Id = 0x1fffff00000000ul | (moduleId << 16) | functionId,
                        ModuleId = 0x1fffff00000000ul | (moduleId << 16),
                        Name = function.FunctionName,
                        Arguments = function.Arguments,
                        NaturalFormat = function.NaturalFormat,
                        IsNative = true,
                        Body = new Byte[] {},
                    });
                }
            }
        }
    }
}
