# 在 Linux 上运行 ArkFieldPS
目录
- [Debian 12](#Debian-12)
- [ArchLinux](#ArchLinux)
- [Fedora](#fedora-workstation)
- [Ubuntu](#ubuntu)

## Debian 12
请先确保你的软件包均为最新。

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

### 安装 ArkFieldPS
1. 下载服务端和服务端数据

```bash
cd \
mkdir ArkFieldPS && cd ArkFieldPS \
wget https://github.com/SuikoAkari/ArkFieldPS/releases/latest/download/ArkFieldPS-master-Linux.zip \
mkdir GameData && cd GameData \
git clone https://github.com/PotRooms/EndFieldData.git
```

2. 解压服务端并复制文件

```bash
cd .. \
unzip ArkFieldPS-master-Linux.zip \
cp -r GameData/EndFieldData/Json Json \
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. 修改配置  
> [!NOTE]提示  
> 如果你打算在公网服务器上运行 ArkFieldPS，请按照以下步骤操作。


先运行一次 ArkFieldPS, ArkFieldPS 会自动生成 `server_config.json` 。
```bash
./ArkFieldPS
```

使用 Vim 或 nano 打开 `server_config.json` 文件，将 `dispatchServer.bindAddress` 和 `gameServer.bindAddress` 修改为 `"0.0.0.0"`，并将 `dispatchServer.accessAddress` 和 `gameServer.accessAddress` 修改为你的服务器 IP 地址。


按需修改 `dispatchServer` 和 `gameServer` 下的 `bindPort` 与 `accessPort`


```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "ArkFieldPS"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessPort": "5000",
    "accessAddress": "你的服务器地址",
    "emailFormat": "@endfield.ps"
  },
  "gameServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 30000,
    "accessAddress": "你的服务器地址",
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

防火墙应当放通 `dispatchServer.accessPort` 和 `gameServer.accessPort` 端口
```bash
./ArkFieldPS
```


## ArchLinux


首先确保你的软件包都为最新
```bash
sudo pacman -Syyu
```

### 安装依赖


使用 `paru` 或 `yay` 获取 AUR 软件包
```bash
paru -S dotnet-runtime-8.0 mongodb-bin
```


我们需要 `git`、`zip`、`wget` 获取 ArkFieldPS 所需文件，使用 `pacman` 安装它们
```bash
sudo pacman -S git zip wget
```


### 安装 ArkFieldPS
1. 下载服务端和服务端数据

```bash
cd \
mkdir ArkFieldPS && cd ArkFieldPS \
wget https://github.com/SuikoAkari/ArkFieldPS/releases/latest/download/ArkFieldPS-master-Linux.zip \
mkdir GameData && cd GameData \
git clone https://github.com/PotRooms/EndFieldData.git
```

2. 解压服务端并复制文件

```bash
cd .. \
unzip ArkFieldPS-master-Linux.zip \
cp -r GameData/EndFieldData/Json Json \
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. 修改配置  
> [!NOTE]提示  
> 如果你打算在公网服务器上运行 ArkFieldPS，请按照以下步骤操作。


先运行一次 ArkFieldPS, ArkFieldPS 会自动生成 `server_config.json` 。
```bash
./ArkFieldPS
```

使用 Vim 或 nano 打开 `server_config.json` 文件，将 `dispatchServer.bindAddress` 和 `gameServer.bindAddress` 修改为 `"0.0.0.0"`，并将 `dispatchServer.accessAddress` 和 `gameServer.accessAddress` 修改为你的服务器 IP 地址。


按需修改 `dispatchServer` 和 `gameServer` 下的 `bindPort` 与 `accessPort`


```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "ArkFieldPS"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessPort": "5000",
    "accessAddress": "你的服务器地址",
    "emailFormat": "@endfield.ps"
  },
  "gameServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 30000,
    "accessAddress": "你的服务器地址",
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

防火墙应当放通 `dispatchServer.accessPort` 和 `gameServer.accessPort` 端口
```bash
./ArkFieldPS
```

## Fedora Workstation
检查仓库更新并安装：
```bash
sudo dnf check-update && sudo dnf upgrade
```

### 安装依赖
> [!NOTE]提示
> Fedora Workstation 在正常情况下已经包含了 `git`、`wget` 和 `zip` 。如果缺失以上软件包，请使用以下命令安装：
```bash 
sudo dnf install git wget zip
```

1. 安装 MongoDB 8：
  * 创建新的 MongoDB 仓库文件：
```bash
sudo nano /etc/yum.repos.d/mongodb-org-8.0.repo
```
  * 在文件中添加以下配置：
```conf
[mongodb-org-8.0]
name=MongoDB Repository
baseurl=https://repo.mongodb.org/yum/redhat/9/mongodb-org/8.0/x86_64/
gpgcheck=1
enabled=1
gpgkey=https://www.mongodb.org/static/pgp/server-8.0.asc
```
  * 安装 MongoDB：
```bash
sudo dnf install mongodb-org
```
  * 启动 `mongod.service` 并检查其状态：
```bash
sudo systemctl start mongod.service && sudo systemctl status mongod.service
```
  *如果你希望服务在开机时自动启动：*
```bash
sudo systemctl enable mongod.service
```

1. 安装 .NET 8：
  * 安装以下包：
```bash
sudo dnf install dotnet-sdk-8.0
```

### 安装 ArkFieldPS
1. 下载服务端和服务端数据
```bash
cd \ 
mkdir ArkFieldPS && cd ArkFieldPS \ 
wget https://github.com/SuikoAkari/ArkFieldPS/releases/latest/download/ArkFieldPS-master-Linux.zip \ 
mkdir GameData && cd GameData \ 
git clone https://github.com/PotRooms/EndFieldData.git
```

2. 解压服务端并复制文件
```bash
cd .. \
unzip ArkFieldPS-master-Linux.zip \ 
cp -r GameData/EndFieldData/Json Json \ 
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. 修改配置
> [!NOTE]提示
> 如果你打算在公网服务器上运行 ArkFieldPS，请按照以下步骤操作。

先运行一次 ArkFieldPS，ArkFieldPS 会自动生成 `server_config.json`。

使用 vim 或 nano 打开 `server_config.json` 文件，将 `dispatchServer.bindAddress` 和 `gameServer.bindAddress` 修改为 `"0.0.0.0"`，并将 `dispatchServer.accessAddress` 和 `gameServer.accessAddress` 修改为你的服务器 IP 地址。

按需修改 `dispatchServer` 和 `gameServer` 下的 `bindPort` 与 `accessPort`

```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "ArkFieldPS"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessAddress": "你的服务器地址",
    "accessPort": 5000,
    "emailFormat": "@endfield.ps"
  },
  "gameServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 30000,
    "accessAddress": "你的服务器地址",
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

4. 运行服务端

防火墙应当放通 `dispatchServer.accessPort` 和 `gameServer.accessPort` 端口（默认为 5000 和 30000）
```bash
./ArkFieldPS
```

## Ubuntu
首先，你应当检查系统更新：
```bash
sudo apt update && sudo apt upgrade
```

### 安装依赖
1. 如果未安装，请安装以下软件包：
```bash
sudo apt install git wget zip
```
2. 安装 MongoDB 8：
    请参考官方 [MongoDB](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-ubuntu/) 网站的安装说明
> [!WARNING]警告
> 在第二步时，请确保你选择了正确的 Ubuntu 版本

1. 安装 .NET 8：
    请参考官方 [Microsoft](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8) 网站的安装说明
> [!WARNING]警告
> 同样，请记得选择正确的 Ubuntu 版本。

### 安装 ArkFieldPS
1. 下载服务端和服务端数据
```bash
cd \ 
mkdir ArkFieldPS && cd ArkFieldPS \ 
wget https://github.com/SuikoAkari/ArkFieldPS/releases/latest/download/ArkFieldPS-master-Linux.zip \ 
mkdir GameData && cd GameData \ 
git clone https://github.com/PotRooms/EndFieldData.git
```

2. 解压服务端并复制文件
```bash
cd .. \
unzip ArkFieldPS-master-Linux.zip \ 
cp -r GameData/EndFieldData/Json Json \ 
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. 修改配置
> [!NOTE]提示
> 如果你打算在公网服务器上运行 ArkFieldPS，请按照以下步骤操作。

先运行一次 ArkFieldPS，ArkFieldPS 会自动生成 `server_config.json`。

使用 vim 或 nano 打开 `server_config.json` 文件，将 `dispatchServer.bindAddress` 和 `gameServer.bindAddress` 修改为 `"0.0.0.0"`，并将 `dispatchServer.accessAddress` 和 `gameServer.accessAddress` 修改为你的服务器 IP 地址。

按需修改 `dispatchServer` 和 `gameServer` 下的 `bindPort` 与 `accessPort`

```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "ArkFieldPS"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessAddress": "你的服务器地址",
    "accessPort": 5000,
    "emailFormat": "@endfield.ps"
  },
  "gameServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 30000,
    "accessAddress": "你的服务器地址",
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

4. 运行服务端

防火墙应当放通 `dispatchServer.accessPort` 和 `gameServer.accessPort` 端口（默认为 5000 和 30000）
```bash
./ArkFieldPS
```