using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public interface IStateListener
{
    void OnStateChange(PlayState NewState);
}