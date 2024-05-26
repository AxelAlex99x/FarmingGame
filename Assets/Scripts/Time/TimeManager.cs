using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }
    [Header("Internal Clock")]
    [SerializeField]
    GameTimeStamp timeStamp;

    public float timeScale = 1.0f;

    [Header("Day n Night cycle")]
    public Transform sunTransform;

    List<ITimeTracker> listeners = new List<ITimeTracker>();
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        timeStamp = new GameTimeStamp(0,GameTimeStamp.Season.Spring,1,6,0);
        StartCoroutine(TimeUpdate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TimeUpdate()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/timeScale);
            Tick();
        }
    }
    public void Tick()
    {
        timeStamp.updateClock();

        foreach(ITimeTracker listener in listeners)
        {
            listener.ClockUpdate(timeStamp);
        }
        
        UpdateSunMovement();
    }

    void UpdateSunMovement()
    {
        int timeInMinutes = GameTimeStamp.HoursToMinute(timeStamp.hour) + timeStamp.minute;

        float sunAngle = .25f * timeInMinutes - 90;

        sunTransform.eulerAngles = new Vector3(sunAngle, 0, 0);
    }

    public GameTimeStamp GetGameTimestamp()
    {
        return new GameTimeStamp(timeStamp);
    }
    public void RegisterTracker(ITimeTracker listener)
    {
        listeners.Add(listener);
    }
    
    public void UnregisteredTracker(ITimeTracker listener)
    {
        listeners.Remove(listener);
    }
}
