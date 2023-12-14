
## 1. 哪些公司在使用Redis

GitHub、Twitter、Stackoverflow、百度

## 2. Redis簡介

Remote dictionary server

可以用於數據庫、緩存、消息隊列等各種場景

最熱門NoSQL數據庫之一

基於內存數據庫(記憶體效能>IO效能，也就是Redis>MySQL效能)

## 3. Redis特點

- 高性能Key-Value

- 多種數據結構，單鍵值對最大支持512M大小的數據

- 豐富的功能

- 支持數據持久化、主從複製、哨兵模式等高可用分佈特性

### 3.1. 數據類型

5種基本數據類型

- 字串 String
- 列表 List
- 集合 Set
- 有序集合 SortedSet
- 哈希 Hash

![image-20231214102318825](https://i.imgur.com/lITTqKX.png)

高級數據類型

- 消息隊列 Stream
- 地理空間 Geospatial
- HyperLogLog
- 位圖 Bitmap
- 位域 Bitfield

### 3.2. 使用方式

主要三種類型

- CLI
- API
  - 簡單來說就是用程式碼(python、java)的方式進行使用
- GUI



## 4. 環境安裝

下載docker image

```
docker pull redis
```

container build

```
docker run --name redis-lab -p 6379:6379 -d redis
```



## 5. Redis GUI 工具

### 5.1. 這次透過指令方式安裝

```bash
choco install another-redis-desktop-manager
```

### 5.2. 執行結果

```bash
C:\Windows\System32>choco install another-redis-desktop-manager
Chocolatey v0.11.1
Installing the following packages:
another-redis-desktop-manager
By installing, you accept licenses for the packages.
Progress: Downloading another-redis-desktop-manager 1.6.1... 100%
```

### 5.3. 確認docker id

```
C:\Windows\System32>docker ps
CONTAINER ID   IMAGE                      COMMAND                  CREATED          STATUS                 PORTS                            NAMES
65b2b326dc9c   redis                      "docker-entrypoint.s…"   12 minutes ago   Up 12 minutes          0.0.0.0:6379->6379/tcp           redis-lab
```

### 5.4. 進入docker操作redis

```
C:\Windows\System32>docker exec -it 65b2 bash
root@65b2b326dc9c:/data# redis-cli
127.0.0.1:6379> ping
PONG
127.0.0.1:6379> set test "hello world"
OK
127.0.0.1:6379> get test
"hello world"
```

### 5.5. 透過GUI工具操作

![image-20231211164508060](https://i.imgur.com/J0Uw01L.png)



## 6. String 字串

### 6.1. 簡介

### 6.2. 使用場景

### 6.3. 常用指令

#### 6.3.1. SET - 設定key 

```bash
127.0.0.1:6379> set kite nice
OK
```

#### 6.3.2. GET - 讀取key 

```bash
127.0.0.1:6379> get kite
"nice"
```

#### 6.3.3. key大小寫有分

```bash
127.0.0.1:6379> set Kite Nice
OK
127.0.0.1:6379> get Kite
"Nice"
```

#### 6.3.4. DEL - 刪除key

```bash
127.0.0.1:6379> del name
(integer) 0

127.0.0.1:6379> get name
(nil)
```

#### 6.3.5. EXISTS - 查詢key

```bash
127.0.0.1:6379> exists name
(integer) 0

127.0.0.1:6379> exists age
(integer) 1
```

#### 6.3.6. KEYS - 萬用字查詢

```bash
127.0.0.1:6379> keys *
1) "backup4"
2) "backup3"
3) "age"
4) "backup1"
5) "backup2"
127.0.0.1:6379> keys ba*
1) "backup4"
2) "backup3"
3) "backup1"
4) "backup2"
```

#### 6.3.7. FLUSHALL - 刪除所有key

```bash
127.0.0.1:6379> flushall
OK
127.0.0.1:6379> keys *
(empty array)
```

#### 6.3.8. 中文顯示說明

```bash
127.0.0.1:6379> set name "悠風"
OK
127.0.0.1:6379> get name
"\xe6\x82\xa0\xe9\xa2\xa8"
```
#### 6.3.9. redis-cli --raw  正常顯示中文方法

```bash
127.0.0.1:6379> quit
# redis-cli --raw
127.0.0.1:6379> get name
悠風
```

#### 6.3.10. EXPIRE - 設定過期

```bash
127.0.0.1:6379> expire name 10
```

#### 6.3.11. TTL - 查詢過期

```bash
127.0.0.1:6379> ttl name
6
127.0.0.1:6379> ttl name
4
127.0.0.1:6379> ttl name
2
127.0.0.1:6379> ttl name
-2

#過期後查詢結果會是空白
127.0.0.1:6379> get name

#過期後查詢結果會是空白
127.0.0.1:6379> keys name

```

#### 6.3.12. SETEX - 設定key + 過期

```bash
127.0.0.1:6379> setex name 5 悠風
OK
127.0.0.1:6379> ttl name
1
127.0.0.1:6379> ttl name
-2
```

#### 6.3.13. SETNX - 該值不存在則建立

```bash
127.0.0.1:6379> setnx name kite
1
```

## 7. LIST 列表

### 7.1. 簡介

### 7.2. 使用場景

#### 7.2.1. 實現最簡單的先進先出隊列

也就是結合`LPUSH`(左進) + `RPOP`(右出)

### 7.3. 常用指令

#### 7.3.1. LPUSH

第一次

```bash
> LPUSH letter a
(integer) 1

> LRANGE letter 0 -1
1) "a"
```

![image-20231213213532063](https://i.imgur.com/AJKwCUc.png)

第二次

```bash
> LPUSH letter b
(integer) 2

> LRANGE letter 0 -1
1) "b"
2) "a"
```



![image-20231213213614346](https://i.imgur.com/SxJcfz8.png)

第三次

```bash
> LPUSH letter c d e
(integer) 5
```



![image-20231213213634536](https://i.imgur.com/JI8OdHv.png)



#### 7.3.2. LRANGE

-1 代表全部

```bash
> LRANGE letter 0 -1
1) "e"
2) "d"
3) "c"
4) "b"
5) "a"
```

#### 7.3.3. RPUSH

```bash
> RPUSH letter f
(integer) 6
> LRANGE letter 0 -1
1) "e"
2) "d"
3) "c"
4) "b"
5) "a"
6) "f"
```

![image-20231213213754020](https://i.imgur.com/ChvUKq0.png)



RPOP 

```bash
> RPOP letter
"f"

> LRANGE letter 0 -1
1) "e"
2) "d"
3) "c"
4) "b"
5) "a"
```

![image-20231213213849151](https://i.imgur.com/GO4i0Gk.png)

LPOP

```bash
> LPOP letter 2
1) "e"
2) "d"
> LRANGE letter 0 -1
1) "c"
2) "b"
3) "a"
```

![image-20231213214017278](https://i.imgur.com/zaNzqqY.png)

#### 7.3.4. LLEN

```bash
> LLEN letter
(integer) 3
```

#### 7.3.5. LTRIM

索引從0開始算，所以 1 ~ 3 是 `d、c、b`

```bash
> LPUSH letter a b c d e
(integer) 5

> LRANGE letter 0 -1
1) "e"
2) "d"
3) "c"
4) "b"
5) "a"

> LTRIM letter 1 3
"OK"

> LRANGE letter 0 -1
1) "d"
2) "c"
3) "b"
```



## 8. SET 集合

### 8.1. 簡介

SET 與 LIST 最大的差異是沒有順序，並且不能有重複值

### 8.2. 使用場景

### 8.3. 常用指令

#### 8.3.1. SADD

```bash
> SADD course Redis
(integer) 1

#故意再加一次，加不進去
> SADD course Redis
(integer) 0
> SMEMBERS course
1) "Redis"
```

#### 8.3.2. SMEMBERS

```bash
> SMEMBERS course
1) "Redis"
```

#### 8.3.3. SISMEMBER

```bash
> SISMEMBER course Redis
(integer) 1

> SISMEMBER course Python
(integer) 0
```

#### 8.3.4. SREM

```bash
> SREM course Redis
(integer) 1

> SMEMBERS course
(empty list or set)
```

#### 8.3.5. 其它學習

SINTER、SUNION、SDIFF

## 9. SortedSet 有序集合

### 9.1. 簡介

### 9.2. 使用場景

### 9.3. 常用指令

#### 9.3.1. ZADD

```bash
127.0.0.1:6379> ZADD result 100 小民 90 小華 80 小新
3
```

#### 9.3.2. ZRANGE

只呈現人名

```bash
127.0.0.1:6379> ZRANGE result 0 -1
小新
小華
小民
```

#### 9.3.3. ZRANGE WITHSCORES

分數跟人一起呈現

```bash
127.0.0.1:6379> ZRANGE result 0 -1 WITHSCORES
小新
80
小華
90
小民
100
```

#### 9.3.4. ZSCORE

呈現分數

```bash
127.0.0.1:6379> ZSCORE result 小民
100
```

#### 9.3.5. ZRANK

有序集合預設是由小到大排序，索引從0開始，小民分數最多，所以索引是2

```bash
127.0.0.1:6379> ZRANK result 小民
2
```

#### 9.3.6. ZREVRANK

rev是指reverse 反轉，也就是由大到小

```bash
127.0.0.1:6379> ZREVRANK result 小民
0
```

#### 9.3.7. 其它學習

比如刪除某個成員，對某個成員分數進行增加，刪除某個排名範圍的成員

## 10. Hash 哈希

### 10.1. 簡介

key-value資料結構，特別適合來儲存物件。

### 10.2. 使用場景

### 10.3. 常用指令

#### 10.3.1. HSET

```bash
127.0.0.1:6379> HSET person name kite
1
127.0.0.1:6379> HSET person age 100
1
```

#### 10.3.2. HGET

```bash
127.0.0.1:6379> HGET person name
kite
```

#### 10.3.3. HGETALL

```bash
127.0.0.1:6379> HGETALL person
name
kite
age
100
```

#### 10.3.4. HDEL

```bash
127.0.0.1:6379> HDEL person age
1
```

#### 10.3.5. HGETALL

```bash
127.0.0.1:6379> HGETALL person
name
kite
```

#### 10.3.6. HEXISTS

```bash
127.0.0.1:6379> HEXISTS person name
1
127.0.0.1:6379> HEXISTS person age
0
```

#### 10.3.7. HKEYS

```bash
127.0.0.1:6379> HKEYS person
name
```

#### 10.3.8. HLEN

```bash
127.0.0.1:6379> HLEN person
1
```

## 11. 發佈訂閱模式

### 11.1. 簡介

### 11.2. 使用場景

### 11.3. 常用指令

#### 11.3.1. subscribe

```bash
subscribe ubereats
```

#### 11.3.2. publish

```bash
publish ubereats order001
```

下圖左邊是publish發佈消息，右邊是兩個訂閱者，當publish時，訂閱者們會同步收到消息。

![image-20231214115332780](https://i.imgur.com/vCBtT5O.png)

## 12. GEO

### 12.1. 簡介

### 12.2. 使用場景

### 12.3. 常用指令

#### 12.3.1. GEOADD

經度(116.405285)、緯度(39.904989)、名稱(beijing)

```bash
#新增一個
127.0.0.1:6379> GEOADD city 116.405285 39.904989 beijing
1

#新增多個
127.0.0.1:6379> GEOADD city 121.472644 31.231706 shanghai 114.085947 22.547 shenzhen 37 23.125178 guangzhou 120.153576 30.287459 hangzhou
4
```

#### 12.3.2. GEOPOS

取得地理位置

```bash
127.0.0.1:6379> GEOPOS city beijing
116.40528291463851929
39.9049884229125027
```

#### 12.3.3. GEODIST

取得北京到上海的距離，預設單位為公尺(M)

```bash
127.0.0.1:6379> GEODIST city beijing shanghai
1067597.9668

#以KM單位顯示
127.0.0.1:6379> GEODIST city beijing shanghai KM
1067.5980
```

#### 12.3.4. GEOSEARCH...FROMMEMBER...BYRADIUS...

以某某地點為中心，採用半徑方式計算鄰近的城市

```bash
127.0.0.1:6379> GEOSEARCH city FROMMEMBER shanghai BYRADIUS 300 KM
hangzhou
shanghai
127.0.0.1:6379> GEOSEARCH city FROMMEMBER shanghai BYRADIUS 2000 KM
shenzhen
hangzhou
shanghai
beijing
```

## 13. HyperLogLog

### 13.1. 簡介

### 13.2. 使用場景

### 13.3. 常用指令

#### 13.3.1. PFADD

建立course課程變數，放三個課程

```bash
127.0.0.1:6379> PFADD course git docker redis
1


```

#### 13.3.2. PFCOUNT

計算課程

```bash
127.0.0.1:6379> PFCOUNT course
3

127.0.0.1:6379> PFADD course nginx
1
127.0.0.1:6379> PFCOUNT course
4
```

#### 13.3.3. PFMEGER

建立course2課程變數，放三個課程，通過`PMERGE`合併課程，再進行計算。

```bash
127.0.0.1:6379> PFADD course2 python git go
1

127.0.0.1:6379> PFMERGE result course course2
OK
127.0.0.1:6379> PFCOUNT result
6

```

## Bitmap 位圖

### 12.1. 簡介

### 12.2. 使用場景

![image-20231214142550274](https://i.imgur.com/kzsdXDW.png)

### 12.3. 常用指令
#### SETBIT

![image-20231214142638511](https://i.imgur.com/dp0k5Nw.png)

```bash
127.0.0.1:6379> SETBIT dianzan 0 1
0
127.0.0.1:6379> SETBIT dianzan 1 0
0
```

#### GETBIT

```bash
127.0.0.1:6379> GETBIT dianzan 0
1
127.0.0.1:6379> GETBIT dianzan 1
0
```

#### SET

![image-20231214142747447](https://i.imgur.com/5azKUk5.png)

```bash
127.0.0.1:6379> SET dianzan "\xF0"
OK

127.0.0.1:6379> GETBIT dianzan 0
1
127.0.0.1:6379> GETBIT dianzan 1
1
127.0.0.1:6379> GETBIT dianzan 2
1
127.0.0.1:6379> GETBIT dianzan 3
1
127.0.0.1:6379> GETBIT dianzan 4
0
127.0.0.1:6379> GETBIT dianzan 5
0
127.0.0.1:6379> GETBIT dianzan 6
0
127.0.0.1:6379> GETBIT dianzan 7
0
```

#### BITCOUNT

```bash
127.0.0.1:6379> BITCOUNT dianzan
4
```

#### BITPOS

```bash
127.0.0.1:6379> BITPOS dianzan 0
4
```

## BitField

### 12.1. 簡介

### 12.2. 使用場景

![image-20231214144020140](https://i.imgur.com/A2UKBL3.png)

### 12.3. 常用指令

#### BITFIELD...SET...

`player:1` ： 變數

`u8` ： 正整數長度8

`#0` ：第一個位置

`1`：值

```bash
#等級1
127.0.0.1:6379> BITFIELD player:1 set u8 #0 1
0
#金錢100
127.0.0.1:6379> BITFIELD player:1 set u32 #1 100
0
```

#### BITFIELD...GET...

```bash
127.0.0.1:6379> BITFIELD player:1 get u8 #0
1

127.0.0.1:6379> BITFIELD player:1 get u32 #1
100
```

#### BITFIELD...incrby...

```bash
#增加100金幣
127.0.0.1:6379> BITFIELD player:1 incrby u32 #1 100
200
```



## Multi

在執行exec之前，所有命令都會放到佇列中，不會立即執行。

收到exec命令後，有任何一命令失敗，並不影響其它命令。

在執行exec之前，其它客戶提出的請求，並不會被插入到multi裡面。

![image-20231214153249916](https://i.imgur.com/koP5xYq.png)

### 簡介

### 使用場景

### 常用指令

#### MULTI

啟動

#### EXEC

結束

以下圖為例，當執行了`MULTI`之後，接著執行指令`SET k1 v1`、`SET k2 v2`之後，系統顯示在queued中。

接著，開啟另一個終端機，試著讀取`k1`，是沒有資料的，當執行了`EXEC`之後，另一個終端機就讀取得到資料。

![image-20231214153837912](https://i.imgur.com/RxLsRi6.png)

以下實驗，證明其中一個指令失敗，並不影響其它執令的執行結果：

```bash
127.0.0.1:6379> SET k3 3
OK
127.0.0.1:6379> SET k4 v4
OK
127.0.0.1:6379> SET k5 5
OK
127.0.0.1:6379> MULTI
OK
127.0.0.1:6379(TX)> INCR k3
QUEUED
127.0.0.1:6379(TX)> INCR k4
QUEUED
127.0.0.1:6379(TX)> INCR k5
QUEUED
127.0.0.1:6379(TX)> EXEC
1) (integer) 4
2) (error) ERR value is not an integer or out of range
3) (integer) 6
127.0.0.1:6379> GET k3
"4"
127.0.0.1:6379> GET k4
"v4"
127.0.0.1:6379> GET k5
"6"
```





## 14. 模擬排隊程式

```c#
// See https://aka.ms/new-console-template for more information
using StackExchange.Redis;

Console.WriteLine("Hello, World!");

// 建立 ConnectionMultiplexer
var redis = ConnectionMultiplexer.Connect("localhost");
IDatabase db = redis.GetDatabase();

// 清空可能存在的相關鍵
db.KeyDelete("queue");
db.KeyDelete("processed");

// 啟動兩個模擬客戶端
Task.Run(() => EnqueueCustomers(redis));
Task.Run(() => ProcessQueue(redis));

// 防止主執行緒退出
Console.ReadLine();


static async Task EnqueueCustomers(ConnectionMultiplexer redis)
{
    IDatabase db = redis.GetDatabase();

    int customerNumber = 1;

    while (true)
    {
        string customer = $"Customer-{customerNumber}";

        // 將客戶加入排隊
        db.ListRightPush("queue", customer);
        Console.WriteLine($"Enqueued {customer}");

        // 模擬客戶隨機到達的時間
        await Task.Delay(TimeSpan.FromSeconds(new Random().Next(1, 5)));

        customerNumber++;
    }
}

static async Task ProcessQueue(ConnectionMultiplexer redis)
{
    IDatabase db = redis.GetDatabase();

    while (true)
    {
        // 從排隊中取得下一位客戶
        string customer = db.ListLeftPop("queue");

        if (customer != null)
        {
            // 處理客戶
            Console.WriteLine($"Processing {customer}");

            // 將已處理的客戶加入已處理清單
            db.ListRightPush("processed", customer);
            
        }
        else
        {
            // 若排隊為空，等待一段時間再檢查
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}
```

### 14.1. 執行過程

![image-20231211164712006](https://i.imgur.com/Qzk5Pxh.png)

### 14.2. 觀查redis狀況

![image-20231211164740246](https://i.imgur.com/7ra7MSN.png)

## 15. 參考連結

- [GUI工具：GitHub:Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager/)
- [【GeekHour】一小时Redis教程](https://www.bilibili.com/video/BV1Jj411D7oG/)
- [谈谈Redis五种数据结构及真实应用场景](https://zhuanlan.zhihu.com/p/443370796)
  - 回過用這篇再補充筆記

- [分布式锁看这篇就够了](https://zhuanlan.zhihu.com/p/42056183)
  - 晚點回來看


## 16. 問題

- 請出redis 關於 「String」 的問題，請提供選項及答案，共5題。
- 請出redis 關於 「List」 的問題，請提供選項及答案，共5題。
- 請出redis 關於 「SET 集合」 的問題，請提供選項及答案，共5題。
- 請出redis 關於 「SortedSet集合」 的問題，請提供選項及答案，共5題。