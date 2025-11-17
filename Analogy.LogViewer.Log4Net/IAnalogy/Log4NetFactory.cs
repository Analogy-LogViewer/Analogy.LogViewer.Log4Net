using Analogy.Interfaces;
using Analogy.Interfaces.Factories;
using Analogy.LogViewer.Log4Net.Properties;
using Analogy.LogViewer.Template.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Analogy.LogViewer.Log4Net
{
    public class Log4NetFactory : PrimaryFactoryWinForms
    {
        internal static Guid Log4NetFactoryId = new Guid("DBC17C75-5212-46E3-B98A-539E779000E3");
        public override Guid FactoryId { get; set; } = Log4NetFactoryId;
        public override string Title { get; set; } = "Log4Net Parser";
        public override IEnumerable<IAnalogyChangeLog> ChangeLog { get; set; } = ChangeLogList.GetChangeLog();
        public override IEnumerable<string> Contributors { get; set; } = new List<string> { "Lior Banai" };
        public override string About { get; set; } = "Log4Net Parser for Analogy Log Viewer";
        public override Image? SmallImage { get; set; } = Resources.log4net16x16;
        public override Image? LargeImage { get; set; } = Resources.log4net32x32;
    }
}