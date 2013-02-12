using System;
using System.Collections.Generic;
using System.Text;

namespace MyDownloader.Core
{
    public enum DownloaderState: byte 
    {
        NeedToPrepare = 0,
        Preparing,
        WaitingForReconnect,
        Prepared,
        Working,
        Pausing,
        Paused,
        Ended,
        EndedWithError
    }

    public enum DownloaderScheduleState : byte
    {
        Paused = 0,
        Waiting,
        Working,
        Error,
        Ended
    }

}
