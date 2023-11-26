

namespace API.DTOs
{
    public class LikeDTO
    {
        public int id { get; set; }
        public string username { get; set; }
        public int age { get; set; }
        public string kwonas { get; set; }
        public string photourl { get; set; }
        public string city { get; set; }
        public List<PhotoDTO> photos {get; set;} = new();
    }
}