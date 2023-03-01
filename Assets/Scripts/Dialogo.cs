using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Dialogo : MonoBehaviour
{
    
    public TextMeshProUGUI componeneteTexto;
    public Canvas canvastexto;
    [TextArea]
    public string[] frases;
    public float velocidadLetras;
    private int index;
    public GameObject carta;
    public cardscript cs;
    // Start is called before the first frame update
    void Start()
    {

        
        componeneteTexto.text = string.Empty;
        empezarDialogo();
        carta.SetActive(false);
        

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(componeneteTexto.text == frases[index])
            {
                sigFrase();
            }
            else
            {
                StopAllCoroutines();
                componeneteTexto.text = frases[index];
            }
        }
    }

    void empezarDialogo()
    {
        
        index = 0;
        StartCoroutine(escribeFrases());
    }

    IEnumerator escribeFrases()
    {
        yield return new WaitForSeconds(0.2f);
        foreach(char c in frases[index].ToCharArray())
        {
            componeneteTexto.text += c;
            yield return new WaitForSeconds(velocidadLetras);
        }
    }

    void sigFrase()
    {
        
        if (index < 2)
        {
            index++;
            componeneteTexto.text = string.Empty;
            StartCoroutine(escribeFrases());

        }

        else if ( index < frases.Length - 1)
        {
            int id = cs.cartaDatos.id;
            if(id < 2 ) {
                escondeUI();
                carta.SetActive(true); 
            }
           
            else {  
                cs.enabled = false;
                carta.SetActive(false);  
                
                canvastexto.enabled = true;
                index++;
                componeneteTexto.text = string.Empty;
                StartCoroutine(escribeFrases());
            }
               
            
        }
        else
        {
            gameObject.SetActive(false);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -1);

        }

        void escondeUI()
        {
            canvastexto.enabled = false;
        }
    }
}
