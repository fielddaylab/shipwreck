﻿using UnityEngine;
using System.Collections;
using Shipwreck;
using Shipwreck.Scene;
public class LoadLevel : SceneSwitch
{
    public string LevelID;

    public void StartLevel()
    {
        PlayerProgress.instance.LoadLevel(LevelID);
        Logging.instance?.LogMissionStart(LevelID);
        GotoDesk();
    }
}
