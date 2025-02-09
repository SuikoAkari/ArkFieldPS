# Run EndFieldPS on Linux
Contents
- [Debian 12](#Debian-12) 
- [ArchLinux](#ArchLinux)



## Debian 12
First you need update your apt sources.

```bash
sudo apt update && sudo apt upgrade
```

### Install Dependencies
1. Install Dependencies
```bash
sudo apt install git wget curl zip
```
2. Install MongoDB 8

    Follow the instruction on [MongoDB Offical Site](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-debian/)

3. Install .Net8

    Follow the instruction on [Microsoft](https://learn.microsoft.com/en-us/dotnet/core/install/linux-debian)   

### Install EndFieldPS
1. Download PS and Data

```bash
cd \ 
mkdir EndFieldPS && cd EndFieldPS \ 
wget https://github.com/SuikoAkari/EndFieldPS/releases/latest/download/EndFieldPS-master-Linux.zip \ 
mkdir GameData && cd GameData \ 
git clone https://github.com/PotRooms/EndFieldData.git
```

2. Extract EndFieldPS and Copy Files

```bash
cd .. \
unzip EndFieldPS-master-Linux.zip \ 
cp -r GameData/EndFieldData/Json Json \ 
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. Modify Config
> [!NOTE]
> If you want to run endfieldps on a public server, please follow the steps.


  Run the PS once, PS will auto generate `server_config.json`

  
  Use vim or nano open `server_config.json` and change `dispatchServer.bindAddress` and `gameServer.bindAddress` to `"0.0.0.0"`, `dispatchServer.accessAddress` and `gameServer.accessAddress` to your server ip.
```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "endfieldps"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessAddress": "your server address",
    "accessPort": 5000,
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

4. Run the PS

  Allow port `dispatchServer.accessPort` and `gameServer.accessPort` on firewall
```bash
./EndFieldPS
```


## ArchLinux

First, use `sudo pacman -Syyu` to make sure your system is up to date.

### Install Dependencies

Use `paru` or `yay` to get AUR packages.
```bash
paru -S dotnet-runtime-8.0 mongodb-bin
```

We need `git`, `zip` and `wget` to get PS files, get them with `pacman`
```bash
sudo pacman -S git zip wget
```

### Install EndFieldPS
1. Download PS and Data

```bash
cd \ 
mkdir EndFieldPS && cd EndFieldPS \ 
wget https://github.com/SuikoAkari/EndFieldPS/releases/latest/download/EndFieldPS-master-Linux.zip \ 
mkdir GameData && cd GameData \ 
git clone https://github.com/PotRooms/EndFieldData.git
```

2. Extract EndFieldPS and Copy Files

```bash
cd .. \
unzip EndFieldPS-master-Linux.zip \ 
cp -r GameData/EndFieldData/Json Json \ 
cp -r GameData/EndFieldData/Proto Proto \
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. Modify Config
> [!NOTE]
> If you want to run endfieldps on a public server, please follow the steps.


  Run the PS once, PS will auto generate `server_config.json`

  
  Use vim or nano open `server_config.json` and change `dispatchServer.bindAddress` and `gameServer.bindAddress` to `"0.0.0.0"`, `dispatchServer.accessAddress` and `gameServer.accessAddress` to your server ip.
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

4. Run the PS

  Don't forget to allow port `dispatchServer.accessPort` and `gameServer.accessPort` on firewall
```bash
./EndFieldPS
```
