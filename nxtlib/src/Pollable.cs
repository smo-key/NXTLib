using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NXTLib
{
    public abstract class Pollable
    {
        public delegate void Polled(Pollable polledItem);
        public Pollable()
        {
            Timer = new Timer(Timer_Callback, null, Timeout.Infinite, Timeout.Infinite);
        }
        private Brick _brick = null;
        internal protected Brick brick
        {
            get
            {
                return _brick;
            }
            set
            {
                _brick = value;
            }
        }

        private int _pollinterval = 0;
        public int PollInterval
        {
            get
            {
                return _pollinterval;
            }
            set
            {
                _pollinterval = value;
                if (_pollinterval > 0)
                {
                    if (brick != null && brick.IsConnected)
                        EnableAutoPoll();
                    else
                        DisableAutoPoll();
                }
                else
                {
                    _pollinterval = 0;
                    DisableAutoPoll();
                }
            }
        }

        internal void EnableAutoPoll()
        {
            if (_pollinterval > 0)
            {
                Timer.Change(_pollinterval, _pollinterval);
            }
        }

        internal void DisableAutoPoll()
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
        }
        private Timer Timer = null;
        private void Timer_Callback(object state)
        {
            try { Poll(); }
            catch (Exception) { }
        }
        public event Polled OnPolled;
        public virtual void Poll()
        {
            if (OnPolled != null) OnPolled(this);
        }
    }
}
