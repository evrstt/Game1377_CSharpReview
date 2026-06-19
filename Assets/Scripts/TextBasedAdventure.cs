using UnityEngine;


public class TextBasedAdventure : MonoBehaviour
{
    [System.Serializable]
    public struct Room
    {
        public string Name;
        public TileType Type;
    }

    [System.Serializable]
    public struct RoomRow
    {
        public Room[] rooms;
    }

    public enum TileType
    {
        Invalid,
        Empty,
        Item,
        Enemy,
        Exit,
    }

    private Room[,] dungeon = { 
                                {   new Room { Name = "Dark Cave",    Type = TileType.Empty},
                                    new Room { Name = "Mossy Tunnel", Type = TileType.Item},
                                    new Room { Name = "Crystal Room", Type = TileType.Empty} },
                                
                                {   new Room { Name = "Bone Chamber", Type = TileType.Enemy },
                                    new Room { Name = "Flooded Hall", Type = TileType.Empty},
                                    new Room { Name = "Iron Gate",    Type = TileType.Exit } }
                              };
    

    private string[,] tileNames = { { "Dark Cave"   /* 0,0 */,  "Mossy Tunnel" /* 0,1 */,   "Crystal Room" /* 0,2 */ },
                                    { "Bone Chamber"/* 1,0 */,  "Flooded Hall" /* 1,1 */,   "Iron Gate"              },
                                    { "Goblin Den",             "Armory",                   "Throne Room"            }
                                    };

    private TileType[,] tileTypes = {   { TileType.Empty, TileType.Item,  TileType.Empty},
                                        { TileType.Enemy, TileType.Empty, TileType.Exit },
                                        { TileType.Empty, TileType.Enemy, TileType.Item }
                                    };

    private int playerRow = 0;
    private int playerCol = 0;
    private int playerHealth = 10;
    private int enemyDamage = 1;
    private int itemHealAmount = 2;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        OutputTileInformation();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool wasKeyPressed = HandleInput(out int newRow, out int newCol);
        if (!wasKeyPressed)
        {
            return;
        }
        SetPlayerPosition(newRow, newCol);
        OutputTileInformation();
    }

    private void OutputTileInformation()
    {
        Debug.Log("You are in: " + tileNames[playerRow, playerCol]);

        switch (tileTypes[playerRow, playerCol])
        {
            case TileType.Empty:
                Debug.Log("There is nothing here.");
                break;
            case TileType.Enemy:
                Debug.Log("Oooo a spooky ghost");
                EncounterEnemy();
                break;
            case TileType.Item:
                Debug.Log("You see a shiny object");
                ItemPickup();
                break;
            case TileType.Exit:
                Debug.Log("You see a way out");
                break;
            default:
                Debug.LogError("Invalid TileType");
                break;
        }
    }

    private void EncounterEnemy()
    {
        PlayerTakeDamage(enemyDamage);
    }
    
    private void ItemPickup()
    {
        PlayerHeal(itemHealAmount);
    }

    private void PlayerHeal(int heal)
    {
        playerHealth += heal;
        Debug.Log("You get healed. Your health is now " + playerHealth);
    }

    private void PlayerTakeDamage(int damage)
    {
        playerHealth -= damage;
        Debug.Log("You get hit. Your health is now " + playerHealth);
        if (playerHealth <= 0)
        {
            playerHealth = 0;
            Debug.Log("You are dead");
        }
    }

    /// <summary>
    /// Sets the player position to a new row and column position
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    private void SetPlayerPosition(int newRow, int newCol)
    {
        if (CheckIfNewPositionInTileBounds(newRow, newCol))
        {
            playerRow = newRow;
            playerCol = newCol;
        }
        else
        {
            Debug.Log("Can't go that way");
        }
    }

    /// <summary>
    /// Determine if the new row and column position are within the bounds of the tiles
    /// </summary>
    /// <param name="newRow"></param>
    /// <param name="newCol"></param>
    /// <returns>True if it is within the bounds, false if not</returns>
    private bool CheckIfNewPositionInTileBounds(int newRow, int newCol)
    {
        return (newRow >= 0 && newRow < tileNames.GetLength(0)) && (newCol >= 0 && newCol < tileNames.GetLength(1));
    }

    /// <summary>
    /// Handles the player's input and sets potential new position in the tileNames array
    /// </summary>
    /// <param name="newRow">new row position</param>
    /// <param name="newCol">new column position</param>
    /// <returns>True if an input was pressed, false if not</returns>
    private bool HandleInput(out int newRow, out int newCol)
    {
        bool hasPressedKey = true;
        newRow = playerRow;
        newCol = playerCol;
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("You pressed " + KeyCode.D);
            newCol++;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("You pressed " + KeyCode.A);
            newCol--;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("You pressed " + KeyCode.W);
            newRow--;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("You pressed " + KeyCode.S);
            newRow++;
        }
        else
        {
            hasPressedKey = false;
        }
        return hasPressedKey;
    }

}
