
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
public class GameManager : MonoBehaviour
{
    public GameObject tarjetaPrefab;
    public Transform tablero;
    public List<Sprite> imagenes; // 6 imágenes

    private List<Sprite> imagenesParaTablero = new List<Sprite>();

    // Nueva lista para los IDs de personajes
    public List<string> personajes; // Ejemplo: "Killa", "Knight", ...  
    private List<string> personajesParaTablero = new List<string>();

    public MovimientoTarjeta primerTarjeta;
    public MovimientoTarjeta segundaTarjeta;

    public float tiempoEspera = 0.5f;
    public bool comparando = false;
    public bool coincidenciaEncontrada = false;

    public int filas = 2;
    public int columnas = 6;
    public float separacion = 1.2f;

    public TextMeshProUGUI score;
    private int puntuacionActual = 0;

    public TextMeshProUGUI timerText;

    public int errores = 0;
    private float tiempoRestante = 60f; // Tiempo en segundos



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
    }





    public static GameManager Instance { get; private set; }

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
        }
        puntuacionActual = 0;
        score.text = puntuacionActual.ToString();


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
        
        puntuacionActual = int.Parse(score.text);
        puntuacionActual += puntos;
        score.text = puntuacionActual.ToString();
        
    }







}






