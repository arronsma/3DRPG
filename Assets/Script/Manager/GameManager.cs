using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    CharacterStats player;

    List<IEndGameObserver> endGameObservers = new List<IEndGameObserver>();
    public void RegisterPlayer(CharacterStats player)
    {
        this.player = player;
    }

    public void AddObserver(IEndGameObserver observer)
    {
        if(!endGameObservers.Contains(observer))
        {
            endGameObservers.Add(observer);
        }
    }

    public void RemoveObserver(IEndGameObserver observer)
    {
        endGameObservers.Remove(observer);
    }

    public void Notify()
    {
        foreach(var observer in endGameObservers)
        {
            observer.EndNotify();
        }
    }
}
