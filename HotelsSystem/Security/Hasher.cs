
namespace HotelsSystem.Security
{
    public class Hasher
    {
        public static string Hash(int id)
        {
            var hash = new Hashids(salt: Util.SecurityGuid, minHashLength: 5);
            var Encoded = hash.Encode(id);

            return Encoded;
        }

        public static int UnHash(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    var hash = new Hashids(salt: Util.SecurityGuid, minHashLength: 5);
                    var Encoded = hash.Decode(id);

                    if (Encoded == null || Encoded.Length < 1)
                    {
                        throw new ArgumentNullException();
                    }
                    return Encoded[0];
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}