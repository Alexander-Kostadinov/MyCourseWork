﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyCourseWork
{
    public class Clear : Command
    {
        public Clear(List<Command> undoCommands, List<Command> redoCommands, 
            List<IDrawable> shapes)
        {
            Shapes = shapes;
            UndoCommands = undoCommands;
            RedoCommands = redoCommands;
        }

        public override void Execute()
        {
            Shapes.Clear();
            UndoCommands.Clear();
            RedoCommands.Clear();
        }
    }
}