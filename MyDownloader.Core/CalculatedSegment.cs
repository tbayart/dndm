using System;
using System.Collections.Generic;
using System.Text;

namespace MyDownloader.Core
{
    [Serializable]
    public struct CalculatedSegment
    {
        private long startPosition;
        private long endPosition;

        public long StartPosition
        {
            get { return startPosition; }
            set { startPosition = value; }
        }

        public long EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }

        public CalculatedSegment(long startPos, long endPos)
        {
            this.endPosition = endPos;
            this.startPosition = startPos;
        }
    }
}
