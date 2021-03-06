using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoSingleton<EventManager>
{
    private static Dictionary<string, Action<EventParam>> eventDictionary = new Dictionary<string, Action<EventParam>>();

    public static void StartListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Add(eventName, listener);
        }
    }

    public static void StopListening(string eventName, Action<EventParam> listener)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            eventDictionary[eventName] = thisEvent;
        }
        else
        {
            eventDictionary.Remove(eventName);
        }
    }

    public static void TriggerEvent(string eventName, EventParam eventParam)
    {
        Action<EventParam> thisEvent;
        if (eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent?.Invoke(eventParam);
        }
    }
}

public struct EventParam
{
    public Inputs input;
    public Information information;
    public Items items;
    public int eventint;
    public float eventFloat;
}

public struct Inputs
{
    public bool isAttack;
    public bool isRun;
    public bool isInventory;
    public bool isStore;
    public bool isSetting;
    public Vector2 moveVector;
}

public struct Information
{
    public int maxHp;
    public int hp;
}
