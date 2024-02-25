using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitaCamara : MonoBehaviour
{
    public float velocidadDeRotacion = 50f;
    private Vector3 minRotation = new Vector3(0.0f, 2.4f, 0.0f);
    private Vector3 maxRotation = new Vector3(0.0f, 3.0f, 0.0f);
    private Vector3 minRotation2 = new Vector3(0.0f, 2.9f, 0.0f);
    private Vector3 maxRotation2 = new Vector3(0.0f, 3.0f, 0.0f);
    private float moveSpeed = 16.0f;
    private bool completadoPrimeraRotacion = false;
    private bool completadoSegundaRotacion = false;
    private bool aux = false;

    private void Start()
    {
        // Obtener la rotación del objeto en el inicio
        Vector3 currentRotation = transform.rotation.eulerAngles;
        //Debug.Log("Rotación inicial: " + currentRotation);
    }

    void Update()
    {

        // Obtener la rotación del objeto en cada frame (actualización)
        Vector3 currentRotation = transform.rotation.eulerAngles;

        //Debug.Log("Rotación actual: " + currentRotation);

        // Pregunta si ya dio la vuelta completa en la primera rotación
        if (!completadoPrimeraRotacion && IsRotationWithinRange(currentRotation, minRotation, maxRotation))
        {
            // Detener la rotación y esperar 3 segundos
            completadoPrimeraRotacion = true;
            aux = true;
            StartCoroutine(EsperarYBajar());

        }
        // Pregunta si ya dio la vuelta completa en la segunda rotación
        else if (completadoPrimeraRotacion && !completadoSegundaRotacion && IsRotationWithinRange(currentRotation, minRotation2, maxRotation2))
        {
            // Detener la rotación y esperar 3 segundos
            completadoSegundaRotacion = true;
            aux = true;
            StartCoroutine(EsperarYContinuar());
        }
        else
        {
            if (!aux)
            {
                // Rotar la cámara en el eje Y
                this.transform.Rotate(new Vector3(0, velocidadDeRotacion * Time.deltaTime, 0));
            }
        }

        // Mover la cámara hacia abajo en el eje Y después de completar la segunda rotación
        if (completadoSegundaRotacion && transform.position.y > -15.29f)
        {
            float moveDistance = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * moveDistance);
        }
    }

    private bool IsRotationWithinRange(Vector3 rotation, Vector3 minRange, Vector3 maxRange)
    {
        return (
            rotation.x >= minRange.x && rotation.x <= maxRange.x &&
            rotation.y >= minRange.y && rotation.y <= maxRange.y &&
            rotation.z >= minRange.z && rotation.z <= maxRange.z
        );
    }

    private IEnumerator EsperarYBajar()
    {
       // Debug.Log("Esperando 3 segundos antes de bajar...");
        yield return new WaitForSeconds(2f);

        // Rotar la cámara hacia la segunda posición
        transform.rotation = Quaternion.Euler(0, 3.3f, 0);
       // Debug.Log("Bajando la cámara...");

        // Mover la cámara hacia abajo en el eje Y
        while (transform.position.y > 0.0f)
        {
            float moveDistance = moveSpeed * Time.deltaTime;
            transform.Translate(Vector3.down * moveDistance);
            yield return null;
        }

       // Debug.Log("Iniciando segunda rotación...");
        completadoPrimeraRotacion = true;
        aux = true;
        StartCoroutine(EsperarYContinuar());
    }


    private IEnumerator EsperarYContinuar()
    {
        //Debug.Log("Esperando 3 segundos después de la segunda rotación...");
        yield return new WaitForSeconds(2f);
       // Debug.Log("Continuando rotación después de la segunda rotación...");
        aux = false;
        // Continuar la rotación
        // velocidadDeRotacion *= -1; // Invertir la dirección de rotación
    }
}
