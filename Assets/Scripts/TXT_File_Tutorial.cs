// This script was created for learning purposes of how to generate .txt files from Unity, it has no functional purpose in this project.
// Tutorial link: https://youtu.be/6bVcLSZWqK8
using UnityEngine;
using System.IO;

public class TXT_File_Tutorial : MonoBehaviour
{
    void CreateText()
    {
        // Path of the file
        string path = Application.dataPath + "/Log.txt";
        // Create File if it doesn't exist
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "Login log \n\n");
        }
        // Content of the file
        string content = "Login date:" + System.DateTime.Now + "\n";
        // Add some text to it.
        File.AppendAllText(path, content);
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateText();
    }
}
