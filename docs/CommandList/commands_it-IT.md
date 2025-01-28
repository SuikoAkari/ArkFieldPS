# Commands
Il server fornisce vari comandi per la gestione degli account, nemici e caricamento di scene. Tutti i comandi, i relativi argomenti e gli esempi per utilizzarli sono elencati di seguito:

Note: <> - required arguments; [] - optional arguments;

|Command        |Scopo                                |Argomenti                   |Target richiesto?|Comando completo                 |Esempio di utilizzo     |
|:--------------|:------------------------------------|:---------------------------|:----------------|:--------------------------------|:-----------------------|
|help           |Mostra tutti i comandi disponibili   |None                        |No               |help                             |help                    |
|target         |Seleziona l'utente attivo            |\<uid>                       |No               |target \<uid>                     |target 740623067        |
|scene          |Carica una scena                     |\<scene id>                  |Yes              |scene \<scene id>                 |scene 209               |
|kick           |Disconnette utente dalla sessione    |None                        |Yes              |kick                             |kick                    |
|spawn          |Spawna nemici vicino al player       |\<Enemy TemplateID> \<level>  |Yes              |spawn \<TID> \<level>              |spawn eny_0007_mimicw 20|
|account        |Crea un account nel database         |\<create\|reset> \<nickname>   |No               |account \<create\|reset> \<nickname>|account create test     |