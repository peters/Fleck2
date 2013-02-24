using System.Collections.Generic;

namespace Fleck2
{
    public class ReadState
    {
        public ReadState()
        {
            Data = new List<byte>();
        }
        public List<byte> Data { get; private set; }
        public FrameType? FrameType { get; set; }
        public void Clear()
        {
            Data.Clear();
            FrameType = null;
        }
    }
}

