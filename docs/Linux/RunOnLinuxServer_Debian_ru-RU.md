[<= Назад к дистрибутивам](../README_ru-RU.md#установка-на-linux)

# Запуск EndFieldPS на Linux

## Debian 12
Для начала вам нужно обновить источники репозиторией apt:

```bash
sudo apt update && sudo apt upgrade
```

### Установка зависимостей
1. Установите зависимости:
```bash
sudo apt install git wget curl zip
```
2. Установите MongoDB 8:

    Следуйте инструкции на официальном сайте [MongoDB](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-debian/)^(англ.)

3. Установите .Net 8:

    Следуйте инструкции на официальном сайте [Microsoft](https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-debian)

### Установка EndFieldPS
1. Установите [приватный сервер](https://github.com/SuikoAkari/EndFieldPS/releases/latest) и [игровые значения](https://github.com/PotRooms/EndFieldData/tree/main):

```bash
cd \ 
mkdir EndFieldPS && cd EndFieldPS \ 
wget https://github.com/SuikoAkari/EndFieldPS/releases/latest/download/EndFieldPS-master-Linux.zip \ 
mkdir GameData && cd GameData \ 
git clone https://github.com/PotRooms/EndFieldData.git
```

2. Извлеките EndFieldPS и скопируйте файлы:

```bash
cd .. \
unzip EndFieldPS-master-Linux.zip \ 
cp -r GameData/EndFieldData/Json Json \ 
cp -r GameData/EndFieldData/Proto Proto \
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```

3. Настройка файла конфигурации
> [!NOTE]
> Если вы хотите запустить EndFieldPS на публичном сервере, пожалуйста, следуйте следующим инструкциям:


  Используя vim или nano (или любой другой текстовой редактор), откройте `server_config.json` и измените `dispatchServer.bindAddress` и `gameServer.bindAddress` на `"0.0.0.0"`, `gameServer.accessAddress` - на ваш ip адрес:
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
    "accessAddress": "ip адрес вашего сервера",
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

4. Запустите приватный сервер:

  > [!NOTE]
  > Не забудьте разрешить порты `gameServer` и `dispatchServer` (по умолчанию это порты 30000 и 5000 соответственно) в вашем брандмауэре, если он включён

```bash
./EndFieldPS
```
