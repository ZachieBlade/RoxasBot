using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace RoxasBot.Commands
{
    public class OwnerCommands : BaseCommandModule
    {
        [Command("ping"), RequireOwner]
        public async Task Ping(CommandContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Restart();
            DiscordMessage msg = await context.RespondAsync("Pong!");
            await msg.ModifyAsync($"Pong! `ms{sw.ElapsedMilliseconds}`");
            sw.Stop();
        }

        [Command("relay"), RequireOwner]
        public async Task RelayAsync(CommandContext ctx, ulong channel, [RemainingText] string msg);
        
    }
}
