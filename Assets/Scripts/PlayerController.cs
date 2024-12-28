using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    
    private bool m_IsGameOver;

    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        MoveTo(cell);
    }
  
    public void MoveTo(Vector2Int cell)
    {
        m_CellPosition = cell;
        transform.position = m_Board.CellToWorld(m_CellPosition);
    }

    private void Update()
    {
        if (m_IsGameOver)
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
            {
                GameManager gameManager = GameManager.Instance;
                gameManager.StartNewGame();
            }
            return;
        }
        
        Vector2Int newCellTarget = m_CellPosition;
        bool hasMoved = false;

        if(Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y += 1;
            hasMoved = true;
        }
        else if(Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            newCellTarget.y -= 1;
            hasMoved = true;
        }
        else if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x += 1;
            hasMoved = true;
        }
        else if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            newCellTarget.x -= 1;
            hasMoved = true;
        }

        if (hasMoved)
        {
            //check if the new position is passable, then move there if it is.
            BoardManager.CellData cellData = m_Board.GetCellData(newCellTarget);

            if(cellData != null && cellData.Passable)
            {
                GameManager.Instance.TurnManager.Tick();

                if (cellData.ContainedObject.IsUnityNull())
                {
                    MoveTo(newCellTarget);
                }
                else if(cellData.ContainedObject.PlayerWantsToEnter())
                {
                    MoveTo(newCellTarget);
                    //Call PlayerEntered AFTER moving the player! Otherwise not in cell yet
                    cellData.ContainedObject.PlayerEntered();
                }
            }
        }
    }
    
    public void GameOver()
    {
        m_IsGameOver = true;
    }
    
    public void ResetGame()
    {
        m_IsGameOver = false;
    }
}
