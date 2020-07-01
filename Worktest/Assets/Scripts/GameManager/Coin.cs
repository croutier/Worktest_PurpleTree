using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public delegate void CoinDelegate();
    public CoinDelegate Score;

    [SerializeField] float animDuration = 2.0f;

    GameObject uICoin;
    Vector2 initialPos;
    Vector2 deletedPos;

    float currentLifespan;
    float maxLifespan;

    bool collected = false;

    private void Update()
    {
        if (!collected)
        {
            if (currentLifespan <= 0)
            {
                DeleteCoin();
            }
            else
            {
                float colorPercent = 1 - (Mathf.Sqrt(currentLifespan) / (Mathf.Sqrt(maxLifespan)));
                GetComponent<SpriteRenderer>().color = (Color.Lerp(Color.white, Color.red, colorPercent));

                currentLifespan -= Time.deltaTime;
            }
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == ("Player"))
        {
            CoinColected();
        }
    }

    public void Spawn(GameObject uICoin)
    {
        this.uICoin = uICoin;
        deletedPos = (Vector2)transform.position;        
    }
    public void Respawn(float posX, float spanTime)
    {
        transform.position = new Vector2(posX, transform.position.y);
        initialPos = new Vector2(posX, transform.position.y);
        currentLifespan = spanTime;
        maxLifespan = spanTime;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    private void CoinColected()
    {
        StartCoroutine(CoinAnimation());
        collected = true;
        GetComponent<SpriteRenderer>().color = Color.white;
    }    
    IEnumerator CoinAnimation()
    {        
        float journey = 0f;
        while (journey <= animDuration)
        {
            journey = journey + Time.deltaTime;
            float percent = Mathf.Clamp01(journey / animDuration);
            transform.position = Vector2.Lerp(initialPos, uICoin.transform.position, percent);
            yield return null;
        }
        if(Score!= null)
        {
            Score();
        }
        DeleteCoin();
        
    }

    private void DeleteCoin()
    {
        gameObject.SetActive(false);
        transform.position = deletedPos;
    }

    private void OnDestroy()
    {
        if (Score != null)
        {
            CleanDelegate();
        }
    }
    private void CleanDelegate()
    {
        Delegate[] functions = Score.GetInvocationList();
        for (int i = 0; i < functions.Length; i++)
        {
            Score -= (CoinDelegate)functions[i];
        }
    }
}
