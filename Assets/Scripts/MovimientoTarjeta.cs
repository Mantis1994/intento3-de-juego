using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;

public class MovimientoTarjeta : MonoBehaviour
{


    public GameObject tarjeta;

    public SpriteRenderer sr;

    public Sprite dorsoTarjeta;
    public Sprite frenteTarjeta;

    public List<Sprite> listaFrentes;


    public bool girando = false;
    public bool volteada = false;

    public float duracion = 0.3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = tarjeta.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0)&&!girando)
        {
            StartCoroutine(FlipTarjeta());
        }

    }




    // EL ENUMERATOR ES PARA HACER EL FLIP DE LA TARJETA, ADEMAS CUANDO LLEGA A LA MITAD SE CAMBIA EL SPRITE POR LA DEL DORSODE LA TARJETA
    IEnumerator FlipTarjeta()
    {
        girando = true;
        volteada = !volteada;
        Vector2 escalaInicial = transform.localScale;
        Vector2 escalaIntermedia = new Vector2(0f, escalaInicial.y); ;
        Vector2 escalaFinal = new Vector2(escalaInicial.x * -1, escalaInicial.y);

        float tiempo = 0;

        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            transform.localScale = Vector2.Lerp(escalaInicial, escalaIntermedia, tiempo / (duracion / 2));
            yield return null;
        }


        

        if (volteada ==false)
        {
            sr.sprite = dorsoTarjeta;

        }
        else
        {
            sr.sprite = listaFrentes[Random.Range(0, listaFrentes.Count)];
        }



        tiempo = 0;

        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            transform.localScale = Vector2.Lerp(escalaIntermedia, escalaFinal, tiempo / (duracion / 2));
            yield return null;
        }

        girando = false;

    }
}
