using UnityEngine;

public class circleSelector : MonoBehaviour
{
    [Header("Lista de Objetos a girar")]
    [SerializeField] GameObject[] items;
    [SerializeField] float radius = 100f;
    [SerializeField] float rotationSpeed = 100f;
    private int selectedIndex = 0;

    void Start()
    {
        ArrangeItemsInCircle();
        HighlightSelectedItem();
    }

    void Update()
    {
        // Cambiar selección con las teclas '1' y '3'
        if (Input.GetKeyDown(KeyCode.Alpha1)) // Tecla '1'
        {
            ChangeSelection(1); // Mover a la izquierda
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) // Tecla '3'
        {
            ChangeSelection(-1); // Mover a la derecha
        }

        if (Input.GetKeyDown(KeyCode.Mouse0)) // Seleccion con "Click Izquierdo"
        {
            Debug.Log("Seleccionaste: " + items[selectedIndex].name);
        }
    }

    void ArrangeItemsInCircle()
    {
        float angleStep = 360f / items.Length;
        for (int i = 0; i < items.Length; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            Vector3 position = new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0
            );
            items[i].transform.localPosition = position;
        }
    }

    void ChangeSelection(int direction)
    {
        // Actualizar indice seleccionado
        selectedIndex = (selectedIndex + direction + items.Length) % items.Length;
        HighlightSelectedItem();
    }

    /*void RotateCircle(float angle)
    {
        transform.Rotate(Vector3.forward, angle);
        selectedIndex = (selectedIndex - Mathf.RoundToInt(angle / (360f / items.Length)) + items.Length) % items.Length;
        HighlightSelectedItem();
    }*/

    void HighlightSelectedItem()
    {
        for (int i = 0; i < items.Length; i++)
        {
            SpriteRenderer renderer = items[i].GetComponent<SpriteRenderer>();
            renderer.color = (i == selectedIndex) ? Color.yellow : Color.white; // Resalta el seleccionado
        }
    }
}