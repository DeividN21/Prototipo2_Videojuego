using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Nivel2_Controller : MonoBehaviour
{
    public void RegresarMapa(string Minimapa)
    {
        // Guardar la escena actual para verificar si es el minimapa
        string escenaActual = SceneManager.GetActiveScene().name;

        // Cargar la escena especificada
        SceneManager.LoadScene(Minimapa);

        // Verificar si la escena cargada es el minimapa
        if (escenaActual == "Minimapa")
        {
            // Realizar acciones adicionales específicas del minimapa

            // Abrir el camino hacia el siguiente nivel
            if (Variables.maxNivel < 3 && Variables.nivel == 2 && Variables.estadoPos3 == 0)
            {
                Variables.maxNivel++;
                Variables.estadoPos3 = 2;
            }

            // Restablecer la capacidad de mover al jugador
            FindObjectOfType<MinimapaController>().RestablecerPuedoMover();
        }
    }
}
