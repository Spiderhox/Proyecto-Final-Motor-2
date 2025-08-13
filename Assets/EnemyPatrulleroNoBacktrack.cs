using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrulleroNoBacktrack : MonoBehaviour
{ public Transform[] puntosPatrulla; 
  public float velocidad = 2f; 
  private int indicePuntoActual = 0;

  private void Start()
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

    }
}
