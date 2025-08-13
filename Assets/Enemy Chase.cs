using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyChase : MonoBehaviour
{
    //este sigue igual que cuando me lo dio el profe

    public enum EstadoEnemigo//
    {
        PATRULLANDO,
        SOSPECHA,
        PERSIGUIENDO
    }

    public Transform[] puntosPatrulla;
    public float velocidad = 2f;
    public float visionRadio = 6f;//
    public LayerMask capaJugador;//
    public LayerMask capaObstaculos;//
    private Rigidbody2D rb; //
    

    private int indicePuntoActual = 0;
    private EstadoEnemigo estado = EstadoEnemigo.PATRULLANDO;//
    private Vector3 puntoSospecha;//
    private Transform objetivoJugador;//

    public static event Action<Vector2> OnAlerta;//

    private void OnDrawGizmosSelected() //google
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, visionRadio);
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();//
        objetivoJugador = GameObject.FindGameObjectWithTag("Player").transform;//
        StartCoroutine(Patrullar());
    }

    private IEnumerator Patrullar()
    {
        while (estado == EstadoEnemigo.PATRULLANDO)
        {
            Vector3 destino = puntosPatrulla[indicePuntoActual].position;
            while (Vector3.Distance(transform.position, destino) > 0.1f && estado == EstadoEnemigo.PATRULLANDO)
            {
                transform.position = Vector2.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);
                yield return null;
            }

            if (estado != EstadoEnemigo.PATRULLANDO)//
            {
                yield break;
            }

            indicePuntoActual = (indicePuntoActual + 1) % puntosPatrulla.Length;
            if (indicePuntoActual == puntosPatrulla.Length)
            {
                indicePuntoActual = 0;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void DetectarJugador()//
    {
        Vector2 direccion = (objetivoJugador.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, visionRadio, direccion, visionRadio, capaJugador | capaObstaculos);

        if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & capaJugador) != 0)
        {
            RaycastHit2D hitObstaculo = Physics2D.Raycast(transform.position, direccion, visionRadio, capaObstaculos);
            if (hitObstaculo.collider == null || hitObstaculo.distance > Vector2.Distance(transform.position, objetivoJugador.position))
            {
                puntoSospecha = objetivoJugador.position;
                estado = EstadoEnemigo.SOSPECHA;
                OnAlerta?.Invoke(puntoSospecha);
                StopAllCoroutines();
                Debug.Log($"{gameObject.name} sus");

            }
        }
    }

    private void IrAPuntoSospecha()//
    {
        transform.position = Vector2.MoveTowards(transform.position, puntoSospecha, velocidad / 1f * Time.deltaTime);
        if (Vector2.Distance(transform.position, puntoSospecha) < 0.2f)
        {
            float distJugador = Vector3.Distance(transform.position, objetivoJugador.position);
            if (distJugador < visionRadio / 2)
            {
                estado = EstadoEnemigo.PERSIGUIENDO;
                OnAlerta?.Invoke(transform.position);
            }
            else
            {
                estado = EstadoEnemigo.PATRULLANDO;
                StartCoroutine(Patrullar());
            }
        }
        else
        {
            Vector2 direccion = (objetivoJugador.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.CircleCast(transform.position, visionRadio, direccion, visionRadio, capaJugador | capaObstaculos);

            if (hit.collider != null && ((1 << hit.collider.gameObject.layer) & capaJugador) != 0)
            {
                float distJugador = Vector3.Distance(transform.position, objetivoJugador.position);
                if (distJugador < visionRadio / 2)
                {
                    estado = EstadoEnemigo.PERSIGUIENDO;
                    OnAlerta?.Invoke(transform.position);
                }
            }
        }
    }

    private void Perseguir()//
    {
        transform.position = Vector2.MoveTowards(transform.position, objetivoJugador.position, velocidad * 2 * Time.deltaTime);
        if (Vector2.Distance(transform.position, objetivoJugador.position) > visionRadio /* 2 */)
        {
            puntoSospecha = objetivoJugador.position;
            estado = EstadoEnemigo.SOSPECHA;
            StopAllCoroutines();
        }
    }

    private void Update()//
    {
        switch (estado)
        {
            case EstadoEnemigo.PATRULLANDO:
                DetectarJugador();
                break;
            case EstadoEnemigo.SOSPECHA:
                IrAPuntoSospecha();
                break;
            case EstadoEnemigo.PERSIGUIENDO:
                Perseguir();
                break;
        }
    }

    private void OnEnable()//
    {
        OnAlerta += RecibirAlerta;
        HazardDamageBehaviour.OnJugadorMuerto += VolverAPatrullar;

    }

    private void OnDisable()//
    {
        OnAlerta -= RecibirAlerta;
        HazardDamageBehaviour.OnJugadorMuerto -= VolverAPatrullar;

    }

    private void VolverAPatrullar()//
    {
        StopAllCoroutines(); //
        if (rb != null) rb.velocity = Vector2.zero; //
        StartCoroutine(EsperarYPatrullar());
    }

    private IEnumerator EsperarYPatrullar()//
    {
        yield return new WaitForSeconds(1f);
        estado = EstadoEnemigo.PATRULLANDO;
        StartCoroutine(Patrullar());
    }

    private void RecibirAlerta(Vector2 posicionJugador)//
    {
        if (estado == EstadoEnemigo.PATRULLANDO)
        {
            puntoSospecha = posicionJugador;
            estado = EstadoEnemigo.SOSPECHA;
            StopAllCoroutines();
        }
    }
}
