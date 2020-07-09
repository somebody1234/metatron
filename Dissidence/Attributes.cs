using System;

namespace Metatron.Dissidence {
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple=true)]
    public class DissidenceRecordInfoAttribute : System.Attribute {
        public String? Name = null;
        public String? Description = null;

        public DissidenceRecordInfoAttribute(String? Name=null, String? Description=null) {
            this.Name = Name; this.Description = Description;
        }
    }

    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple=true)]
    public class DissidenceMemberInfoAttribute : System.Attribute {
        public String? Name = null;
        public String? Description = null;

        public DissidenceMemberInfoAttribute(String? Name=null, String? Description=null) {
            this.Name = Name; this.Description = Description;
        }
    }
}
