using UnityEngine;
using System.Collections;
using System;

public class HackySavegameInteraction : MonoBehaviour {

    private int currentlyDetecting = 0;
    private bool saved = false;
    private bool loaded = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.F1))
        {
            DetectSaveLoad(1);
        }
        else if (Input.GetKey(KeyCode.F2))
        {
            DetectSaveLoad(2);
        }
        else if (Input.GetKey(KeyCode.F3))
        {
            DetectSaveLoad(3);
        }
        else if (Input.GetKey(KeyCode.F4))
        {
            DetectSaveLoad(4);
        }
        else if (Input.GetKey(KeyCode.F5))
        {
            DetectSaveLoad(5);
        }
        else {
            saved = false;
            loaded = false;
            currentlyDetecting = 0;
        }

    }

    private void DetectSaveLoad(int ID)
    {
        currentlyDetecting = ID;

        if (Input.GetKeyUp(KeyCode.S))
        {
            LevelSerializer.SaveGame("SkyldSave"+ID);
            saved = true;
        }
        else if (Input.GetKeyUp(KeyCode.L))
        {
            foreach (var save in LevelSerializer.SavedGames[LevelSerializer.PlayerName])
            {
                if (save.Name == "SkyldSave" + ID)
                {
                    LevelSerializer.LoadNow(save.Data);
                    loaded = true;
                }
            }
        }
    }

    void OnGUI()
    {
        if (currentlyDetecting > 0)
        {
            if (saved)
            {
                GUI.Label(
                    new Rect(10, 10, 1000, 100), "Slot [" + currentlyDetecting + "]: SAVED");
            }
            else if (loaded)
            {
                GUI.Label(
                    new Rect(10, 10, 1000, 100), "Slot [" + currentlyDetecting + "]: LOADED");
            }
            else
                GUI.Label(
                    new Rect(10, 10, 1000, 100), "Slot [" + currentlyDetecting + "]: Press 'S' to save, 'L' to load.");
        }
    }
}
