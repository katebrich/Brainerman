using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bomberman.MainClasses
{
    enum ExitState { covered, opened, closed };
    
    class Exit
    {
        public int Row;
        public int Col;
        public ExitState State = ExitState.covered;

        public Exit(int row, int col)
        {
            this.Row = row;
            this.Col = col;
        }
    }
}
