using UnityEngine;

public class MovimientoTarjeta : MonoBehaviour
{


    public GameObject tarjeta;

    public float speedRotation = 10f;

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
            tarjeta.transform.Rotate(0, 180, 0);
        }

    }
}
