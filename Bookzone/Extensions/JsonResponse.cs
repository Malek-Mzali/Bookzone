namespace Bookzone.Extensions
{
    public class JsonResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        
        public string Extra { get; set; }

        public JsonResponse()
        { }
    }
}
