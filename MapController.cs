using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;

public class MapController : MonoBehaviour
{
    public GameObject LevelButton;
    public void LoadGame(string map, string level, bool locked) {
        if (locked)
        {
            return;
        }
        PlayerPrefs.SetString("MapGroup", map);
        PlayerPrefs.SetString("Level", level);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Start()
    {
        Texture2D[] levels =  Resources.LoadAll<Texture2D>("Maps/" + this.name + "/Levels");
        int len = levels.Length;
        if (len > 18) {
            Debug.LogError("In one map, we create less than 18 levels for the best visual. Some level will be ignored");
            len = 18;
        }
        // Load data
        List<SaveLevelInfo> data = LoadData(len, levels);
        for (int i = 0; i < len; i++) {
            GameObject level = Instantiate(LevelButton, Vector3.zero, Quaternion.identity);
            level.transform.SetParent(transform);
            level.name = levels[i].name;
            // add event listener when clicked
            bool locked = new bool(); locked =  data[i].locked;
            level.GetComponent<Button>().onClick.AddListener(delegate { LoadGame(this.name, level.name, locked); });
            Image[] child = level.GetComponentsInChildren<Image>();

            level.GetComponentInChildren<Text>().text = (i + 1).ToString();
            // child 1,2,3 = star 1,2,3 image and  child 4 = locked image
            for (int j = 6; j > data[i].star + 3; j--) {
                child[j].gameObject.SetActive(false);
            }
            if (data[i].locked == false) {
                child[7].gameObject.SetActive(false);
            }
            else
            {
                level.GetComponentInChildren<Text>().gameObject.SetActive(false);
            }
        }
    }
    private List<SaveLevelInfo> LoadData(int length, Texture2D[] levels) {
        string filePath = Application.persistentDataPath + gameObject.name + "LevelInfomations";
        if (File.Exists(filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fi = File.Open(filePath, FileMode.Open);
            List<SaveLevelInfo> data = (List<SaveLevelInfo>)bf.Deserialize(fi);
            // if num of level not equal in data.
            if (data.Count < length)
            {
                for (int i = data.Count; i < length; i++)
                {
                    SaveLevelInfo ele = new SaveLevelInfo();
                    ele.name = levels[i].name;
                    data.Add(ele);
                }
                // first level will be unlocked.
                if (data[0].locked == true)
                {
                    data[0].locked = false;
                }
                // create a new file and save data for it
                fi.Close();
                File.Delete(filePath);
                FileStream fi1 = File.Create(filePath);
                bf.Serialize(fi1, data);
            }
            else if (data.Count > length)
            {
                bool find = false;
                for (int i = 0; i < data.Count; i++)
                {
                    find = false;
                    for (int j = 0; j < length; j++)
                    {
                        if (data[i].name == levels[j].name)
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        data.RemoveAt(i);
                    }
                }
                // first level will be unlocked.
                if (data[0].locked == true)
                {
                    data[0].locked = false;
                }
                fi.Close();
                // create a new file and save data for it
                File.Delete(filePath);
                FileStream fi1 = File.Create(filePath);
                bf.Serialize(fi1, data);
            }
            else {
                // first level will be unlocked.
                if (data[0].locked == true)
                {
                    data[0].locked = false;
                }
                fi.Close();
            }
            return data;
        }
        else {
            // if not exist data file, we will create the file and add data for each level
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fi = File.Create(filePath);
            List<SaveLevelInfo> data = new List<SaveLevelInfo>();
            for (int i = 0; i < length; i++) {
                SaveLevelInfo ele = new SaveLevelInfo();
                ele.name = levels[i].name;
                data.Add(ele);
            }
            // first level will be unlocked.
            if (data[0].locked == true)
            {
                data[0].locked = false;
            }
            bf.Serialize(fi, data);
            fi.Close();
            return data;
        }
    }
    [Serializable]
    // this class stores data at each level.
    public class SaveLevelInfo
    {
        public string name;
        public bool locked;
        // star from 1 to 3
        [Range(0, 4)]
        public int star;

        public SaveLevelInfo()
        {
            name = "";
            locked = true;
            star = 0;
        }
    }
}


