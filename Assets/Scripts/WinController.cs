using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using static MapController;

public class WinController : MonoBehaviour
{
    public Text Score;

    private Animator anim;
    public void victory(int Score, int Star) {
        anim = GetComponent<Animator>();
        this.Score.text = Score.ToString();
        if (Star < 1 || Star > 3) {
            Debug.Log("something wrong when star reference");
        }
        gameObject.SetActive(true);
        // play animation
        anim.Play("LvComplete" + Star.ToString() + "S");
        saveInfo(Star);
    }

    private void saveInfo(int star) {
        string mapGroup = PlayerPrefs.GetString("MapGroup");
        string level = PlayerPrefs.GetString("Level");
        string filepath = Application.persistentDataPath + mapGroup + "LevelInfomations";
        if (File.Exists(filepath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fi = File.Open(filepath, FileMode.Open);
            List<SaveLevelInfo> data = new List<SaveLevelInfo>();
            data = (List<SaveLevelInfo>)bf.Deserialize(fi);
            for (int i = 0; i < data.Count - 1; i++) {
                if (data[i].name == level) {
                    data[i].star = star;
                    data[i + 1].locked = false;
                    // store data
                    fi.Close();
                    File.Delete(filepath);
                    FileStream fi1 = File.Create(filepath);
                    bf.Serialize(fi1, data);
                    fi1.Close();
                    break;
                }
            }
        }
        else {
            Debug.LogError("some thing wrong when getting the data file path");
        }
    }
    public void BackToHome() {
        PlayerPrefs.SetInt("played", 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
