using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    // Declaracion de variables

    [Header("Jump Settings")]
    private bool grounded = false;
    private bool canjump;
    //[SerializeField]float groundCheckRadius = 0.2f;
    [SerializeField] float maxSpeed;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Collider2D collisionBox2D;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpPower;
    

    [Header("Movement Settings")]
    [SerializeField] [Range(1, 10)] float velocidad; //Velocidad del jugador
    private Rigidbody2D rb2d;
    private SpriteRenderer spRd;

    void Start ()
    {
        // Capturo y asocio los componentes Rigidbody2D y Sprite Renderer del Jugador
        rb2d = GetComponent<Rigidbody2D>();
        spRd = GetComponent<SpriteRenderer>();
    }
	
	void Update()
    {
        /*
            - Accion para el movimiento
            - Tomando el Eje X como movimiento 
                [Teclas predeterminadas por Unity 'A' 'D']
        */  
        float movimientoH = Input.GetAxisRaw("Horizontal");
        rb2d.linearVelocity = new Vector2(movimientoH * velocidad, rb2d.linearVelocity.y);
        // Girar el render del jugador
        if (movimientoH > 0)
            spRd.flipX = false;
        else if (movimientoH < 0)
            spRd.flipX = true;

        /* 
            - Accion para la fuerza del salto
        */
        if(grounded && Input.GetKeyDown(KeyCode.Space))
        {
            canjump = true;
        }
        grounded = Physics2D.IsTouchingLayers(collisionBox2D); // Tomamos el Collider2D del checkGround
        //grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer); // // Tomamos el radio del checkGround
    }

    void FixedUpdate()
    {
        /* 
            - Ajustes para la fuerza del salto
        */
        if(canjump)
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
            rb2d.AddForce (new Vector2(0,jumpPower), ForceMode2D.Impulse);
            canjump = false;
        }
    }
}