namespace XetAPI.Model
{
    public class ConversationModel
    {
        public string Answer { get; set; } = string.Empty;
        public string Question { get; set; } = string.Empty;
        public EContext Context { get; set; } = EContext.JSON;
    }
}
