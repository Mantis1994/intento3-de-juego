using UnityEngine;

public class MovimientoTarjeta : MonoBehaviour
{


    public GameObject tarjeta;

    public float speedRotation = 10f;

    public bool girar = false;

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
            if (girar == false)
            {
                tarjeta.transform.localScale += new Vector3(0f, -(1.5006f * 2), 0f);
                girar = true;
            }
            else if (girar == true)
            {
                tarjeta.transform.localScale += new Vector3(0f, (1.5006f * 2), 0f);
                girar = false;
            }
        }

    }
}
