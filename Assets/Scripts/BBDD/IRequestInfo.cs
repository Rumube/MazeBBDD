using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BBDD{
    public interface IRequestInfo
    {
        public IEnumerator GetRequest(string uri);
        public IEnumerator Login(string username, string password);
        public IEnumerator RegisterUser(string username, string password);
         public IEnumerator GetMaze();
        public IEnumerator CreateMaze(int seed);
    }
}