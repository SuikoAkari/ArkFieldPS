# 在 Linux 上运行 EndFieldPS

## Debian 12
请先更新你的apt软件源。

```bash
sudo apt update && sudo apt upgrade
```

### 安装依赖
1. 安装所需依赖
```bash
sudo apt install git wget curl zip
```
2. 安装 MongoDB 8

    请参考 [MongoDB 官方文档](https://www.mongodb.com/zh-cn/docs/manual/tutorial/install-mongodb-on-debian/) 中的安装说明。

3. 安装 .NET 8

    请参考 [Microsoft 官方文档](https://learn.microsoft.com/zh-cn/dotnet/core/install/linux-debian) 中的安装说明。

### 安装 EndFieldPS
1. 下载服务端和服务端数据

```bash
cd \
mkdir EndFieldPS && cd EndFieldPS \
wget https://github.com/SuikoAkari/EndFieldPS/releases/latest/download/EndFieldPS-master-Linux.zip \
mkdir GameData && cd GameData \
git clone https://github.com/PotRooms/EndFieldData.git
```

2. 解压服务端并复制文件

```bash
cd .. \
unzip EndFieldPS-master-Linux.zip \
cp -r GameData/EndFieldData/Json Json \
cp -r GameData/EndFieldData/Proto Proto \
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. 修改配置  
> [!NOTE]提示  
> 如果你打算在公网服务器上运行 EndFieldPS，请按照以下步骤操作。

使用 Vim 或 nano 打开 `server_config.json` 文件，将 `dispatchServer.bindAddress` 和 `gameServer.bindAddress` 修改为 `"0.0.0.0"`，并将 `gameServer.accessAddress` 修改为你的服务器 IP 地址。

```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "endfieldps"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "emailFormat": "@endfield.ps"
  },
  "gameServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 30000,
    "accessAddress": "your server address",
    "accessPort": 30000
  },
  "serverOptions": {
    "defaultSceneNumId": 98,
    "maxPlayers": 20
  },
  "logOptions": {
    "packets": false,
    "debugPrint": true
  }
}
```

4. 运行 PS

防火墙应当放通 5000 和 30000 端口
```bash
./EndFieldPS
```
