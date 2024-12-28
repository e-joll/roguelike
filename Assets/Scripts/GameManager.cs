using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
  
    public BoardManager BoardManager;
    public PlayerController PlayerController;

    public TurnManager TurnManager { get; private set;}
    
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    private Label m_LevelLabel;
    
    private int m_FoodAmount = 100;
    
    private int m_CurrentLevel = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
      
        Instance = this;
    }
  
    void Start()
    {
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;
      
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        
        m_LevelLabel = UIDoc.rootVisualElement.Q<Label>("LevelLabel");
        m_LevelLabel.text = "Level : " + m_CurrentLevel;
        
        NewLevel();
    }
    
    void OnTurnHappen()
    {
        ChangeFood(-1);
    }
    
    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel.text = "Food : " + m_FoodAmount;
    }
    
    public void NewLevel()
    {
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));

        m_CurrentLevel++;
        
        m_LevelLabel.text = "Level : " + m_CurrentLevel;
    }
}
