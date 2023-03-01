using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cardscript : MonoBehaviour
{
    public BaseCarta cartaDatos;
    private SpriteRenderer spriteCarta;

    public bool isMouseOver = false;
    public bool isDragging = false;

    [SerializeField]public TextMeshProUGUI nombreCarta;
    [SerializeField] public TextMeshProUGUI descripcionCarta;
    [SerializeField] public TextMeshProUGUI descripcionLado;

    [SerializeField] public SpriteRenderer fondoTexto;
    [SerializeField] public Image imagen;

    private void Start()
    {
        spriteCarta = GetComponent<SpriteRenderer>();
        UpdateCartaUI(true);
    }
  
    private void OnMouseOver()
    {
        isMouseOver = true;
    }

     private void OnMouseDrag()
    {
        isDragging = true;
    }

    private void OnMouseExit()
    {
        isMouseOver = false;

    }
   


    public int izquierda()
    {
        //cartaDatos.luz += cartaDatos.izqElect;
       // Debug.Log("has escogida izquierda");
        Debug.Log(cartaDatos.id + " es mi id jeje, y " + cartaDatos.name + " mi nombre");
        

        return cartaDatos.sigIDizq;
    }

    public int derecha()
    {
        // Debug.Log("derecha");


        return cartaDatos.sigIDder;//cambiar a derecha
    }

    public void UpdateCartaUI(bool ladoIzq)
    {
        nombreCarta.text = cartaDatos.nombreCarta;
        descripcionCarta.text = cartaDatos.descripcionCarta;
        imagen.sprite = cartaDatos.imagen;
        //descripcionLado.text = cartaDatos.textoIzq;
        
        if (cartaDatos.aleatoria)
            SetSigID(ladoIzq);
    }

    private void SetSigID(bool izq)
    {
        int aleatorioID = Random.Range(6, 51);
        while(cartaDatos.id == aleatorioID)//solo par no pillarse a si misma//poner condicion cartas prohibidas//mirar como hacer un array xD
        {
            aleatorioID = Random.Range(6, 51);
        }

        cartaDatos.sigIDizq = aleatorioID;
        cartaDatos.sigIDder = aleatorioID;

        /*int aleatorio2 = aleatorioID;
        while (cartaDatos.id == aleatorio2 && aleatorio2 == aleatorioID)//solo par no pillarse a si misma//poner condicion cartas prohibidas//mirar como hacer un array xD
        {
            aleatorio2 = Random.Range(3, 6);
        }
        cartaDatos.sigIDder = aleatorio2;*/
        /*if (izq)
            cartaDatos.sigIDizq = aleatorioID;

        else
            cartaDatos.sigIDder = aleatorioID;*/
    }
}