# Commands
The server provides various commands for managing accounts, enemies, and loading scenes. All commands, their arguments, and usage examples are listed below:

Note: <> - required arguments; [] - optional arguments;

|Command        |Purpose                              |Arguments                   |Target required? |Full command                        |Example usage           |
|:--------------|:------------------------------------|:---------------------------|:----------------|:-----------------------------------|:-----------------------|
|help           |Displays all available commands      |None                        |No               |help                                |help                    |
|target         |Selects the active user              |\<uid>                      |No               |target \<uid>                       |target 740623067        |
|scene          |Loads a scene                        |\<scene id>                 |Yes              |scene \<scene id>                   |scene 209               |
|kick           |Disconnects the user from the session|None                        |Yes              |kick                                |kick                    |
|spawn          |Spawns enemies near the player       |\<Enemy TemplateID> \<level>|Yes              |spawn \<TID> \<level>               |spawn eny_0007_mimicw 20|
|heal           |Heals characters from the team       |None                        |Yes              |heal                                |heal                    |
|add            |Adds items, weapons, characters      |\<item\|weapon\|char> \<id> \<amount/lvl>|Yes              |add \<item\|weapon\|char> \<id> \<amount/lvl>|add char chr_0007_ikut 79|
|remove         |Removes character                    |\<id>                       |Yes              |remove \<id>                        |remove chr_0007_ikut    |
|account        |Creates an account in the database   |\<create\|reset> \<nickname>|No               |account \<create\|reset> \<nickname>|account create test     |
|players        |Shows a list of all players          |None                        |No               |players                             |players                 |
|clear          |Clears console                       |None                        |No               |clear                               |clear                   |