using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pruebaStats : MonoBehaviour
{
    public ModificaTama�o gente;
    public ModificaTama�o felicidad;
    public ModificaTama�o comida;
    public ModificaTama�o electricidad;
    public ModificaTama�o dinero;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //comida
        if (Input.GetKeyUp(KeyCode.M) && comida.ValorStat > 0f)
        {
            comida.ValorStat -= 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.J) && comida.ValorStat < 1f)
        {
            comida.ValorStat += 0.1f;
        }
        //electricidad
        if (Input.GetKeyUp(KeyCode.N) && electricidad.ValorStat > 0f)
        {
            electricidad.ValorStat -= 0.1f;
        }
        if (Input.GetKeyUp(KeyCode.H) && electricidad.ValorStat < 1f)
        {
            electricidad.ValorStat += 0.1f;
        }

    }

    /*
    public void subirElectricidad()
    {
        if (Input.GetMouseButton(1))
        {
            comida.statComida -= 0.1f;
        }
        
    }
    */
}
