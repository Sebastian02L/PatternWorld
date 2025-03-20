using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerDataManager : Singleton<PlayerDataManager>
{
    const int numberOfMinigames = 6;

    //Data that the game will store
    int playerID;
    float globalVolume;
    float soundEffectsVolume;
    float musicVolume;
    //For each minigame, we store a list of bools that represent the rounds that have been completed.
    List<List<bool>> minigamesData = new List<List<bool>>(numberOfMinigames);

    //Getters
    public float GetGlobalVolume => globalVolume;
    public float GetSoundEffectsVolume => soundEffectsVolume;
    public float GetMusicVolume => musicVolume;

    public List<List<bool>> GetMinigameRounds()
    {
        return minigamesData;
    }

    public void Start()
    {
        ////TEMPORARY INSTRUCTIONS TO DELETE ALL DATA
        //PlayerPrefs.DeleteAll();
        //return;

        //List of List initialization
        for (int i = 0; i < numberOfMinigames; i++)
        {
            minigamesData.Add(new List<bool>());
        }

        if (!IsPlayerKnown())
        {
            Debug.Log("No player data was found. Creating default data structure...");

            playerID = 1;
            globalVolume = 0f;
            soundEffectsVolume = 0f;
            musicVolume = 0f;

            SetPlayerIDData(playerID);
            SetGlobalVolumeData(globalVolume);
            SetSoundEffectsVolumeData(soundEffectsVolume);
            SetMusicVolumeData(musicVolume);
            SetMinigameRoundData();
            SaveData();

            Debug.Log("Default data structure created!");
        }
        else
        {
            playerID = PlayerPrefs.GetInt("PlayerID");
            globalVolume = PlayerPrefs.GetFloat("GlobalVolume");
            soundEffectsVolume = PlayerPrefs.GetFloat("SoundEffectsVolume");
            musicVolume = PlayerPrefs.GetFloat("MusicVolume");

            for (int i = 0; i < numberOfMinigames; i++)
            {  
                string minigameString = PlayerPrefs.GetString("Minigame" + i);
                List<string> minigameList = minigameString.Split('.').ToList();

                List<bool> minigameBool = new List<bool>();
                foreach (string round in minigameList)
                {
                    minigameBool.Add(bool.Parse(round));
                }
                minigamesData[i] = minigameBool;
            }

            Debug.Log("Data succesfully loaded!");
        }

        //TEMPORARY INSTRUCTONS TO MODIFY DATA
        //SetMinigameRound(0, new List<bool>() { true, false, false });
        //SetMinigameRound(1, new List<bool>() { true, true, true });
    }

    ///////////////////////////
    ///Data Management methods.
   
    public bool IsPlayerKnown()
    {
        return PlayerPrefs.HasKey("PlayerID");
    }
    public void SaveData()
    {
        PlayerPrefs.Save();
    }
    //Clean the local and stored data.
    //Create the defualt structure
    //Update the sliders of the Settings UI
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        minigamesData.Clear();

        ResetStructure();
    }
    //Creates the default structure when you are in the Settings UI, then makes other scritps execute the code that reads the new structure.
    void ResetStructure()
    {
        Start();
        GameObject.FindFirstObjectByType<SceneLoaderManager>(FindObjectsInactive.Include).SetNextScene("MenuScene");
    }

    //////////////////////////////////////////////////////////////////////////////////////////
    /// This public methods are used to update the local data and then update the stored data.

    public void SetPlayerID(int ID)
    {
        playerID = ID;
        SetPlayerIDData(playerID);
        SaveData();
    }
    public void SetGlobalVolume(float value)
    {
        globalVolume = value;
        SetGlobalVolumeData(globalVolume);
        SaveData();
    }
    public void SetSoundEffectsVolume(float value)
    {
        soundEffectsVolume = value;
        SetSoundEffectsVolumeData(soundEffectsVolume);
        SaveData();
    }
    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        SetMusicVolumeData(musicVolume);
        SaveData();
    }
    public void SetMinigameRound(int minigameCode, List<bool> list)
    {
        minigamesData[minigameCode] = list;
        SetMinigameRoundData(minigameCode);
        SaveData();
    }

    //////////////////////////////////////////////////////////////////////////////
    /// This methods are used to update stored data. Invoked by the methods above.

    void SetPlayerIDData(int ID)
    {
        PlayerPrefs.SetInt("PlayerID", ID);
    }
    void SetGlobalVolumeData(float value)
    {
        PlayerPrefs.SetFloat("GlobalVolume", value);
    }
    void SetSoundEffectsVolumeData(float value)
    {
        PlayerPrefs.SetFloat("SoundEffectsVolume", value);
    }
    void SetMusicVolumeData(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
    }
    void SetMinigameRoundData(int minigameCode = -1)
    {
        //-1 means that we need to create the default structure.
        if (minigameCode == -1)
        {
            List<bool> newList = new List<bool>() {false, false, false};
            for (int i = 0; i < numberOfMinigames; i++)
            {
                minigamesData[i].Clear();
                minigamesData[i] = newList;
                PlayerPrefs.SetString("Minigame" + i, CreateStringFromList(minigamesData[i]));
            }
        }
        else //Update data of a specific minigame.
        {
            PlayerPrefs.SetString("Minigame" + minigameCode, CreateStringFromList(minigamesData[minigameCode]));
        }
    }

    ///////////////////
    /// Utility methods
    string CreateStringFromList(List<bool> roundList)
    {
        return string.Join(".", roundList);
    } 
}
