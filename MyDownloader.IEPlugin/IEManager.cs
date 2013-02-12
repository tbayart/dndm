using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using BandObjectLib;
using System.Runtime.InteropServices;
using SHDocVw;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Threading;

namespace MyDownloader.IEPlugin
{
    //http://msdn.microsoft.com/en-us/library/aa753588.aspx
    //http://msdn.microsoft.com/en-us/library/aa753590.aspx
    [Guid("3F93D37D-54B8-4d5d-92CB-6F5644E7A65A")]
    [BandObject("MyDownloader_List", BandObjectStyle.Vertical | BandObjectStyle.ExplorerBar )]
	public class IEManager : BandObject
    {
        public IEManager()
        {
            //InitializeComponent();
        }
    }
}
