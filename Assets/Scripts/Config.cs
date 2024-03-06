﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;


public class Config : MonoBehaviour
{

#if UNITY_ANDROID && !UNITY_EDITOR
    private static string dir = Application.persistentDataPath;
#else
    private static string dir = Directory.GetCurrentDirectory();
#endif

    private static string file = @"\gameData.ini";
    private static string path = dir + file;


    // delete file if player completes a level
    public static void DeleteFile()
    {
        Debug.Log("File deleted");
        File.Delete(path);
    }

    public static void SaveBoardData(GameData.GameBoardData boardData, int boardIndex, int errorNumber)
    {
        // save time, levelSelected, gameData and mistakes
        File.WriteAllText(path, string.Empty);
        StreamWriter writer = new StreamWriter(path, false);
        string currentTime = "#time:" + Timer.GetCurrentTime();
        string errorNumberString = "#errors:" + errorNumber;
        string boardIndexString = "#boardIndex:" + boardIndex.ToString();
        string extraCluesString = "#extraClues:";
        string unsolvedString = "#unsolved:";
        string solvedString = "#solved:";

        foreach (var extraClue in boardData.clues_data)
        {
            extraCluesString += extraClue + ",";
        }
        foreach (var unsolvedData in boardData.unsolved_data)
        {
            unsolvedString += unsolvedData.ToString() + ",";
        }
        foreach (var solvedData in boardData.solved_data)
        {
            solvedString += solvedData.ToString() + ",";
        }

        writer.WriteLine(currentTime);
        writer.WriteLine(errorNumberString);
        writer.WriteLine(boardIndexString);
        writer.WriteLine(extraCluesString);
        writer.WriteLine(unsolvedString);
        writer.WriteLine(solvedString);

        writer.Close();
    }


    public static GameData.GameBoardData ReadGridData()
    {
        // read gameData from gameData.ini
        string line;
        StreamReader file = new StreamReader(path);

        string[] extraClues_data = new string[121];
        int[] unsolved_data = new int[121];
        int[] solved_data = new int[121];

        int extraCluesIndex = 0;
        int unsolvedIndex = 0;
        int solvedIndex = 0;


        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#extraClues")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (string value in substrings)
                {
                    if (extraCluesIndex < 121)
                    {
                        Debug.Log(value);
                        extraClues_data[extraCluesIndex] = value;
                        extraCluesIndex++;
                    }
                }
            }

            if (word[0] == "#unsolved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        unsolved_data[unsolvedIndex] = squareNumber;
                        unsolvedIndex++;
                    }
                }
            }

            if (word[0] == "#solved")
            {
                string[] substrings = Regex.Split(word[1], ",");

                foreach (var value in substrings)
                {
                    int squareNumber = -1;
                    if (int.TryParse(value, out squareNumber))
                    {
                        solved_data[solvedIndex] = squareNumber;
                        solvedIndex++;
                    }
                }
            }
        }

        file.Close();
        return new GameData.GameBoardData(extraClues_data, unsolved_data, solved_data);
    }

    public static int ReadGameLevel()
    {
        // read selectedLevel from gameData.ini
        int level = -1;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#boardIndex")
            {
                int.TryParse(word[1], out level);
            }
        }

        file.Close();
        return level;
    }

    public static float ReadTime()
    {
        // read time from gameData.ini
        float time = -1.0f;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#time")
            {
                float.TryParse(word[1], out time);
            }
        }

        file.Close();
        return time;
    }

    public static int ErrorNumber()
    {
        // read mistakes from gameData.ini
        int errors = 0;
        string line;
        StreamReader file = new StreamReader(path);

        while ((line = file.ReadLine()) != null)
        {
            string[] word = line.Split(':');
            if (word[0] == "#errors")
            {
                int.TryParse(word[1], out errors);
            }
        }

        file.Close();
        return errors;
    }

    // check if gameData.ini exists
    public static bool GameFileExist()
    {
        return File.Exists(path);
    }
}