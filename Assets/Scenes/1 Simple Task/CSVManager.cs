using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CSVManager : MonoBehaviour
{
    private static string fileFolderName = "Saved Data";
    private static string fileName = "UserDetails.csv";
    private static string separator = ",";
    private static string[] fileHeaders = new string[6]
    {
        "Last Name", 
        "First Name",
        "Course",
        "Matriculation Nr.",
        "Correct Attempts",
        "Incorrect Attempts"
    };

    static void verifyDirectory()
    {
        string directory = getDirPath();
        if (!Directory.Exists(directory)) 
        {
            Directory.CreateDirectory(directory);
        }
    }

    static void verifyFile()
    {
        string file = getFilePath();
        if (!File.Exists(file))
        {
            createFile();
        }
    }

    static string getDirPath()
    {
       
        return Application.dataPath + "/" + fileFolderName;
    }
    
    static string getFilePath()
    {
        return getDirPath() + "/" + fileName;
    }

    public static void createFile()
    {
        verifyDirectory();
        using (StreamWriter streamWriter = File.CreateText(getFilePath()))
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

    public static void appendtoFile(string[] entries)
    {
        verifyDirectory();
        verifyFile();
        using (StreamWriter streamWriter = File.AppendText(getFilePath()))
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
