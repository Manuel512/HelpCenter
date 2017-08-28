using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace HelpCenter.Models
{
    public class EditSection
    {
        public sp_GetModules_Result Modules { get; set; }
        public sp_GetSections_Result Section { get; set; }
    }
}