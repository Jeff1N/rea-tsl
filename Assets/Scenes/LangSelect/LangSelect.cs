using UnityEngine;
using UnityEngine.SceneManagement;

public class LangSelect : MonoBehaviour {
    
    public void Select (int language) {
        PlayerPrefs.SetInt("lang", language);
        SceneManager.LoadScene("MainMenu");
    }
}
