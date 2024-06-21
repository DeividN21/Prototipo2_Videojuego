using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Opciones Generales")]
    [SerializeField] int volumenMusica;
    [SerializeField] int volumenSonido;
    [SerializeField] GameObject pantallaMenu;
    [SerializeField] GameObject pantallaOpciones;
    [SerializeField] float tiempoCambiaOpcion;


    [Header("Elementos de Opciones")]
    [SerializeField] SpriteRenderer musica;
    [SerializeField] SpriteRenderer sonido;
    [SerializeField] SpriteRenderer volver;


    [Header("Elementos del Menu")]
    [SerializeField] SpriteRenderer comenzar;
    [SerializeField] SpriteRenderer opciones;
    [SerializeField] SpriteRenderer salir;

    [Header("Sprites del Menu")]
    [SerializeField] Sprite comenzar_off;
    [SerializeField] Sprite comenzar_on;
    [SerializeField] Sprite opciones_off;
    [SerializeField] Sprite opciones_on;
    [SerializeField] Sprite salir_off;
    [SerializeField] Sprite salir_on;


    [Header("Sprites de Opciones")]

    [SerializeField] Sprite musica_off;
    [SerializeField] Sprite musica_on;
    [SerializeField] Sprite sonido_off;
    [SerializeField] Sprite sonido_on;
    [SerializeField] Sprite volver_off;
    [SerializeField] Sprite volver_on;
    [SerializeField] Sprite vol_off;
    [SerializeField] Sprite vol_on;
    [SerializeField] SpriteRenderer[] musica_spr;
    [SerializeField] SpriteRenderer[] sonido_spr;


    [Header("Sonidos")]
    [SerializeField] AudioSource musicaMenu;
    [SerializeField] AudioSource snd_opcion;
    [SerializeField] AudioSource snd_seleccion;

    int pantalla;
    int opcionMenu, opcionMenuAnt;
    int opcionOpciones, opcionOpcionesAnt;
    bool pulsadoSubmit;
    float v,h;
    float tiempoV, tiempoH;
    void Awake()
    {
        LeerPreferencias();
        pantalla = 0; 
        tiempoV = tiempoH = 0;
        opcionMenu = opcionMenuAnt = 1;
        AjustaOpciones();

    }

    void AjustaOpciones(){
        AjustaMusica();
        AjustaSonido();
    }

    
    void Update()
    {
        v = Input.GetAxisRaw("Vertical");
        h = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonUp("Submit")) pulsadoSubmit = false;
        if (v ==0) tiempoV = 0;
        if (pantalla == 0 ) MenuPrincipal();
        if (pantalla == 1 ) MenuOpciones();
    }

    // Control de opciones del menú
    void MenuPrincipal(){
        if (v != 0)
        {
            if (tiempoV == 0 || tiempoV > tiempoCambiaOpcion)
            {
                if (v== 1 && opcionMenu > 1) SeleccionaMenu(opcionMenu - 1);
                if (v== -1 && opcionMenu < 3) SeleccionaMenu(opcionMenu + 1);
                if (tiempoV > tiempoCambiaOpcion) tiempoV = 0;
            }
            tiempoV += Time.deltaTime;
        }
        if (Input.GetButtonDown("Submit") && !pulsadoSubmit)
        {
            snd_seleccion.Play();
            if (opcionMenu == 1) SceneManager.LoadScene("Minimapa");
            if (opcionMenu == 2) CargaPantallaOpciones();
            if (opcionMenu == 3) Application.Quit();
        }
    }

    // Control de la pantalla de opciones
    void MenuOpciones(){
        if (v != 0)
        {
            if (tiempoV == 0 || tiempoV > tiempoCambiaOpcion)
            {
                if (v== 1 && opcionOpciones > 1) SeleccionaOpcion(opcionOpciones - 1);
                if (v== -1 && opcionOpciones < 3) SeleccionaOpcion(opcionOpciones + 1);
                if (tiempoV > tiempoCambiaOpcion) tiempoV = 0;
            }
            tiempoV += Time.deltaTime;
        }
        if (h == 0) tiempoH = 0;
        else
        {
            if ((tiempoH == 0 || tiempoH > tiempoCambiaOpcion) && (opcionOpciones == 1 || opcionOpciones == 2))
            {
                if (opcionOpciones == 1 && ((h < 0 && volumenMusica > 0) || (h > 0 && volumenMusica < 10)))
                {
                    volumenMusica += (int)h;
                    AjustaMusica();
                }
                if (opcionOpciones == 2 && ((h < 0 && volumenSonido > 0) || (h > 0 && volumenSonido < 10)))
                {
                    volumenSonido += (int)h;
                    AjustaSonido();
                    snd_opcion.Play();
                }
                if (tiempoH > tiempoCambiaOpcion) tiempoH = 0;

            }
            tiempoH += Time.deltaTime;
        }

        if (Input.GetButtonDown("Submit") && opcionOpciones == 3 && !pulsadoSubmit) 
        {
            GuardarPreferencias();
            CargaPantallaMenu();
        }
    }

    // Para guardar valores establecidos
    private void GuardarPreferencias()
    {
        PlayerPrefs.SetInt("VolumenMusica", volumenMusica);
        PlayerPrefs.SetInt("VolumenSonido", volumenSonido);
        PlayerPrefs.Save();
    }

    private void LeerPreferencias()
    {
        volumenMusica = PlayerPrefs.GetInt("VolumenMusica",5);
        volumenSonido = PlayerPrefs.GetInt("VolumenSonido",4);
    }

    private void AjustaMusica()
    {
        if (volumenMusica == 0) musica_spr[0].enabled = true;
        else musica_spr[0].enabled = false;
        for (int i = 1; i <= 10; i++)
        {
            if(i <= volumenMusica) musica_spr[i].sprite = vol_on;
            else musica_spr[i].sprite = vol_off;
        }
        musicaMenu.volume = (volumenMusica / 10F);
    }
    private void AjustaSonido()
    {
        if (volumenSonido == 0) sonido_spr[0].enabled = true;
        else sonido_spr[0].enabled = false;
        for (int i = 1; i <= 10; i++)
        {
            if(i <= volumenSonido) sonido_spr[i].sprite = vol_on;
            else sonido_spr[i].sprite = vol_off;
        }
        GameObject[] sonidos = GameObject.FindGameObjectsWithTag("Sonidos");
        foreach (GameObject sonido in sonidos)
        {
            sonido.GetComponent<AudioSource>().volume = volumenSonido / 10F;
        }
    }

    void CargaPantallaMenu(){
        pulsadoSubmit = true;
        snd_seleccion.Play();
        pantalla = 0;
        pantallaOpciones.SetActive(false);
        pantallaMenu.SetActive(true);

    }
    void CargaPantallaOpciones(){
        pulsadoSubmit = true;
        pantallaMenu.SetActive(false);
        pantalla = 1;
        opcionOpciones = opcionOpcionesAnt = 1;
        musica.sprite = musica_on;
        sonido.sprite = sonido_off;
        volver.sprite = volver_off;
        pantallaOpciones.SetActive(true);

    }

    void SeleccionaMenu(int op){
        snd_opcion.Play();
        opcionMenu = op;
        if (op == 1) comenzar.sprite = comenzar_on;
        if (op == 2) opciones.sprite = opciones_on;
        if (op == 3) salir.sprite = salir_on;
        if(opcionMenuAnt == 1) comenzar.sprite = comenzar_off;
        if(opcionMenuAnt == 2) opciones.sprite = opciones_off;
        if(opcionMenuAnt == 3) salir.sprite = salir_off;
        opcionMenuAnt = op;
    }

    void SeleccionaOpcion(int op){
        snd_opcion.Play();
        opcionOpciones = op;
        if (op == 1) musica.sprite = musica_on;
        if (op == 2) sonido.sprite = sonido_on;
        if (op == 3) volver.sprite = volver_on;
        if(opcionOpcionesAnt == 1) musica.sprite = musica_off;
        if(opcionOpcionesAnt == 2) sonido.sprite = sonido_off;
        if(opcionOpcionesAnt == 3) volver.sprite = volver_off;
        opcionOpcionesAnt = op;
    }
}
