using ServiceStack.Redis;

namespace BlazorRedis.Utility
{

    //This Utility makes use of the Redis ServiceStack Package
    public static class RedisUtility
    {
        private static string redisAddress = "127.0.0.1";
        private static int redisPort = 1020; //Convert.ToInt32(ConfigurationManager.AppSettings["RedisPort"] ?? "6379");
        private static string redisPassword = null;
        private static string instanceName = "Instance Name";

        public static bool SaveRecordAsync<T>(string key, T value, TimeSpan? timeSpan = null)
        {
            bool response = false;
            try
            {
                using (RedisClient redisClient = new RedisClient(redisAddress, redisPort, redisPassword))
                {
                    TimeSpan expires = timeSpan ?? TimeSpan.FromDays(10);
                    key = instanceName + key;
                    redisClient.As<T>().SetValue(key, value, expires);
                    response = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                response = false;
            }

            return response;
        }

        public static T GetRecordAsync<T>(string key)
        {
            try
            {
                using (RedisClient redisClient = new RedisClient(redisAddress, redisPort, redisPassword))
                {
                    key = instanceName + key;
                    var data = redisClient.As<T>().GetValue(key);
                    if (data == null) return default(T);
                    return data;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return default(T);
            }

        }


        public static bool DeleteRecordsAsync(List<string> keys)
        {
            try
            {
                using (var client = new RedisClient(redisAddress, redisPort, redisPassword))
                {
                    client.RemoveAll(keys);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static bool DeleteRecordAsync(string key)
        {
            try
            {
                using (var client = new RedisClient(redisAddress, redisPort, redisPassword))
                {
                    client.Remove(key);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}
