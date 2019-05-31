using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using RoxasBot.Services;
using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace RoxasBot
{

    public class Bot : IDisposable
    {
        Config _config;
        DiscordClient _client;
        public readonly CancellationTokenSource _cts;
        private CommandsNextExtension _cnext;

        public Bot()
        {
            if (Config.UseFile)
            {
                if (!File.Exists("config.json"))
                {
                    new Config().SaveToFile("config.json");
                    #region !! Report to user that config has not been set yet !! (aesthetics)
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.Black;
                    WriteCenter("▒▒▒▒▒▒▒▒▒▄▄▄▄▒▒▒▒▒▒▒", 2);
                    WriteCenter("▒▒▒▒▒▒▄▀▀▓▓▓▀█▒▒▒▒▒▒");
                    WriteCenter("▒▒▒▒▄▀▓▓▄██████▄▒▒▒▒");
                    WriteCenter("▒▒▒▄█▄█▀░░▄░▄░█▀▒▒▒▒");
                    WriteCenter("▒▒▄▀░██▄░░▀░▀░▀▄▒▒▒▒");
                    WriteCenter("▒▒▀▄░░▀░▄█▄▄░░▄█▄▒▒▒");
                    WriteCenter("▒▒▒▒▀█▄▄░░▀▀▀█▀▒▒▒▒▒");
                    WriteCenter("▒▒▒▄▀▓▓▓▀██▀▀█▄▀▀▄▒▒");
                    WriteCenter("▒▒█▓▓▄▀▀▀▄█▄▓▓▀█░█▒▒");
                    WriteCenter("▒▒▀▄█░░░░░█▀▀▄▄▀█▒▒▒");
                    WriteCenter("▒▒▒▄▀▀▄▄▄██▄▄█▀▓▓█▒▒");
                    WriteCenter("▒▒█▀▓█████████▓▓▓█▒▒");
                    WriteCenter("▒▒█▓▓██▀▀▀▒▒▒▀▄▄█▀▒▒");
                    WriteCenter("▒▒▒▀▀▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒");
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    WriteCenter("WARNING", 3);
                    Console.ResetColor();
                    WriteCenter("Thank you Mario!", 1);
                    WriteCenter("But our config.json is in another castle!");
                    WriteCenter("(Please fill in the config.json that was generated.)", 2);
                    WriteCenter("Press any key to exit..", 1);
                    Console.SetCursorPosition(0, 0);
                    Console.ReadKey();
                    #endregion
                    Environment.Exit(0);
                }

                _config = Config.LoadFromFile("config.json");
            }
            else
            {
                _config = Config.LoadFromCS();
            }
             #region !! Welcome Message !! (aesthetics)
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            WriteCenter(@"Booting Roxas Bot");
            WriteCenter();
            Console.ResetColor();
            #endregion

            _client = new DiscordClient(new DiscordConfiguration()
            {
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                Token = _config.Token,
                LogLevel = DSharpPlus.LogLevel.Debug,
                UseInternalLogHandler = false,
            });

            _cnext = _client.UseCommandsNext(new CommandsNextConfiguration()
            {
                CaseSensitive = false,
                EnableDefaultHelp = true,
                EnableMentionPrefix = true,
                StringPrefixes = new[] { _config.Prefix },
                IgnoreExtraArguments = true
            });

            RegisterCommands();

            _cts = new CancellationTokenSource();

            //Keep this bit at the end of the CTOR
            _client.Ready += OnReadyAsync;
        }
        internal void WriteCenter(string value = "", int skipline = 0)
        {
            for (int i = 0; i < skipline; i++)
            {
                Console.WriteLine();
            }
            try
            {
                Console.SetCursorPosition((Console.WindowWidth - value.Length) / 2, Console.CursorTop);
            }
            catch
            {
            }
            Console.WriteLine(value);
        }
        public async Task RunAsync()
        {
            await _client.ConnectAsync();
            await WaitForCancellationAsync();
        }

        private async Task WaitForCancellationAsync()
        {
            while (!_cts.IsCancellationRequested)
            {
                await Task.Delay(500);
            }
        }
        public void Dispose()
        {
        }
        private async Task OnReadyAsync(ReadyEventArgs e)
        {
            _client.DebugLogger.LogMessage(DSharpPlus.LogLevel.Info, "RoxasBot", "RoxasBot Finished Booting!", DateTime.Now);
            await Task.Yield();
        }
        private void RegisterCommands()
        {
            _cnext.RegisterCommands(Assembly.GetExecutingAssembly());

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            WriteCenter("Loaded commands:");
            Console.ResetColor();
            WriteCenter("-----");
            foreach (System.Collections.Generic.KeyValuePair<string, Command> command in _cnext.RegisteredCommands)
            {
                WriteCenter(_config.Prefix + command.Key + $" {(command.Value.IsHidden ? "(Hidden)" : "")}");
            }
            WriteCenter("-----");
            WriteCenter();

        }


    }

}
