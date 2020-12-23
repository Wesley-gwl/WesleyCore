using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace WesleyUntity
{
    public class NPOIMemoryStream : MemoryStream
    {
        public NPOIMemoryStream()
        {
            AllowClose = true;
        }

        public bool AllowClose { get; set; }

        public override void Close()
        {
            if (AllowClose)
                base.Close();
        }
    }
}