using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiasRandom : MonoBehaviour
{
    public GameObject TextBox;

    public int DiasTranscurridos = 0;

    public void RandomGenerator()
    {
        DiasTranscurridos = DiasTranscurridos + Random.Range(10, 41);
        TextBox.GetComponent<Text>().text = DiasTranscurridos + " dias como alcalde";
    }
}
