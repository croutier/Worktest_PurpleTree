using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics
{
    Dictionary<string, BoxCollider2D> collisionAreas;


    static private CustomPhysics instance = null;

    static public CustomPhysics Instance 
    { get 
        { 
        if(instance == null){
            instance = new CustomPhysics();
            }
        return instance; 
        }
    }

    public void UpdateCollision(string key, BoxCollider2D area)
    {
        if (collisionAreas.ContainsKey(key))
        {
            collisionAreas[key] = area;
        }
        else
        {
            collisionAreas.Add(key, area);
        }
    }

    public void RemoveCollision(string key)
    {
        if (collisionAreas.ContainsKey(key))
        {
            collisionAreas.Remove(key);
        }
    }

    public List<BoxCollider2D> GetCollisions()
    {
        List<BoxCollider2D> areas = new List<BoxCollider2D>();
        foreach(string key in collisionAreas.Keys)
        {
            areas.Add(collisionAreas[key]);
        }
        return areas;
    }

}
