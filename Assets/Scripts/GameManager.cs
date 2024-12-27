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
    private int m_FoodAmount = 100;

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
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;
        
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;
      
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1,1));
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
}
