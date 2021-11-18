using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    public string savedName;
    public string currentName = "Name";
    public int bestScore = 0;
    public TMP_InputField nameText;
    public TextMeshProUGUI bestScoreText;

    private void Awake()
    {
        //File.Delete(Application.persistentDataPath + "/savefile.json");
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInfo();
        bestScoreText.text = "Best Score: " + savedName + ": " + bestScore;
    }

    [System.Serializable]
    class SaveData
    {
        public string name;
        public int bestScore;
    }

    public void SaveInfo()
    {
        SaveData data = new SaveData();
        data.name = savedName;
        data.bestScore = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadInfo()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            
            savedName = data.name;
            bestScore = data.bestScore;
        }
    }

    public void OnStartClick()
    {
        currentName = nameText.text;
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif

        SaveInfo();
    }
}
