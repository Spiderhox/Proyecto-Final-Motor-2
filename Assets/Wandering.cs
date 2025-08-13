using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{
    //google, basicamente le esta diciendo que eliga un punto equis en una "brujula" y que
    //de ahi se mande, aun no se como hacer lo de el limite con el area effector pero no
    //se si importa del todo 

    public float speed = 2f; //que tan rapido va 
    public float CambioDireccion = 1.5f; //cuantos segundos hasta que cambia de dirreccion

    private Rigidbody2D rb; //rigid body para movimiento a base de fisica
    private Vector2 movementDireccion; //vector de direccion para el movimiento 

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Object inside effector: " + other.name);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //esto es algo que coge el rigibody que esta puesto al game object, se asocia al rb para poder 
        //usarlo en el script, sin agarrar el component primero el codigo no sabria a que rb esta accediendo 
        DireccionEquis();
        InvokeRepeating(nameof(DireccionEquis), CambioDireccion, CambioDireccion); 
        //invoke llama repetidamente a direccion equis y cambio de direccion dependiendo
        //de los segundos establecidos, eso es para que cambie de direccion
    }

    void FixedUpdate()
    {
        rb.velocity = movementDireccion * speed; 
        //aplica la velocidad del objeto a la direccion
    }

    void DireccionEquis()
    {
        float angle = Random.Range(0f, 360f); //angulo random de la brujula pues, es toooodo los angulos, si quisiera que no fuera asi solo cambie el 360
        movementDireccion = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)).normalized;
        //cosine y sine se usan para convertir la el angulo a vector 
        //normalize para que este a una velocidad constante
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 normal = collision.contacts[0].normal; 
        //el [0] lo que es es un array de puntos donde el objeto toco otro collider
        //se usa el 0 porque es el primero y es el unico que se usa 
        //collision, es para que haga contacto con la pared
        movementDireccion = Vector2.Reflect(movementDireccion, normal);
        //refleja el movimiento de direccion como si fuera una bola rebotando de una superficie 
        //movement direccion es la velocidad del vector (la direccion y magnitud)
        //el normal es como el vector apuntando away de la pared, y cuando se usa reflect, 
        //lo que esta haciendo es di reflejando ese angulo para que se devuelva, usando el angulo de impacto
        //y la orintacion de la pared 
    }

}