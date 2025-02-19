# ArkField PS
[EN](../README.md) | [IT](./README_it-IT.md) | [RU](./README_ru-RU.md) | [CN](./README_zh-CN.md) | [NL](./README_nl-NL.md)

![Logo](https://socialify.git.ci/SuikoAkari/ArkFieldPS/image?custom_description=Private+server+for+EndField&amp;description=1&amp;font=Jost&amp;forks=1&amp;issues=1&amp;language=1&amp;logo=https%3A%2F%2Farknights.wiki.gg%2Fimages%2F3%2F31%2FArknights_Endfield_logo.png&amp;name=1&amp;pattern=Circuit+Board&amp;pulls=1&amp;stargazers=1&amp;theme=Dark)

ArkFieldPS - приватный сервер для Arknights EndField CBT2.

## Текущие возможности

* Вход;
* Переключение персонажей;
* Переключение отрядов;
* Переключение карт (пока работает только для некоторых карт);
* Спавн врагов;
* Сохранение данных с помощью MongoDB;

## Шаги для установки (Windows)

1. Установите следующие программы, если у вас их ещё нет: 
   * [.NET SDK](https://dotnet.microsoft.com/en-us/download) (рекомендуется версия 8.0.12);
   * [MongoDB](https://www.mongodb.com/try/download/community);
   * [Fiddler Classic](https://www.telerik.com/fiddler/fiddler-classic) (или [mitmproxy](https://mitmproxy.org/));
   1. При установке *Fiddler Classic* убедитесь, что вы **включили** "Decrypt HTTPS traffic" и **установили** сертификат!
   2. Вам нужно включить две функции: перейдите в Tools (в левом верхнем меню) -> Options -> HTTPS -> Включите "Capture HTTPS CONNECTs" и "Decrypt HTTPS traffic". Вы также можете переустановить сертификаты с помощью Actions (напротив "Capture HTTPS CONNECTs") -> Trust Root Certificate и нажать "Yes".
2. Скачайте [скомпилированную версию](https://github.com/SuikoAkari/ArkFieldPS/releases/latest) или скомпилируйте самостоятельно.
3. Положите `Json` и `TableCfg` папки рядом с `ArkFieldPS.exe` (вы можете скачать их [здесь](https://github.com/PotRooms/EndFieldData/tree/main)).
4. Запустите сервер (`ArkFieldPS.exe`).
5. Измените `C:\Users\<ВашеИмяПользователя>\Documents\Fiddler2\Scripts\CustomRules.js` скрипт (или сохраните копию стандартного скрипта и создайте новый файл с таким-же названием) со следующим скриптом:
    * Вы также можете запустить *Fiddler Classic*, затем перейти в `Rules -> Customize Rules` (CTRL + R) и сохранить скрипт, либо выбрать вкладку *FiddlerScript*.

    ```javascript
    import System;
    import System.Windows.Forms;
    import Fiddler;
    import System.Text.RegularExpressions;

    class Handlers
    {
        static function OnBeforeRequest(oS: Session) {
            if(
                oS.fullUrl.Contains("discord") ||
                oS.fullUrl.Contains("steam") ||
                oS.fullUrl.Contains("git") ||
                oS.fullUrl.Contains("yandex")
            //you can add any addresses if some sites don't work
            ) {
                oS.Ignore();
            }
        
            if (!oS.oRequest.headers.HTTPMethod.Equals("CONNECT")) {
                if(oS.fullUrl.Contains("gryphline.com") || oS.fullUrl.Contains("hg-cdn.com")) {
                    oS.fullUrl = oS.fullUrl.Replace("https://", "http://");
                    oS.host = "localhost"; // place another ip if you need
                    oS.port = 5000; //and port
                }
            }
        }
    };
    ```
    By Xannix
    Или вы можете использовать команду mitmproxy:

    ```shell
    mitmproxy -s ak.py
    ```

    ak.py:

    ```py
    import mitmproxy
    from mitmproxy import ctx, http
    class EndFieldModifier:
        def requestheaders(self,flow: mitmproxy.http.HTTPFlow):
            if "gryphline.com" in flow.request.host or "hg-cdn.com" in flow.request.host:
                if flow.request.method=="CONNECT":
                    return
                
                flow.request.scheme="http"
                flow.request.cookies.update({
                    "OriginalHost":flow.request.host,
                    "OriginalUrl":flow.request.url
                })
                flow.request.host="localhost"
                flow.request.port=5000
                ctx.log.info("URL:"+flow.request.url)
                
                
                
    addons=[
        EndFieldModifier()
    ]
    ```

6. Запустите *Fiddler Classic*, он должен запуститься с новым *скриптом Custom Rules* (вы можете проверить это во вкладке *FiddlerScript*).
7. Запустите клиент игры и начните играть! (Прим.: пока что поддерживается только клиентская ОС).
8. Вам нужно создать аккаунт, используя `account create (имя пользователя)` в консоли сервера, затем войдите в игру с помощью почты по типу `(имя пользователя)@любойформатпочты.пишитечтохотите`. Пароль отсутствует, поэтому вы можете ввести случайный пароль в соответствующее поле.

## Установка на Linux

Почитать руководство на различных дистрибутивах Linux Вы можете здесь:
|Linux дистрибутив |Ссылка|
|:-----------------|:-----|
|Debian            |[Ссылка](./Linux/RunOnLinuxServer_ru-RU.md#debian-12)|
|Arch Linux        |[Ссылка](./Linux/RunOnLinuxServer_ru-RU.md#arch-linux)|
|Fedora            |[Ссылка](./Linux/RunOnLinuxServer_ru-RU.md#fedora-workstation)|
|Ubuntu            |[Ссылка](./Linux/RunOnLinuxServer_ru-RU.md#ubuntu)|

## Дополнительно

Описание всех команд сервера вы можете найти [здесь](./CommandList/commands_ru-RU.md).<br>
Список всех сцен находится [тут](./LevelsTable.md).<br>
Список всех врагов - [тут](./EnemiesTable.md).<br>
Список всех персонажей - [тут](docs/CharactersTable.md).<br>
Вы можете открыть внутриигровую консоль, перейдя во вкладку `Settings -> Platform & Account -> Account Settings (кнопка Access Account)`. Чтобы просмотреть доступные команды, пропишите `help`.

## Discord

Если вы хотите обсудить проект или помочь в его разработке, присоединяйтесь к нашему [Discord серверу](https://discord.gg/gPvqhfdMU6)!

## Примечание

Этот проект не был создан с целью замены официальных серверов. По вопросам удаления или уточнениям обращайтесь ко мне в Telegram: @SuikoAkari.
