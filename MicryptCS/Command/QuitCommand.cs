using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicryptCS.Command
{
    public class QuitCommand : BaseCommand
    {
        public override string Text => "Quit";

        public override void Execute(Crypto crypto)
        {
            Environment.Exit(0);
        }

    }
}
