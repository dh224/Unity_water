using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitState
{
    Defind = 0,
    StandBy=1,
    Falling=2,
    Collisioned = 3,
}
public enum FruitsType
{
    one = 0,
    two = 1,
    three = 2,
    four = 3,
    five = 4,
    six = 5,
    seven = 6,
    eight = 7,
    nine = 8,
    ten = 9,
    eleven = 10
}
public class Fruits : MonoBehaviour
{
    public FruitsType fruitsType = FruitsType.one;
    public FruitState fruitState = FruitState.Defind;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        this.fruitState = FruitState.Collisioned;
        
        if (collision.gameObject.CompareTag("Fruit"))
        {
            collision.gameObject.GetComponent<Fruits>().fruitState = FruitState.Collisioned;
            if (collision.gameObject.GetComponent<Fruits>().fruitsType == this.fruitsType)
            {
                float pos_xy = this.transform.position.x + this.transform.position.y;
                float collision_xy = collision.transform.position.x + collision.transform.position.y;

                if(pos_xy > collision_xy && fruitsType < FruitsType.eleven)
                {
                    Vector2 pos = (this.transform.position + collision.transform.position) / 2.0f;
                    GameManager._instanceOfGameManager.CombineNewFruits(fruitsType, pos);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
