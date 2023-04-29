namespace HotelsSystem.Security
{
    public class Protection
    {
        public static T? Decrypt<T>(string val)
        {
            try
            {
                var Decoded = HT.Core.Encrypt.Decode(val, key: Util.SecurityGuid);

                var Deserialized = JsonConvert.DeserializeObject<byte[]>(Decoded);

                if (Deserialized == null)
                    return default;

                var Result = JsonConvert.DeserializeObject<T>(StringCompression.Decompress(Deserialized));

                return Result;
            }
            catch
            {
                return default;
            }
        }

        public static string Encrypt<T>(T obj)
        {
            var Result = JsonConvert.SerializeObject(obj);
            var Compress = StringCompression.Compress(Result);
            var SerializeByte = JsonConvert.SerializeObject(Compress);

            var Encoded = HT.Core.Encrypt.Encode(SerializeByte, key: Util.SecurityGuid);

            return Encoded;
        }
        public static async Task<SPResult> GetDecryptedSession(IJSRuntime JSRuntime, ISqlDataAccess db,ISessionStorageService storage)
        {
            try
            {
                string Cookie = await storage.GetItemAsync<string>(Util.CookieName);
                // string Cookie = await JSRuntime.InvokeAsync<string>("blazorExtensions.getCookie",Util.CookieName);

                var session = Decrypt<SPResult>(Cookie);

                if (session == null || session == null || session.Result <= 0)
                // return new SPResult{Result=1};
                throw new Exception();

                ClS_Config config = new ClS_Config(db, session);

                session.Roles = (await config.GetAllRoles()).ToList();

                return session;
            }
            catch
            {

                throw;
            }
        }

        public static async Task SetEncryptedSession(SPResult obj, IJSRuntime jSRuntime,ISessionStorageService storage)
        {
            var session = Encrypt(obj);
            // await jSRuntime.InvokeVoidAsync("blazorExtensions.WriteCookie", session,Util.CookieName);
            await storage.SetItemAsync<string>(Util.CookieName,session);
        }
    }
}