using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBDD;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    int mazeId;
    bool isCompleted;
    public GameObject deadMenu;
    public Text deadMessageText;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator UpdateGame(bool isEndGame)
    {
        yield return StartCoroutine(setMazeId());
        yield return StartCoroutine(IsMazeFinised());

        if (isCompleted && isEndGame)
        {

            //te lo has pasado pero alguien antes tb
        }
        else if (!isCompleted && isEndGame)
        {
            //te lo has pasado el primero
        }
        else if (!isCompleted && !isEndGame)
        {
            print("Entre aqui");
            //CONTINUA
            //Vaciar lista señales
            ServiceLocator.Instance.GetService<IPullMessage>().clearPullList();
            //Bajar señales
            yield return StartCoroutine(ServiceLocator.Instance.GetService<IRequestInfo>().GetMessages(mazeId));
            //Subir señales
            foreach (Consumer.Message nextMessage in ServiceLocator.Instance.GetService<IPullMessage>().getPushList())
            {
                yield return StartCoroutine(ServiceLocator.Instance.GetService<IRequestInfo>().CreateMessages(nextMessage._msg, nextMessage._user, nextMessage._position, mazeId));
            }
            ServiceLocator.Instance.GetService<IPullMessage>().clearPushList();
            ServiceLocator.Instance.GetService<IPullMessage>().updateMessages();
        }
        else
        {
            //has muerto pero se ha completado
        }
    }

    public IEnumerator playerDead()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Controller>().SetIsDead(true);
        yield return UpdateGame(false);
        string message = "";
        float time = 0.0f;
        deadMenu.SetActive(true);
        if (isCompleted)
        {
            //TERMINADO
            message = "El laberinto ha sido completado, eres el perdedor";
            time = 6.0f;
        }
        else
        {
            message = "Has muerto";
            time = 3.0f;

        }
        yield return StartCoroutine(DeadMessage(message, time));
        if (!isCompleted)
        {
            Transform playerT = player.transform;
            playerT.position = ServiceLocator.Instance.GetService<MazeRender>().getStartPosition();
        }
        player.GetComponent<Controller>().SetIsDead(false);
        deadMenu.SetActive(false);
    }

    IEnumerator DeadMessage(string message, float seconds)
    {
        for (int i = 0; i < seconds; ++i)
        {
            deadMessageText.text = message + " " + (seconds - i);
            yield return new WaitForSeconds(1f);
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

    public void setIsCompleted(bool isCompleted)
    {
        this.isCompleted = isCompleted;
    }
}
