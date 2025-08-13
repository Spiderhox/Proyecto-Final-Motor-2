using System.Collections;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    //ping pong movement no funciono, cuando se chocaba, el enemigo patinaba, tuve que googlearlo
    //se quito = estado enemigos, vision, capa jugador y obstaculos, estado enemigo, sospecha
    //objetico jugador , on alerta, player layer, detectar jugador, ir a punto sospecha, perseguir
    //switch, volver a patrullar, esperar y patrullar, recibir alerta
    //ayuda de internet
    //rb velocity supuestamente si finciona para los area effector???
    //le quite lo del capa jugador porque como el jugador empieza el juego en otro cuarto, el codigo no lo encuentra y se despicha 


    public Transform[] puntosPatrulla;
    public float velocidad = 2f;
    private int indicePuntoActual = 0;

    private bool Adelante = true; 
    //trackea el si es que va pa delante, true, o si va para detras, false, se toggle cuando
    //el patrulero llega al final del camino 

    private void Start()
    {
        
        StartCoroutine(Patrullar());
    }

    private IEnumerator Patrullar()
    {
        while (true)
        {
            Vector3 destino = puntosPatrulla[indicePuntoActual].position;

                while (Vector3.Distance(transform.position, destino) > 0.1f)
                {
                    transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                    yield return null;
                }

            // Wait before moving to next point
            yield return new WaitForSeconds(1f);

            // Update the patrol index based on direction
            //en vez de siempre incrementar el indice de direccion, se samcia entre ++ y --
            //basandose en el bool adelante, tons cuando se llega al final, simplemente se hace el cambio
            //y cuando se llega al inicio, se cambia hacia Adelante
            if (Adelante)
            {
                indicePuntoActual++;
                if (indicePuntoActual >= puntosPatrulla.Length)
                {
                    indicePuntoActual = puntosPatrulla.Length - 2;
                    Adelante = false;
                }
            }

            else
            {
                indicePuntoActual--;
                if (indicePuntoActual < 0)
                {
                    indicePuntoActual = 1;
                    Adelante = true;
                }
            }

            // el -2 y 1 funcionan para que no se tome el mismo point does veces, no entendi muy bien como pero eso hace 
                     
        }
    }
}

/*public Transform[] puntosPatrulla; 
 * public float velocidad = 2f; 
 * private int indicePuntoActual = 0;

 * private void Start()
{
    
    StartCoroutine(Patrullar());
}

private IEnumerator Patrullar()
{
    while (true)
    {
        while (true)
        {
            Vector3 destino = puntosPatrulla[indicePuntoActual].position;

            while (Vector3.Distance(transform.position, destino) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                yield return null;
            }

            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
            if (indicePuntoActual == puntosPatrulla.Length)
            {
                indicePuntoActual = 0;
            }
            yield return new WaitForSeconds(1f);
        }

    }
}*/