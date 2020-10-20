namespace Stemma.Services.DTOs.Request
{
    public class GalleryPerson
    {
        public long Id { get; set; }
        public long GalleryIDF { get; set; }
        public long PersonIDF { get; set; }
        public bool IsProfilePicture { get; set; }
    }
}
