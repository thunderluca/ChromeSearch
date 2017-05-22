using System;
using System.Collections.Generic;
using System.Text;

namespace ChromeSearch.Shared.Messages
{
    public class StatusBarMessage
    {
        public StatusBarMessage(bool showStatusBar)
        {
            this.ShowStatusBar = showStatusBar;
        }

        public bool ShowStatusBar { get; }
    }
}
