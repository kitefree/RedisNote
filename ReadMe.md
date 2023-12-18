---
title: Redis學習筆記

date: 2023-10-16 11:01

categories: ['後端']

tags: ['Redis','高可用']


---

## 1. Redis簡介

- Remote dictionary server

- 可以用於數據庫、緩存、消息隊列等各種場景

- 最熱門NoSQL數據庫之一

- 基於內存數據庫(記憶體效能>IO效能，也就是Redis>MySQL效能)

- GitHub、Twitter、Stackoverflow、百度都在使用

<!--more-->

## 2. Redis特點

- 高性能Key-Value

- 多種數據結構，單鍵值對最大支持512M大小的數據

- 豐富的功能

- 支持數據持久化、主從複製、哨兵模式等高可用分佈特性

### 2.1. 數據類型

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

### 2.2. 使用方式

主要三種類型

- CLI
  - redis-cli

- API
  - 簡單來說就是用程式碼(python、java)的方式進行使用
- GUI
  - another-redis-desktop-manager


## 3. 環境安裝

下載docker image

```
docker pull redis
```

container build

```
docker run --name redis-lab -p 6379:6379 -d redis
```

## 4. Redis GUI 工具

### 4.1. 這次透過指令方式安裝

```bash
choco install another-redis-desktop-manager
```

### 4.2. 執行結果

```bash
C:\Windows\System32>choco install another-redis-desktop-manager
Chocolatey v0.11.1
Installing the following packages:
another-redis-desktop-manager
By installing, you accept licenses for the packages.
Progress: Downloading another-redis-desktop-manager 1.6.1... 100%
```

### 4.3. 確認docker id

```
C:\Windows\System32>docker ps
CONTAINER ID   IMAGE                      COMMAND                  CREATED          STATUS                 PORTS                            NAMES
65b2b326dc9c   redis                      "docker-entrypoint.s…"   12 minutes ago   Up 12 minutes          0.0.0.0:6379->6379/tcp           redis-lab
```

### 4.4. 進入docker操作redis

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

### 4.5. 透過GUI工具操作

