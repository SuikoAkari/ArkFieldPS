# Scene list

|ID |Workability |Json name               |Image                          |Note  |
|:--|:-----------|:-----------------------|:------------------------------|------|
|99 |True        |base01_dg001            |![Image](./LevelImages/99.png) |Id's 99, 220, 160 - same scene|
|220|True        |base01_dg002            |![Image](./LevelImages/220.png)|      |
|98 |True        |base01_lv001            |![Image](./LevelImages/98.png) |      |
|160|True        |base01_lv002            |![Image](./LevelImages/160.png)|      |
|167|True        |blackbox_assebling_1    |![Image](./LevelImages/167.png)|Id's 167-151 (all `blackbox`) - it's the same scene|
|156|True        |blackbox_assebling_2    |![Image](./LevelImages/156.png)|      |
|159|True        |blackbox_asseblingbomb_1|                               |      |
|89 |True        |blackbox_basic_1        |                               |      |
|90 |True        |blackbox_belt_1         |                               |      |
|169|True        |blackbox_belt_2         |                               |      |
|157|True        |blackbox_bus_1          |                               |      |
|165|True        |blackbox_bus_2          |                               |      |
|94 |True        |blackbox_cmpt_1         |                               |      |
|161|True        |blackbox_connector_1    |                               |      |
|166|True        |blackbox_converger_1    |                               |      |
|153|True        |blackbox_filling_1      |                               |      |
|162|True        |blackbox_filling_2      |                               |      |
|168|True        |blackbox_filling_3      |                               |      |
|195|True        |blackbox_filling_4      |                               |      |
|202|True        |blackbox_filling_5      |                               |      |
|95 |True        |blackbox_grinder_1      |                               |      |
|96 |True        |blackbox_hub_1          |                               |      |
|199|True        |blackbox_liquidcleaner_1|                               |      |
|97 |True        |blackbox_miner_1        |                               |      |
|150|True        |blackbox_miner_2        |                               |      |
|197|True        |blackbox_miner_3        |                               |      |
|194|True        |blackbox_mix_1          |                               |      |
|203|True        |blackbox_mix_2          |                               |      |
|204|True        |blackbox_pipe_1         |                               |      |
|201|True        |blackbox_pipe_2         |                               |      |
|164|True        |blackbox_planter_1      |                               |      |
|196|True        |blackbox_planter_2      |                               |      |
|92 |True        |blackbox_power_1        |                               |      |
|163|True        |blackbox_powerstation_1 |                               |      |
|93 |True        |blackbox_shaper_1       |                               |      |
|200|True        |blackbox_shaper_2       |                               |      |
|152|True        |blackbox_splitter_1     |                               |      |
|198|True        |blackbox_squirter_1     |                               |      |
|91 |True        |blackbox_storager_1     |                               |      |
|158|True        |blackbox_thickener_1    |                               |      |
|151|True        |blackbox_winder_1       |                               |      |
|210|True        |dung01_bdg001           |![Image](./LevelImages/210.png)|      |
|209|True        |dung01_bdg002           |![Image](./LevelImages/209.png)|One of the largest available scenes|
|175|True        |dung01_bdg003           |![Image](./LevelImages/175.png)|      |
|117|True        |dung01_cdg001           |![Image](./LevelImages/117.png)|      |
|118|True        |dung01_cdg002           |![Image](./LevelImages/118.png)|      |
|120|True        |dung01_cdg003           |![Image](./LevelImages/120.png)|      |
|75 |True        |dung01_cdg004           |![Image](./LevelImages/75.png) |      |
|77 |True        |dung01_cdg005           |![Image](./LevelImages/77.png) |      |
|76 |True        |dung01_cdg006           |![Image](./LevelImages/76.png) |      |
|123|True        |dung01_cdg007           |![Image](./LevelImages/123.png)|      |
|122|True        |dung01_cdg008           |![Image](./LevelImages/122.png)|      |
|119|(?)         |dung01_cdg009           |                               |Isn't loading for some reason|
|121|True        |dung01_cdg010           |![Image](./LevelImages/121.png)|      |
|130|True        |dung01_rdg001           |![Image](./LevelImages/130.png)|      |
|131|True        |dung01_rdg002           |![Image](./LevelImages/131.png)|      |
|129|True        |dung01_sdg001           |![Image](./LevelImages/129.png)|      |
|141|True        |dung01_sdg002           |                               |Same scene as 129|
|142|True        |dung01_sdg003           |                               |Same scene as 129|
|140|True        |dung01_sdg004           |![Image](./LevelImages/140.png)|      |
|143|True        |dung01_sdg005           |                               |Same scene as 140|
|144|True        |dung01_sdg006           |                               |Same scene as 140|
|87 |True        |indie_dg002             |![Image](./LevelImages/87.png) |      |
|216|True        |indie_dg003             |![Image](./LevelImages/216.png)|      |
|128|True        |indie_race001           |![Image](./LevelImages/128.png)|      |
|124|False       |indie_rpg001            |                               |      |
|21 |False       |map01_lv001             |                               |All main maps (`map01_lv*`) aren't working at the moment|
|2  |False       |map01_lv002             |                               |      |
|3  |False       |map01_lv003             |                               |      |
|34 |False       |map01_lv005             |                               |      |
|35 |False       |map01_lv006             |                               |      |
|28 |False       |map01_lv007             |                               |      |
|101|True        |map02_lv001             |![Image](./LevelImages/101.png)|      |

## Additional Information

You can achieve a teleportation effect by editing the `players` entry in the `endfieldps` collection of your MongoDB. This allows you to explore areas that are normally blocked off by Invisible Walls, such as `dung01_bdg001`:
![Image](./LevelImages/210_1.png)
```
position:Object
x:-851
y:142
z:101

rotation:Object
x:0
y:182
z:0
curSceneNumId:210
```
`dung01_bdg003`
![Image](./LevelImages/175_1.png)
```
position:Object
x:-63
y:202
z:337

rotation:Object
x:0
y:148
z:0
curSceneNumId:175
```