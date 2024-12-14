using System;
using Palmmedia.ReportGenerator.Core.Common;
using UnityEngine;
using System.Threading.Tasks;

public class ThirdTaskScript : MonoBehaviour
{
    public int AmountOfRunners;
    public float distanceBetweenRunners;
    public float Speed;
    public float PassDistance;

    public GameObject Camera;
    public GameObject CharacterPrefab;

    private GameObject[] humanRunners;
    private CameraScript cameraScript;

    private int currentRunner = -1;
    private bool activeTask = false;
    private bool firstLaunch = true;
    private Vector3 target;
    private Animator currAnimator;
    private bool isRunnerStopped = false;

    void Start()
    {
        cameraScript = Camera.GetComponent<CameraScript>();

        humanRunners = new GameObject[AmountOfRunners];

        for (int i = 0; i < humanRunners.Length; i++) {
            humanRunners[i] = Instantiate(CharacterPrefab, transform);
            humanRunners[i].transform.parent = transform;
            humanRunners[i].name = $"Runner{i + 1}";
            humanRunners[i].transform.position = new Vector3(i * distanceBetweenRunners, -10, 0);
            //runners[i].GetComponentInChildren<Renderer>().sharedMaterial = i % 2 == 0 ? RedMaterial : BlueMaterial;
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

        humanRunners[currentRunner].transform.position = Vector3.MoveTowards(humanRunners[currentRunner].transform.position, target, Time.deltaTime * Speed);

        if (Mathf.Abs(Vector3.Distance(humanRunners[currentRunner].transform.position, target)) <= PassDistance) RunnersMet();
        else
        {
            currAnimator = humanRunners[currentRunner].GetComponent<Animator>();
            if (currAnimator) currAnimator.SetFloat("MoveSpeed", Speed);
        }
    }

    void RunnersMet()
    {
        //isRunnerStopped = true;
        //if (currentRunner >= 0)
        //{
        //    currAnimator.SetFloat("MoveSpeed", 1f);
        //    currAnimator.SetBool("Pickup", true);

        //}
        //Task.Delay(new TimeSpan(0, 0, 0, 0, 1000)).ContinueWith(o => {
        isRunnerStopped = false;
        activeTask = false;
        if (currentRunner >= 0) {
            currAnimator.SetFloat("MoveSpeed", 0f);
            if (currentRunner < AmountOfRunners - 2) humanRunners[currentRunner + 1].transform.LookAt(humanRunners[currentRunner].transform);
            currAnimator.SetBool("Pickup", true);
        }
        currentRunner++;      

        if (currentRunner >= humanRunners.Length) currentRunner = 0;
        //humanRunners[currentRunner].GetComponent<Animator>().SetBool("Pickup", true);

        target = new Vector3(humanRunners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);

        Task.Delay(new TimeSpan(0, 0, 0, 0, 2000)).ContinueWith(o => {
            humanRunners[currentRunner].transform.LookAt(target);
            activeTask = true;
        });

        humanRunners[currentRunner].transform.LookAt(target);

        if (currentRunner < humanRunners.Length && humanRunners[currentRunner] != null)
        {
            target = new Vector3(humanRunners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);
            Debug.Log($"Наблюдаем за бегуном {humanRunners[currentRunner].name}");
            cameraScript.PickObjectToFollow(humanRunners[currentRunner]);
        }
    }

    public void ChangeActiveTask(bool status)
    {
        activeTask = status;
        firstLaunch = true;
        currentRunner = -1;
        if (status) {
            for (int i = 0; i < AmountOfRunners; i++)
            {
                humanRunners[i].transform.position = new Vector3(i * distanceBetweenRunners, -10, 0);
            }
        }
        
    }
}
