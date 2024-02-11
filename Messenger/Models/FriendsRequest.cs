namespace Messenger.Models
{
    public class FriendsRequest
    {
        public int id { get; set; }
        public int from { get; set; }
        public int to { get; set; }
        public FriendsRequest(int from, int to)
        {
            this.from = from;
            this.to = to;
        }
    }
}
