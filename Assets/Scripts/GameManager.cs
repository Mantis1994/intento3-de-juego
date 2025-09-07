
using UnityEngine;
using System.Collections.Generic;
public class GameManager : MonoBehaviour
{
    public GameObject tarjetaPrefab;
    public Transform tablero;
    public List<Sprite> imagenes; // 6 imágenes

    private List<Sprite> imagenesParaTablero = new List<Sprite>();

    public int filas = 2;
    public int columnas = 6;
    public float separacion = 1.2f;

    void Start()
    {
        PrepararImagenes();
        SpawnearTablero();
    }

    void PrepararImagenes()
    {
        imagenesParaTablero.Clear();
        // Repetir cada imagen 2 veces
        foreach (var img in imagenes)
        {
            imagenesParaTablero.Add(img);
            imagenesParaTablero.Add(img);
        }

        // Mezclar la lista
        for (int i = 0; i < imagenesParaTablero.Count; i++)
        {
            Sprite temp = imagenesParaTablero[i];
            int randomIndex = Random.Range(i, imagenesParaTablero.Count);
            imagenesParaTablero[i] = imagenesParaTablero[randomIndex];
            imagenesParaTablero[randomIndex] = temp;
        }
    }

    void SpawnearTablero()
    {
        int indiceImagen = 0;

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
                // mt.dorsoTarjeta = ... // Asigna aquí el dorso si lo necesitas
                mt.sr = carta.GetComponent<SpriteRenderer>();
                indiceImagen++;
            }
        }
    }
}






