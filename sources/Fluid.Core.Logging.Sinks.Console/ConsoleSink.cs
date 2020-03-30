// Copyright 2017 Serilog Contributors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;
using System.IO;
using System.Text;
using Fluid.Core.Logging.Core;
using Fluid.Core.Logging.Events;
using Fluid.Core.Logging.Formatting;
using Fluid.Core.Logging.Sinks.Console.Platform;
using Fluid.Core.Logging.Sinks.Console.Themes;

namespace Fluid.Core.Logging.Sinks.Console
{
    internal class ConsoleSink : ILogEventSink
    {
        private const int DefaultWriteBuffer = 256;
        private readonly ITextFormatter _formatter;
        private readonly LogEventLevel? _standardErrorFromLevel;
        private readonly object _syncRoot = new object();
        private readonly ConsoleTheme _theme;

        static ConsoleSink()
        {
            WindowsConsole.EnableVirtualTerminalProcessing();
        }

        public ConsoleSink(
            ConsoleTheme theme,
            ITextFormatter formatter,
            LogEventLevel? standardErrorFromLevel)
        {
            _standardErrorFromLevel = standardErrorFromLevel;
            _theme = theme ?? throw new ArgumentNullException(nameof(theme));
            _formatter = formatter;
        }

        public void Emit(LogEvent logEvent)
        {
            var output = SelectOutputStream(logEvent.Level);

            // ANSI escape codes can be pre-rendered into a buffer; however, if we're on Windows and
            // using its console coloring APIs, the color switches would happen during the off-screen
            // buffered write here and have no effect when the line is actually written out.
            if (_theme.CanBuffer)
            {
                var buffer = new StringWriter(new StringBuilder(DefaultWriteBuffer));
                _formatter.Format(logEvent, buffer);
                lock (_syncRoot)
                {
                    output.Write(buffer.ToString());
                    output.Flush();
                }
            }
            else
            {
                lock (_syncRoot)
                {
                    _formatter.Format(logEvent, output);
                    output.Flush();
                }
            }
        }

        private TextWriter SelectOutputStream(LogEventLevel logEventLevel)
        {
            if (!_standardErrorFromLevel.HasValue)
                return System.Console.Out;

            return logEventLevel < _standardErrorFromLevel ? System.Console.Out : System.Console.Error;
        }
    }
}