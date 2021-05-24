using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVManager : MonoBehaviour
{
    private static string fileFolderName = "Saved Data";
    private static string separator = ",";
    private static string[] fileHeaders = new string[3]
    {
        "Trial number", 
        "Result",
        "Time",
    };

    static void verifyDirectory()
    {
        string directory = getDirPath();
        if (!Directory.Exists(directory)) 
        {
            Directory.CreateDirectory(directory);
        }
    }

    static void verifyFile(string fileName)
    {
        string file = getFilePath(fileName);
        if (!File.Exists(file))
        {
            createFile(fileName);
        }
    }

    static string getDirPath()
    {
        return Application.dataPath + "/" + fileFolderName;
    }
    
    static string getFilePath(string fileName)
    {
        return getDirPath() + "/" + fileName;
    }

    public static void createFile(string fileName)
    {
        verifyDirectory();
        using (StreamWriter streamWriter = File.CreateText(getFilePath(fileName)))
        {
            string fileEntry = "";
            for(int i = 0; i < fileHeaders.Length; i++)
            {
                if(fileEntry != "")
                {
                    fileEntry += separator;
                }
                fileEntry += fileHeaders[i];
            }
            streamWriter.WriteLine(fileEntry);
        }
    }

    public static void appendtoFile(string fileName, string[] entries)
    {
        verifyDirectory();
        verifyFile(fileName);
        using (StreamWriter streamWriter = File.AppendText(getFilePath(fileName)))
        {
            string fileEntry = "";
            for (int i = 0; i < fileHeaders.Length; i++)
            {
                if (fileEntry != "")
                {
                    fileEntry += separator;
                }
                fileEntry += entries[i];
            }
            streamWriter.WriteLine(fileEntry);
        }
    }
}
