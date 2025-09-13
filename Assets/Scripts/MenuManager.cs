using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }




    public TMP_InputField usuarioText; // Asigna desde el inspector
    public static string nombreUsuario; // Variable estática para guardar el nombre


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
        nombreUsuario = usuarioText.text;
        if (string.IsNullOrEmpty(nombreUsuario))
        {
            Debug.Log("Por favor, ingresa un nombre.");
            return;
        }
        SceneManager.LoadScene("GameScene");
    }

    public void SalirJuego()
    {
        Application.Quit();
        Debug.Log("Salir del juego"); // Solo para ver en el editor que se llama a la función
    }


    public void ReiniciarJuego()
    {
        // Recargar la escena actual para reiniciar el juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    



    void IngresarUsuario()
    {
        // Lógica para ingresar el usuario
        Debug.Log("Usuario ingresado");
    }
}
