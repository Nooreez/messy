using Messenger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Messenger.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly MessContext _context;

        private readonly ILogger<MessController> _logger;

        private readonly IConfiguration _configuration;
        public UserController(MessContext context) {
            _context = context;
        }
        [HttpGet("chats")]
        public async Task<ActionResult<IEnumerable<Friends>>> ChatsList()
        {
            var user = User.Identity.Name;
            var userid = await _context.Users.FirstOrDefaultAsync(i => i.username == user);
            var list = await _context.Friends
                .Where(i => i.id1 == userid.id || i.id2 == userid.id)
                .OrderBy(i => i.lastMessageDate)
                .ToListAsync();

            return list;
        }
        [HttpPut("user/edit")]
        public async Task<IActionResult> Edit(Users user)
        {
            var item = await _context.Users.FirstOrDefaultAsync(i => i.id == user.id);
            item.name = user.name;
            _context.SaveChangesAsync();
            item.password = user.password;
            _context.SaveChangesAsync();
            item.email = user.email;
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("messages/get")]
        public async Task<ActionResult<IEnumerable<Messages>>> PrivateChat(string user2)
        {
            var user1 = User.Identity.Name;
            var list = await _context.Messages
                .Where(i =>
                    (i.to.Equals(user1) && i.from.Equals(user2)) ||
                    (i.to.Equals(user2) && i.from.Equals(user1))
                )
                .OrderBy(i => i.date)
                .ToListAsync();
            return list;
        }
        [HttpPost("messages/add")]
        public async Task<IActionResult> AddMessage(string to, string text)
        {
            string from = User.Identity.Name;
            var userto = await _context.Users.FirstOrDefaultAsync(i=>i.username == to);
            int toid = userto.id;
            var userfrom = await _context.Users.FirstOrDefaultAsync(i => i.username == from);
            int fromid = userfrom.id;
            DateTimeOffset currentUtcTime = DateTimeOffset.UtcNow;
            Messages messages = new Messages
            {
                from = fromid,
                to = toid,
                text = text,
                date = currentUtcTime
            };
            _context.Messages.Add(messages);

            var changeDate = await _context.Friends.FirstOrDefaultAsync(i =>
                (i.id1 == fromid && i.id2 == toid) ||
                (i.id1 == toid && i.id2 == fromid)
            );

            changeDate.lastMessageDate = currentUtcTime;

            await _context.SaveChangesAsync(); 
            
            changeDate.lastMessageText = text;

            await _context.SaveChangesAsync();
            return Ok(messages);
        }


        [HttpGet("friends")]
        public async Task<ActionResult<IEnumerable<string>>> FriendsList()
        {
            // Get the username from the authenticated user's claims
            var username = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(i => i.username == username);
            int userid = user.id;
            var friends = await _context.Friends.Where(i => i.id1==userid || i.id2.Equals(userid)).ToListAsync();
            var stringFriends = new List<string>();
            foreach (var friend in friends)
            {
                var user1 = await _context.Users.FirstOrDefaultAsync(i=>i.id == friend.id1);
                string z = user1.username;
                if (z == username)
                {
                    var user2 = await _context.Users.FirstOrDefaultAsync(i => i.id ==friend.id2);
                    z = user2.username; 
                }
                stringFriends.Add(z);
            }
            return stringFriends;
        }
        [HttpPost("friends/add")]
        public async Task<IActionResult> FriendsAdd(string username2)
        {
            var username1 = User.Identity.Name;
            var user1 = await _context.Users.FirstOrDefaultAsync(i => i.username == username1);
            var user2 = await _context.Users.FirstOrDefaultAsync(i => i.username == username2);
            int id1 = user1.id;
            int id2 = user2.id;
            if (username1 == null || username2 == null) return NotFound();

            _context.Friends.Add(new Friends(id1, id2));
            RequestDelete(username1,username2);
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("friends/check")]
        public async Task<ActionResult<bool>> FriendsCheck(string username1, string username2)
        {
            var user1 = await _context.Users.FirstOrDefaultAsync(i => i.username == username1);
            int id1 = user1.id;
            var user2 = await _context.Users.FirstOrDefaultAsync(i => i.username == username2);
            int id2 = user2.id;
            var requested = _context.Friends.Any(i => (i.id1 == id1 && i.id2 == id2) || (i.id1 == id2 && i.id2 == id1));
            return requested;
        }
        [HttpDelete("friends/delete")]
        public async Task<IActionResult> FriendsDelete(string to, string from)
        {
            var user1 = await _context.Users.FirstOrDefaultAsync(i => i.username == to);
            int id1 = user1.id;
            var user2 = await _context.Users.FirstOrDefaultAsync(i => i.username == from);
            int id2 = user2.id;
            var x = _context.Friends.FirstOrDefault(i => (i.id1 == id1 && i.id2 == id2) || (i.id1 == id2 && i.id2 == id1));
            _context.Friends.Remove(x);
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("requests/get")]
        public async Task<ActionResult<IEnumerable<RequestModel>>> FriendsRequestList(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i=>i.username==username);
            var id = user.id;
            var friendRequests = await _context.FriendsRequest.Where(i => i.to.Equals(id)).ToListAsync();
            var requestList = new List<RequestModel>();
            foreach (var friendRequest in friendRequests)
            {
                requestList.Add(new RequestModel
                {
                    from = _context.Users.FirstOrDefaultAsync(i => i.id == friendRequest.from).Result.username,
                    to = username
                });
            }
            return requestList;
        }
        [HttpGet("requests/send")]
        public async Task<ActionResult<IEnumerable<RequestModel>>> RequestList(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(i => i.username == username);
            var id = user.id;
            var friendRequests = await _context.FriendsRequest.Where(i => i.from.Equals(id)).ToListAsync();
            var requestList = new List<RequestModel>();
            foreach(var friendRequest in friendRequests)
            {
                requestList.Add(new RequestModel
                {
                    from = username,
                    to = _context.Users.FirstOrDefaultAsync(i => i.id == friendRequest.to).Result.username
                }) ; 
            }
            return requestList;
        }
        [HttpPost("request/add")]
        public async Task<IActionResult> RequestAdd(string to)
        {
            var from = User.Identity.Name;
            var userfrom = await _context.Users.FirstOrDefaultAsync(i => i.username == from);
            var userto = await _context.Users.FirstOrDefaultAsync(i => i.username == to);
            int fromid = userfrom.id;
            int toid = userto.id;
            _context.FriendsRequest.Add(new FriendsRequest(fromid, toid));
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("request/delete")]
        public async Task<IActionResult> RequestDelete(string to , string from)
        {
            var userfrom = await _context.Users.FirstOrDefaultAsync(i => i.username == from);
            var userto = await _context.Users.FirstOrDefaultAsync(i => i.username == to);
            int fromid = userfrom.id;
            int toid = userto.id;
            var x = _context.FriendsRequest.FirstOrDefault(i=> (i.to == toid && i.from == fromid) || (i.to == fromid && i.from == toid));
            _context.FriendsRequest.Remove(x);
            _context.SaveChangesAsync();
            return Ok();
        }
        [HttpGet("request/check")]
        public async Task<ActionResult<bool>> RequestsCheck(string from, string to)
        {
            var userfrom = await _context.Users.FirstOrDefaultAsync(i => i.username == from);
            var userto = await _context.Users.FirstOrDefaultAsync(i => i.username == to);
            int fromid = userfrom.id;
            int toid = userto.id;
            var requested = _context.FriendsRequest.Any(i => i.from==fromid && i.to == toid);
            return requested;
        }
    }
}
