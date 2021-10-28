using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Consumer
{
    public class Message : IMessage
    {
        public int _id { get; private set; }
        public string _msg { get; private set; }
        public int _user { get; private set; }
        public string _position { get; private set; }
        public int _chunk { get; private set; }
        public string _date { get; private set; }

        public List<Message> _messages = new List<Message>();
        public Message()
        {

        }
        public Message(int id, string msg, int user, string position, int chunk, string date)
        {
            _id = id;
            _msg = msg;
            _user = user;
            _position = position;
            _chunk = chunk;
            _date = date;
        }
        public void SetInfo(int id, string msg, int user, string position, int chunk, string date)
        {
            _messages.Add(new Message(id, msg, user, _position, chunk, date));
        }

        public void GetInfo(int index)
        {
            Debug.Log("Message id: " + _messages[index]._id + " message: " + _messages[index]._msg + " user_: " + _messages[index]._user + " position: " + _messages[index]._position + " chunk: " + _messages[index]._chunk + " date: " + _messages[index]._date);
        }

    }
}