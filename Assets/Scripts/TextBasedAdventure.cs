using System;
using UnityEditor.ShaderGraph;
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
        Invalid,    //0
        Empty,      //1
        Item,       //2
        Enemy,      //3
        Blockade,   //4
        Teleporter, //5
        Exit        //6
    }

       
        
        
        
        /// <summary>
        /// Sets 4x4 rooms in this struct
        /// </summary>
    
    private Room[,] dungeon = { 
                                {   new Room { Name = "Dark Cave",          Type = TileType.Empty       },
                                    new Room { Name = "Mossy Tunnel",       Type = TileType.Item        },
                                    new Room { Name = "Crystal Room",       Type = TileType.Teleporter  },
                                    new Room { Name = "Crimson Cathedral",  Type = TileType.Blockade    }
                                },
                                    
                                {   new Room { Name = "Bone Chamber",       Type = TileType.Enemy       },
                                    new Room { Name = "flooded Hall",      Type = TileType.Empty        },
                                    new Room { Name = "Iron Gate",          Type = TileType.Exit        },
                                    new Room { Name = "Ancient Library",    Type = TileType.Blockade    }
                                },
                                
                                {   new Room { Name = "Goblin Den",         Type = TileType.Empty       },
                                    new Room { Name = "Armory",             Type = TileType.Enemy       },
                                    new Room { Name = "Throne Room",        Type = TileType.Item        },
                                    new Room { Name = "Damaged Portal",     Type = TileType.Teleporter  }
                                },

                                {   new Room { Name = "Destroyed Camp",     Type = TileType.Empty       },
                                    new Room { Name = "Spider Nest",        Type = TileType.Empty       },
                                    new Room { Name = "Broken Bridge",      Type = TileType.Blockade    },
                                    new Room { Name = "Spooky Forest",      Type = TileType.Enemy       },
                                }
                              };
    


        /// <summary>
        /// descriptions for each room, index for this strong matches the Room Structs Indexing
        /// </summary>

    private string[,] tileDescriptions =
    {
        {
            "The cave is cold and dark... water drips from the unlit ceiling",
            "The tunnel's walls are covered in green moss, on the floor... there is something shiny",
            "Crystals are scattered about the room, in the center stands what appears to be a portal",
            "There is a Crimson glow to the room, there is fallen Church Organ and stacked pews blocking the path",
        },

        {
            "Bones float around the Bone Chamber, Looking as if they were scrafices to the Bone God! There is some sort of figure standing near the largest pile...",
            "The long hall has standing water that comes up to one's knees, the stench of mold and mildew fills the air... this water has clearly been here for a while",
            "A Large Iron gate stands ahead, this may be the way out of this place...",
            "Dust and cobwebbs coat the bookshelves of this room that has clearly not seen regular visitors anytime in the recent past"
        },

        {
            "It appears this room is the dwelling of at least one, if not several goblins... although it's inhabitence don't appear to be home...",
            "This Room appears to be the armory, there are swords, spears, and shields lined along the walls and on shelves, near the far end of the room there appears to be some sort of figure",
            "The feeling of a once magnificent room is clearly present upon entering this area, there is a Throne along the far wall and ancient, decayed royal banners hanging from either side, it appears there is something sitting in the actual throne seat",
            "Bricks and chunks of rubble lay about the area surrounding a Portal... and surprisingly, even with all the damage, the portal still looks to work"
        },

        {
            "It looks like some fellow adventurers once rested here... although you hope you don't meet the same fate, This Adventurer Camp is destroyed and ripped to shreads, and anything of value was stripped from it long ago",
            "Upon Entering, there doesn't seem to be anything interesting here, the giant cobwebs and spider egg sacks are enough to warn you that you shouldn't stay too long or search any harder than a glance around",
            "Ahead lies the remains of a bridge, at one point you might have been able to use this to get out of here... but those days are long gone... and the river it spans looks far too dangerous to attempt to cross",
            "This wooded area has a creepy feel to it... and that feeling is all the more cemented in your mind when you see some sort of figure lurking in the shadows just ahead"
        }
    };
    /// <summary>
    /// this bool array is here to check if a room is visited, it's a 4x4 grid that matches our dungeon struct format
    /// </summary>
    private bool[,] visitedRooms = new bool[4,4];
    private int playerRow = 0;
    private int playerCol = 0;
    private int playerHealth = 10;
    private int enemyDamage = 1;
    private int itemHealAmount = 2;

   

    
    void Start()
    {
        OutputTileInformation();
    }

    
    void Update()
    {
        /// this handles our keybinds for our look and use teleporter/portal interaction


        if (Input.GetKeyDown(KeyCode.L))
        {
            Look();
            return;
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            UseTeleporter();
            return;
        }
        
        
        
        
        bool wasKeyPressed = HandleInput(out int newRow, out int newCol);
        if (!wasKeyPressed)
        {
            return;
        }

        bool moved = SetPlayerPosition(newRow, newCol);

        if (moved)
        {
            OutputTileInformation();
        }

    }
    private void OutputTileInformation()
    {
        Debug.Log("You are in: " + dungeon[playerRow, playerCol].Name);

        /// this if statement is where we are checking if a room has been visited, and if its false it prints our tileDescription to the debug.log and switches the bool for that tile space to be true


        if (!visitedRooms[playerRow, playerCol])
        {
        Debug.Log(tileDescriptions[playerRow,playerCol]);
        visitedRooms[playerRow, playerCol] = true;
        }

        switch (dungeon[playerRow, playerCol].Type)
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

            case TileType.Blockade:
                Debug.Log("A blockade blocks your path.");
                break;

            case TileType.Teleporter:
                Debug.Log("You see a strange portal. Press T to use it.");
                break;

            case TileType.Exit:
                Debug.Log("You see a way out");
                break;
            default:
                Debug.LogError("Invalid TileType");
                break;
        }
    }


    /// <summary>
    /// Look() and UseTeleport() are dfined here, Look just prints the tileDescription, while our Useteleport method checks to see if there is or is not a portal before printing the correct action
    /// this is done with an if and else if statement
    /// </summary>
    private void Look()
    {
        Debug.Log("You look around.");
        Debug.Log(tileDescriptions[playerRow, playerCol]);
    }

    private void UseTeleporter()
    {
        if (dungeon[playerRow, playerCol].Type != TileType.Teleporter)
        {
            Debug.Log("There is no Portal here.");
            return;
        }

        if (playerRow == 0 && playerCol == 2)
        {
            playerRow = 2;
            playerCol = 3;
        }

        else if (playerRow == 2 && playerCol == 3)
        {
            playerRow = 0;
            playerCol = 2;
        }

        Debug.Log("You step into the portal...");
        OutputTileInformation();
    }
    /// <summary>
    /// Everything from this point down, excluding the tileType.Blockade debug.log message is the original script, I didn't change anything in here, I did remove the original notes to clean it up
    /// </summary>
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

    private bool SetPlayerPosition(int newRow, int newCol)
    {
        if (!CheckIfNewPositionInTileBounds(newRow, newCol))
        {
            Debug.Log("Can't go that way");
            return false;
        }

        if (dungeon[newRow, newCol].Type == TileType.Blockade)
        {
            Debug.Log("A blockade blocks your path.");
            return false;
        }

        playerRow = newRow;
        playerCol = newCol;
        return true;
    }

    private bool CheckIfNewPositionInTileBounds(int newRow, int newCol)
    {
        return (newRow >= 0 && newRow < dungeon.GetLength(0)) &&
               (newCol >= 0 && newCol < dungeon.GetLength(1));
    }

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

    