using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Analogy.Interfaces;

namespace Analogy.LogViewer.Log4Net
{
    public class Parser
    {
        private AnalogyLogMessage _current;

        public List<AnalogyLogMessage> messages = new List<AnalogyLogMessage>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        public void ParseLine(string line)
        {
            var entry = ParseEntry(line);
            if (entry != null)
            {
                _current = entry;
                messages.Add(_current);
            }
            else
            {
                if (_current == null)
                {
                    _current = new AnalogyLogMessage() {Text = line};
                }
                else
                {
                    _current.Text += Environment.NewLine + line;
                }
            }
        }

        readonly List<RegExPattern> _logPatterns = new List<RegExPattern>
        {
            new RegExPattern(
                @"\$(?<date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})+\|+(?<thread>\d{2})+\|(?<level>\w+)+\|+(?<logger>.*)\|(?<message>.*)",
                "yyyy-MM-dd HH:mm:ss,fff"),
            new RegExPattern(
//date time level thread logger whut message
                @"^
($<date>\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2},\d{3})\s+
(?<level>\w+)\s+
\[(?<thread>[^\]]+)\]\s*
(?<logger>\S+)\s+
(?<whut>\S+)\s+
(?<message>.*)$
", "yyyy-MM-dd HH:mm:ss,fff"),
            new RegExPattern(
//date time level thread logger whut message
                @"^
(?<date>\d{4}-\d{2}-\d{2}
\s
\d{2}:\d{2}:\d{2},\d{3})\s+
(?<level>\w+)\s+
\[(?<thread>[^\]]+)\]\s*
(?<logger>\S+)\s+
(?<whut>\S+)\s+
(?<message>.*)$
", "yyyy-MM-dd HH:mm:ss,fff"),
//date time thread level logger whut message
            new RegExPattern(@"^
(?<date>\d{4}-\d{2}-\d{2}
\s
\d{2}:\d{2}:\d{2},\d{3})\s+
\[(?<thread>[^\]]+)\]\s*
(?<level>\w+)\s+
(?<logger>\S+)\s+
(?<whut>\S+)\s+
(?<message>.*)$
", "yyyy-MM-dd HH:mm:ss,fff"),
            //Azure log format
            new RegExPattern(@"^
(?<date>\d{4}-\d{2}-\d{2}
T
\d{2}:\d{2}:\d{2})\s+
(?<process>\S+)\s+
(?<level>\w+)\s+
(?<logger>\S+)\s+
(?<message>.*)$
", "yyyy-MM-ddTHH:mm:ss")
        };

        private IEnumerable<RegExPattern> LogPatterns
        {
            get
            {
                if (_lastUsedPattern != null)
                    yield return _lastUsedPattern;
                var oldLastUsedPattern = _lastUsedPattern;
                foreach (var logPattern in _logPatterns)
                {
                    //skip last used pattern (returned first)
                    if (oldLastUsedPattern == logPattern) continue;
                    _lastUsedPattern = logPattern;

                    yield return _lastUsedPattern;
                }
            }
        }

        private RegExPattern _lastUsedPattern;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        private AnalogyLogMessage ParseEntry(string line)
        {

            foreach (var logPattern in LogPatterns)
            {
                var match = Regex.Match(line, logPattern.Pattern,
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (match.Success)
                {
                    string thread = null;
                    if (match.Groups["thread"].Success)
                    {
                        thread = match.Groups["thread"].Value;
                    }

                    string process = null;
                    if (match.Groups["process"].Success)
                    {
                        process = match.Groups["process"].Value;
                    }

                    var m = new AnalogyLogMessage()
                    {
                        Date = DateTime.ParseExact(match.Groups["date"].Value, logPattern.DateTimeFormat,
                            CultureInfo.InvariantCulture),
                        Thread = thread != null ? int.Parse(thread) : 0,
                        ProcessID = process != null ? int.Parse(process) : 0,
                        Source = match.Groups["logger"].Value,
                        Text = match.Groups["message"].Value
                    };
                    switch (match.Groups["level"].Value)
                    {
                        case "OFF":
                            m.Level = AnalogyLogLevel.Disabled;
                            break;
                        case "TRACE":
                            m.Level = AnalogyLogLevel.Trace;
                            break;
                        case "DEBUG":
                            m.Level = AnalogyLogLevel.Debug;
                            break;
                        case "INFO":
                            m.Level = AnalogyLogLevel.Event;
                            break;
                        case "WARN":
                            m.Level = AnalogyLogLevel.Warning;
                            break;
                        case "ERROR":
                            m.Level = AnalogyLogLevel.Error;
                            break;
                        case "FATAL":
                            m.Level = AnalogyLogLevel.Critical;
                            break;
                        default:
                            m.Level = AnalogyLogLevel.Unknown;
                            break;
                    }

                    return m;
                }
            }

            return null;
        }

        
        public async Task<List<AnalogyLogMessage>> ParseLog(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            var parser = new Parser();
            using (StreamReader reader = File.OpenText(fileName))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    parser.ParseLine(line);
                    if (token.IsCancellationRequested)
                    {
                        messagesHandler.AppendMessages(parser.messages, fileName);
                        return parser.messages;
                    }
                }

        
            }
            messagesHandler.AppendMessages(parser.messages, fileName);
            return parser.messages;
        }

    }
    }


