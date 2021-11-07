using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Consumer;
using System.Globalization;

public class PullMessage : MonoBehaviour, IPullMessage
{

    public List<Consumer.Message> pullMessages = new List<Consumer.Message>();
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
            Vector4 currentPosition = splitPositio(newMessage._position);
            GameObject newSing = Instantiate(singGO,new Vector3(currentPosition.x, currentPosition.y, currentPosition.z), Quaternion.Euler(0.0f,currentPosition.w,0.0f));
            Message messageScript = newSing.GetComponent<Message>();
            messageScript.id = newMessage._id;
            messageScript.message = newMessage._msg;
        }
    }

    public void updatePushMessages(Consumer.Message newMessage)
    {//SE GUARDAN DATOS HACIA LA BASE DE DATOS
        pushMessages.Add(newMessage);
    }

    private Vector4 splitPositio(string stringV3)
    {
        Vector4 newVector;
        Debug.Log(stringV3);
        string[] coordenadas = stringV3.Split(',');
        Debug.Log(coordenadas[1]);
        newVector = new Vector4(float.Parse(coordenadas[0], CultureInfo.InvariantCulture), 
            float.Parse(coordenadas[1], CultureInfo.InvariantCulture), float.Parse(coordenadas[2], CultureInfo.InvariantCulture), float.Parse(coordenadas[3], CultureInfo.InvariantCulture));
        Debug.Log("Coordenadas --> " + newVector);
        return newVector;
    }

    public void clearPullList()
    {
        pullMessages.Clear();
    }

    public void clearPushList()
    {
        pushMessages.Clear();
    }

    public List<Consumer.Message> getPushList()
    {
        return pushMessages;
    }

}
