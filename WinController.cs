using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        Debug.Log("LvComplete" + Star.ToString() + "S");
        anim.Play("LvComplete" + Star.ToString() + "S");
    }
    public void BackToHome() { 
        // go back to home screen
    }

    public void Restart() {
        // save infor
        // Todo:
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
