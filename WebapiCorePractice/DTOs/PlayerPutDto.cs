using WebapiCorePractice.VaildationAttribute;

namespace WebapiCorePractice.DTOs
{
    public class PlayerPutDto
    {
        public int Id { get; set; }
        [TodoName]
        public string Name { get; set; }

        public int Rank { get; set; }
    }
}
