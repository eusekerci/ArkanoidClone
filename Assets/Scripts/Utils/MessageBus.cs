using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid
{
    public abstract class ArkEvent { }

    public static class MessageBus
    {
        public static void Publish<T>(T evnt) where T : ArkEvent
        {
            UniRx.MessageBroker.Default.Publish(evnt);
        }

        public static UniRx.IObservable<T> OnEvent<T>() where T : ArkEvent
        {
            return UniRx.MessageBroker.Default.Receive<T>();
        }

        public static void ClearAllEvents()
        {
            Type[] allTypes = Utils.GetChildrenTypesOf<ArkEvent>();
            foreach (var type in allTypes)
            {
                UniRx.MessageBroker.Default.Remove(type);
            }
        }
    }
}