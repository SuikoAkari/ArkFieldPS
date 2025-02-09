# 指令列表  

服务端提供了多种指令用于管理账户、生成敌人以及加载场景。所有指令、参数及使用示例如下：  

**注意：**`<>` 表示必填参数，`[]` 表示可选参数。  

| 指令      | 功能                                             | 参数                                    | 需要设置目标角色？ | 完整指令                                    | 使用示例                  |
|-----------|--------------------------------------------------|-----------------------------------------|--------------------|---------------------------------------------|---------------------------|
| help      | 显示所有可用指令                                 | 无                                      | 否                 | help                                        | help                      |
| target    | 选择当前用户                                     | `<uid>`                                 | 否                 | target `<uid>`                              | target 740623067          |
| scene     | 加载指定场景                                     | `<场景ID>`                              | 是                 | scene `<场景ID>`                            | scene 209                 |
| kick      | 断开用户连接                                     | 无                                      | 是                 | kick                                        | kick                      |
| spawn     | 在玩家角色附近生成敌人                           | `<敌人模板ID> <等级>`                   | 是                 | spawn `<模板ID> <等级>`                     | spawn eny_0007_mimicw 20  |
| heal      | 为队伍中的角色回复生命值                         | 无                                      | 是                 | heal                                        | heal                      |
| add       | 添加物品（item）、武器(weapon)或角色(char)       | `<item\|weapon\|char> <ID> <数量/等级>` | 是                 | add `<item\|weapon\|char> <ID> <数量/等级>` | add char chr_0007_ikut 80 |
| remove    | 移除角色                                         | `<角色ID>`                              | 是                 | remove `<角色ID>`                           | remove chr_0007_ikut      |
| level     | 设置物品或角色的等级                             | `[角色或武器ID] <等级>`                 | 是                 | level `[角色或武器ID] <等级>`               | level wpn_funnel_0010 80  |
| charinfo  | 显示角色信息                                     | `<角色ID>`                              | 是                 | charinfo `<角色ID>`                         | charinfo chr_0007_ikut    |
| nickname  | 修改游戏内昵称                                   | `<昵称>`                                | 是                 | nickname `<昵称>`                           | nickname blob             |
| account   | 在数据库中创建或重置账户                         | `<create\|reset> <用户名>`              | 否                 | account `<create\|reset> <用户名>`          | account create test       |
| players   | 显示用户清单                                     | 无                                      | 否                 | players                                     | players                   |
| clear     | 清空控制台日志                                   | 无                                      | 否                 | clear                                       | clear                     |
| unlockall | 解锁所有权限                                     | 无                                      | 是                 | unlockall                                   | unlockall                 |
| idlist    | 显示所有角色(chars)、敌人(enemies)和场景(scenes)的id | `<chars\|enemies\|scenes>`              | 否                 | idlist `<chars\|enemies\|scenes>`           | idlist chars              |

---
> [!WARNING]警告
> `level` 指令: 如果你没有指定具体的`id`字段, 那么等级变化将会被应用在**所有**武器和干员身上