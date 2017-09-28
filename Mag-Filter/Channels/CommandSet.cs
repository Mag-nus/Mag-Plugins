using System;
using System.Collections.Generic;

namespace MagFilter.Channels
{
    class CommandSet
    {
        public readonly IList<Command> Commands;
        public readonly DateTime Acknowledgement;
        public CommandSet(IList<Command> commands, DateTime acknowledgement)
        {
            this.Commands = commands;
            this.Acknowledgement = acknowledgement;
        }
    }
}
