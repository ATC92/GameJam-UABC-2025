using System.IO;
using UnityEngine;

public class circleSelector : MonoBehaviour
{
    /* Objeto de Player */
    [Header("Referencia al objeto player")]
    [SerializeField] GameObject player;

    /* Lista de objetos para utilizar*/
    [Header("Prefabs de las burbujas")]
    [SerializeField] GameObject[] items;
    [Tooltip("Radio del circulo para el seleccionador de burbujas")]
    [SerializeField] float radius = 100f;
    [Tooltip("Grados qde rotacion por cada paso")]
    [SerializeField] float rotationStep = 360f; // Grados de rotacion por paso
    private int selectedIndex = 0;
    
    /* Cooldown para el jugador al colocar una burbuja, el jugador debera de esperar un corto tiempo */

    [Header("Cooldown entre colocacion de burbujas")]
    [Tooltip("Tiempo de espera (en segundos) antes de poder seleccionar otro objeto.")]
    [SerializeField] private float selectionCooldown = 1f;
    private float lastSelectionTime; // Tiempo en el que se realizó la ultima seleccion

    /* Previsualizacion de objetos*/
    private GameObject previewObject; // Objeto para la previsualizacion

    /* Tamaño predeterminado del objeto al ser placeable */
    [Header("Especificaciones para la colocacion de objetos")]
    [Tooltip("Reescalado del objeto a colocar")]
    [SerializeField] Vector3 placedObjectScale = new Vector3(1f, 1f, 1f);
    [Tooltip("Rango maximo para colocar objetos desde tu posicion actual (Player)")]
    [SerializeField] float placementRange = 5f; // Rango maximo donde se puede colocar el objeto
    private Transform placementCenter; // Centro del rango permitido
    private bool canPlace;
    private bool isOnCooldown = false;

    /*
    

    
    */
    void Start()
    {
        placementCenter = player.transform;
        DisablePhysicsInItems(); // Desactivar fisica y colisiones en los objetos
        ArrangeItemsInCircle(); // Distribuir los elementos al inicio
        HighlightSelectedItem(); // Resaltar el primer elemento
    }

    void Update()
    {
        // Cambiar rotacion con las teclas '1' y '3'
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla '1' para girar a la izquierda
        {
            RotateCircle(-1); // Rotar en sentido horario
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tecla '3' para girar a la derecha
        {
            RotateCircle(1); // Rotar en sentido antihorario
        }

        // Confirmar seleccion con "Click izquierdo"
        if (Input.GetMouseButtonDown(0))
        {
            // Comenzar la previsualizacion del objeto seleccionado
            StartPreview();
        }

        if (previewObject != null)
        {
            // Actualizamos la posicion actual de donde esta el objeto
            UpdatePreviewPosition();

            // Verificar si está dentro del rango permitido
            canPlace = IsWithinPlacementRange(previewObject);

            // Cambiar el color según si está dentro o fuera del rango
            SpriteRenderer renderer = previewObject.GetComponent<SpriteRenderer>();
            renderer.color = canPlace ? new Color(0f, 1f, 0f, 0.5f) : new Color(1f, 0f, 0f, 0.5f);

            if (Input.GetMouseButtonDown(1)  && canPlace) // Clic derecho para colocar el objeto
            {
                PlaceObject();
            }
        }
    }

    void DisablePhysicsInItems()
    {
        // Desactiva Rigidbody2D y Collider2D en todos los objetos de la rueda
        foreach (GameObject item in items)
        {
            var rb = item.GetComponent<Rigidbody2D>();
            if (rb != null) rb.simulated = false; // Desactiva la simulacion del Rigidbody2D

            var collider = item.GetComponent<Collider2D>();
            if (collider != null) collider.enabled = false; // Desactiva el Collider2D
        }
    }

    void ArrangeItemsInCircle()
    {
        float angleStep = 360f / items.Length; // Angulo entre elementos
        for (int i = 0; i < items.Length; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );
            items[i].transform.localPosition = position;

            // Ajustar la orientación del sprite para que mire hacia afuera
            float angleDegrees = i * angleStep;
            items[i].transform.localRotation = Quaternion.Euler(0, 0, angleDegrees - 45f);
    
        }
    }

    void RotateCircle(int direction)
    {
        // Rotar el circulo completo
        transform.Rotate(Vector3.forward, direction * (rotationStep / items.Length));

        // Actualizar indice seleccionado (inverso porque rota el circulo, no el indice)
        selectedIndex = (selectedIndex - direction + items.Length) % items.Length;

        HighlightSelectedItem();
    }

    void HighlightSelectedItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            SpriteRenderer renderer = items[i].GetComponent<SpriteRenderer>();
            // Cambiar color: amarillo para seleccionado (parte superior), blanco para el resto
            renderer.color = (i == selectedIndex) ? Color.yellow : Color.white;
        }
    }

    void StartPreview()
    {

        // Instanciar un objeto para la previsualizacion
        if (previewObject != null) Destroy(previewObject); // Eliminar cualquier preview previo

        previewObject = Instantiate(items[selectedIndex]); // Crear una copia del objeto seleccionado
        previewObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f); // Hacerlo translucido

        // Cambiar el tamaño del objeto antes de colocarlo
        previewObject.transform.localScale = placedObjectScale;

        // Ajustaamos la rotacion en 'Z' a 0.
        Vector3 rotation = previewObject.transform.eulerAngles;
        rotation.z = 0;
        previewObject.transform.eulerAngles = rotation;

        // Desactivar colisiones y gravedad durante la previsualizacion
        var rb = previewObject.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = false;

        var collider = previewObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = false;
    }

    void UpdatePreviewPosition()
    {
        // Mover el preview al puntero del raton
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Asegurarse de que este en el plano 2D
        previewObject.transform.position = mousePosition;
    }

    void PlaceObject()
    {
        // Cambiar el tamaño del objeto antes de colocarlo
        previewObject.transform.localScale = placedObjectScale;

        // Hacer que el objeto sea fisico al colocarlo
        previewObject.GetComponent<SpriteRenderer>().color = Color.white; // Restaurar el color
        var rb = previewObject.GetComponent<Rigidbody2D>();
        if (rb != null) rb.simulated = true;

        var collider = previewObject.GetComponent<Collider2D>();
        if (collider != null) collider.enabled = true;

        previewObject = null; // Limpiar el objeto de previsualizacion
    }

    bool IsWithinPlacementRange(GameObject objectToCheck)
    {
        // Calcular la distancia entre el objeto y el centro permitido
        float distance = Vector3.Distance(objectToCheck.transform.position, placementCenter.position);
        return distance <= placementRange;
    }
}