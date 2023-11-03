using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BoardCntrl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            RotateTile(false);
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RotateTile(true);
        }
    }

    private void RotateTile(bool turn)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mousePosition), out RaycastHit hit, 1000))
        {
            if (hit.transform.parent.TryGetComponent<TileCntrl>(out TileCntrl controller))
            {
                controller.rotateTile(turn);
            }
        }

    }
}
