using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using System.Threading.Tasks;

public class SecondTaskScript : MonoBehaviour
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

    void Start()
    {
        cameraScript = Camera.GetComponent<CameraScript>();

        runners = new GameObject[AmountOfRunners];

        for (int i = 0; i < runners.Length; i++) {
            runners[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            runners[i].transform.parent = transform;
            runners[i].name = $"Runner{i + 1}";
            runners[i].transform.position = new Vector3(i * distanceBetweenRunners, -10, 0);
            runners[i].GetComponentInChildren<Renderer>().sharedMaterial = i % 2 == 0 ? RedMaterial : BlueMaterial;
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
        //if (currentRunner < 0 || currentRunner >= runners.Length || runners[currentRunner] == null) return;



        runners[currentRunner].transform.position = Vector3.MoveTowards(runners[currentRunner].transform.position, target, Time.deltaTime * Speed);
        if (Mathf.Abs(Vector3.Distance(runners[currentRunner].transform.position, target)) <= PassDistance) RunnersMet();
    }

    void RunnersMet()
    {
        currentRunner++;

        if (currentRunner >= runners.Length) currentRunner = 0;

        target = new Vector3(runners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);

        if (currentRunner < runners.Length && runners[currentRunner] != null)
        {
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
