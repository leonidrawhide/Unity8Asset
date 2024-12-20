using System;
using UnityEngine;
using System.Timers;
using System.Threading.Tasks;

public class ButtonsScript : MonoBehaviour
{
    public GameObject FirstTaskObjects;
    public GameObject SecondTaskObjects;
    public GameObject ThirdTaskObjects;

    public GameObject Camera;
    public GameObject CameraReturnObject;

    public float Speed;

    private int upperTask;

    private float LowerYIndex = -10;
    private float UpperYIndex = 0;

    private Vector3 targetUpperPosition = Vector3.zero;
    private Vector3 targetLowerPosition = new(0, -10, 0);

    private CameraScript cameraScript;
    private GameObject firstTaskSphere;
    private FirstTaskScript firstTaskScript;
    private SecondTaskScript secondTaskScript;
    private ThirdTaskScript thirdTaskScript;

    void Start()
    {
        cameraScript = Camera.GetComponent<CameraScript>();
        cameraScript.PickObjectToFollow(CameraReturnObject);
        firstTaskSphere = FirstTaskObjects.transform.Find("FirstTaskSphere").gameObject;

        firstTaskScript = firstTaskSphere.GetComponent<FirstTaskScript>();
        secondTaskScript = SecondTaskObjects.GetComponent<SecondTaskScript>();
        thirdTaskScript = ThirdTaskObjects.GetComponent<ThirdTaskScript>();
    }

    void Update()
    {
        if (upperTask == 1)
        {
            SecondTaskObjects.transform.position = Vector3.MoveTowards(SecondTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);
            ThirdTaskObjects.transform.position = Vector3.MoveTowards(ThirdTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);

            if (SecondTaskObjects.transform.position == targetLowerPosition && ThirdTaskObjects.transform.position == targetLowerPosition)
            {
                FirstTaskObjects.transform.position = Vector3.MoveTowards(FirstTaskObjects.transform.position, targetUpperPosition, Time.deltaTime * Speed);
                if (FirstTaskObjects.transform.position == targetUpperPosition)
                {
                    Task.Delay(new TimeSpan(0, 0, 0, 0, 500)).ContinueWith(o => { 
                        firstTaskScript.ChangeActiveTask(true);
                        cameraScript.PickObjectToFollow(firstTaskSphere);
                    });
                    upperTask = 0;
                }
            }

        }
        else if (upperTask == 2)
        {
            FirstTaskObjects.transform.position = Vector3.MoveTowards(FirstTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);
            ThirdTaskObjects.transform.position = Vector3.MoveTowards(ThirdTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);

            if (FirstTaskObjects.transform.position == targetLowerPosition && ThirdTaskObjects.transform.position == targetLowerPosition)
            {
                SecondTaskObjects.transform.position = Vector3.MoveTowards(SecondTaskObjects.transform.position, targetUpperPosition, Time.deltaTime * Speed);
                if (SecondTaskObjects.transform.position == targetUpperPosition) {
                    Task.Delay(new TimeSpan(0, 0, 0, 0, 500)).ContinueWith(o => { secondTaskScript.ChangeActiveTask(true); });
                    upperTask = 0;
                }
            }
        }
        else if (upperTask == 3) {
            SecondTaskObjects.transform.position = Vector3.MoveTowards(SecondTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);
            FirstTaskObjects.transform.position = Vector3.MoveTowards(FirstTaskObjects.transform.position, targetLowerPosition, Time.deltaTime * Speed);

            if (SecondTaskObjects.transform.position == targetLowerPosition && FirstTaskObjects.transform.position == targetLowerPosition)
            {
                ThirdTaskObjects.transform.position = Vector3.MoveTowards(ThirdTaskObjects.transform.position, targetUpperPosition, Time.deltaTime * Speed);
                if (ThirdTaskObjects.transform.position == targetUpperPosition) {
                    Task.Delay(new TimeSpan(0, 0, 0, 0, 500)).ContinueWith(o => { thirdTaskScript.ChangeActiveTask(true); });
                    upperTask = 0;
                }
            }
        }
    }

    public void ShowTask(int taskNum)
    {
        firstTaskScript.ChangeActiveTask(false);
        secondTaskScript.ChangeActiveTask(false);
        thirdTaskScript.ChangeActiveTask(false);
        cameraScript.PickObjectToFollow(CameraReturnObject);
        Task.Delay(new TimeSpan(0, 0, 0, 0, 500)).ContinueWith(o => { upperTask = taskNum; });
    }
}
