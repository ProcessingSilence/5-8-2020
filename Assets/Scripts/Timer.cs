using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.IO;

public class Timer : MonoBehaviour
{
    private float timeUsed;
    float displayNumber;
    public int levelWon;
    public int deaths;
    public string sceneName;
    private string tempSceneName;
    private string path;
    public Scene getSceneName;
    public Vector2 GUICoords;
    
    public float totalTime;
    public float totalDeaths;
    
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        getSceneName = SceneManager.GetActiveScene();
        sceneName = getSceneName.name;
        path = Application.dataPath + "/PlayerLog.txt";
        if (!File.Exists(path))
        {
            File.WriteAllText(path, "***** Player Log Info ***** \n\n");
            Debug.Log(Application.dataPath + "/PlayerLog.txt" + "has been created successfully");
        }
        File.AppendAllText(path, "Login date: " + System.DateTime.Now + "\n");
        Debug.Log("Date of: " + System.DateTime.Now);
    }
    void Update()
    {
        switch (levelWon)
        {
            case 0:
                timeUsed += Time.deltaTime;
                displayNumber = timeUsed;
                break;
            case 1:
                levelWon = 5;
                if (sceneName != "ending")
                {
                    File.AppendAllText(path, "Level: " + sceneName + "\n Time:  " + timeUsed + "\n Deaths: " + deaths + "\n\n");
                    Debug.Log("Level info logged successfully.");
                    Debug.Log("Level: " + sceneName + "\n Time:  " + timeUsed + "\n Deaths: " + deaths + "\n\n");
                    totalTime += timeUsed;
                    totalDeaths += deaths;
                }
                if (sceneName == "Level 9")
                {
                    File.AppendAllText(path, "\n GAME COMPLETE! \nTotal Time: " + totalTime + " Total Deaths: " + totalDeaths);
                }
                levelWon = 2;
                break;
            case 2:
                getSceneName = SceneManager.GetActiveScene();
                tempSceneName = getSceneName.name;
                if (sceneName != tempSceneName)
                {
                    levelWon = 4;
                    Debug.Log("New scene detected: " + getSceneName.name);
                    sceneName = getSceneName.name;
                    timeUsed = 0;
                    deaths = 0;
                    levelWon = 0;                   
                }		              
                break;
        }
    }
	
    void OnGUI()
    {         
        GUI.Label(new Rect(GUICoords.x, GUICoords.y, 200, 100), "Time In Level : " + displayNumber.ToString("F2"));   
    }
}