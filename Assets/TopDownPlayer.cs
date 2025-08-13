using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TopdownPlayer : MonoBehaviour
{
    public float speed = 4.5f;
    private Rigidbody2D rb;
    private Vector2 input;
    public static TopdownPlayer instance;
    //public static event Action OnJugadorMuerto;
    public static Action CallPlayerDead;
    private Animator animator;


    private void Awake()
    {
        if (instance == null) //single, si la instancia ya existe la otra instancia se autodestruye
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;

        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        animator.SetFloat("Speed", input.sqrMagnitude);


    }

    private void FixedUpdate()
    {
        rb.velocity = input * speed;
    }

    public void OnJugadorMuerto()
    {
        this.gameObject.SetActive(false);
        CallPlayerDead?.Invoke();
    }

}

