using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationCntrl : MonoBehaviour
{

    void Start()
    {
        //StartCoroutine(RotateTile());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateTile(bool turn)
    {
        if (turn)
        {
            StartCoroutine(RotateTileClockwise());
        } else
        {
            StartCoroutine(RotateTileCounterClockwise());
        }
        
    }

    private IEnumerator RotateTileClockwise()
    {
        for (float startDeg = 0.0f; startDeg < 90.0f; startDeg+=1.0f)
        {
            transform.Rotate(new Vector3(0.0f, 1.0f, 0.0f));
            yield return null;
        }
    }

    private IEnumerator RotateTileCounterClockwise()
    {
        for (float startDeg = 0.0f; startDeg < 90.0f; startDeg += 1.0f)
        {
            transform.Rotate(new Vector3(0.0f, -1.0f, 0.0f));
            yield return null;
        }
    }
}
