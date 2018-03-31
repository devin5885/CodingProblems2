# Minesweeper

## Problem
Design & implement a text-based minesweeper game. Minesweeper is the classic
 single-player computer game where an NxN grid has B mines (or bombs) hidden
 across the grid. The remaining cells are either blank or have a number behind
 them. The numbers reflect the number of bombs in the surrounding eight cells.
The user then uncovers a cell. If it is a bomb, the player loses. If it is a
number, the number is exposed. If it is a blank cell, this cell and all adjacent
blank cells (up to and including the surrounding numeric cells) are exposed.
 The player wins when all non-bomb cells are exposed. The player can also flag
certain places as potential bombs. THis doesn't affect game play, other than
to block the user from accidentally clicking a cell that is thought to have a
bomb.

## Requirements

### Possible Future Enhancements

- (Safe uncover) Classic Minesweeper also supports clicking both mouse buttons
at the same time on a numbered cell this causes any flagged cells to be ignored
 and adjacent number cells to be exposed. Note that this operation fails if there
(and is a no-op) if are non-exposed, non flagged cells, which may be a bomb (this
behavior will not be supported.)

- This version of minesweeper will not support a timer.

- This version will not support customizing the grid size and number of bombs. The
game will always be 9x9 and have 10 bombs.

- This version will not keep track of previously won and lost games.

### Classes

---

#### Cell

##### Description
Describes a cell in the game board.

##### Publicly Settable/Retrievable Data
- Whether this cell is exposed. (Defaults to false)
- Whether the cell is flagged. (Defaults to false)
- Whether this cell is a bomb. (Defaults to false)
- The bomb count for adjacent cells. (Defaults to 0) Only significant for non-bomb cells.

##### Public Behaviors
- Constructor - Initializes the cell data to defaults.

##### Design Notes
- The cell will not have any knowledge of the game board or its position
 there-in (i.e.) rows & columns, this is inappropriate information for
 this level.

- The cell will not have a flip or toggleGuess method, these methods provide
little value.
 
- In some cases it doesn't make sense for the properties to be changed
after the game has been initialized, so we could have a flag that would
lock the settings once the game is initialized, but for simplicity we won't.
 
----

#### Board

##### Publicly Retrievable Data
- The size of the board.
- The bomb count of the board.

##### Private Data
- The game board grid of cells.

- The bomb flags list, this is a list of boolean values using during game initialization
indicating which cells will contain bombs. The list is a flattened out version of the
cell locations in the game grid. The list is normally generated randomly during game
initialization but can be specified directly for testing purposes.

##### Public Behaviors

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | Constructor                                                        |
| Parameters  | The game board size                                                |
|             | The count of bombs                                                 |
| Returns     | None                                                               |
| Description | Constructs the game board using the specified size and bomb count. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | Constructor                                                        |
| Parameters  | The game board size                                                |
|             | Bomb flags list                                                    |
| Returns     | None                                                               |
| Description | Constructs the game board using the specified size and bomb flags list (used to create a specific bomb layout, primarily for testing). |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | InitBoard                                                          |
| Parameters  | None                                                               |
| Returns     | None                                                               |
| Description | Initializes the board, including creating cell objects, initializing the appropriate cells to bombs & updating the counts for non-bomb cells. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | OnTurn                                                             |
| Parameters  | Turn Operation                                                     |
|             | Selected Row Index                                                 |
|             | Selected Column Index                                              |
| Returns     | Turn Result                                                        |
| Description | See below                                                          |

Handles the players turn by:
1. Getting the specified cell & checking for null. If null returning an invalid cell selection result.
2. Handling the operation:

Operations:

Unflag:
1. Check that the cell is flagged, if not return an cell is not flagged result.
2. Unflag the cell and return a cell unflagged result.

Flag
1. Check that the cell is exposed, if so return an cell is already exposed result.
2. Check that the cell is already flagged, if not return an cell is already flagged result.
3. Flag the cell and return a cell flagged result.
  
Expose
1. Check that the cell is exposed, if so return an cell is already exposed result.
2. Check whether the cell is a bomb, if so return a player loses result. 
3. Expose the cell and adjacent cells.
4. Check for a player win. If so return player wins.
5. Return a cell exposed result.

Unknown
1. Return unknown operation result.

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | GetCell                                                            |
| Parameters  | Row Index                                                          |
|             | Column Index                                                       |
| Returns     | The cell object                                                    |
| Description | Retrieves the specified cell given the row & column. Also, checks whether the row & column are valid. If not returns null. (Expected case). |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | GetUnFlaggedBombCountRemaining                                     |
| Parameters  | None                                                               |
| Returns     | The count                                                          |
| Description | Retrieves the # of bombs minus the number of flagged cells. Note that if a cell is incorrectly flagged this # could be negative. |

##### Private Helpers

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | InitBombCells                                                      |
| Parameters  | None                                                               |
| Returns     | None                                                               |
| Description | See below:                                                         |

