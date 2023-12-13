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