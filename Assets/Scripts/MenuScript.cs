using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void goToAutoScene()
    {
        SceneManager.LoadScene("auto");
    }

    public void goToManualScene()
    {
        SceneManager.LoadScene("manual");
    }
}
