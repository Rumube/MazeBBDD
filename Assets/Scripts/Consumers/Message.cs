using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Consumer
{
    public class Message : MonoBehaviour, IMessage
    {
        public int _id { get; private set; }
        public string _msg { get; private set; }
        public int _user { get; private set; }
        public string _position { get; private set; }
        public int _chunk { get; private set; }
        public string _date { get; private set; }

        public List<string> messages = new List<string>();

        //public List<Message> _messages = new List<Message>();
        public Message()
        {

        }

        private void Start()
        {
            messages.Add(_msg);
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
            _id = id;
            _msg = msg;
            _user = user;
            _position = position;
            _chunk = chunk;
            _date = date;
        }

        public void GetInfo(int index)
        {
            Debug.Log("Message id: " + _id + " message: " + _msg + " user_: " + _user + " position: " + _position + " chunk: " + _chunk + " date: " + _date);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Message"))
            {
                Message other = collision.gameObject.GetComponent<Message>();

                if (other.GetInstanceID() < transform.GetInstanceID())
                {
                    foreach (string msg in other.messages)
                    {
                        messages.Add(msg);
                    }
                    Destroy(collision.transform.parent.gameObject);
                }
                else Destroy(transform.parent.gameObject);
            }
        }

    }
}