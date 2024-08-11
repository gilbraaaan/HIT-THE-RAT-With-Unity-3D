using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPaluDeteksi : MonoBehaviour
{
    Manager manager;
    private void Awake()
    {
        manager = FindObjectOfType<Manager>();
    }
    void OnTriggerEnter(Collider TikusTerdeteksi)
    {
        if (TikusTerdeteksi.gameObject.tag == "Lubang")
        {
            manager.TikusTidakTerpukul();
        }        
        else if (TikusTerdeteksi.gameObject.tag == "AdaTikus")
        {
            manager.TikusTerpukul();
        }
    }
}
