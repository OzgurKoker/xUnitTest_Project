namespace ZarenTest.API.DTOs
{
    public class ResponseDTO
    {
        public Data Data { get; set; }
        public string Message { get; set; }
        public int Status { get; set; }
    }
    public class ResponseItem
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public int Provider { get; set; }
    }

    public class Data
    {
        public List<ResponseItem> Items { get; set; }
    }

}
