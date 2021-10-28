using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BBDD{
    public interface IRequestInfo
    {
        public IEnumerator Login(string username, string password);
        public IEnumerator RegisterUser(string username, string password);
         public IEnumerator GetMaze();
        public IEnumerator CreateMaze(int seed);
        public IEnumerator UpdateMaze(int id);
        public IEnumerator IsMazeFinished(int id);
        public IEnumerator GetMessages(int idMaze);
    }
}