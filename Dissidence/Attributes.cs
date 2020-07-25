using System;

namespace Metatron.Dissidence.Attributes {
    public class InfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;

        public InfoAttribute(String? Module=null, String? Name=null, String? Description=null) {
            this.Module = Module; this.Name = Name; this.Description = Description;
        }
    }
}
