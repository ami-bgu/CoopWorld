using UnityEngine;
using System.Collections;
using CoopWorld;

public class StartScript : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        Debug.Log("starting!");
        CoopWorldInput.Init();

        CoopEvents.Instance.RegisterToConnectEvent(Spawn);

        InvokeRepeating("HandleCoopEvents", 0, 0.2f);

 
    }

    // Update is called once per frame
    void Update()
    {

    }

    // This function is called when the MonoBehaviour will be destroyed
    public void OnDestroy()
    {
        CoopWorldInput.Destroy();
    }

    public void HandleCoopEvents()
    {
        CoopEvents.Instance.HandleEvents();
    }

    public void Spawn(int playerId)
    {
        GameObject gameObject = (GameObject)Instantiate(Resources.Load("CoopRobot"));
        gameObject.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f));
        gameObject.SendMessage("CoopStart", playerId);
    }
}
