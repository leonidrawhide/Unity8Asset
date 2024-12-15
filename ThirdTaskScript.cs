using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Threading.Tasks;

public class ThirdTaskScript : MonoBehaviour
{
    public int AmountOfRunners;
    public float distanceBetweenRunners;
    public float Speed;
    public float PassDistance;

    public GameObject Camera;
    public Material RedMaterial;
    public Material BlueMaterial;

    private GameObject[] runners;
    private CameraScript cameraScript;

    private int currentRunner = -1;
    private bool activeTask = false;
    private bool firstLaunch = true;
    private Vector3 target;
    private float maxRotation = 45f;
    private int multiplier = 1;
    

    void Start()
    {
        cameraScript = Camera.GetComponent<CameraScript>();

        runners = new GameObject[AmountOfRunners];

        for (int i = 0; i < runners.Length; i++)
        {
            GameObject runnnerModel = new GameObject();
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject leftLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject rightLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);

            GameObject leftLegPivot = new GameObject();
            GameObject rightLegPivot = new GameObject();

            body.transform.parent = runnnerModel.transform;
            leftLeg.transform.parent = runnnerModel.transform;
            rightLeg.transform.parent = runnnerModel.transform;
            rightHand.transform.parent = runnnerModel.transform;
            leftHand.transform.parent = runnnerModel.transform;
            head.transform.parent = runnnerModel.transform;

            leftLegPivot.transform.parent = runnnerModel.transform;
            rightLegPivot.transform.parent = runnnerModel.transform;

            leftLeg.transform.name = "leftLeg";
            rightLeg.transform.name = "rightLeg";
            rightHand.transform.name = "rightHand";
            leftHand.transform.name = "leftHand";
            head.transform.name = "head";

            leftLegPivot.transform.name = "leftLegPivot";
            rightLegPivot.transform.name = "rightLegPivot";

            body.transform.position = new Vector3(0f, 1f, 0f);
            leftLeg.transform.position = new Vector3(0f, 0f, -0.23f);
            rightLeg.transform.position = new Vector3(0f, 0f, 0.31f);
            rightHand.transform.position = new Vector3(0f, 1f, -0.6f);
            leftHand.transform.position = new Vector3(0f, 1f, 0.6f);
            head.transform.position = new Vector3(0f, 1.8f, 0f);

            leftLegPivot.transform.position = new Vector3(0f, 0.5f, -0.31f);
            rightLegPivot.transform.position = new Vector3(0f, 0.5f, 0.31f);

            body.transform.transform.localScale = new Vector3(0.4f, 1f, 1f);
            leftLeg.transform.transform.localScale = new Vector3(0.25f, 1f, 0.25f);
            rightLeg.transform.transform.localScale = new Vector3(0.25f, 1f, 0.25f);
            rightHand.transform.transform.localScale = new Vector3(0.25f, 1f, 0.25f);
            leftHand.transform.transform.localScale = new Vector3(0.25f, 1f, 0.25f);
            head.transform.transform.localScale = new Vector3(0.55f, 0.58f, 0.45f);

            body.GetComponentInChildren<Renderer>().sharedMaterial = i % 2 == 0 ? RedMaterial : BlueMaterial;
            
            runners[i] = runnnerModel;

            runners[i].transform.parent = transform;
            runners[i].name = $"Runner{i + 1}";
            runners[i].transform.position = new Vector3(i * distanceBetweenRunners, -10, 0);
        }
    }

    void Update()
    {
        if (AmountOfRunners == 0 || !activeTask) return;
        if (firstLaunch)
        {
            firstLaunch = false;
            RunnersMet();
        }

        runners[currentRunner].transform.position = Vector3.MoveTowards(runners[currentRunner].transform.position, target, Time.deltaTime * Speed);

        if (Mathf.Abs(Vector3.Distance(runners[currentRunner].transform.position, target)) <= PassDistance) RunnersMet();

        runners[currentRunner].transform.GetChild(1).gameObject.transform.RotateAround(runners[currentRunner].transform.GetChild(6).gameObject.transform.position, new Vector3(0, 0, 1), multiplier * Speed * Time.deltaTime * 30);
        runners[currentRunner].transform.GetChild(2).gameObject.transform.RotateAround(runners[currentRunner].transform.GetChild(7).gameObject.transform.position, new Vector3(0, 0, 1), -1 * multiplier * Speed * Time.deltaTime * 30);

        if (runners[currentRunner].transform.GetChild(1).gameObject.transform.rotation.z >= 0.35) multiplier = -1;
        if (runners[currentRunner].transform.GetChild(1).gameObject.transform.rotation.z <= -0.35) multiplier = 1;

    }

    void RunnersMet()
    {
        if (currentRunner >= 0) {
            runners[currentRunner].transform.GetChild(1).gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            runners[currentRunner].transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0f, 0f, -0.23f);
            runners[currentRunner].transform.GetChild(2).gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 1));
            runners[currentRunner].transform.GetChild(2).gameObject.transform.localPosition = new Vector3(0f, 0f, 0.31f);
        }

        currentRunner++;

        if (currentRunner >= runners.Length) currentRunner = 0;

        target = new Vector3(runners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);

        if (currentRunner < runners.Length && runners[currentRunner] != null) {
            target = new Vector3(runners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);
            Debug.Log($"Наблюдаем за бегуном  {runners[currentRunner].name}");
            cameraScript.PickObjectToFollow(runners[currentRunner]);
        }
    }

    public void ChangeActiveTask(bool status)
    {
        activeTask = status;
        firstLaunch = true;
        currentRunner = -1;
        for (int i = 0; i < AmountOfRunners; i++)
        {
            runners[i].transform.position = new Vector3(i * distanceBetweenRunners, -10, 0);
        }
    }
}
