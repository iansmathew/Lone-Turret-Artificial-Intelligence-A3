using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuScript : MonoBehaviour {

	public void ShowMenu()
    {
        SceneManager.LoadScene(0);
    }
}
