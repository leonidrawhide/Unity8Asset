using UnityEngine;

public class FirstTaskScript : MonoBehaviour
{
    public int AmountOfPoints;
    public int Speed;
    public float XStep;
    public bool Loop;

    private int currPoint = 0;
    private Vector3[] targets;
    private bool forward = true;

    private bool activeTask = false;

    void Start()
    {
        targets = new Vector3[AmountOfPoints + 1];
        targets[0] = new Vector3(0, 0, 0);

        for (int i = 1; i < AmountOfPoints + 1; i++) {
            targets[i] = new Vector3(XStep * i, 0, 0);
        }
    }

    void Update()
    {
        if (AmountOfPoints == 0 || !activeTask) return;
        transform.position = Vector3.MoveTowards(transform.position, targets[currPoint], Time.deltaTime * Speed);
        if (transform.position == targets[currPoint])
        {
            if (forward) currPoint++;
            else if (Loop) currPoint--;

            if (currPoint == AmountOfPoints) forward = false;
            if (currPoint == 0) forward = true;
        }

    }

    public void ChangeActiveTask(bool status) {
        activeTask = status;
    }
}
