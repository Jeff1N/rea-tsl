using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour {

	public void Back() {
        SceneManager.LoadScene("MainMenu");
    }
}
