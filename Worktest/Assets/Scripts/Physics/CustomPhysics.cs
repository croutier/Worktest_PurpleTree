using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPhysics
{
    Dictionary<string, BoxCollider2D> collisionAreas = new Dictionary<string, BoxCollider2D>();


    static private CustomPhysics instance = null;

    static public CustomPhysics Instance
    { get
        {
            if (instance == null) {
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
        foreach (string key in collisionAreas.Keys)
        {
            areas.Add(collisionAreas[key]);
        }
        return areas;
    }

    public BoxCollider2D CustomRaycast2D(Vector2 origin, Vector2 direction, float magnitude)
    {

        foreach (string key in collisionAreas.Keys)
        {
            if (CheckCollision(origin, direction.normalized * magnitude, collisionAreas[key]))
            {
                return collisionAreas[key];
                // It shoud compare if this detects multiple colisions and return the closer one, but in this case there is only 3 collisons and the magnitudes should be low
            }
        }
        return null;
    }
    public BoxCollider2D CustomRaycast2D(Vector2 origin, Vector2 direction)
    {
        foreach (string key in collisionAreas.Keys)
        {
            if (CheckCollision(origin, direction, collisionAreas[key]))
            {
                return collisionAreas[key];
            }
        }
        return null;
    }
    public BoxCollider2D CustomRaycast2D(Vector2[] origins, Vector2 direction)
    {
        foreach (string key in collisionAreas.Keys)
        {
            foreach(Vector2 origin in origins)
            {
                if (CheckCollision(origin, direction, collisionAreas[key]))
                {
                    return collisionAreas[key];
                }
            }            
        }
        return null;
    }
    public BoxCollider2D CustomLinearRaycast2D(Vector2[] origins, Vector2 direction)
    {
        if(direction.x * direction.y != 0)
        {
            Debug.LogError("this function only accept one dimensional vectors");
            return null;
        }
        foreach (string key in collisionAreas.Keys)
        {
            foreach (Vector2 origin in origins)
            {
                if (CheckLinearCollision(origin, direction, collisionAreas[key]))
                {
                    return collisionAreas[key];
                }
            }
        }
        return null;
    }

    public bool CheckCollision(Vector2 origin, Vector2 direction, BoxCollider2D collision)
    {
        Vector2 colCenter = (Vector2)(collision.bounds.center) - origin;
        int yDirection = colCenter.y < 0 ? -1 : 1;
        int xDirection = colCenter.x < 0 ? -1 : 1;        
        //check if the BoxCollider isnt too far and if its in the same direction
        if (((Vector2)collision.bounds.ClosestPoint(origin)- origin).magnitude > direction.magnitude) 
        {
            return false;
        }
        // y  = x/vectro.x * vector.y || x = y/vector.y * vector.x
        float intersectX = (colCenter.x - (xDirection * collision.bounds.size.x / 2)) * (direction.y != 0 ? direction.x / direction.y : 1);
        float intersectY = (colCenter.y - (yDirection * collision.bounds.size.y / 2)) * (direction.x != 0 ? direction.y / direction.x : 1);

        Vector2 verticalIntersect = new Vector2(intersectX, (intersectX / direction.x) * direction.y);
        Vector2 horizontalIntersect = new Vector2((intersectY / direction.y) * direction.x, intersectY);
        //check if the intersection with the box is in the magnitude range and inside the boundries
        if (verticalIntersect.magnitude < direction.magnitude &&
            verticalIntersect.y > colCenter.y - collision.bounds.size.y / 2 &&
            verticalIntersect.y < colCenter.y + collision.bounds.size.y / 2){
            return true;
        }
        if (horizontalIntersect.magnitude < direction.magnitude &&
           horizontalIntersect.x > colCenter.x - collision.bounds.size.x / 2 &&
           horizontalIntersect.x < colCenter.x + collision.bounds.size.x / 2)
        {
            return true;
        }
        return false;
    }
    //optimal function to use for linear cases
    public bool CheckLinearCollision(Vector2 origin, Vector2 direction, BoxCollider2D collision)
    {
        int paralelCoord = direction.y == 0 ? 0 : 1;
        int perpendicularCoord = (paralelCoord - 1) * -1;
        Vector2 colCenter = (Vector2)(collision.bounds.center);
       // int sign = direction[paralelCoord] > 0? 1 : -1;
        
        //check if the vector is in the box
        if (origin[perpendicularCoord] > colCenter[perpendicularCoord] - collision.bounds.size[perpendicularCoord] / 2 &&
            origin[perpendicularCoord] < colCenter[perpendicularCoord] + collision.bounds.size[perpendicularCoord] / 2 &&
            (origin[paralelCoord] + direction[paralelCoord]) > (colCenter[paralelCoord] - collision.bounds.size[paralelCoord] / 2) &&
            (origin[paralelCoord] + direction[paralelCoord])  < (colCenter[paralelCoord] + collision.bounds.size[paralelCoord] / 2) )
        {
            return true;
        }
        return false;
    }
}
