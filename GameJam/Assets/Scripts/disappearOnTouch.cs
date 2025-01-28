using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearOnTouch : MonoBehaviour
{
    [SerializeField] float delayBeforeDestroy = 1f; // Tiempo en segundos antes de desaparecer

    private void OnCollisionEnter2D(Collision2D other) 
    {
        // Comprobar si el objeto que lo toca tiene la etiqueta "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Inicia la corrutina para destruir el objeto con un retraso
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        float elapsedTime = 0f;
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color originalColor = sprite.color;

        while (elapsedTime < delayBeforeDestroy)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / delayBeforeDestroy);
            sprite.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        Destroy(gameObject);
    }
}