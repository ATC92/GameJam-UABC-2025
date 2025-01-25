using System.IO;
using UnityEngine;

public class circleSelector : MonoBehaviour
{
    [Header("Lista de Objetos a girar")]
    [SerializeField] GameObject[] items;
    [SerializeField] float radius = 100f;
    [SerializeField] float rotationStep = 360f; // Grados de rotacion por paso
    private int selectedIndex = 0;

    void Start()
    {
        ArrangeItemsInCircle(); // Distribuir los elementos al inicio
        HighlightSelectedItem(); // Resaltar el primer elemento
    }

    void Update()
    {
        // Cambiar rotacion con las teclas '1' y '3'
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla '1' para girar a la izquierda
        {
            RotateCircle(1); // Rotar en sentido horario
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tecla '3' para girar a la derecha
        {
            RotateCircle(-1); // Rotar en sentido antihorario
        }

        // Confirmar selección con "Enter"
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Debug.Log("Seleccionaste: " + items[selectedIndex].name);
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
}