using System.Collections;
using UnityEngine;

public class MovimientoTarjeta : MonoBehaviour
{


    public GameObject tarjeta;


    public bool girar = false;

    public float duracion = 0.3f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
           StartCoroutine(FlipTarjeta());
        }

    }





    IEnumerator FlipTarjeta()
    {
        girar = true; ;

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

        tiempo = 0;

        while (tiempo < duracion / 2)
        {
            tiempo += Time.deltaTime;
            transform.localScale = Vector2.Lerp(escalaIntermedia, escalaFinal, tiempo / (duracion / 2));
            yield return null;
        }

        girar = false;

    }
}
