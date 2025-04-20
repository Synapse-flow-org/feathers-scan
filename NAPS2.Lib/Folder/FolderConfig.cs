using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NAPS2.Folder
{
    public class FolderConfig
    {
        public string? Num { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? CIN { get; set; }


        public event EventHandler<EventArgs>? ConfigUpdated;

        public void FolderConfigUpdate()
        {
            ConfigUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
