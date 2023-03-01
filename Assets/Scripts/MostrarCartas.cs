using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MostrarCartas : MonoBehaviour
{
    public BaseCarta card;

    public TextMeshProUGUI nombreCarta;
    public TextMeshProUGUI descripcionCarta;

    public Image imagen;

    void Start()
    {
        nombreCarta.text = card.nombreCarta;
        descripcionCarta.text = card.descripcionCarta;

        imagen.sprite = card.imagen;
    }

}
