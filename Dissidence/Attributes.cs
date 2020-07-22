using System;

namespace Metatron.Dissidence.Attributes {
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple=true)]
    public class RecordInfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;

        public RecordInfoAttribute(String? Module=null, String? Name=null, String? Description=null) {
            this.Module = Module; this.Name = Name; this.Description = Description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple=true)]
    public class MemberInfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;

        public MemberInfoAttribute(String? Module=null, String? Name=null, String? Description=null) {
            this.Module = Module; this.Name = Name; this.Description = Description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Interface, AllowMultiple=true)]
    public class InterfaceInfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;

        public InterfaceInfoAttribute(String? Module=null, String? Name=null, String? Description=null) {
            this.Module = Module; this.Name = Name; this.Description = Description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple=true)]
    public class FunctionInfoAttribute : Attribute {
        public String? Module = null;
        public String? Name = null;
        public String? Description = null;

        public FunctionInfoAttribute(String? Module=null, String? Name=null, String? Description=null) {
            this.Module = Module; this.Name = Name; this.Description = Description;
        }
    }
}
