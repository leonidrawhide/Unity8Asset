using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class VectromExample : MonoBehaviour
{
    public Transform Point1;
    public Transform Point2;
    public float Speed;
    public bool EnableMove;
    public bool ZRotation;
    public float RotationSpeed;

    private Vector3 startPosition;
    private Vector3 endPosition;


    // Start is called before the first frame update
    void Start()
    {
        //Vector3 vec = new(1,1,0);
        ////vec.Set(5, 5, 5);
        ////transform.position = point1.position;
        //Debug.Log(Vector3.Angle(Vector3.right, Vector3.up));
        //transform.rotation = Quaternion.Euler(45, 45, 45);
   
        startPosition = Point1.position;
        endPosition = Point2.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (ZRotation) transform.Rotate(0, 0, Time.deltaTime * RotationSpeed);
        if (!EnableMove) return;
        //transform.LookAt(point1);
        transform.position = Vector3.Lerp(transform.position, endPosition, Time.deltaTime * Speed);
        //Debug.Log(transform.position);
        //Debug.Log(startPosition);
        //Debug.Log(endPosition);
        //Debug.Log(Vector3.Distance(transform.position, endPosition));
        if (Vector3.Distance(transform.position, endPosition) < 0.01) {
            Debug.Log("called");
            Vector3 tmp = startPosition;
            startPosition = endPosition;
            endPosition = tmp;
        }
    }
}