![image-20231211164508060](https://i.imgur.com/J0Uw01L.png)

## 5. String 字串

### 5.1. 簡介

1. **Key-Value 存儲**: Redis 的 STRING 是一個 key-value 存儲結構，每個 STRING 都有一個唯一的 key 用來標識。
2. **二進制安全**: Redis 的 STRING 不僅可以存儲文本字符串，還可以存儲任何二進制數據，這使得它非常適合作為緩存系統。
3. **操作豐富**: Redis 提供了豐富的 STRING 操作命令，包括設置、獲取、刪除、追加等，使得對字符串的操作非常靈活。

### 5.2. 使用場景

#### 5.2.1. 快取 (Caching)

作為一個支持高性能的快取系統，Redis 的 STRING 可以用來存儲經常被訪問的數據，例如網頁內容、查詢結果，以減輕後端數據庫的壓力。

```bash
# 範例：將結果存入 Redis 快取
SET user:1234:profile "{'id':1234, 'name':'John Doe', 'age':30}"
```

#### 5.2.2. 計數器 (Counters)

可以使用 Redis 的自增（INCR）和自減（DECR）操作，實現計數器的功能，適用於需要跟蹤計數的場景，比如網站訪問次數、商品庫存等。

```bash
# 範例：增加訪問次數
INCR website:visit:count
```

#### 5.2.3. 會話管理 (Session Management)

存儲和管理用戶的會話數據，比如登錄信息、訪問權限等。

```bash
# 範例：存儲用戶會話信息
SET session:userid:1234 "{'token':'abc123', 'expires':'2023-12-31'}"
```

### 5.3. 常用指令

#### 5.3.1. SET - 設定key 

```bash
127.0.0.1:6379> set kite nice
OK
```

#### 5.3.2. GET - 讀取key 

```bash
127.0.0.1:6379> get kite
"nice"
```

#### 5.3.3. key大小寫有分

```bash
127.0.0.1:6379> set Kite Nice
OK
127.0.0.1:6379> get Kite
"Nice"
```

#### 5.3.4. DEL - 刪除key

```bash
127.0.0.1:6379> del name
(integer) 0

127.0.0.1:6379> get name
(nil)
```

#### 5.3.5. EXISTS - 查詢key

```bash
127.0.0.1:6379> exists name
(integer) 0

127.0.0.1:6379> exists age
(integer) 1
```

#### 5.3.6. KEYS - 萬用字查詢

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

#### 5.3.7. FLUSHALL - 刪除所有key

```bash
127.0.0.1:6379> flushall
OK
127.0.0.1:6379> keys *
(empty array)
```

#### 5.3.8. 中文顯示說明

```bash
127.0.0.1:6379> set name "悠風"
OK
127.0.0.1:6379> get name
"\xe6\x82\xa0\xe9\xa2\xa8"
```

#### 5.3.9. redis-cli --raw  正常顯示中文方法

```bash
127.0.0.1:6379> quit
# redis-cli --raw
127.0.0.1:6379> get name
悠風
```

#### 5.3.10. EXPIRE - 設定過期

```bash
127.0.0.1:6379> expire name 10
```

#### 5.3.11. TTL - 查詢過期

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

#### 5.3.12. SETEX - 設定key + 過期

```bash
127.0.0.1:6379> setex name 5 悠風
OK
127.0.0.1:6379> ttl name
1
127.0.0.1:6379> ttl name
-2
```

#### 5.3.13. SETNX - 該值不存在則建立

```bash
127.0.0.1:6379> setnx name kite
1
```

## 6. LIST 列表

### 6.1. 簡介

在 Redis 中，LIST 是一種有序、可重複的資料結構，它允許存儲一個有序的元素列表。每個元素都有一個索引，從 0 開始。LIST 提供了豐富的操作命令，可以在列表的頭部或尾部執行插入、刪除等操作，同時支持範圍查詢和修剪。可實現最簡單的先進先出隊列也就是結合`LPUSH`(左進) + `RPOP`(右出)

LIST 資料結構的特點：

1. **有序性**: 元素在列表中的存儲順序是有序的，可以按照索引進行訪問。
2. **重複元素**: 元素可以重複存在於列表中，這使得 LIST 可以用來表示多個相同類型的元素。
3. **支持隊列和堆疊操作**: LIST 可以被當作隊列（先進先出，FIFO）或堆疊（後進先出，LIFO）來使用。

### 6.2. 使用場景

#### 6.2.1. 日誌記錄

隊列的特點是先進先出,後進後出.我們可以使用lpush 命令從隊列的左邊放入,然後利用rpop命令從右邊取出,這樣就模擬實現了隊列.可以用來記錄日誌等

#### 6.2.2. 抽獎,搶票

list 是線程安全的,所有的pop操作是原子性的,適用於抽獎,搶票等場景,用來防止超賣問題.這裡重點解釋一下抽獎:主要是分為三步

1. 全部獎品打散放入list中
2. 呼叫pop指令從list中取出
3. 將中獎記錄寫入資料庫

#### 6.2.3. 流量消峰

將所有的請求全部放到list中,然後開啟多個線程來處理後續請求,減輕伺服器壓力,用來處理一些高並發場景.



### 6.3. 常用指令

#### 6.3.1. LPUSH

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



#### 6.3.2. LRANGE

-1 代表全部

```bash
> LRANGE letter 0 -1
1) "e"
2) "d"
3) "c"
4) "b"
5) "a"
```

#### 6.3.3. RPUSH

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

#### 6.3.4. LLEN

```bash
> LLEN letter
(integer) 3
```

#### 6.3.5. LTRIM

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



## 7. SET 集合

### 7.1. 簡介

SET 與 LIST 最大的差異是沒有順序，並且不能有重複值

#### 7.1.1. 什麼是 Redis SET 資料結構？

在 Redis 中，SET 是一種集合型資料結構，用來存儲多個唯一且無序的元素。SET 不允許相同的元素存在，這意味著每個元素在 SET 中都是唯一的。

#### 7.1.2. Redis SET 的主要特性：

- **唯一性（Unique）**：SET 中的每個元素都是唯一的，不允許重複的元素存在。
- **無序性（Unordered）**：SET 中的元素是無序的，不像 LIST 或 ZSET 有順序的概念。
- **操作豐富**：Redis 提供了豐富的 SET 操作命令，包括添加成員、刪除成員、檢查成員是否存在等。

### 7.2. 使用場景

redis set是集合類型的資料結構，那麼集合類型就比較適合用於聚合分類。

#### 7.2.1. 標籤

例如我們部落格網站常常使用到的興趣標籤，把一個個有著相同嗜好，關注類似內容的用戶利用一個標籤把他們進行歸併。 

#### 7.2.2. 共同

共同好友功能，共同喜好，或是可以引申到二度好友之類的擴充應用。

#### 7.2.3. 統計

統計網站的獨立IP。利用set集合當中元素不唯一性，可以快速即時統計存取網站的獨立IP。

### 7.3. 常用指令

#### 7.3.1. SADD

```bash
> SADD course Redis
(integer) 1

#故意再加一次，加不進去
> SADD course Redis
(integer) 0
> SMEMBERS course
1) "Redis"
```

#### 7.3.2. SMEMBERS

```bash
> SMEMBERS course
1) "Redis"
```

#### 7.3.3. SISMEMBER

```bash
> SISMEMBER course Redis
(integer) 1

> SISMEMBER course Python
(integer) 0
```

#### 7.3.4. SREM

```bash
> SREM course Redis
(integer) 1

> SMEMBERS course
(empty list or set)
```

#### 7.3.5. 其它學習

SINTER、SUNION、SDIFF

## 8. SortedSet 有序集合

### 8.1. 簡介

#### 8.1.1. 什麼是 Redis Sorted Set 資料結構？

在 Redis 中，Sorted Set（有序集合）是一種集合型資料結構，它與 SET 類似，但每個成員都關聯著一個分數（score）。這個分數用來排序集合中的元素，因此 Sorted Set 是根據分數有序排列的。

#### 8.1.2. Redis Sorted Set 的主要特性：

- **有序性（Ordered）**：元素根據其分數的大小進行排序，可以按照分數區間或排名獲取元素。
- **唯一性（Unique）**：每個元素在集合中是唯一的，不允許重複的成員存在。
- **操作豐富**：Redis 提供了豐富的 Sorted Set 操作命令，包括添加成員、刪除成員、獲取排名、按照分數區間獲取成員等。

### 8.2. 使用場景

#### 8.2.1. 排行榜（Leaderboard）

Sorted Set 非常適用於實現排行榜，其中成員的分數表示其在排行中的位置，可以根據分數的高低快速獲取排名。

```bash
# 範例：將玩家分數存入排行榜
ZADD leaderboard 1000 "Player1"
ZADD leaderboard 950 "Player2"
```

#### 8.2.2. 過期時間管理

可以使用 Sorted Set 來管理帶有過期時間的數據，分數表示過期時間的時間戳，並使用定期的清理操作來刪除過期的數據。

```bash
# 範例：將帶有過期時間的數據存入有序集合
ZADD expiration:tasks <timestamp> "Task1"
ZADD expiration:tasks <timestamp> "Task2"
```

#### 8.2.3. 區間查詢

Sorted Set 支持根據分數區間進行查詢，這使得它適用於需要根據數值範圍檢索數據的場景。

```bash
# 範例：獲取分數在指定區間的成員
ZRANGEBYSCORE temperature 0 25
```

#### 8.2.4. 數據匹配和搜索

Sorted Set 可以用於實現模糊搜索，分數可以表示相似度或匹配程度，使得可以按照相似度檢索數據。

```bash
# 範例：根據匹配程度獲取成員
ZRANGEBYSCORE similarity 0.8 1.0
```

#### 8.2.5. 即時排名

Sorted Set 可以用於即時排名的場景，例如遊戲中的實時得分排名。

```bash
# 範例：遊戲中即時更新玩家分數
ZADD realtime:scoreboard 1200 "Player1"
```

### 8.3. 常用指令

#### 8.3.1. ZADD

```bash
127.0.0.1:6379> ZADD result 100 小民 90 小華 80 小新
3
```

#### 8.3.2. ZRANGE

只呈現人名

```bash
127.0.0.1:6379> ZRANGE result 0 -1
小新
小華
小民
```

#### 8.3.3. ZRANGE WITHSCORES

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

#### 8.3.4. ZSCORE

呈現分數

```bash
127.0.0.1:6379> ZSCORE result 小民
100
```

#### 8.3.5. ZRANK

有序集合預設是由小到大排序，索引從0開始，小民分數最多，所以索引是2

```bash
127.0.0.1:6379> ZRANK result 小民
2
```

#### 8.3.6. ZREVRANK

rev是指reverse 反轉，也就是由大到小

```bash
127.0.0.1:6379> ZREVRANK result 小民
0
```

#### 8.3.7. 其它學習

比如刪除某個成員，對某個成員分數進行增加，刪除某個排名範圍的成員

## 9. Hash 哈希

### 9.1. 簡介

key-value資料結構，特別適合來儲存物件。

### 9.2. 使用場景

#### 9.2.1. 對象存儲

Hash 非常適用於存儲對象的屬性，例如用戶對象、產品對象等，其中每個字段表示對象的一個屬性，對應的值是這個屬性的值。

```bash
# 範例：存儲用戶對象
HSET user:1234 name "John Doe"
HSET user:1234 age 30
HSET user:1234 email "john@example.com"
```

#### 9.2.2. 網站會話管理

Hash 可以用於存儲網站中的會話信息，其中每個字段表示一個會話屬性，對應的值是這個會話屬性的值。

```bash
# 範例：存儲會話信息
HSET session:abc123 userid 1234
HSET session:abc123 last_access "2023-12-31 23:59:59"
```

#### 9.2.3. 配置存儲

Hash 可以用於存儲應用程序的配置信息，其中每個字段表示一個配置項，對應的值是這個配置項的值。

```bash
# 範例：存儲應用程序配置
HSET app:config max_connections 100
HSET app:config log_level "info"
```

### 9.3. 常用指令

#### 9.3.1. HSET

```bash
127.0.0.1:6379> HSET person name kite
1
127.0.0.1:6379> HSET person age 100
1
```

#### 9.3.2. HGET

```bash
127.0.0.1:6379> HGET person name
kite
```

#### 9.3.3. HGETALL

```bash
127.0.0.1:6379> HGETALL person
name
kite
age
100
```

#### 9.3.4. HDEL

```bash
127.0.0.1:6379> HDEL person age
1
```

#### 9.3.5. HGETALL

```bash
127.0.0.1:6379> HGETALL person
name
kite
```

#### 9.3.6. HEXISTS

```bash
127.0.0.1:6379> HEXISTS person name
1
127.0.0.1:6379> HEXISTS person age
0
```

#### 9.3.7. HKEYS

```bash
127.0.0.1:6379> HKEYS person
name
```

#### 9.3.8. HLEN

```bash
127.0.0.1:6379> HLEN person
1
```

## 10. 發佈訂閱模式

### 10.1. 常用指令

#### 10.1.1. subscribe

```bash
subscribe ubereats
```

#### 10.1.2. publish

```bash
publish ubereats order001
```

下圖左邊是publish發佈消息，右邊是兩個訂閱者，當publish時，訂閱者們會同步收到消息。

![image-20231214115332780](https://i.imgur.com/vCBtT5O.png)

## 11. GEO

### 11.1. 常用指令

#### 11.1.1. GEOADD

經度(116.405285)、緯度(39.904989)、名稱(beijing)

```bash
#新增一個
127.0.0.1:6379> GEOADD city 116.405285 39.904989 beijing
1

#新增多個
127.0.0.1:6379> GEOADD city 121.472644 31.231706 shanghai 114.085947 22.547 shenzhen 37 23.125178 guangzhou 120.153576 30.287459 hangzhou
4
```

#### 11.1.2. GEOPOS

取得地理位置

```bash
127.0.0.1:6379> GEOPOS city beijing
116.40528291463851929
39.9049884229125027
```

#### 11.1.3. GEODIST

取得北京到上海的距離，預設單位為公尺(M)

```bash
127.0.0.1:6379> GEODIST city beijing shanghai
1067597.9668

#以KM單位顯示
127.0.0.1:6379> GEODIST city beijing shanghai KM
1067.5980
```

#### 11.1.4. GEOSEARCH...FROMMEMBER...BYRADIUS...

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

## 12. HyperLogLog

### 12.1. 常用指令

#### 12.1.1. PFADD

建立course課程變數，放三個課程

```bash
127.0.0.1:6379> PFADD course git docker redis
1


```

#### 12.1.2. PFCOUNT

計算課程

```bash
127.0.0.1:6379> PFCOUNT course
3

127.0.0.1:6379> PFADD course nginx
1
127.0.0.1:6379> PFCOUNT course
4
```

#### 12.1.3. PFMEGER

建立course2課程變數，放三個課程，通過`PMERGE`合併課程，再進行計算。

```bash
127.0.0.1:6379> PFADD course2 python git go
1

127.0.0.1:6379> PFMERGE result course course2
OK
127.0.0.1:6379> PFCOUNT result
6

```

## 13. Bitmap 位圖

![image-20231214142550274](https://i.imgur.com/kzsdXDW.png)

### 13.1. 常用指令

#### 13.1.1. SETBIT

![image-20231214142638511](https://i.imgur.com/dp0k5Nw.png)

```bash
127.0.0.1:6379> SETBIT dianzan 0 1
0
127.0.0.1:6379> SETBIT dianzan 1 0
0
```

#### 13.1.2. GETBIT

```bash
127.0.0.1:6379> GETBIT dianzan 0
1
127.0.0.1:6379> GETBIT dianzan 1
0
```

#### 13.1.3. SET

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

#### 13.1.4. BITCOUNT

```bash
127.0.0.1:6379> BITCOUNT dianzan
4
```

#### 13.1.5. BITPOS

```bash
127.0.0.1:6379> BITPOS dianzan 0
4
```

## 14. BitField

![image-20231214144020140](https://i.imgur.com/A2UKBL3.png)

### 14.1. 常用指令

#### 14.1.1. BITFIELD...SET...

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

#### 14.1.2. BITFIELD...GET...

```bash
127.0.0.1:6379> BITFIELD player:1 get u8 #0
1

127.0.0.1:6379> BITFIELD player:1 get u32 #1
100
```

#### 14.1.3. BITFIELD...incrby...

```bash
#增加100金幣
127.0.0.1:6379> BITFIELD player:1 incrby u32 #1 100
200
```



## 15. Multi

在執行exec之前，所有命令都會放到佇列中，不會立即執行。

收到exec命令後，有任何一命令失敗，並不影響其它命令。

在執行exec之前，其它客戶提出的請求，並不會被插入到multi裡面。

![image-20231214153249916](https://i.imgur.com/koP5xYq.png)

### 15.1. 常用指令

#### 15.1.1. MULTI

啟動

#### 15.1.2. EXEC

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

## 16. 持久化

### 16.1. 常用指令

### 16.2. RDB(Redis Database)

分為自動與手動，自動部分需要去`redis.config`檔進行修正，以下是截取部分：

```bash
# Unless specified otherwise, by default Redis will save the DB:

#  定時1小時檢查，是否有有1次以上異動，才符合自動儲存條件
#   * After 3600 seconds (an hour) if at least 1 change was performed
#  定時5分鐘檢查，是否有100次以上異動，才符合自動儲存條件
#   * After 300 seconds (5 minutes) if at least 100 changes were performed
#  定時60秒檢查，是否有10000次以上異動，才符合自動儲存條件
#   * After 60 seconds if at least 10000 changes were performed
```

我們可以觀查到，第一個參數表示秒數，第二參數表示執行次數。而docker redis 預設是沒有包`redis.config`檔，所以如果要的話請去官網下載，可到下方參考連結去使用，下載檔案之後，在啟動docker時記得mount此份文件，修改其內容。

手動部分實驗如下：

```bash
# redis-cli
127.0.0.1:6379> keys *
(empty array)

127.0.0.1:6379> set k1 k1
OK
127.0.0.1:6379> set k2 k2
OK

#手動儲存
127.0.0.1:6379> save
OK

#跳出redis
127.0.0.1:6379> exit

#查看文件
# ls
dump.rdb

#使用xxd指令查看檔案
# xxd dump.rdb
00000000: 5245 4449 5330 3031 31fa 0972 6564 6973  REDIS0011..redis
00000010: 2d76 6572 0537 2e32 2e33 fa0a 7265 6469  -ver.7.2.3..redi
00000020: 732d 6269 7473 c040 fa05 6374 696d 65c2  s-bits.@..ctime.
00000030: b358 7c65 fa08 7573 6564 2d6d 656d c2c8  .X|e..used-mem..
00000040: 3420 00fa 0861 6f66 2d62 6173 65c0 00fe  4 ...aof-base...
00000050: 00fb 0200 0002 6b32 026b 3200 026b 3102  ......k2.k2..k1.   
00000060: 6b31 ff7c 8471 bff0 fe75 7c              k1.|.q...u|    
```

可以看到最後兩行有剛剛k1與k2的資料，代表已經實質的從記憶體儲放一份到硬碟上，至於多久要寫入一次的要根據伺服器可用的效能去評估與設定。

> 補充說明：xxd是一個可以查看2進制或16進制文件內容的linux命令

另外，通常在使用redis時配給的資源通常會給比較多，而在使用`save`指令時，redis是無法接受任何請求的，所以有可能會發生阻塞。於是redis又提供了另一個指令`bgsave`，這個指令可以直接多開一個執行緒，讓它獨立做記憶體搬到IO的工作。這樣的作法可以讓阻塞問題得到緩解。但是在fork出一個新的執行緒的過程中還是會佔用效能，而且在fork過程式，還是沒有辦法處理任何請求，沒有辦法做到秒級的快照。

為了解決這個問題，又提供了AOF的持久化方式。

### 16.3. AOF(Append Only File)

`AOF`意思是指追加文件，當使用指令時，同時寫入`記憶體`與`追加文件`，文件以日誌的方式記錄每一個寫入操作，當REDIS重啟動時，就會通過重新執行`AOF`文件中的命令來在記憶體中重建整個資料庫的內容。開啟AOF的方式也很簡單，只要在`redis.config`將`appendonly` 後方的`no` 改為`yes`即可。

## 17. Replication 主從複製

Master(主)節點主要負責`寫`的處理，Slave(從)節點負責`讀`的處理，主節點的數據變化會通過非同步方式發送給從節點，從節點收到資料後，會更新自己的數據

![Redis: Master-Slave Architecture. Redis is based on Master-Slave… | by  Sunny Garg | Medium](https://i.imgur.com/tMREXiC.png)

### 17.1. Master 建置

這次實驗透過docker進行，準備1個master，2個slave。

docker預設都是吃同一個switch設定，所以自然可以透過區網方式互相訪問。

開始建置master容器，容器取名為`redis-master`，指令如下：

```bash
docker run --name redis-master -d -p 6379:6379 redis
```

我們為了要取得master區網IP，因此需進入容器中，安裝`net-tools`工具：

```bash
apt-get update
apt-get install net-tools
```

使用`ifconfig`指令，從訊息中得知ip為`172.17.0.2`：

```bash
# ifconfig
eth0: flags=4163<UP,BROADCAST,RUNNING,MULTICAST>  mtu 1500
        inet 172.17.0.2  netmask 255.255.0.0  broadcast 172.17.255.255
        ether 02:42:ac:11:00:02  txqueuelen 0  (Ethernet)
        RX packets 7614  bytes 9949276 (9.4 MiB)
        RX errors 0  dropped 0  overruns 0  frame 0
        TX packets 2430  bytes 162974 (159.1 KiB)
        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0

lo: flags=73<UP,LOOPBACK,RUNNING>  mtu 65536
        inet 127.0.0.1  netmask 255.0.0.0
        loop  txqueuelen 1000  (Local Loopback)
        RX packets 61  bytes 208513 (203.6 KiB)
        RX errors 0  dropped 0  overruns 0  frame 0
        TX packets 61  bytes 208513 (203.6 KiB)
        TX errors 0  dropped 0 overruns 0  carrier 0  collisions 0
```

### 17.2. Slave 建置

方法1

使用此方法，不需要準備`redis.conf`檔

```bash
docker run -it --name redis-slave01 -p 6380:6379 redis redis-server --slaveof 172.17.0.2 6379
```

方法2

建置slave容器之前，我們準備一個`redis.conf`檔，僅說明需要修改部分(完整檔案放在參考連結那)：

```bash
replicaof 172.17.0.2 6379
```

使用方法2，開始建置slave容器，容器取名為`redis-slave01`，指令如下：

```bash
docker run --name redis-slave01 -d -p 6380:6379 -v C:/docker_data/redis-6380.conf:/usr/local/etc/redis/redis-6380.conf redis redis-server /usr/local/etc/redis/redis-6380.conf
```

說明相關指令

`v C:/docker_data/redis-6380.conf:/usr/local/etc/redis/redis-6380.conf redis`：將準備好的檔案放在c槽，並以volumn 方式放到docker指定位置。

` redis-server /usr/local/etc/redis/redis-6380.conf`：redis啟動後參考此config設定

### 17.3. 實驗階段

#### 17.3.1. 進入`Master`容器中，簡單設定幾個變數

```bash
# redis-cli
127.0.0.1:6379> keys *
(empty array)
127.0.0.1:6379> set k1 k1
OK
127.0.0.1:6379> set k2 k2
OK
```

#### 17.3.2. 進入`Slave`容器中，確認同步狀況

```bash
docker exec -it 274f /bin/bash
root@274f984345a9:/data# redis-cli
127.0.0.1:6379> keys *
1) "k2"
2) "k1"
```

#### 17.3.3. 再增加一個`Slave`，這次特別使用方法1：

```bash
docker run -it --name redis-slave02 -p 6381:6379 redis redis-server --slaveof 172.17.0.2 6379
```

節點資源最後如下

![image-20231216103946054](https://i.imgur.com/XBw6qQy.png)

#### 17.3.4. Master開始寫入資料，觀查Slave是否同步，如下：

![image-20231216103824221](https://i.imgur.com/zXP20s8.png)

## 18. 哨兵模式

如果某個節點發生問題，那麼哨兵就會通過發佈訂閱模式來通知其它節點。當主節點不能正常工作時，哨兵會開始一個自動故障轉移的操作，它會將一個從節點升級為新的主節點，然後再將其他從節點指向新的主節點。

![圖片](https://i.imgur.com/XHcuLXb.png)

### 18.1. 首先，準備四個docker(1主、2從、1哨)

```bash
docker run -it --name redis-master -p 6379:6379 redis redis-server --slaveof 172.17.0.2 6379
docker run -it --name redis-slave01 -p 6380:6379 redis redis-server --slaveof 172.17.0.2 6379
docker run -it --name redis-slave02 -p 6381:6379 redis redis-server --slaveof 172.17.0.2 6379
docker run -it --name redis-sentinel -p 6382:6379 redis
```

結果如下：

![redis002](https://i.imgur.com/0xUrsaR.png)

### 18.2. 建立`sentinel.conf`檔

進入到`redis-sentinel`容器中，建立`sentinel.conf`檔，檔案內容如下：

```bash
sentinel monitor mymaster 172.17.0.2 6379 1
```

參數說明

- `mymaster` 自取名稱，任意填寫

- `172.17.0.2` 填master ip

- `6379` master port

- `1` 投票人數，這邊只有一個哨兵所以就填1即可。

### 18.3. 啟動哨兵模式

在`redis-sentinel`容器中，執行指令`redis-sentinel sentinel.conf`，成功啟動預期如下圖：

![redis001](https://i.imgur.com/B7boGE0.png)

### 18.4. 模擬master中斷

![redis003](https://i.imgur.com/helyHzC.png)

### 18.5. 觀查哨兵自動轉換

哨兵在自動切換需要一些時間，等待一下…就會看到訊息開始在運作，透過`info replication`查看，可以觀查到原本的`slave01`升級變為`master`，實驗結束，結果如下圖：

![redis004](https://i.imgur.com/52ZiVNr.png)

## 19. 模擬排隊程式

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

### 19.1. 執行過程

![image-20231211164712006](https://i.imgur.com/Qzk5Pxh.png)

### 19.2. 觀查redis狀況

![image-20231211164740246](https://i.imgur.com/7ra7MSN.png)

## 20. 參考連結

- [GUI工具：GitHub:Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager/)
- [【GeekHour】一小时Redis教程](https://www.bilibili.com/video/BV1Jj411D7oG/)
- [谈谈Redis五种数据结构及真实应用场景](https://zhuanlan.zhihu.com/p/443370796)
- [分布式锁看这篇就够了](https://zhuanlan.zhihu.com/p/42056183)
  - 有空記得看
- [官網Redis 7.2 config 參考](https://raw.githubusercontent.com/redis/redis/7.2/redis.conf)
- [個人筆記-RedisNote](https://github.com/kitefree/RedisNote)
  - 包含程式碼範例、redis config檔