# 指令列表  

服务端提供了多种指令用于管理账户、生成敌人以及加载场景。所有指令、参数及使用示例如下：  

**注意：**`<>` 表示必填参数，`[]` 表示可选参数。  

|  指令   |           功能           |                参数                | 是否需要目标？ |                    完整指令                    |           使用示例           |
| :-----: | :----------------------: | :--------------------------------: | :------------: | :--------------------------------------------: | :--------------------------: |
|  help   |     显示所有可用指令     |                 无                 |       否       |                      help                      |             help             |
| target  |       选择当前用户       |              `<uid>`               |       否       |                 target `<uid>`                 |       target 740623067       |
|  scene  |       加载指定场景       |             `<场景ID>`             |       是       |                scene `<场景ID>`                |          scene 209           |
|  kick   |       断开用户连接       |                 无                 |       是       |                      kick                      |             kick             |
|  spawn  |  在玩家角色附近生成敌人  |       `<敌人模板ID> <等级>`        |       是       |            spawn `<模板ID> <等级>`             |   spawn eny_0007_mimicw 20   |
| account | 在数据库中创建或重置账户 |     `<create\|reset> <用户名>`     |       否       |       account `<create\|reset> <用户名>`       |     account create test      |
| players |       显示用户清单       |                 无                 |       否       |                    players                     |           players            |
|  clear  |      清空控制台日志      |                 无                 |       否       |                     clear                      |            clear             |
|  level  |    更新角色或物品等级    | <char_id/item_id> <等级> 或 <等级> |       是       | level <char_id/item_id> <等级> 或 level <等级> | level chr_001 80 或 level 80 |
---