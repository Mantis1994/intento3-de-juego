
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public GameObject tarjetaPrefab;
    public Transform tablero;
    public List<Sprite> imagenes; // 6 imágenes

    private List<Sprite> imagenesParaTablero = new List<Sprite>();

    // Nueva lista para los IDs de personajes
    public List<string> personajes; // Ejemplo: "Killa", "Knight", ...  
    private List<string> personajesParaTablero = new List<string>();

    private List<GameObject> tarjetasEnTablero = new List<GameObject>();

    public Button reiniciarButton;

    public MovimientoTarjeta primerTarjeta;
    public MovimientoTarjeta segundaTarjeta;

    public float tiempoEspera = 0.5f;
    public bool comparando = false;
    public bool coincidenciaEncontrada = false;

    public bool inGame = true;

    public int filas = 2;
    public int columnas = 6;
    public float separacion = 1.2f;

    public TextMeshProUGUI score;
    private int puntuacionActual = 0;

    public TextMeshProUGUI timerText;
    public int errores = 0;
    private float tiempoRestante = 60f; // Tiempo en segundos

    public TextMeshProUGUI scoreFinal;

    private void Update()
    {
        if (tiempoRestante > 0)
        {

            tiempoRestante -= Time.deltaTime;

            timerText.text = Mathf.Ceil(tiempoRestante).ToString();
        }
        else
        {
            // Tiempo agotado
            timerText.text = "0";
            // Aquí puedes agregar la lógica para manejar el fin del tiempo
        }




        if (tiempoRestante == 0 && comparando == false && inGame == false)
        {
            MostrarScoreFinal(puntuacionActual, errores);
        }
    }





    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        puntuacionActual = 0;
        score.text = puntuacionActual.ToString();
        tiempoRestante = 60f;
        inGame = true;
        scoreFinal.text = "";
        errores = 0;
        primerTarjeta = null;
        segundaTarjeta = null;
        comparando = false;
        coincidenciaEncontrada = false;
        tarjetasEnTablero.Clear();



        scoreFinal.gameObject.SetActive(false);


        PrepararImagenes();
        SpawnearTablero();
    }

    void PrepararImagenes()
    {
        if (imagenes.Count != personajes.Count)
        {
            Debug.LogError("La cantidad de imágenes y personajes debe ser igual.");
            return;
        }
        imagenesParaTablero.Clear();
        personajesParaTablero.Clear();
        // Repetir cada imagen 2 veces
        for (int i = 0; i < imagenes.Count; i++)
        {
            imagenesParaTablero.Add(imagenes[i]);
            imagenesParaTablero.Add(imagenes[i]);
            personajesParaTablero.Add(personajes[i]);
            personajesParaTablero.Add(personajes[i]);
        }


        for (int i = 0; i < imagenesParaTablero.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, imagenesParaTablero.Count);

            // Mezclar imagenes
            Sprite tempImg = imagenesParaTablero[i];
            imagenesParaTablero[i] = imagenesParaTablero[randomIndex];
            imagenesParaTablero[randomIndex] = tempImg;

            // Mezclar personajes
            string tempID = personajesParaTablero[i];
            personajesParaTablero[i] = personajesParaTablero[randomIndex];
            personajesParaTablero[randomIndex] = tempID;
        }
    }

    void SpawnearTablero()
    {
        int indiceImagen = 0;
        int totalCartas = filas * columnas;
        if (totalCartas != imagenesParaTablero.Count)
        {
            Debug.LogError("La cantidad de cartas a crear (" + totalCartas + ") no coincide con la cantidad de imágenes/personajes preparados (" + imagenesParaTablero.Count + ").");
            return;
        }

        // Calcular el offset para centrar el tablero
        float offsetX = (columnas - 1) * separacion / 2f;
        float offsetY = (filas - 1) * separacion / 2f;

        for (int fila = 0; fila < filas; fila++)
        {
            for (int columna = 0; columna < columnas; columna++)
            {
                Vector3 posicion = new Vector3(
                    columna * separacion - offsetX,
                    -fila * separacion + offsetY,
                    0
                );
                GameObject carta = Instantiate(tarjetaPrefab, posicion, Quaternion.identity, tablero);

                // Asignar el sprite correspondiente a la carta
                MovimientoTarjeta mt = carta.GetComponent<MovimientoTarjeta>();
                mt.frenteTarjeta = imagenesParaTablero[indiceImagen];

                mt.sr = carta.GetComponent<SpriteRenderer>();
                mt.personajeID = personajesParaTablero[indiceImagen]; // <--- NUEVO
                indiceImagen++;
            }
        }
        // Almacenar todas las tarjetas en la lista
        tarjetasEnTablero.AddRange(GameObject.FindGameObjectsWithTag("Tarjeta"));
    }


    public void CompararTarjetas(MovimientoTarjeta tarjeta)
    {
        if (primerTarjeta == null)
        {
            primerTarjeta = tarjeta;
        }
        else if (segundaTarjeta == null && tarjeta != primerTarjeta)
        {
            segundaTarjeta = tarjeta;
            comparando = true;
            StartCoroutine(VerificarCoincidencia());

        }
    }


    IEnumerator<WaitForSeconds> VerificarCoincidencia()
    {

        yield return new WaitForSeconds(tiempoEspera);

        if (primerTarjeta.frenteTarjeta == segundaTarjeta.frenteTarjeta)
        {
            // Coincidencia encontrada
            coincidenciaEncontrada = true;
            AudioManager.Instance.PlaySFX(primerTarjeta.personajeID); // Reproduce la voz del personaje
            ActualizarScore(10); // Sumar puntos por coincidencia

            // Destruir las tarjetas coincidentes
            Destroy(primerTarjeta.gameObject);
            Destroy(segundaTarjeta.gameObject);
            tarjetasEnTablero.Remove(primerTarjeta.gameObject);
            tarjetasEnTablero.Remove(segundaTarjeta.gameObject);

            // Verificar si quedan tarjetas en el tablero
            if (tarjetasEnTablero.Count == 0)
            {
                MostrarScoreFinal(puntuacionActual, errores);
                inGame = false;

            }
        }
        else
        {
            // No hay coincidencia, voltear las tarjetas de nuevo
            coincidenciaEncontrada = false;
            errores++;
            primerTarjeta.Ocultar();
            segundaTarjeta.Ocultar();
        }

        primerTarjeta = null;
        segundaTarjeta = null;
        comparando = false;

    }


    public void ActualizarScore(int puntos)
    {

        int.TryParse(score.text, out puntuacionActual);
        puntuacionActual += puntos;
        score.text = puntuacionActual.ToString();

    }


    public void MostrarScoreFinal(int score, int errores)
    {
        int scoreFinal = score - (errores * 5); // Ejemplo: restar 5 puntos por cada error
        this.scoreFinal.text = $"Score: {score} \n Errores: {errores} \n Score final: {scoreFinal}";
        this.scoreFinal.gameObject.SetActive(true);
    }


    public void ReiniciarJuego()
    {
        // Recargar la escena actual para reiniciar el juego
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }






}






