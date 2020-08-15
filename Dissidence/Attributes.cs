using System;
using System.Runtime.CompilerServices;

namespace Metatron.Dissidence.Attributes {
    public class InfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;
        public Int32 Order = 0;

        public InfoAttribute(String? Module=null, String? Name=null, String? Description=null, [CallerLineNumber]Int32 Order=0) {
            this.Module = Module; this.Name = Name; this.Description = Description; this.Order = Order;
        }
    }

    public class DissidenceOnlyAttribute : Attribute {}
    public class NativeOnlyAttribute : Attribute {}
}
