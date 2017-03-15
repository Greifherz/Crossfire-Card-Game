using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class SettingsManager
{
    public static CheatSettings Cheats
    {
        get
        {
            return Resources.Load<CheatSettings>("Tools/CheatSettings");
        }
    }

}

