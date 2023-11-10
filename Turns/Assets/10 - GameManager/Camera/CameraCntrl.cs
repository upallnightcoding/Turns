using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCntrl : MonoBehaviour
{
    [SerializeField] private GameData gameData;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateCameraPosition(int index)
    {
        Vector3 p = gameObject.transform.position;

        gameObject.transform.position = 
            new Vector3(p.x, gameData.cameraPosition[index], p.z);
    }
}
