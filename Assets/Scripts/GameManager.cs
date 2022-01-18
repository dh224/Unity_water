using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameState
{
    ready = 0,
    runing = 1,
    end = 2,
}
public class GameManager : MonoBehaviour
{

    public static GameManager _instanceOfGameManager;
    public GameState gameState = GameState.ready;
    public GameObject readline;
    public GameObject[] fruits;
    public Transform startTransform;
    public List<GameObject> currentFruitList;
    public UnityEngine.UI.Button startBtn;

    public UnityEngine.UI.Text currentSocreText;
    public UnityEngine.UI.Text historyScoreText;
    public int currentScore = 0;
    public int historyScore = 0;
    public Vector2 basewallpos;
    public GameObject basewall;
    // Start is called before the first frame update

    private void Awake()
    {
        _instanceOfGameManager = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Application.Quit();
        }
        if(gameState != GameState.runing)
        {
            return;
        }
        remindTheRedLine();
        if (currentFruitList.Count > 0)
        {
            if (Input.GetMouseButton(0))
            {
                var currentFruitWidth = currentFruitList[0].GetComponent<Collider2D>().bounds.size.x;
                float xVal =  Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                var xValLeft = xVal - currentFruitWidth/2;
                var xValRight = xVal + currentFruitWidth/2;
                if(xValLeft> -2.27 && xValRight < 2.27)
                {
                    currentFruitList[0].transform.position = new Vector3(xVal, currentFruitList[0].transform.position.y, currentFruitList[0].transform.position.z);
                }
                else
                {
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                currentFruitList[0].GetComponent<Rigidbody2D>().gravityScale = 2f;
                currentFruitList[0].GetComponent<Fruits>().fruitState = FruitState.Falling;
                currentFruitList.RemoveAt(0);
                Invoke("getRandomFruit",0.75f);
            }
        }
        Rigidbody2D rigidbody = basewall.GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector3.zero;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        rigidbody.constraints = RigidbodyConstraints2D.None;
        basewall.transform.position = basewallpos;
    }

    void getRandomFruit()
    {
        gameState = GameState.runing;
        int index = (int)Random.Range(0,9);
        GameObject fruitPrefabs = fruits[index];
        GameObject newFruit = Instantiate(fruitPrefabs, startTransform.position, fruitPrefabs.transform.rotation);
        newFruit.GetComponent<Rigidbody2D>().gravityScale = 0;
        newFruit.GetComponent<Fruits>().fruitState = FruitState.StandBy;
        currentFruitList.Add(newFruit);
    }
    public void GameStart()
    {
        if(fruits.Length == 0 || startTransform == null)
        { return; }
        
        int historyHighestScore = PlayerPrefs.GetInt("Highest Score in History.");
        historyScoreText.text = historyHighestScore.ToString();
        readline.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0f);
        basewallpos= basewall.transform.position;
        if (startBtn)
        {
            startBtn.gameObject.SetActive(false);
        }

        Invoke("getRandomFruit", 0.75f);
    }
    public void GameOver()
    {

        float historyHighestScore = PlayerPrefs.GetInt("Highest Score in History.");
        if(historyHighestScore < currentScore)
        {
            PlayerPrefs.SetInt("Highest Score in History.", currentScore);
        }
        if (startBtn)
        {
            startBtn.gameObject.SetActive(true);
        }
        Invoke("ReloadScene", 1f);
    }
    public void CombineNewFruits(FruitsType old,Vector2 next)
    {
        int index = (int)old + 1;
        GameObject fruitObj = fruits[index];
        GameObject newFruit = Instantiate(fruitObj, next, fruitObj.transform.rotation);

        currentScore += index;
        if (currentSocreText)
        {
            currentSocreText.text = currentScore.ToString();
        }
    }
    void remindTheRedLine()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Fruit");
        bool isHav = false;
        foreach (var item in list)
        {
            if(item.GetComponent<Fruits>().fruitState == FruitState.Collisioned)
            {
                if(item.transform.position.y >0.75f)
                {
                    isHav = true;
                    break;
                   
                }
            }
        }
        if (isHav)
        {
            readline.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 1f);
        }
        else
        {
            readline.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0f);
        }
    }


    void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("main");
    }
}
