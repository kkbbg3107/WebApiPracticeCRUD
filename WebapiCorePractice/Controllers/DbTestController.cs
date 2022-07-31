using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebapiCorePractice.DTO;
using WebapiCorePractice.DTOs;
using WebapiCorePractice.Helper;
using WebapiCorePractice.Models;
using WebapiCorePractice.Services;

namespace WebapiCorePractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbTestController : Controller
    {
        private readonly TodoContext _todoContext;
        private readonly GetService _getService;

        public DbTestController(TodoContext todoContext, GetService getService)
        {
            _todoContext = todoContext;
            _getService = getService;
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
        public IEnumerable<object> GetAll([FromBody] JsonBody value)
        {
            if (_todoContext.Players is null)
            {
                return null;
            }


            return _getService.GetAllData(value);
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
            
            return _getService.GetSingleData(value);
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
 