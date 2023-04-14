using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public interface ICommand
    {
        List<IDrawable> Shapes { get; set; }
        List<Command> UndoCommands { get; set; }
        List<Command> RedoCommands { get; set; }

        void Execute();
    }
}
