using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMinimapCamera : MonoBehaviour
{
    public GameObject player;

    private void LateUpdate()
    {
        Vector3 newPosition = player.transform.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;

       // transform.rotation = Quaternion.Euler(90f, player.transform.eulerAngles.y, 0f);
    }
}
