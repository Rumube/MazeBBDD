using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBDD;

public class GameManager : MonoBehaviour
{
    int mazeId;
    bool isCompleted;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator UpdateGame()
    {
        yield return StartCoroutine(setMazeId());
        yield return StartCoroutine(IsMazeFinised());
        if (isCompleted)
        {
            //TERMINADO
        }
        else
        {
            //CONTINUA
            //Vaciar lista señales
            ServiceLocator.Instance.GetService<IPullMessage>().clearPullList();
            //Bajar señales
            yield return StartCoroutine(ServiceLocator.Instance.GetService<IRequestInfo>().GetMessages(mazeId));
            //Subir señales
            foreach(Consumer.Message nextMessage in ServiceLocator.Instance.GetService<IPullMessage>().getPushList())
            {
                string newPos = nextMessage._position.Replace("(", "");
                newPos = newPos.Replace(")", "");
                yield return StartCoroutine(ServiceLocator.Instance.GetService<IRequestInfo>().CreateMessages(nextMessage._msg, nextMessage._user, newPos, mazeId));
            }
            
        }
    }

    public IEnumerator IsMazeFinised()
    {
        yield return StartCoroutine(
            ServiceLocator.Instance.GetService<IRequestInfo>().AskIfMazeFinished(mazeId)
        );
        isCompleted = ServiceLocator.Instance.GetService<IMazeInfo>().getCompleted();
        yield return 0;
    }

    public IEnumerator setMazeId()
    {
        mazeId = ServiceLocator.Instance.GetService<IMazeInfo>().getId();
        yield return 0;
    }

}
