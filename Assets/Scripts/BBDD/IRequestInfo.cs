using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BBDD{
    public interface IRequestInfo
    {
        public IEnumerator Login(string username, string password);
        public IEnumerator RegisterUser(string username, string password);
        public IEnumerator UpdateUser(string username, int currentPoints, int global_points);
        public IEnumerator GetMaze();
        public IEnumerator CreateMaze(int seed);
        public IEnumerator UpdateMaze(int id);
        public IEnumerator AskIfMazeFinished(int id);
        public IEnumerator GetMessages(int idMaze);
        public IEnumerator CreateMessages(string message, int userId, string position, int chunk, string date, int idMaze);
        public IEnumerator CreateTraps(int idMaze);
        public IEnumerator GetLeaderboard();
    }
}