Initializes the bomb cells randomly, by:  
1. Constructing a list of booleans, all set to false.  
2. Setting the first n elements in the list to true, where n is the bomb count.  
3. For each of these elements, generating a random index & swapping the list entries.  
4. Finally, for each entry in the list that is true, set the corresponding cell to a bomb cell.  

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | InitBombCounts                                                     |
| Parameters  | None                                                               |
| Returns     | None                                                               |
| Description | Goes through all cells & increments the bomb counts for non-bomb cells using the helper function, IncrementBombCount |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | IncrementBombCount                                                 |
| Parameters  | Row Index                                                          |
|             | Column Index                                                       |
| Returns     | None                                                               |
| Description | Increments the bomb count for a cell based on the bombs around the cell. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | ShowAdjacentCells                                                  |
| Parameters  | Row Index                                                          |
|             | Column Index                                                       |
| Returns     | None                                                               |
| Description | Recursively shows the specified cell and all adjacent cells with a 0 bomb count. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | ShowAllCells                                                       |
| Parameters  | None                                                               |
| Returns     | None                                                               |
| Description | Shows all cells.                                                   |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | CheckForPlayerWin                                                  |
| Parameters  | None                                                               |
| Returns     | None                                                               |
| Description | Checks whether the player has won the game.                        |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | IsValidRowIndex                                                    |
| Parameters  | Row Index                                                          |
| Returns     | True if the index is valid, false otherwise.                       |
| Description | Checks whether the specified row index is valid.                   |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | IsValidColumnIndex                                                 |
| Parameters  | Column Index                                                       |
| Returns     | True if the index is valid, false otherwise.                       |
| Description | Checks whether the specified column index is valid.                |

##### Design Notes
- The board will not keep the count of remaining unflagged bomb count cells this will be
calculated after each turn (using GetUnFlaggedBombCountRemaining).

---

#### Game

##### Private Data
- The game board.

##### Behaviors

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | Constructor                                                        |
| Parameters  | The game board size                                                |
|             | The count of bombs                                                 |
| Returns     | None                                                               |
| Description | Constructs the game board using the specified size and bomb count. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | Constructor                                                        |
| Parameters  | The game board size                                                |
|             | Bomb flags list                                                    |
| Returns     | None                                                               |
| Description | Constructs the game board using the specified size and bomb flags list (used to create a specific bomb layout, primarily for testing). |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | Play                                                               |
| Parameters  | None                                                               |
| Returns     | The Game Result                                                    |
| Description | Main game driver, plays the game. Returns when the current game ends. |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | GetCell                                                            |
| Parameters  | Row Index                                                          |
|             | Column Index                                                       |
| Returns     | The cell object                                                    |
| Description | Pass-through to Board.GetCell                                      |

|             |                                                                    |
|-------------|--------------------------------------------------------------------|
| Name        | GetUnFlaggedBombCountRemaining                                     |
| Parameters  | None                                                               |
| Returns     | The count                                                          |
| Description | Pass-through to Board.GetUnFlaggedBombCountRemaining.              |

##### Events

- UpdateGameBoard - Called when the game board has been updated & thus UI needs to be updated.  
-- Data: None

- PlayersTurn - Called when it is the players turn.

##### Enumerations

###### Game Result
| Game Result             | Description                                                        |
|------------------------ |--------------------------------------------------------------------|
| Player Wins             | The player wins the game.                                          |
| Player Loses            | The player loses the game.                                         |
| Player Quits            | The player quit the game.                                          |
| Game Aborted            | The game aborted due to an error.                                  |

###### Data
| Event Name     | Input/Output | Description                                         |
|----------------|--------------|-----------------------------------------------------|
| Turn Operation | Input        | The operation that the player selected (See below)  |
| Row Index      | Input        | The column index the player selected.               |
| Column Index   | Input        | The column index the player selected.               |

- TurnResult - Called when the players action from a turn has been processed.

###### Data
| Event Name     | Input/Output | Description                                         |
|----------------|--------------|-----------------------------------------------------|
| Turn Result    | Output       | The result of the turn. (See below)                 |
| Row Index      | Output       | The column index that was acted on.                 |
| Column Index   | Output       | The column index that was acted on.                 |

##### Event Data

###### Turn Operation
| Turn Operation | Description                                            |
|--------------- |:-------------------------------------------------------|
| Unknown        | An unknown operation was specified (should not occur). |
| Quit           | The player wants to quit.                              |
| Expose         | The player wants to expose the selected cell.          |
| Flag           | The player wants to flag the selected cell.            |
| Unflag         | The player wants to unflag the selected cell.          |

Note: This referred to as UserPlay in CtCI.

###### Turn Result
| Turn Result             | Description                                                        |
|------------------------ |:-------------------------------------------------------------------|
| Invalid Cell Selected   | An invalid cell was specified (should not occur).                  |
| Unknown Error           | An unknown error occurred processing the request.                  |
| Cell Is Already Exposed | The specified cell is already visible, thus no operation occurred. |
| Cell Is Already Flagged | The specified cell is flagged, thus no operation occurred.         |
| Cell Is Not Flagged     | The specified cell not flagged, thus no operation occurred.        |
| Cell Exposed            | The selected non-bomb cell (and corresponding adjacent cells as appropriate) were exposed. |
| Cell Flagged            | The selected cell was flagged. |
| Cell Unflagged          | The selected cell was unflagged. |
| Player Loses            | A bomb cell was selected and the player loses. Note that before this event is fired, an Update Game Board event will be received with the bomb cell unhidden. (The game will end immediately after the turn result is processed). |
| Player Wins             | There are no more hidden non-bomb cells, thus the player wins. Note that before this event is fired, an Update Game Board event will be received with the game board completely unhidden.  (The game will end immediately after the turn result is processed). |

Note: This referred to as UserPlayResult in CtCI.

##### Design Notes
