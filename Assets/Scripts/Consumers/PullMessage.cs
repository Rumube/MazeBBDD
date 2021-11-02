using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Consumer;
using System.Globalization;

public class PullMessage : MonoBehaviour, IPullMessage
{

    public List<Consumer.Message> pullMessages = new List<Consumer.Message>();
    [SerializeField]
    public List<Consumer.Message> pushMessages = new List<Consumer.Message>();
    public GameObject singGO;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(Consumer.Message newMessage in pushMessages)
        {
            Debug.Log("MENSAJE -->" + newMessage._msg);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            updateMessages();
        }
    }

    public void addToList(Consumer.Message newMessage)
    {
        pullMessages.Add(newMessage);
    }

    public void updateMessages()
    {
        foreach(Consumer.Message newMessage in pullMessages)
        {
            GameObject newSing = Instantiate(singGO, splitPositio(newMessage._position), Quaternion.identity);
            Message messageScript = newSing.GetComponent<Message>();
            messageScript.id = newMessage._id;
            messageScript.message = newMessage._msg;
        }
    }

    public void updatePullMessages(Consumer.Message newMessage)
    {
        pullMessages.Add(newMessage);
    }

    private Vector3 splitPositio(string stringV3)
    {
        Vector3 newVector;
        Debug.Log(stringV3);
        string[] coordenadas = stringV3.Split(',');
        Debug.Log(coordenadas[1]);
        newVector = new Vector3(float.Parse(coordenadas[0], CultureInfo.InvariantCulture), 
            float.Parse(coordenadas[1], CultureInfo.InvariantCulture), float.Parse(coordenadas[2], CultureInfo.InvariantCulture));
        Debug.Log("Coordenadas --> " + newVector);
        return newVector;
    }

}
