using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLine : MonoBehaviour
{
    public bool isOver = false;
    public float speed = -2f;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Fruit"))
        {
           if( collision.gameObject.GetComponent<Fruits>().fruitState == FruitState.Collisioned)
            {
                GameManager._instanceOfGameManager.gameState = GameState.end;
                Invoke("GameEnd", 1f);
            }

            if (GameManager._instanceOfGameManager.gameState == GameState.end && isOver)
            {
                GameManager._instanceOfGameManager.currentScore += (int)(collision.gameObject.GetComponent<Fruits>().fruitsType + 1);
                GameManager._instanceOfGameManager.currentSocreText.text = GameManager._instanceOfGameManager.currentScore.ToString();
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.CompareTag("Floor"))
        {
            GameManager._instanceOfGameManager.GameOver();
        }
    }

    
    private void Update()
    {
        if (isOver)
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
        }
    }
    void GameEnd()
    {
        isOver = true;
    }
}
