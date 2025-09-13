using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
    }




    public void IniciarJuego()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}
