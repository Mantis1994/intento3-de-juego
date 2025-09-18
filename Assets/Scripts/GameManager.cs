
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public MovimientoTarjeta tarjetaScript;
    public MovimientoTarjeta primerTarjeta;
    public MovimientoTarjeta segundaTarjeta;




    public GameObject tarjetaPrefab;
    public GameObject menuPausa;

    public GameObject explosionPrefab;
    public Transform tablero;





    public List<Sprite> imagenes; // 6 imágenes
    private List<Sprite> imagenesParaTablero = new List<Sprite>();
    // Nueva lista para los IDs de personajes
    public List<string> personajes; // Ejemplo: "Killa", "Knight", ...  
    private List<string> personajesParaTablero = new List<string>();
    private List<GameObject> tarjetasEnTablero = new List<GameObject>();


    public Button reiniciarButton;



    public bool juegoPausado = false;
    public bool comparando = false;
    public bool coincidenciaEncontrada = false;
    public bool algunaGirando = false;
    public bool inGame = true;








    public int errores = 0;
    private int puntuacionActual = 0;
    public int filas = 2;
    public int columnas = 6;
    public float separacion = 1.2f;
    public float tiempoEspera = 0.5f;
    private float tiempoRestante = 60f; // Tiempo en segundos







    public TextMeshProUGUI score;
    public TextMeshProUGUI usuarioText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreFinal;



    private void Update()
    {
        if (tiempoRestante > 0 && inGame == true)
        {

            tiempoRestante -= Time.deltaTime;

            timerText.text = Mathf.Ceil(tiempoRestante).ToString();
        }
        else
        {
            // Tiempo agotado
            timerText.text = "0";

        }
        // Mostrar la puntuación final cuando el tiempo se agota
        if (tiempoRestante == 0 && comparando == false && inGame == false)
        {
            MostrarScoreFinal(puntuacionActual, errores);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inGame == true)
        {


            AlternarPausa();


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
        usuarioText.text = MenuManager.nombreUsuario;
        tarjetaScript = tarjetaPrefab.GetComponent<MovimientoTarjeta>();





        scoreFinal.gameObject.SetActive(false);


        PrepararImagenes();
        SpawnearTablero();
    }


    void Start()
    {
        PlayMusic();
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

            var explosion1 = Instantiate(explosionPrefab, primerTarjeta.transform.position, Quaternion.identity);
            var explosion2 = Instantiate(explosionPrefab, segundaTarjeta.transform.position, Quaternion.identity);

            // Ocultar los sprites antes de destruir las tarjetas

           
           

            Destroy(explosion1,0.7f);
            Destroy(explosion2,0.7f);

            Destroy(primerTarjeta.gameObject);
            Destroy(segundaTarjeta.gameObject);
            AudioManager.Instance.EfectoDeSonido("Acierto");



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
            Debug.Log("ANTES DE FUNCIÓN OCULTAR");
            primerTarjeta.Ocultar();
            segundaTarjeta.Ocultar();
            Debug.Log("despúes DE FUNCIÓN OCULTAR");
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
        this.score.gameObject.SetActive(false);
        this.timerText.gameObject.SetActive(false);
        this.scoreFinal.gameObject.SetActive(true);
        MenuManager.Instance.GuardarTop3(scoreFinal, MenuManager.nombreUsuario);
    }



    public void PlayMusic()
    {



        AudioManager.Instance.musicSource.loop = true;
        AudioManager.Instance.musicSource.clip = AudioManager.Instance.musicaInGame;
        AudioManager.Instance.musicSource.Play();
        Debug.Log("Reproduciendo música de fondo.");


    }


    public void VolverAlInicio()
    {
        SceneManager.LoadScene("MenuScene");
    }


    public void AlternarPausa()
    {


        if (juegoPausado == false)
        {
            menuPausa.SetActive(true);
            Time.timeScale = 0f; // Pausar el juego
            AudioManager.Instance.musicSource.Pause();
            juegoPausado = !juegoPausado;
        }
        else
        {
            menuPausa.SetActive(false);
            Time.timeScale = 1f; // Reanudar el juego
            AudioManager.Instance.musicSource.UnPause();
            juegoPausado = !juegoPausado;
        }

    }


    public bool PuedeGirarTarjeta(MovimientoTarjeta tarjeta)
    {
        // Solo permite girar si no se está comparando y hay menos de 2 tarjetas giradas
        if (comparando) return false;
        if (primerTarjeta == null) return true;
        if (segundaTarjeta == null && tarjeta != primerTarjeta) return true;
        return false;
    }


}






