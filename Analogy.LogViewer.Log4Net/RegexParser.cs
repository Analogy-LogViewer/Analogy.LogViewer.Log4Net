using Analogy.Interfaces;
using Analogy.Interfaces.DataTypes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Analogy.LogViewer.Log4Net
{
    public class RegexParser
    {
        private AnalogyLogMessage? _current;
        private RegexPattern? _lastUsedPattern;
        private readonly List<IAnalogyLogMessage> _messages = new List<IAnalogyLogMessage>();
        private readonly UserSettings _settings;
        private readonly bool updateUIAfterEachParsedLine;
        private ILogger Logger { get; }

        private IEnumerable<RegexPattern> LogPatterns
        {
            get
            {
                if (_lastUsedPattern != null)
                {
                    yield return _lastUsedPattern;
                }

                var oldLastUsedPattern = _lastUsedPattern;
                foreach (var logPattern in _settings.RegexPatterns)
                {
                    //skip last used pattern (returned first)
                    if (oldLastUsedPattern == logPattern)
                    {
                        continue;
                    }

                    _lastUsedPattern = logPattern;
                    yield return _lastUsedPattern;
                }
            }
        }

        public static IEnumerable<string> RegexMembers { get; }
        private static Dictionary<string, AnalogyLogMessagePropertyName> regexMapper;

        static RegexParser()
        {
            var names = Enum.GetNames(typeof(AnalogyLogMessagePropertyName));
            RegexMembers = names;
            regexMapper = new Dictionary<string, AnalogyLogMessagePropertyName>(names.Length);
            foreach (var name in names)
            {
                var enumValue = (AnalogyLogMessagePropertyName)Enum.Parse(typeof(AnalogyLogMessagePropertyName), name);
                regexMapper.Add(name, enumValue);
            }
        }

        public RegexParser(UserSettings settings, bool updateUIAfterEachLine, ILogger logger)
        {
            Logger = logger;
            _settings = settings;
            updateUIAfterEachParsedLine = updateUIAfterEachLine;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryParse(string line, RegexPattern regex, out AnalogyLogMessage message)
        {
            try
            {
                Match match = Regex.Match(line, regex.Pattern,
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (match.Success)
                {
                    var m = new AnalogyLogMessage();
                    foreach (var regexMember in regexMapper)
                    {
                        string value = match.Groups[regexMember.Key].Success
                            ? match.Groups[regexMember.Key].Value
                            : string.Empty;
                        switch (regexMember.Value)
                        {
                            case AnalogyLogMessagePropertyName.Date:
                                if (!string.IsNullOrEmpty(value) &&
                                    DateTimeOffset.TryParseExact(value, regex.DateTimeFormat, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
                                {
                                    m.Date = date;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Id:
                                if (!string.IsNullOrEmpty(value) &&
                                    Guid.TryParseExact(value, regex.GuidFormat, out var guidValue))
                                {
                                    m.Id = guidValue;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Text:
                                m.Text = value;
                                continue;
                            case AnalogyLogMessagePropertyName.Source:
                                m.Source = value;
                                continue;
                            case AnalogyLogMessagePropertyName.Module:
                                m.Module = value;
                                continue;
                            case AnalogyLogMessagePropertyName.MethodName:
                                m.MethodName = value;
                                continue;
                            case AnalogyLogMessagePropertyName.FileName:
                                m.FileName = value;
                                continue;
                            case AnalogyLogMessagePropertyName.User:
                                m.User = value;
                                continue;
                            case AnalogyLogMessagePropertyName.LineNumber:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var lineNum))
                                {
                                    m.LineNumber = lineNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.ProcessId:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var processNum))
                                {
                                    m.ProcessId = processNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.ThreadId:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var threadNum))
                                {
                                    m.ThreadId = threadNum;
                                }

                                continue;
                            case AnalogyLogMessagePropertyName.Level:
                                switch (value)
                                {
                                    case "OFF":
                                        m.Level = AnalogyLogLevel.None;
                                        break;
                                    case "TRACE":
                                        m.Level = AnalogyLogLevel.Trace;
                                        break;
                                    case "DEBUG":
                                        m.Level = AnalogyLogLevel.Debug;
                                        break;
                                    case "INFO":
                                        m.Level = AnalogyLogLevel.Information;
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

                                continue;
                            case AnalogyLogMessagePropertyName.Class:
                                if (string.IsNullOrEmpty(value))
                                {
                                    m.Class = AnalogyLogClass.General;
                                }
                                else
                                {
                                    m.Class = Enum.TryParse(value, true, out AnalogyLogClass cls) &&
                                              Enum.IsDefined(typeof(AnalogyLogClass), cls)
                                        ? cls
                                        : AnalogyLogClass.General;
                                }
                                continue;
                            case AnalogyLogMessagePropertyName.MachineName:
                                m.MachineName = value;
                                break;
                            case AnalogyLogMessagePropertyName.RawText:
                                m.RawText = value;
                                break;
                            case AnalogyLogMessagePropertyName.RawTextType:
                                m.RawTextType = AnalogyRowTextType.PlainText;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Error in message type {regexMember.Value}");
                        }
                    }

                    message = m;
                    return true;
                }

                message = null;
                return false;
            }
            catch (Exception e)
            {
                string error = $"Error parsing line: {e.Message}";
                Logger?.LogError(e, error);
                message = new AnalogyLogMessage(error, AnalogyLogLevel.Error, AnalogyLogClass.General,
                    nameof(RegexParser));
                return false;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool CheckRegex(string line, RegexPattern regex, out AnalogyLogMessage message)
        {
            try
            {
                Match match = Regex.Match(line, regex.Pattern,
                    RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
                if (match.Success)
                {
                    var m = new AnalogyLogMessage();
                    foreach (var regexMember in regexMapper)
                    {
                        string value = match.Groups[regexMember.Key].Success
                            ? match.Groups[regexMember.Key].Value
                            : string.Empty;
                        switch (regexMember.Value)
                        {
                            case AnalogyLogMessagePropertyName.Date:
                                if (!string.IsNullOrEmpty(value) &&
                                    DateTimeOffset.TryParseExact(value, regex.DateTimeFormat, CultureInfo.InvariantCulture,
                                        DateTimeStyles.None, out var date))
                                {
                                    m.Date = date;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Id:
                                if (!string.IsNullOrEmpty(value) &&
                                    Guid.TryParseExact(value, regex.GuidFormat, out var guidValue))
                                {
                                    m.Id = guidValue;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Text:
                                m.Text = value;
                                break;
                            case AnalogyLogMessagePropertyName.Source:
                                m.Source = value;
                                break;
                            case AnalogyLogMessagePropertyName.Module:
                                m.Module = value;
                                break;
                            case AnalogyLogMessagePropertyName.MethodName:
                                m.MethodName = value;
                                break;
                            case AnalogyLogMessagePropertyName.FileName:
                                m.FileName = value;
                                break;
                            case AnalogyLogMessagePropertyName.User:
                                m.User = value;
                                break;
                            case AnalogyLogMessagePropertyName.LineNumber:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var lineNum))
                                {
                                    m.LineNumber = lineNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.ProcessId:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var processNum))
                                {
                                    m.ProcessId = processNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.ThreadId:
                                if (!string.IsNullOrEmpty(value) &&
                                    int.TryParse(value, out var threadNum))
                                {
                                    m.ThreadId = threadNum;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.Level:
                                switch (value)
                                {
                                    case "OFF":
                                        m.Level = AnalogyLogLevel.None;
                                        break;
                                    case "TRACE":
                                        m.Level = AnalogyLogLevel.Trace;
                                        break;
                                    case "DEBUG":
                                        m.Level = AnalogyLogLevel.Debug;
                                        break;
                                    case "INFO":
                                        m.Level = AnalogyLogLevel.Information;
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

                                break;
                            case AnalogyLogMessagePropertyName.Class:
                                if (string.IsNullOrEmpty(value))
                                {
                                    m.Class = AnalogyLogClass.General;
                                }
                                else
                                {
                                    m.Class = Enum.TryParse(value, true, out AnalogyLogClass cls) &&
                                              Enum.IsDefined(typeof(AnalogyLogClass), cls)
                                        ? cls
                                        : AnalogyLogClass.General;
                                }

                                break;
                            case AnalogyLogMessagePropertyName.MachineName:
                                m.MachineName = value;
                                break;
                            case AnalogyLogMessagePropertyName.RawText:
                                m.RawText = value;
                                break;
                            case AnalogyLogMessagePropertyName.RawTextType:
                                m.RawTextType = AnalogyRowTextType.PlainText;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException($"Error in message type {regexMember.Value}");
                        }
                    }

                    message = m;
                    return true;
                }

                message = null;
                return false;
            }
            catch (Exception e)
            {
                string error = $"Error parsing line: {e.Message}";
                message = new AnalogyLogMessage(error, AnalogyLogLevel.Error, AnalogyLogClass.General,
                    nameof(RegexParser));
                return false;
            }
        }

        public async Task<List<IAnalogyLogMessage>> ParseLog(string fileName, CancellationToken token,
            ILogMessageCreatedHandler messagesHandler)
        {
            _messages.Clear();
            try
            {
                using (FileStream logFileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader reader = new StreamReader(logFileStream))
                    {
                        string line;
                        long count = 0;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            AnalogyLogMessage entry = null;
                            foreach (var logPattern in LogPatterns)
                            {
                                if (TryParse(line, logPattern, out entry))
                                {
                                    break;
                                }
                            }

                            if (entry != null)
                            {
                                if (updateUIAfterEachParsedLine)
                                {
                                    messagesHandler.AppendMessage(entry, fileName);
                                    count++;
                                    messagesHandler.ReportFileReadProgress(new AnalogyFileReadProgress(AnalogyFileReadProgressType.Incremental, 1, count, count));
                                }

                                _current = entry;
                                _messages.Add(_current);
                            }
                            else
                            {
                                if (_current == null)
                                {
                                    _current = new AnalogyLogMessage { Text = line };
                                }
                                else
                                {
                                    _current.Text += Environment.NewLine + line;
                                }
                            }

                            if (token.IsCancellationRequested)
                            {
                                messagesHandler.AppendMessages(_messages, fileName);
                                return _messages;
                            }
                        }
                    }
                }

                if (!updateUIAfterEachParsedLine) //update only at the end
                {
                    messagesHandler.AppendMessages(_messages, fileName);
                }

                // Reset so that any settings-changes are reflected next time we run
                _lastUsedPattern = null;

                return _messages;
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Error reading file: {e.Message}", e);
                AnalogyLogMessage error = new AnalogyLogMessage($"Error reading file: {e.Message}", AnalogyLogLevel.Critical, AnalogyLogClass.General, "Analogy", "Analogy");
                messagesHandler.AppendMessages(_messages, fileName);
                _messages.Add(error);
                return _messages;
            }
        }
    }
}