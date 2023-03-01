using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SpriteGlow;

public class Deslizar : MonoBehaviour
{

    public GameObject carta;
    public GameObject baraja;
    public cardscript cs;

    SpriteGlowEffect spriteglow;

    public ModificaTamaño statElectricidad;
    public ModificaTamaño statGente;
    public ModificaTamaño statFelicidad;
    public ModificaTamaño statDinero;
    public ModificaTamaño statComida;
    public int DiasTranscurridos = 0;

    SpriteRenderer spr;
    public float velCarta = .5f;


    [SerializeField]

    private Sprite faceSprite, backSprite, barajaSprite;

    [SerializeField] public TextMeshProUGUI TextoDias;

    private bool coroutineAllowed, facedUp;

    Coroutine lastRoutine;


    [SerializeField] private BaseCarta[] cartasQueUsamos;

    public void RandomGenerator()
    {
        DiasTranscurridos = DiasTranscurridos + Random.Range(10, 25);
        TextoDias.GetComponent<TextMeshProUGUI>().text = DiasTranscurridos + " dias como alcalde";
    }

    private void SigCarta(int cartaPos)
    {
        cs.cartaDatos = cartasQueUsamos[cartaPos];
    }

    // Start is called before the first frame update
    void Start()
    {
        //INICIALIZAR VALORES DE LA ESCENA
        spr = carta.GetComponent<SpriteRenderer>();
        spriteglow = carta.GetComponent<SpriteGlowEffect>();
        spriteglow.enabled = false;
        spr.sprite = faceSprite;
        coroutineAllowed = true;
        facedUp = true;
        cs.descripcionCarta.enabled = false;
        cs.fondoTexto.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (cs.isMouseOver)
        {
            cs.descripcionCarta.enabled = true;
            cs.fondoTexto.enabled = true;
        }
        else
        {
            cs.descripcionCarta.enabled = false;
            cs.fondoTexto.enabled = false;
        }
        //rotacion de la carta al moverse
        if (coroutineAllowed)
        {  //no está en la coroutina, por lo que no choca con ello
            carta.transform.rotation = Quaternion.Euler(0f, 0f, -2 * carta.transform.position.x);
        }

        if (coroutineAllowed && Input.GetMouseButtonDown(1))
        {
            lastRoutine = StartCoroutine(RotateCard(0.02f));
        }


        if (carta.transform.position.x > 2) //PARA LA DERECHA
        {

            cs.descripcionLado.enabled = true;
            cs.descripcionLado.text = cs.cartaDatos.textoDer;
            spriteglow.enabled = true;
            //spr.color = Color.green; //AÑADIR EFECTO PARA VER QUE SE VA A ELEGIR A LA CARTA

            if (Input.GetMouseButtonUp(0))
            {
                ChangeStats(true);
                int aux = cs.derecha();
                //restan--;
                RandomGenerator();
                SigCarta(aux);
                cs.UpdateCartaUI(false);
                //cs.cartaDatos.derElect  - - - - > acceder a las estadisticas de carta
                //aqui se carga siguiente carta
                //tambien se tiene que actualizar el siguiente dorso de la baraja

                ocultarUIcarta();

                if (coroutineAllowed)
                {
                    StartCoroutine(RotateNewCard());
                }
            }
        }
        else if (carta.transform.position.x < -2)   //PARA LA IZQUIERDA
        {

            cs.descripcionLado.enabled = true;
            cs.descripcionLado.text = cs.cartaDatos.textoIzq;
            spriteglow.enabled = true;
            //spr.color = Color.red; //AÑADIR EFECTO PARA VER QUE SE VA A ELEGIR A LA CARTA

            if (Input.GetMouseButtonUp(0))
            {
                ChangeStats(false);
                int aux = cs.izquierda();
                //restan--;
                RandomGenerator();
                SigCarta(aux);
                cs.UpdateCartaUI(true);

                //aqui se carga siguiente carta
                //tambien se tiene que actualizar el siguiente dorso de la baraja

                ocultarUIcarta();

                if (coroutineAllowed)
                {
                    StartCoroutine(RotateNewCard());
                }
            }

        }
        else
        {
            cs.descripcionLado.enabled = false;
            spriteglow.enabled = false;
            spr.color = Color.white;
        }




        if (Input.GetMouseButton(0) && cs.isMouseOver)
        {

            Vector2 posicion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            carta.transform.position = posicion;
        }
        else
        {
            carta.transform.position = Vector2.MoveTowards(transform.position, new Vector2(0, 0), velCarta);
        }
    }

    private IEnumerator RotateCard(float tiempoRotacion)
    {

        FindObjectOfType<audioManager>().Play("flipCard");
        float tiempoRot = tiempoRotacion;

        coroutineAllowed = false;

        if (!facedUp)
        {

            for (float i = 0f; i <= 90f; i += 10f)
            {
                carta.transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return new WaitForSeconds(tiempoRot);
            }
            cs.imagen.enabled = true;
            cs.nombreCarta.enabled = true;
            cs.fondoTexto.enabled = true;
            spr.sprite = faceSprite;
            for (float i = 90f; i >= 0f; i -= 10f)
            {
                carta.transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return new WaitForSeconds(tiempoRot);
            }

        }
        if (facedUp)
        {
            for (float i = 0f; i <= 90f; i += 10f)
            {
                carta.transform.rotation = Quaternion.Euler(0f, i, 0f);
                yield return new WaitForSeconds(tiempoRot);
            }
            cs.imagen.enabled = false;
            cs.nombreCarta.enabled = false;
            cs.fondoTexto.enabled = false;
            spr.sprite = backSprite;

            for (float i = 90f; i >= 0f; i -= 10f)
            {
                carta.transform.rotation = Quaternion.Euler(0f, i, 0f);


                yield return new WaitForSeconds(tiempoRot);
            }

        }
        coroutineAllowed = true;

        facedUp = !facedUp;
    }


    private IEnumerator RotateNewCard()
    {
        yield return new WaitForSeconds(0.5f);//tiempo de espera para girar nueva carta
        coroutineAllowed = false;

        if (!facedUp)
        {


            StartCoroutine(RotateCard(0.02f));
        }

    }

    private void ChangeStats(bool der)
    {
        if (der)
        {
            statElectricidad.ValorStat += cs.cartaDatos.derElect * 0.1f;
            statGente.ValorStat += cs.cartaDatos.derGente * 0.1f;
            statFelicidad.ValorStat += cs.cartaDatos.derFelic * 0.1f;
            statDinero.ValorStat += cs.cartaDatos.derDinero * 0.1f;
            statComida.ValorStat += cs.cartaDatos.derComida * 0.1f;
        }
        else
        {
            statElectricidad.ValorStat += cs.cartaDatos.izqElect * 0.1f;
            statGente.ValorStat += cs.cartaDatos.izqGente * 0.1f;
            statFelicidad.ValorStat += cs.cartaDatos.izqFelic * 0.1f;
            statDinero.ValorStat += cs.cartaDatos.izqDinero * 0.1f;
            statComida.ValorStat += cs.cartaDatos.izqComida * 0.1f;
        }

    }

    private void ocultarUIcarta()
    {
        spr.sprite = backSprite;
        facedUp = false;
        cs.imagen.enabled = false;
        cs.nombreCarta.enabled = false;
        cs.fondoTexto.enabled = false;
    }
}
