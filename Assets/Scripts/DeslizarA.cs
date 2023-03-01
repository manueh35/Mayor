using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using SpriteGlow;
using UnityEngine.SceneManagement;

public class DeslizarA : MonoBehaviour
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

    public SpriteRenderer circuloElectricidad;
    public SpriteRenderer circuloPoblacion;
    public SpriteRenderer circuloFelicidad;
    public SpriteRenderer circuloDinero;
    public SpriteRenderer circuloComida;

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
        //Solo salatamos a la siguiente carta si no hemos perdidpo por (pasarnos/quedarnos cortos) con un stat
        if(!esDerrota())
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

        if (carta.transform.position.x > -2 & carta.transform.position.x < 2)
        {
            circuloElectricidad.color = Color.grey;
            circuloPoblacion.color = Color.grey;
            circuloFelicidad.color = Color.grey;
            circuloDinero.color = Color.grey;
            circuloComida.color = Color.grey;
        }

        if (carta.transform.position.x > 2) //PARA LA DERECHA
        {
            ChangeCircleStats();

            cs.descripcionLado.enabled = true;
            cs.descripcionLado.text = cs.cartaDatos.textoDer;
            spriteglow.enabled = true;
            //spr.color = Color.green; //AÑADIR EFECTO PARA VER QUE SE VA A ELEGIR A LA CARTA

            if (Input.GetMouseButtonUp(0))
            {
                if (esDerrota() && cs.cartaDatos.nombreCarta == "Derrota")
                {
                    PlayerPrefs.SetInt("player_score", DiasTranscurridos);
                    SceneManager.LoadScene("LeaderBoard");
                }
                        
                ChangeStats(true);
                int aux = cs.derecha();
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
            ChangeCircleStats();

            cs.descripcionLado.enabled = true;
            cs.descripcionLado.text = cs.cartaDatos.textoIzq;
            spriteglow.enabled = true;
            //spr.color = Color.red; //AÑADIR EFECTO PARA VER QUE SE VA A ELEGIR A LA CARTA

            if (Input.GetMouseButtonUp(0))
            {
                if (esDerrota() && cs.cartaDatos.nombreCarta == "Derrota")
                {
                    PlayerPrefs.SetInt("player_score", DiasTranscurridos);
                    SceneManager.LoadScene("LeaderBoard");
                }
                ChangeStats(false);
                int aux = cs.izquierda();
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

    private bool esDerrota()
    {
        if ((statElectricidad.ValorStat > -0.3f && statGente.ValorStat > -0.3f && statFelicidad.ValorStat > -0.3f && statDinero.ValorStat > -0.3f && statComida.ValorStat > -0.3f &&
            statElectricidad.ValorStat <= 1.3f && statGente.ValorStat <= 1.3f && statFelicidad.ValorStat <= 1.3f && statDinero.ValorStat <= 1.3f && statComida.ValorStat <= 1.3f))
            return false;
        //Debug.Log((statElectricidad.ValorStat + cs.cartaDatos.derElect * 0.1f) + "E------------- " + (statGente.ValorStat + cs.cartaDatos.derGente * 0.1f) + "G------------- " + (statFelicidad.ValorStat + cs.cartaDatos.derFelic * 0.1f) + "F------------- " + (statDinero.ValorStat + cs.cartaDatos.derDinero * 0.1f) + "D------------- " + (statComida.ValorStat + cs.cartaDatos.derComida) * 0.1f);
        return true;
    }

    private void ChangeCircleStats()
    {
        
        
        if (carta.transform.position.x > 2)
        {
            if (cs.cartaDatos.derElect < 0){circuloElectricidad.color = Color.red;}
            if (cs.cartaDatos.derElect > 0) { circuloElectricidad.color = Color.green; }

            if (cs.cartaDatos.derGente < 0) { circuloPoblacion.color = Color.red; }
            if (cs.cartaDatos.derGente > 0) { circuloPoblacion.color = Color.green; }

            if (cs.cartaDatos.derFelic < 0) { circuloFelicidad.color = Color.red; }
            if (cs.cartaDatos.derFelic > 0) { circuloFelicidad.color = Color.green; }

            if (cs.cartaDatos.derDinero < 0) { circuloDinero.color = Color.red; }
            if (cs.cartaDatos.derDinero > 0) { circuloDinero.color = Color.green; }

            if (cs.cartaDatos.derComida < 0) { circuloComida.color = Color.red; }
            if (cs.cartaDatos.derComida > 0) { circuloComida.color = Color.green; }

        }
        if (carta.transform.position.x < -2)
        {
            if (cs.cartaDatos.izqElect < 0) { circuloElectricidad.color = Color.red; }
            if (cs.cartaDatos.izqElect > 0) { circuloElectricidad.color = Color.green; }

            if (cs.cartaDatos.izqGente < 0) { circuloPoblacion.color = Color.red; }
            if (cs.cartaDatos.izqGente > 0) { circuloPoblacion.color = Color.green; }

            if (cs.cartaDatos.izqFelic < 0) { circuloFelicidad.color = Color.red; }
            if (cs.cartaDatos.izqFelic > 0) { circuloFelicidad.color = Color.green; }

            if (cs.cartaDatos.izqDinero < 0) { circuloDinero.color = Color.red; }
            if (cs.cartaDatos.izqDinero > 0) { circuloDinero.color = Color.green; }

            if (cs.cartaDatos.izqComida < 0) { circuloComida.color = Color.red; }
            if (cs.cartaDatos.izqComida > 0) { circuloComida.color = Color.green; }
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
        // Si es MENOR O IGUAL a -0.3 saltamos a una carta concreta de derrota
        if (statElectricidad.ValorStat <= -0.3f)
        {
            //Debug.Log("Perder E");
            cs.cartaDatos = cartasQueUsamos[60];
        }
        else if (statGente.ValorStat <= -0.3f)
        {
            //Debug.Log("Perder G");
            cs.cartaDatos = cartasQueUsamos[61];
        }
        else if (statFelicidad.ValorStat <= -0.3f)
        {
            //Debug.Log("Perder F");
            cs.cartaDatos = cartasQueUsamos[62];
        }
        else if (statDinero.ValorStat <= -0.3f)
        {
            //Debug.Log("Perder D");
            cs.cartaDatos = cartasQueUsamos[63];
        }
        else if (statComida.ValorStat <= -0.3f)
        {
            //Debug.Log("Perder C");
            cs.cartaDatos = cartasQueUsamos[64];
        }

        // Si es MAYOR que 1.3 saltamos a una carta concreta de derrota
        else if (statElectricidad.ValorStat > 1.3f)
        {
            Debug.Log("Pasarse E");
            cs.cartaDatos = cartasQueUsamos[65];
            statElectricidad.ValorStat = 1.31f;
        }
        else if (statGente.ValorStat > 1.3f)
        {
            //Debug.Log("Pasarse G");
            cs.cartaDatos = cartasQueUsamos[66];
            statGente.ValorStat = 1.31f;
        }
        else if (statFelicidad.ValorStat > 1.3f)
        {
            //Debug.Log("Pasarse F");
            cs.cartaDatos = cartasQueUsamos[67];
            statFelicidad.ValorStat = 1.31f;
        }
        else if (statDinero.ValorStat > 1.3f)
        {
            //Debug.Log("Pasarse D");
            cs.cartaDatos = cartasQueUsamos[68];
            statDinero.ValorStat = 1.31f;
        }
        else if (statComida.ValorStat > 1.3f)
        {
            //Debug.Log("Pasarse C");
            cs.cartaDatos = cartasQueUsamos[69];
            statComida.ValorStat = 1.31f;
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
