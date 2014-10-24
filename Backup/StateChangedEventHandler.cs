using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _2048.Core
{
    public delegate void StateChangedEventHandler(object sender, StateChangedEventArgs e);

    public class StateChangedEventArgs : EventArgs
    {
        public StateChangedEventArgs(Coordinate[] newCoordinates)
        {
            NewCoordinates = newCoordinates;
        }
        public Coordinate[] NewCoordinates;
    }
}
