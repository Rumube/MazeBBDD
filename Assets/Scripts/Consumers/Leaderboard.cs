using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour, ILeaderboard
{
    Dictionary<string, int> leaders;
    public void SetInfo(string nombre, int globalPoints)
    {
        print("Nombre: "+nombre+" global points "+globalPoints);
        leaders.Add(nombre,globalPoints);
    }

    // Start is called before the first frame update
    void Start()
    {
        leaders = new Dictionary<string, int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
