using System.IO;

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