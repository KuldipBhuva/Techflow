using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    public class DataCenterModel
    {
        public int DCID { get; set; }
        public string DataCenter { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
