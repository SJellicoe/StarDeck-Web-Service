//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Service
{
    using System;
    using System.Collections.Generic;
    
    public partial class post
    {
        public post()
        {
            this.comments = new HashSet<comment>();
        }
    
        public int posts_id { get; set; }
        public string posts_title { get; set; }
        public string posts_text { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> category_id { get; set; }
    
        public virtual category category { get; set; }
        public virtual ICollection<comment> comments { get; set; }
        public virtual user user { get; set; }
    }
}