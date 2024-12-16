using UnityEngine;

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
    private GameObject stick;
    private CameraScript cameraScript;

    private int currentRunner = -1;
    private bool activeTask = false;
    private bool firstLaunch = true;
    private int stickPassingCycle = 1;
    private Vector3 target;
    private float maxRotation = 45f;
    private int multiplier = 1;
    
    void Start()
    {
        cameraScript = Camera.GetComponent<CameraScript>();

        runners = new GameObject[AmountOfRunners];
        stick = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        stick.transform.name = "stick";

        for (int i = 0; i < runners.Length; i++)
        {
            GameObject runnnerModel = new GameObject();
            GameObject body = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject leftLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject rightLeg = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject rightHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject leftHand = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GameObject head = GameObject.CreatePrimitive(PrimitiveType.Cube);

            GameObject rightHandPivot = new GameObject();
            GameObject leftLegPivot = new GameObject();
            GameObject rightLegPivot = new GameObject();

            body.transform.parent = runnnerModel.transform;
            leftLeg.transform.parent = runnnerModel.transform;
            rightLeg.transform.parent = runnnerModel.transform;
            rightHand.transform.parent = runnnerModel.transform;
            leftHand.transform.parent = runnnerModel.transform;
            head.transform.parent = runnnerModel.transform;

            if (i == 0) {
                stick.transform.parent = rightHand.transform;
                stick.transform.position = new Vector3(0f, -0.4f, 0f);
                stick.transform.Rotate(Vector3.forward, 90);
                stick.transform.transform.localScale = new Vector3(0.25f, 2f, 0.8f);
                stick.GetComponentInChildren<Renderer>().sharedMaterial = RedMaterial;
            }

            rightHandPivot.transform.parent = runnnerModel.transform;
            leftLegPivot.transform.parent = runnnerModel.transform;
            rightLegPivot.transform.parent = runnnerModel.transform;

            leftLeg.transform.name = "leftLeg";
            rightLeg.transform.name = "rightLeg";
            rightHand.transform.name = "rightHand";
            leftHand.transform.name = "leftHand";
            head.transform.name = "head";

            rightHandPivot.transform.name = "rightHandPivot";
            leftLegPivot.transform.name = "leftLegPivot";
            rightLegPivot.transform.name = "rightLegPivot";

            body.transform.position = new Vector3(0f, 1f, 0f);
            leftLeg.transform.position = new Vector3(0f, 0f, -0.23f);
            rightLeg.transform.position = new Vector3(0f, 0f, 0.31f);
            rightHand.transform.position = new Vector3(0f, 1f, -0.6f);
            leftHand.transform.position = new Vector3(0f, 1f, 0.6f);
            head.transform.position = new Vector3(0f, 1.8f, 0f);

            rightHandPivot.transform.position = new Vector3(0f, 1.5f, 0.5f);
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
        if (currentRunner > 0) {
            if (stickPassingCycle == 2) runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.RotateAround(runners[currentRunner - 1].transform.GetChild(6).gameObject.transform.position, new Vector3(0, 0, 1), Speed * Time.deltaTime * 30);
            if (runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.rotation.z >= 0.5) {
                stickPassingCycle = 3;
                runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.GetChild(0).transform.parent = runners[currentRunner].transform.GetChild(3).gameObject.transform;
                runners[currentRunner].transform.GetChild(3).gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f, -0.4f, 0f);
                runners[currentRunner].transform.GetChild(3).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                runners[currentRunner].transform.GetChild(3).gameObject.transform.GetChild(0).transform.transform.localScale = new Vector3(0.25f, 2f, 0.8f);
            }

            if (stickPassingCycle == 3) runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.RotateAround(runners[currentRunner - 1].transform.GetChild(6).gameObject.transform.position, new Vector3(0, 0, 1), -1 * Speed * Time.deltaTime * 30);
            if (runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.rotation.z <= 0) {
                stickPassingCycle = 1;
            }
        } else if (currentRunner == 0) {
            activeTask = true;
            stickPassingCycle = 1;
        } 


        if (AmountOfRunners == 0 || !activeTask || stickPassingCycle != 1) return;
        if (firstLaunch)
        {
            firstLaunch = false;
            RunnersMet();
        }

        runners[currentRunner].transform.position = Vector3.MoveTowards(runners[currentRunner].transform.position, target, Time.deltaTime * Speed);

        if (Mathf.Abs(Vector3.Distance(runners[currentRunner].transform.position, target)) <= PassDistance) RunnersMet();

        runners[currentRunner].transform.GetChild(1).gameObject.transform.RotateAround(runners[currentRunner].transform.GetChild(7).gameObject.transform.position, new Vector3(0, 0, 1), multiplier * Speed * Time.deltaTime * 30);
        runners[currentRunner].transform.GetChild(2).gameObject.transform.RotateAround(runners[currentRunner].transform.GetChild(8).gameObject.transform.position, new Vector3(0, 0, 1), -1 * multiplier * Speed * Time.deltaTime * 30);

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
        activeTask = false;

        if (currentRunner >= runners.Length)
        {
            runners[currentRunner - 1].transform.GetChild(3).gameObject.transform.GetChild(0).transform.parent = runners[0].transform.GetChild(3).gameObject.transform;
            runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f, -0.4f, 0f);
            runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 90f);
            runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.transform.localScale = new Vector3(0.25f, 2f, 0.8f);
            currentRunner = 0;
        }
        else stickPassingCycle = 2;

        target = new Vector3(runners[currentRunner].transform.position.x + distanceBetweenRunners, 0, 0);
        activeTask = true;
        cameraScript.PickObjectToFollow(runners[currentRunner]);

        if (currentRunner < runners.Length && runners[currentRunner] != null)
        {
            cameraScript.PickObjectToFollow(runners[currentRunner]);
            Debug.Log($"Наблюдаем за бегуном {runners[currentRunner].name}");
        } else activeTask = true;
    }

    public void ChangeActiveTask(bool status)
    {
        activeTask = status;
        firstLaunch = true;
        currentRunner = -1;
        for (int i = 0; i < AmountOfRunners; i++)
        {
            runners[i].transform.localPosition = new Vector3(i * distanceBetweenRunners, 0, 0);
            if (i != 0 && runners[i].transform.GetChild(3).gameObject.transform.childCount > 0) {
                runners[i].transform.GetChild(3).gameObject.transform.GetChild(0).transform.parent = runners[0].transform.GetChild(3).gameObject.transform;
                runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.localPosition = new Vector3(0f, -0.4f, 0f);
                runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.rotation = Quaternion.Euler(0f, 0f, 90f);
                runners[0].transform.GetChild(3).gameObject.transform.GetChild(0).transform.transform.localScale = new Vector3(0.25f, 2f, 0.8f);
            }
        }
    }
}
