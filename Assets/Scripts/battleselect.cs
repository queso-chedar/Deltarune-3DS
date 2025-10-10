using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class battleselect : MonoBehaviour
{
    public GameObject[] Objectstodeactivate;
    public GameObject[] ObjectstoActivate;

    Text changetext;

    void Start()
    {
    }

    public void ButtonPressed()
    {
        foreach (GameObject Object in ObjectstoActivate)
        {
            Object.SetActive(true);
        }
        foreach (GameObject Object in Objectstodeactivate)
        {
            Object.SetActive(false);
        }
    }

    void Update()
    {
    }
}
