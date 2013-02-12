using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Test.CommandLineParsing;
using System.ComponentModel;
using System.IO;

namespace MyDownloader.App
{
    class CommandLineArguments
    {
        public CommandLineArguments()
        {
            this.path = Path.Combine(System.Environment.GetEnvironmentVariable("USERPROFILE"), "Downloads");
            Segments = 1;
            username = "";
            password = "";
        }

        [Description("Starts the application minimized to the tray.")]
        public bool? StartMinimized { get; set; }

        // Backwards compatibility
        public bool? @as { get { return this.StartMinimized; } set { this.StartMinimized = value; } }

        public string File { get; set; }

        public int Segments { get; set; }
        
        public string path { get; set; }

        public string username { get; set; }

        public string password { get; set; }
    }
}
