using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class leaderboardScript : MonoBehaviour
{
    public GameObject prefabFila;
    public Transform padreFila;


    public InputField input;
    public GameObject CampoEscritura;


    int diasJugador;
    static List<string> listaNombrePueblos = new List<string>();
    static List<int> listaDiasPueblos = new List<int>();

    private void Start()
    {
        diasJugador = PlayerPrefs.GetInt("player_score");
        if (listaNombrePueblos.Count == 0)
        {
            listaNombrePueblos.Add("Benicasim");
            listaDiasPueblos.Add(100);
            listaNombrePueblos.Add("Altea");
            listaDiasPueblos.Add(90);
            listaNombrePueblos.Add("Villareal");
            listaDiasPueblos.Add(80);
            listaNombrePueblos.Add("Burjasot");
            listaDiasPueblos.Add(70);
            listaNombrePueblos.Add("Paiporta");
            listaDiasPueblos.Add(60);
            listaNombrePueblos.Add("Onda");
            listaDiasPueblos.Add(50);
            listaNombrePueblos.Add("Peniscola");
            listaDiasPueblos.Add(40);
            listaNombrePueblos.Add("Benidorm");
            listaDiasPueblos.Add(30);
            for (int i = 0; i < listaDiasPueblos.Count; i++)
            {
                insertaFila((i + 1).ToString(), listaNombrePueblos[i], listaDiasPueblos[i]);
            }
        }
    }

    private void addValue(string pueblo)
    {
        listaNombrePueblos.Add(pueblo);
        listaDiasPueblos.Add(diasJugador);

        BubbleSort();

        foreach (Transform item in padreFila)
        {
            Destroy(item.gameObject);
        }
        for (int i = 0; i < listaDiasPueblos.Count; i++)
        {
            insertaFila((i + 1).ToString(), listaNombrePueblos[i], listaDiasPueblos[i]);
        }
    }

    public void insertaFila(string pos, string nombrePueblo, int nDias)
    {
        
        GameObject newGo = Instantiate(prefabFila, padreFila);
        Text[] texts = newGo.GetComponentsInChildren<Text>();
        texts[0].text = pos;
        texts[1].text = nombrePueblo;
        texts[2].text = nDias.ToString() + " dias";
        if(nombrePueblo == input.text)
        {
            texts[0].color = Color.green;
            texts[1].color = Color.green;
        }
        
    }

    public static void BubbleSort()
    {
        bool itemMoved = false;
        do
        {
            itemMoved = false;
            for (int i = 0; i < listaDiasPueblos.Count - 1; i++)
            {
                if (listaDiasPueblos[i] < listaDiasPueblos[i + 1])
                {
                    int lowerValue = listaDiasPueblos[i + 1];
                    string lowerName = listaNombrePueblos[i + 1];

                    listaDiasPueblos[i + 1] = listaDiasPueblos[i];
                    listaNombrePueblos[i + 1] = listaNombrePueblos[i];

                    listaDiasPueblos[i] = lowerValue;
                    listaNombrePueblos[i] = lowerName;

                    itemMoved = true;
                }
            }
        } while (itemMoved);
    }

    public void Menu()
    {
        SceneManager.LoadScene("ScenaManu");
    }

    public void enterName()
    {
        addValue(input.text);
        CampoEscritura.SetActive(false);
    }
}
