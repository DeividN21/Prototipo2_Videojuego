using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinimapaController : MonoBehaviour
{
    [SerializeField] float velocidad = 5;
    [SerializeField] Transform[] pos;
    [SerializeField] SpriteRenderer[] posIco;
    [SerializeField] Transform[] camino;
    [SerializeField] Transform player;
    [SerializeField] SpriteRenderer texto;
    [SerializeField] Sprite[] spr_texto;
    [SerializeField] AudioSource snd_camino;
    int playerPos = 0;
    int maxNivel;
    bool puedoMover = false;
    string escena;

    void OnEnable()
    {
        // Asegurarse de que el jugador pueda moverse y abrir el camino hacia el siguiente nivel
        RestablecerPuedoMover();
    }

    public void RestablecerPuedoMover()
    {
        // Restablecer la capacidad de mover al jugador
        puedoMover = true;

        // Obtener la información del jugador desde las variables estáticas
        playerPos = Variables.nivel;
        texto.sprite = spr_texto[playerPos];
        maxNivel = Variables.maxNivel;

        // Posicionar al jugador en la posición correspondiente
        player.position = pos[playerPos].position;

        // Verificar y abrir el camino hacia el siguiente nivel si es necesario
        if (Variables.maxNivel < 3 && Variables.nivel == 1 && Variables.estadoPos2 == 0)
        {
            StartCoroutine("AbreCamino", 1);
        }
    }
    void Awake()
    {
        if(!Variables.iniciado)
        {
            Variables.nivel = 0;
            Variables.maxNivel = 1;
            Variables.iniciado = true;
            Variables.estadoPos1 = 2;
            Variables.estadoPos2 = 0;
            Variables.estadoPos3 = 0;
        }

        playerPos = Variables.nivel;
        texto.sprite = spr_texto[playerPos];
        maxNivel = Variables.maxNivel;

        puedoMover = false;
        player.position = pos[playerPos].position;

        if (Variables.estadoPos1 == 1)
        {
            camino[0].localScale = new Vector3(1, 1, 1);
            posIco[0].enabled = true;
        } else if (Variables.estadoPos1 == 2)
        {
            StartCoroutine("AbreCamino", 0);
        }
    }

    //Animación del camino
    IEnumerator AbreCamino(int numCamino){
        yield return new WaitForSeconds(0.5F);
        snd_camino.Play();
        float porcentaje = 0;
        do
        {
            porcentaje += 0.01F;
            if (porcentaje < 1) camino[numCamino].localScale = new Vector3(porcentaje, 1, 1);
            yield return new WaitForSeconds(0.01F);
        }while (porcentaje < 1);
        snd_camino.Stop();
        puedoMover = true;
        posIco[numCamino].enabled = true; //Actualizacion para ir al siguiente nivel
        if (numCamino == 0) Variables.estadoPos1 = 1;
        if (numCamino == 1) Variables.estadoPos2 = 1;
        if (numCamino == 2) Variables.estadoPos3 = 1;
    }
    // Movimiento del Player
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if (h > 0.25F) h = 1;
        else if (h < -0.25F) h = -1;
        else h = 0;
        if ( (h == 1 && playerPos < maxNivel && puedoMover) || (h == -1 && playerPos > 0 && puedoMover))
        {
            puedoMover = false;
            StartCoroutine("MuevePlayer", h);
        }

        if (Input.GetButtonDown("Submit") && puedoMover)
        {
            Variables.nivel = playerPos;
            if (playerPos == 0) escena = "Menu";
            if (playerPos == 1) escena = "Nivel_1";
            if (playerPos == 2) escena = "Escena2";
            SceneManager.LoadScene(escena);
        }
    }
    
    //Control para el movimiento
    IEnumerator MuevePlayer(int mov){
        playerPos += mov;
        Vector3 distancia = Vector3.zero;
        do
        {
            player.transform.Translate(0.025F * mov, 0, 0);
            distancia = pos[playerPos].position - player.position;
            yield return new WaitForSeconds(0.01F);
        } while (distancia.sqrMagnitude > 0.001F);
        texto.sprite = spr_texto[playerPos]; //Actualizar titulo
        player.position = pos[playerPos].position;
        yield return new WaitForSeconds(0.15F);
        puedoMover = true;
    }

    private void OnApplicationQuit()
    {
        Variables.iniciado = false;
    }
}
