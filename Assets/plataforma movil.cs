using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

/*public class PlataformaMovil : MonoBehaviour
{
    private float tiempo;
    public float velocidad = 2f;
    public Transform pointA;
    public Transform pointB;
    private Vector3 lastPosition;
    public GameObject target;

    private bool waiting = false;
    private float delay = 2f;

    private void Update()
    {
        
        
          PingPongMovement();
        

    }

    public void PingPongMovement()
    {
        float t = Mathf.PingPong(Time.deltaTime * velocidad, 1f);
        Vector3 newPosition = Vector3.Lerp(pointA.position, pointB.position, t);
        transform.position = newPosition;

        float distancia = Vector3.Distance(transform.position, target.transform.position); //disdtance entre los dos puntos
        Vector3 direccion = (target.transform.position - transform.position).normalized;

        if (distancia > 0.1f)
        {
            transform.position += direccion * velocidad * Time.deltaTime;
        }

        else
        {
            transform.position = target.transform.position;
        }

        if (transform.position.magnitude > 0.1f && transform.position != lastPosition) //change appereance
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }

        lastPosition = transform.position;  
    } 
     
    public void Aumentartiempo()
    {   
        tiempo += Time.deltaTime * velocidad;        
        tiempo = Mathf.Clamp01(tiempo);
    } 
}*/

/*public class PlataformaMovil : MonoBehaviour
{
    private float tiempo;
    public float velocidad = 2f;
    public Transform pointA;
    public Transform pointB;
    private bool waiting = false;
    private float delay = 2f;
    private Vector3 lastPosition;
    public GameObject target;
    

    private void Update()
    {      
            PingPongMovement();     
    }

    public void PingPongMovement()
    {       
        tiempo += Time.deltaTime * velocidad;
        float t = Mathf.PingPong(tiempo, 1f);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

        float distancia = Vector3.Distance(transform.position, target.transform.position); //disdtance entre los dos puntos
        Vector3 direccion = (target.transform.position - transform.position).normalized; // normalized hace que no se salga entre zero y uno

        if (distancia > 0.1f)
        {
            transform.position += direccion * velocidad * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.red;
          
        }
        else
        {
            transform.position = target.transform.position;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public void Aumentartiempo()
    {
        tiempo += Time.deltaTime * velocidad;
        tiempo = Mathf.Clamp01(tiempo);
    }

}*//// <summary>


public class PlataformaMovil : MonoBehaviour
{
    private float tiempo;
    public float velocidad = 1f;
    public Transform pointA;
    public Transform pointB;
    private bool waiting = false;
    private float delay = 1f;
    private Vector3 lastPosition;
    private bool forward = true;


    private void Update()
    {
        if (!waiting)
        {
            PingPongMovement();
        }

    }

    public void PingPongMovement()
    {

        if (forward)
        {
            tiempo += Time.deltaTime * velocidad;

        }
        else
        {
            tiempo -= Time.deltaTime * velocidad;

        }


        float t = Mathf.PingPong(tiempo, 2f);
        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);

        if (Vector3.Distance(transform.position, pointB.position) < 0.1f)
        {

            StartCoroutine(WaitAtPointB());


        }
        else if (Vector3.Distance(transform.position, pointA.position) < 0.1f && !forward)
        {
            forward = true;

        }
    }

    private IEnumerator WaitAtPointB()
    {
        waiting = true;
        yield return new WaitForSeconds(delay); 
        forward = false;
        waiting = false;

    }

}
