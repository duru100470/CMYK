using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

namespace MessageChannel
{
    public class Channel<T>
    {
        private Action<T> _subscribers;

        public void Subscribe(Action<T> perform)
        {
            if (perform == null)
                throw new NullReferenceException();

            _subscribers += perform;
        }

        public void Unsubscribe(Action<T> perform)
        {
            _subscribers -= perform;
        }

        public void Notify(T message)
        {
            _subscribers?.Invoke(message);
        }
    }
}