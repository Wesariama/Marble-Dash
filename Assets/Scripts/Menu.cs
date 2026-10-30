
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu: MonoBehaviour
{
    public string action;   // "play" или "quit"

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (action == "play")
        {
            SceneManager.LoadScene("Game");
        }
        else if (action == "quit")
        {
            Application.Quit();
        }
    }
}
