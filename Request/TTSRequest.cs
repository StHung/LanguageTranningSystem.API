namespace LanguageTranningSystem.API.Request
{
    public record TTSRequest
    {
        public string Model { get; init; } = "tts-1-hd";
        public string Input { get; init; }
        public string Voice { get; init; } = "shimmer";
        public string? Response_format { get; init; } = "mp3";
        public double speed { get; set; } = 1;
    }
}
