using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebapiCorePractice.DTO;
using WebapiCorePractice.DTOs;
using WebapiCorePractice.Helper;
using WebapiCorePractice.Models;

namespace WebapiCorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbTestController : Controller
    {
        private readonly TodoContext _todoContext;

        //private readonly ITypeConvert _typeConvert;

        public DbTestController(
            TodoContext todoContext
            //, ITypeConvert typeConvert
            )
        {
            _todoContext = todoContext;
            //_typeConvert = typeConvert;
        }

        [HttpGet()]
        public ActionResult<IEnumerable<Player>> Get()
        {
            return _todoContext.Players;
        }

        [HttpGet("GetTarGet/{id}")]
        public ActionResult<Player> GetTarGet(int id)
        {
            var player = _todoContext.Players.ToDictionary(data => data.Id);

            if (!player.TryGetValue(id, out var value))
            {
                return NotFound($"找不到id {id}");
            }
            return Ok(value);
        }

        /// <summary>
        /// 讀全樣本
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost("GetAll")]
        public ActionResult<Player> GetAll([FromBody] Player value)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (_todoContext.Players is null)
            {
                return NotFound();
            }

            var collection = _todoContext.Players.ToArray();

            return Ok(collection);
        }

        /// <summary>
        /// 讀指定樣本
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // todo json序列化
        [HttpPost("GetTarget/{typeName}")]
        public ActionResult<object> GetTarget([FromRoute] string typeName, [FromBody] JsonBody value)
        {
            //_typeConvert.Convert(typeName);

            var json = JsonConvert.DeserializeObject<Player>(value.Json.ToString());
            var collection = _todoContext.Players.ToArray();

            var element = collection.Where(x => x.Id == json.Id).First();

            return element;
        }

        /// <summary>
        /// 練習取DTO資料
        /// </summary>
        /// <param name="value">source</param>
        /// <returns>dto</returns>
        [HttpPost("GetName")]
        public ActionResult<PlayerDTO> GetName([FromBody] Player value)
        {
            var collection = _todoContext.Players.ToDictionary(key => key.Id);

            if (!collection.TryGetValue(value.Id, out var element))
            {
                return NotFound();
            }

            var result = new PlayerDTO(element.Id, element.Name);

            return result;
        }

        /// <summary>
        /// 新增資料表Player資料
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost()]
        public ActionResult<Player> Post([FromBody] Player value)
        {
            _todoContext.Players.Add(value);
            _todoContext.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }

        [HttpPost("{typeName}")]
        public ActionResult<object> AddTarget([FromRoute] string typeName, [FromBody] JsonBody value)
        {
            var json = JsonConvert.DeserializeObject<Player>(value.Json.ToString());

            _todoContext.Players.Add(json);
            _todoContext.SaveChanges();


            return CreatedAtAction(nameof(Get), new { id = json.Id }, json);
        }

        [HttpPost("AddDto")]
        public void Post([FromBody] PlayerPostDto value)
        {
            Player insert = new Player
            {
                Id = value.Id,
                Name = value.Name,
                Rank = value.Rank,
            };

            _todoContext.Players.Add(insert);
            _todoContext.SaveChanges();
        }

        [HttpPut("UpdateDto/{id}")]
        public void Put(int id,[FromBody] PlayerPutDto value)
          {
            var update = _todoContext.Players.Where(x => x.Id == id).FirstOrDefault();

            if (update is not null)
            {
                update.Id = id;
                update.Name = value.Name;
                update.Rank = value.Rank;

                _todoContext.SaveChanges();
            }


        }

        [HttpPut("{id}")]
        public IActionResult Put(int id,[FromBody] Player value)
        {
            if (id != value.Id)
            {
                return BadRequest();
            }

            _todoContext.Entry(value).State = EntityState.Modified;

            try
            {
                _todoContext.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (!_todoContext.Players.Any(x => x.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "存取失敗");
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var delete = _todoContext.Players.Where(x => x.Id == id).FirstOrDefault();

            if (delete is null)
            {
                return NotFound();
            }

            _todoContext.Players.Remove(delete);
            _todoContext.SaveChanges();

            return NoContent();           
        }
    }
}
 