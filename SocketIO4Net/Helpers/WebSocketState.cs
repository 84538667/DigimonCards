using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketIO4Net.Helpers
{
    public enum WebSocketState
    {
        None = -1,
        Connecting = 0,
        Connected = 1,
        Closing = 2,
        Closed = 3
    }

}
