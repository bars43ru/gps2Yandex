# Описание настроек в settings.json

```
{
  "Catalogs": {
    "Directory": "Z:\\" 
  },
  "WialonListen": {
    "Host": "0.0.0.0",
    "Port": 17968   
  },
  "Yandex": {
    "Clid": "",
    "Host": "http://extjams.maps.yandex.net/mtr_collect/1.x/",
    "Interval": 5
  }
}
```

Где:

|Атрибут|Описание|
|---|---|
|**Catalogs**||
|Directory|Директория в которой храняться файлы - справочники|
|**WialonListen**||
|Host|IP с которого принимать ретранслированные сигналы с Wialon, в случае 0.0.0.0 - принимать со всех|
|Port|Порт на котором слушаем входящие соединения от Wialon|
|**Yandex**||
|Clid|Идентификатор участника программы, выдается яндексом|
|Host|End point|
|Interval|Интервал отправки данных|

