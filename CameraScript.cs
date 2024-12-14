using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private GameObject currentObjectToFollow;

    void Update()
    {
        if (currentObjectToFollow) transform.position = currentObjectToFollow.transform.position + new Vector3(0, 1, -5);
    }

    public void PickObjectToFollow(GameObject followObject)
    {
        currentObjectToFollow = followObject;
    }
}
