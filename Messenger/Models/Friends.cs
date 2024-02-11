namespace Messenger.Models
{
    public class Friends
    {
        public int id { get; set; }
        public int id1 { get; set; }
        public int id2 { get; set; }
        public DateTimeOffset lastMessageDate { get; set; }
        public string lastMessageText { get; set; }

        public Friends(int id1, int id2)
        {
            this.id1 = id1;
            this.id2 = id2;
            this.lastMessageDate = DateTimeOffset.MinValue;
            this.lastMessageText = string.Empty;
        }
    }
}