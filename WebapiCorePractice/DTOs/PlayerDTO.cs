using WebapiCorePractice.VaildationAttribute;

namespace WebapiCorePractice.DTO
{
    public class PlayerDTO
    {

        public PlayerDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }

        
        public int Id { get; }

        [TodoName]
        public string Name { get; }
    }
}
