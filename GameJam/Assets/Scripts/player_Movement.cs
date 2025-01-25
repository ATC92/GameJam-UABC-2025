using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_Movement : MonoBehaviour
{
    // Declaracion de variables

    [Header("Jump Settings")]
    [SerializeField] float maxSpeed;
    bool grounded = false;
    float groundCheckRadius = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] float jumpPower;
    
    [Header("Movement Settings")]
    [SerializeField] [Range(1, 10)] float velocidad; //Velocidad del jugador
    Rigidbody2D rb2d;
    SpriteRenderer spRd;

    void Start () {

        // Capturo y asocio los componentes Rigidbody2D y Sprite Renderer del Jugador
        rb2d = GetComponent<Rigidbody2D>();
        spRd = GetComponent<SpriteRenderer>();

    }
	
	void FixedUpdate () {

        // Movimiento horizontal
        float movimientoH = Input.GetAxisRaw("Horizontal");
        rb2d.linearVelocity = new Vector2(movimientoH * velocidad, rb2d.linearVelocity.y);

        // Sentido horizontal (para girar el render del jugador)
        if (movimientoH > 0)
        {
            spRd.flipX = false;
        }
        else if (movimientoH < 0)
        {
            spRd.flipX = true;
        }

        if(grounded && Input.GetAxis("Jump") > 0 )
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, 0f);
            rb2d.AddForce (new Vector2(0,jumpPower), ForceMode2D.Impulse);
            grounded = false;
        }
        grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, groundLayer);
    }
}
