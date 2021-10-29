using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UI;

namespace BBDD
{
    public class DatabaseConnectionsAdapter : IRequestInfo
    {
        public IEnumerator Login(string nickname, string password)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("nick", nickname);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/Login.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.error);
                    yield return null;
                }
                else
                {
                    if (www.downloadHandler.text.Contains("Wrong credentials"))
                    {
                        Debug.Log(www.downloadHandler.text);
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError("Wrong credentials");
                        yield return null;
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        string jsonArrayString = www.downloadHandler.text;
                        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
                        ServiceLocator.Instance.GetService<IUserInfo>().SetInfo(nickname, password, jsonArray[0].AsObject["CURRENT_POINTS"], jsonArray[0].AsObject["GLOBAL_POINTS"]);
                        ServiceLocator.Instance.GetService<IUserInfo>().GetUserInfo();
                        ServiceLocator.Instance.GetService<UI.UI>().OpenSpecificMenu("Play");
                    }
                }
            }
        }

        public IEnumerator RegisterUser(string nickname, string password)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("nick", nickname);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/RegisterUser.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);
                }
                else
                {
                    if (www.downloadHandler.text.Contains("Username is already taken"))
                    {
                        Debug.Log("Username is already taken");
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);

                    }
                    else if (www.downloadHandler.text.Contains("Error"))
                    {
                        Debug.Log(www.downloadHandler.text);
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);
                    }
                    else
                    {
                        Login(nickname, password);
                    }
                }
            }
        }

        public IEnumerator UpdateUser(string nickname, int currentPoints, int globalPoints)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("NICK", nickname);
            form.AddField("CURRENT_POINTS", currentPoints);
            form.AddField("GLOBAL_POINTS", globalPoints);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UpdateUser.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);
                }
                else
                {
                    if (www.downloadHandler.text.Contains("Record updated successfully"))
                    {
                        Debug.Log("Record updated successfully");
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);

                    }
                    else if (www.downloadHandler.text.Contains("Error"))
                    {
                        Debug.Log(www.downloadHandler.text);
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.downloadHandler.text);
                    }
                }
            }
        }

        public IEnumerator GetMaze()
        {
            WWWForm form = new WWWForm();

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/GetMaze.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("There isn't mazes to complete"))
                {
                    Debug.Log("There isn't mazes to complete. Creating a new one...");
                    //Hacer alguna viana aqui como un bool
                }
                else
                {
                    //Show results as text
                    Debug.Log(www.downloadHandler.text);
                    string jsonArrayString = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
                    ServiceLocator.Instance.GetService<IMazeInfo>().SetInfo(jsonArray[0].AsObject["ID"], jsonArray[0].AsObject["SEED"]);
                    ServiceLocator.Instance.GetService<IMazeInfo>().GetInfo();
                    //for (int i = 0; i < jsonArray.Count; i++)
                    //{
                    //    maze.id = jsonArray[i].AsObject["ID"];
                    //    maze.seed = jsonArray[i].AsObject["SEED"];
                    //}
                    yield return GetMessages(jsonArray[0].AsObject["ID"]);
                }
            }

        }

        public IEnumerator CreateMaze(int seed)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("seed", seed);
            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CreateMaze.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("Error"))
                {
                    Debug.Log("Error in maze creation");
                }
                else if (www.downloadHandler.text.Contains("New maze created"))
                {
                    Debug.Log("A new maze has been generated");
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }
        public IEnumerator UpdateMaze(int id)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("idMaze", id);
            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/UpdateMaze.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("Error"))
                {
                    Debug.Log("Error in maze updating");
                }
                else if (www.downloadHandler.text.Contains("Maze updated"))
                {
                    Debug.Log(www.downloadHandler.text);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                }
            }
        }

        public IEnumerator AskIfMazeFinished(int id)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("idMaze", id);
            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/AskIfMazeFinished.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                string result = www.downloadHandler.text;
            }
        }

        public IEnumerator GetMessages(int idMaze)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("idMaze", idMaze);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/GetMessagesFromMaze.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("There isn't messages"))
                {
                    Debug.Log("There isn't messages");
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    string jsonArrayString = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        ServiceLocator.Instance.GetService<IMessage>().SetInfo(jsonArray[i].AsObject["ID"],
                                                                               jsonArray[i].AsObject["MESSAGE"],
                                                                               jsonArray[i].AsObject["USER"],
                                                                               jsonArray[i].AsObject["POSITION"],
                                                                               jsonArray[i].AsObject["CHUNK"],
                                                                               jsonArray[i].AsObject["DATE"]);
                        ServiceLocator.Instance.GetService<IMessage>().GetInfo(i);

                    }
                }
            }
        }

        public IEnumerator CreateMessages(string message, int userId,string position,int chunk,string date, int idMaze)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("message", message);
            form.AddField("user", userId);
            form.AddField("position", position);
            form.AddField("chunk", chunk);
            form.AddField("date", date);
            form.AddField("idMaze", idMaze);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CreateMessage.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("Error"))
                {
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    string jsonArrayString = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        //Coger info del mensaje que se acaba de crear

                    }
                }
            }
        }

        public IEnumerator CreateTraps(int idMaze)
        {
            WWWForm form = new WWWForm();
            //Data we want to validate in php
            form.AddField("MAZE", idMaze);

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/CreateTraps.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                if (www.downloadHandler.text.Contains("Error"))
                {
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.error);
                }
                else
                {
                    Debug.Log(www.downloadHandler.text);
                    string jsonArrayString = www.downloadHandler.text;
                    JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

                    for (int i = 0; i < jsonArray.Count; i++)
                    {
                        //Coger info de la trampa que se acaba de crear

                    }
                }
            }
        }
        public IEnumerator GetLeaderboard()
        {
            WWWForm form = new WWWForm();

            using (UnityWebRequest www = UnityWebRequest.Post("http://localhost/GetLeaderboard.php", form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                    ServiceLocator.Instance.GetService<ErrorMessages>().ShowError(www.error);
                    yield return null;
                }
                else
                {
                    if (www.downloadHandler.text.Contains("0"))
                    {
                        Debug.Log("There isn't users registered yet");
                        ServiceLocator.Instance.GetService<ErrorMessages>().ShowError("There insn't users registered yet");
                        yield return null;
                    }
                    else
                    {
                        Debug.Log(www.downloadHandler.text);

                        string jsonArrayString = www.downloadHandler.text;
                        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;
                        for (int i = 0; i < jsonArray.Count; i++)
                        {
                           //Aqui guardar la info del leaderboard
                        }
                    }
                }
            }
        }


    }
}