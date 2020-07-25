using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Metatron.Dissidence.Formats {
    public static class Natural {
        [System.AttributeUsage(System.AttributeTargets.Method)]
        public class NaturalFormatAttribute : Attribute {
            public String Format;
    
            public NaturalFormatAttribute(String Format) {
                this.Format = Format;
            }
        }

        private static String NaturalFormatPlural(String name) {
            return name + "s"; // TODO
        }

        private static System.Text.RegularExpressions.Regex rNewWord = new System.Text.RegularExpressions.Regex(@"(?=[A-Z])", System.Text.RegularExpressions.RegexOptions.Compiled);
        private static String NaturalFormatTypeName<T>() {
            // TODO: if type has DissidenceRecordInfo attribute use name from that
            return string.Join(" ", rNewWord.Split(typeof(T).Name).Select(word => word.ToLower()));
        }

        public interface INaturalFormat {
            public String NaturalFormat();
        }

        public static String NaturalFormat<T>(T item) {
            if (item is INaturalFormat) {
                return ((INaturalFormat) item).NaturalFormat();
            } else {
                return $"{NaturalFormatTypeName<T>()}";
            }
        }

        public static String NaturalFormat<T>(IEnumerable<T> items) {
            var count = items.Count();
            switch (count) {
                case 0:
                    return $"no {NaturalFormatPlural(NaturalFormatTypeName<T>())}";
                case 1:
                    foreach (var item in items) {
                        return NaturalFormat(item);
                    }
                    return ""; // NOTE: should never happen
                default:
                    var result = new StringBuilder();
                    foreach (var item in items) {
                        if (count-- == 0) {
                            // TODO: i18n
                            result.Append(" and ");
                        } else {
                            result.Append(", ");
                        }
                        result.Append(NaturalFormat(item));
                    }
                    return result.ToString();
            }
        }

        // TODO: impl for node
    }
}
