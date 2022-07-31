using Newtonsoft.Json;
using WebapiCorePractice.Helper;
using WebapiCorePractice.Models;

namespace WebapiCorePractice.Services
{
    public class GetService
    {
        private readonly TodoContext _todoContext;
        
        public GetService(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public object GetSingleData(JsonBody value)
        {
            var json = JsonConvert.DeserializeObject<Player>(value.Json.ToString());
            var collection = _todoContext.Players.ToArray();

            var element = collection.Where(x => x.Id == json.Id).First();

            return element;
        }

        public IEnumerable<object> GetAllData(JsonBody value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }               

            var collection = _todoContext.Players.ToArray();

            return collection;
        }
    }
}
