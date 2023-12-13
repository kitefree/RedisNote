

## 哪些公司在使用Redis

GitHub、Twitter、Stackoverflow、百度

## Redis特點

- 高性能Key-Value

- 多種數據結構

- 豐富的功能

- 高可用分佈支持



## 環境安裝

下載docker image

```
docker pull redis
```

container build

```
docker run --name redis-lab -p 6379:6379 -d redis
```



## Redis GUI 工具

### 這次透過指令方式安裝

```bash
choco install another-redis-desktop-manager
```

### 執行結果

```bash
C:\Windows\System32>choco install another-redis-desktop-manager
Chocolatey v0.11.1
Installing the following packages:
another-redis-desktop-manager
By installing, you accept licenses for the packages.
Progress: Downloading another-redis-desktop-manager 1.6.1... 100%
```

### 確認docker id

```
C:\Windows\System32>docker ps
CONTAINER ID   IMAGE                      COMMAND                  CREATED          STATUS                 PORTS                            NAMES
65b2b326dc9c   redis                      "docker-entrypoint.s…"   12 minutes ago   Up 12 minutes          0.0.0.0:6379->6379/tcp           redis-lab
```

### 進入docker操作redis

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

### 透過GUI工具操作

![image-20231211164508060](https://i.imgur.com/J0Uw01L.png)

## 模擬排隊程式

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

### 執行過程

![image-20231211164712006](https://i.imgur.com/Qzk5Pxh.png)

### 觀查redis狀況

![image-20231211164740246](https://i.imgur.com/7ra7MSN.png)

## 參考連結

[GitHub:Another Redis Desktop Manager](https://github.com/qishibo/AnotherRedisDesktopManager/)