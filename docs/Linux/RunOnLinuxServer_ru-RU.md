[<= Назад к дистрибутивам](../README_ru-RU.md#установка-на-linux)

# Запуск ArkFieldPS на Linux

## Оглавление
- Установка зависимостей:
  - [Debian 12](#debian-12) 
  - [ArchLinux](#arch-linux)
  - [Fedora](#fedora-workstation)
  - [Ubuntu](#ubuntu)
- [Установка ArkFieldPS](#установка-ArkFieldPS)
- [Настройка ArkFieldPS](#настройка-ArkFieldPS)

## Установка зависимостей

### Debian 12
Для начала вам нужно обновить источники репозиториев apt:
```bash
sudo apt update && sudo apt upgrade
```

1. Установите зависимости:
```bash
sudo apt install git wget curl zip
```
2. Установите MongoDB 8:
    Следуйте инструкции на официальном сайте [MongoDB](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-debian/)<sup>(англ.)</sup>

3. Установите .Net 8:
    Следуйте инструкции на официальном сайте [Microsoft](https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-debian?tabs=dotnet8)

[Установка ArkFieldPS =>](#установка-ArkFieldPS)

---

### Arch Linux
Для начала убедитесь, что ваша система обновлена до актуальной версии:
```bash
sudo packman -Syyu
```

1. Скачайте .Net и MongoDB пакеты через AUR репозиторий, используя `paru` или `yay`:
```bash
paru -S dotnet-runtime-8.0 mongodb-bin
```

2. Для копирования и распаковки файлов приватного сервера понадобятся пакеты `git`, `zip` и `wget`. Установите их через `pacman`:
```bash
sudo pacman -S git zip wget
```

[Установка ArkFieldPS =>](#установка-ArkFieldPS)

---

### Fedora Workstation
Проверьте обновления репозиториев и установите их:
```bash
sudo dnf check-update && sudo dnf upgrade
```

> [!NOTE]
> По умолчанию Fedora Workstation уже содержит пакеты `git`, `wget` и `zip`. Если их нет - установите их:
```bash 
sudo dnf install git wget zip
```

1. Установка MongoDB 8:
  * Создайте новый файл перозитория mongodb-org:
```bash
sudo nano /etc/yum.repos.d/mongodb-org-8.0.repo
```
  * Добавьте в этот файл следующую конфигурацию:
```conf
[mongodb-org-8.0]
name=MongoDB Repository
baseurl=https://repo.mongodb.org/yum/redhat/9/mongodb-org/8.0/x86_64/
gpgcheck=1
enabled=1
gpgkey=https://www.mongodb.org/static/pgp/server-8.0.asc
```
  * Установите MongoDB:
```bash
sudo dnf install mongodb-org
```
  * Запустите службу `mongod.service` и проверьте его работу:
```bash
sudo systemctl start mongod.service && sudo systemctl status mongod.service
```
  *Если вам нужно, чтобы служба запускалась при каждом запуске системы:*
```bash
sudo systemctl enable mongod.service
```

2. Установка .Net 8:
  * Установите следующий пакет:
```bash
sudo dnf install dotnet-sdk-8.0
```

[Установка ArkFieldPS =>](#установка-ArkFieldPS)

---

### Ubuntu
Для начала вам нужно проверить систему на наличие обновлений:
```bash
sudo apt update && sudo apt upgrade
```

1. Установите зависимости:
```bash
sudo apt install git wget zip
```
2. Установите MongoDB 8:
    Следуйте инструкции на официальном сайте [MongoDB](https://www.mongodb.com/docs/manual/tutorial/install-mongodb-on-ubuntu/)<sup>(англ.)</sup>
> [!WARNING]
> На втором шаге не забудьте выбрать вашу версию Ubuntu, иначе установка выдаст ошибку.

3. Установите .Net 8:
    Следуйте инструкции на официальном сайте [Microsoft](https://learn.microsoft.com/ru-ru/dotnet/core/install/linux-ubuntu-install?tabs=dotnet8)
> [!WARNING]
> Также не забудьте выбрать вашу версию Ubuntu.

[Установка ArkFieldPS =>](#установка-ArkFieldPS)

---

## Установка ArkFieldPS
1. Установите [приватный сервер](https://github.com/SuikoAkari/ArkFieldPS/releases/latest) и [игровые значения](https://github.com/PotRooms/EndFieldData/tree/main):

```bash
cd
mkdir ArkFieldPS && cd ArkFieldPS
wget https://github.com/SuikoAkari/ArkFieldPS/releases/latest/download/ArkFieldPS-master-Linux.zip
mkdir GameData && cd GameData
git clone https://github.com/PotRooms/EndFieldData.git
```

2. Извлеките ArkFieldPS и скопируйте файлы:

```bash
cd ..
unzip ArkFieldPS-master-Linux.zip
cp -r GameData/EndFieldData/Json Json
cp -r GameData/EndFieldData/DynamicAssets DynamicAssets
cp -r GameData/EndFieldData/TableCfg/. TableCfg
```



## Настройка ArkFieldPS

1. Настройка файла конфигурации
> [!NOTE]
> Если вы хотите запустить ArkFieldPS на публичном сервере, пожалуйста, следуйте следующим инструкциям:


  Используя vim или nano (или любой другой текстовой редактор), откройте `server_config.json` и измените `dispatchServer.bindAddress` и `gameServer.bindAddress` на `"0.0.0.0"`, `gameServer.accessAddress` - на ваш ip адрес:
```json
{
  "mongoDatabase": {
    "uri": "mongodb://localhost:27017",
    "collection": "ArkFieldPS"
  },
  "dispatchServer": {
    "bindAddress": "0.0.0.0",
    "bindPort": 5000,
    "accessAddress": "ip адрес вашего сервера",
    "accessPort": 5000,
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

2. Запустите приватный сервер:

  > [!NOTE]
  > Не забудьте разрешить порты `gameServer` и `dispatchServer` (по умолчанию это порты 30000 и 5000 соответственно) в вашем брандмауэре, если он включён

```bash
./ArkFieldPS
```
