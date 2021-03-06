using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setting : MonoBehaviour
{
    public void clearData() { 
        string filepath = Application.persistentDataPath + "Map 1" + "LevelInfomations";
        File.Delete(filepath);
    }
}
