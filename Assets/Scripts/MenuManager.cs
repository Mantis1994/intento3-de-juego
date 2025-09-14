using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    int max1, max2, max3;
    string user1, user2, user3;

    public TextMeshProUGUI top3Puntajes; // Asigna desde el inspector



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
        if (SceneManager.GetActiveScene().name == "MenuScene")
        {
            MostrarTop3();
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

    public void InicioMenu()
    {
        SceneManager.LoadScene("MenuScene");
    }




    void IngresarUsuario()
    {
        // Lógica para ingresar el usuario
        Debug.Log("Usuario ingresado");
    }




    public void GuardarTop3(int nuevoScore, string nuevoUsuario)
{
    // Leer los actuales
    int max1 = PlayerPrefs.GetInt("Max1", 0);
    int max2 = PlayerPrefs.GetInt("Max2", 0);
    int max3 = PlayerPrefs.GetInt("Max3", 0);

    string user1 = PlayerPrefs.GetString("Name1", "Nadie");
    string user2 = PlayerPrefs.GetString("Name2", "Nadie");
    string user3 = PlayerPrefs.GetString("Name3", "Nadie");

    if (nuevoScore > max1)
    {
        // Desplazar todos
        PlayerPrefs.SetInt("Max3", max2);
        PlayerPrefs.SetString("Name3", user2);

        PlayerPrefs.SetInt("Max2", max1);
        PlayerPrefs.SetString("Name2", user1);

        PlayerPrefs.SetInt("Max1", nuevoScore);
        PlayerPrefs.SetString("Name1", nuevoUsuario);
    }
    else if (nuevoScore > max2)
    {
        // Desplazar segundo al tercero
        PlayerPrefs.SetInt("Max3", max2);
        PlayerPrefs.SetString("Name3", user2);

        PlayerPrefs.SetInt("Max2", nuevoScore);
        PlayerPrefs.SetString("Name2", nuevoUsuario);
    }
    else if (nuevoScore > max3)
    {
        PlayerPrefs.SetInt("Max3", nuevoScore);
        PlayerPrefs.SetString("Name3", nuevoUsuario);
    }

    PlayerPrefs.Save();
}
    public void MostrarTop3()
    {
        int max1 = PlayerPrefs.GetInt("Max1", 0);
        int max2 = PlayerPrefs.GetInt("Max2", 0);
        int max3 = PlayerPrefs.GetInt("Max3", 0);

        string user1 = PlayerPrefs.GetString("Name1", "Nadie");
        string user2 = PlayerPrefs.GetString("Name2", "Nadie");
        string user3 = PlayerPrefs.GetString("Name3", "Nadie");


        top3Puntajes.text = $"1. {user1}   {max1}\n2. {user2}   {max2}\n3. {user3}   {max3}";

    }



}
