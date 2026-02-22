using Chat;
using System.Collections.Generic;
using System.Linq;

namespace PhinixServer
{
    /// <inheritdoc />
    /// <summary>
    /// Command handler for the help command. This is only implemented to be displayed by the help command, all processing for the help command occurs in the <see cref="CommandInterpreter"/> class.
    /// </summary>
    public class BanWordCommand : Command
    {
        public override string CommandName => "banWord";

        public override HelpEntry[] HelpEntries => new HelpEntry[]
        {
            new HelpEntry("banWord ban", new string[]{"word"}, "Blacklist a specific word"), 
            new HelpEntry("banWord unban", new string[]{"word"}, "Unblacklist a specific word")
        };

        public override bool Execute(List<string> args)
        {
            if (args.Count != 2)
            {
                return false;
            }
            if (args.ElementAt(0).ToLower() == "ban")
            {
                ServerChat.GetInstance().AddToBlacklist(args.ElementAt(1).ToLower());
            }
            else if (args.ElementAt(0).ToLower() == "unban")
            {
                ServerChat.GetInstance().RemoveFromBlacklist(args.ElementAt(1).ToLower());
            }
            else
                return false;


            return true;
        }
    }
}