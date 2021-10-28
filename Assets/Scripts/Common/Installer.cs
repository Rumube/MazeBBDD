using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BBDD;
using UI;
using Consumer;
namespace Common{
    public class Installer : MonoBehaviour
    {
        public UI.UI _ui;
        private void Awake()
        {
            var databaseConnectionAdapter = new DatabaseConnectionsAdapter();
            ServiceLocator.Instance.RegisterService<IRequestInfo>(databaseConnectionAdapter);

            var userInfo = new UserInfo();
            ServiceLocator.Instance.RegisterService<IUserInfo>(userInfo);

            var mazeInfo = new Maze();
            ServiceLocator.Instance.RegisterService<IMazeInfo>(mazeInfo);

            var messagesInfo = new Consumer.Message();
            ServiceLocator.Instance.RegisterService<IMessage>(messagesInfo);

            var errorMessages = new ErrorMessages();
            ServiceLocator.Instance.RegisterService(errorMessages);

            ServiceLocator.Instance.RegisterService(_ui);
        }

        private void Start()
        {
            //No recuerdo para que era esto
            ServiceLocator.Instance.GetService<IUserInfo>().Init();

        }
    }
}