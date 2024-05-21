using AktywniAPI.Model;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Data.SqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AktywniAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AktywniController : ControllerBase
    {
        readonly IConfiguration _config;
        public AktywniController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("LastAktywnosc")]
        public async Task<ActionResult<AktywnośćUsr>> LastAktywnosc()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var userAkt = await connection.QueryAsync<AktywnośćUsr>("Select Top 1 AktywnośćId from AktywnośćUser order by AktywnośćId desc");
            return Ok(userAkt);
        }

        [HttpGet("GetUsers")]
        public async Task<ActionResult<List<Users>>> GetUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<Users>("Select * from Users");
            return Ok(users);
        }
        [HttpGet("GetInfo")]
        public async Task<ActionResult<List<Users>>> GetInfo()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<Users>("Select Nick,Email from Users");
            return Ok(users);
        }


        [HttpGet("GetUserById/{idU}")]
        public async Task<ActionResult<List<Users>>> GetUsersById(int idU )
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstOrDefaultAsync<Users>("Select * from Users where IdU = @idU ",
                new { idU = idU, });
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("GetUserInfo/{email}/{password}")]
        public async Task<ActionResult<List<Users>>> GetUsersByEmail(string email, string password)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var user = await connection.QueryFirstOrDefaultAsync<Users>("Select * from Users where Email = @email and Password = @password ",
                new { email = email, password = password });
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpGet("GetNazwaAktywności")]
        public async Task<ActionResult<List<Aktywność>>> GetNazwa()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<Aktywność>("Select * from Aktywność");
            return Ok(users);
        }
        [HttpGet("GetIdAktywności/{nazwa}")]
        public async Task<ActionResult<List<Aktywność>>> GetId(string nazwa)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var users = await connection.QueryAsync<Aktywność>("Select IdAktywności from Aktywność where Nazwa = @nazwa ", new {nazwa = nazwa});
            return Ok(users);
        }

        [HttpGet("GetActive/{Id}")]
        public async Task<ActionResult<List<AktywnośćUser>>> GetAktywnośćDostępna(int Id)
        {
            var czas = DateTime.Now.Date;
            var godzinaOd = DateTime.UtcNow.TimeOfDay;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var dostepna = await connection.QueryAsync<AktywnośćUser>($"Select distinct Aktywność.Obrazek, AktywnośćUser.AktywnośćId, AktywnośćUser.Opis,Aktywność.Nazwa, AktywnośćUser.Data,AktywnośćUser.GodzinaOd,AktywnośćUser.GodzinaDo,AktywnośćUser.MiejsceDoc,AktywnośćUser.IlośćMiejsc,AktywnośćUser.Count from AktywnośćUser, AktywnośćUsr,Aktywność" +
                $" where Aktywność.IdAktywności = AktywnośćUser.IdAktywności and " +
                $"(Data > @czas or (Data = (  SELECT FORMAT (@czas, 'yyyy-MM-dd ')) and GodzinaOd >= @godzinaOd))" +
                $" and AktywnośćUser.AktywnośćId = AktywnośćUsr.AktywnośćId  and AktywnośćUser.AktywnośćId not in" +
                $" ( Select AktywnośćId from AktywnośćUsr where IdU = @Id)", new { czas = czas, godzinaOd = godzinaOd, Id = Id });
            return Ok(dostepna);
        }
        [HttpGet("GetAktywnośćNadchodząca/{Id}")]
        public async Task<ActionResult<List<AktywnośćUser>>> GetAktywnośćZaplanowana(int Id)
        {
            var czas = DateTime.Now.Date;
            var godzinaOd = DateTime.UtcNow.TimeOfDay;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var zaplanowana = await connection.QueryAsync<AktywnośćUser>(@"Select AktywnośćUser.AktywnośćId,Aktywność.Nazwa, Aktywność.Obrazek, AktywnośćUser.Data,AktywnośćUser.GodzinaOd, AktywnośćUser.GodzinaDo, AktywnośćUser.MiejsceDoc, AktywnośćUser.Opis, AktywnośćUser.Count, AktywnośćUser.IlośćMiejsc From Aktywność, AktywnośćUsr, AktywnośćUser where (Data > @czas or (Data = (  SELECT FORMAT (@czas, 'yyyy-MM-dd ')) and GodzinaOd >= @godzinaOd)) and AktywnośćUsr.IdU = @Id and Aktywność.IdAktywności = AktywnośćUser.IdAktywności and AktywnośćUsr.AktywnośćId = AktywnośćUser.AktywnośćId
            ", new {czas = czas, godzinaOd = godzinaOd, Id = Id });
            return Ok(zaplanowana);
        }

        [HttpGet("GetHistoria/{Id}")]
        public async Task<ActionResult<List<HistoriaUser>>> GetHistorieAktywności(int Id)
        {
            var czas = DateTime.Now.Date;
            var godzinaOd = DateTime.UtcNow.TimeOfDay;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var history = await connection.QueryAsync<AktywnośćUser>("Select AktywnośćUser.AktywnośćId, Aktywność.Nazwa, Aktywność.Obrazek, AktywnośćUser.Data, AktywnośćUser.MiejsceDoc, AktywnośćUser.Opis, AktywnośćUser.GodzinaOd, AktywnośćUser.GodzinaDo, AKtywnośćUser.Count, AktywnośćUser.IlośćMiejsc From Aktywność,AktywnośćUsr, AktywnośćUser where (AktywnośćUser.Data < @czas or ( AktywnośćUser.Data =@czas and AktywnośćUser.GodzinaOd<@godzinaOd)) and AktywnośćUsr.IdU = @Id and Aktywność.IdAktywności = AktywnośćUser.IdAktywności and AktywnośćUsr.AktywnośćId = AktywnośćUser.AktywnośćId", new {czas= czas, godzinaOd =godzinaOd, Id = Id});
            return Ok(history);
        }

        [HttpGet("GetLastHistoria/{Id}")]
        public async Task<ActionResult<List<HistoriaUser>>> GetLastHistoria(int Id)
        {
            var czas = DateTime.Now.Date;
            var godzinaOd = DateTime.UtcNow.TimeOfDay;
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var history = await connection.QueryAsync<AktywnośćUser>("Select Top 1 AktywnośćUser.AktywnośćId, Aktywność.Nazwa, Aktywność.Obrazek, AktywnośćUser.Data, AktywnośćUser.MiejsceDoc, AktywnośćUser.Opis, AktywnośćUser.GodzinaOd, AktywnośćUser.GodzinaDo, AKtywnośćUser.Count, AktywnośćUser.IlośćMiejsc From Aktywność,AktywnośćUsr, AktywnośćUser where (AktywnośćUser.Data < @czas or ( AktywnośćUser.Data =@czas and AktywnośćUser.GodzinaOd<@godzinaOd)) and AktywnośćUsr.IdU = @Id and Aktywność.IdAktywności = AktywnośćUser.IdAktywności and AktywnośćUsr.AktywnośćId = AktywnośćUser.AktywnośćId order by AktywnośćUser.Data desc ", new { czas = czas, godzinaOd = godzinaOd, Id = Id });
            return Ok(history);
        }

        [HttpGet("GetAllPost")]
        public async Task<ActionResult<List<PostUser>>> GetPostUsers()
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var history = await connection.QueryAsync<PostUser>("Select Users.Nick, PostUsers.IdPost, Aktywność.Nazwa, Aktywność.Obrazek,PostUsers.Temat, PostUsers.Opis , PostUsers.DataWpisu, PostUsers.IdU, PostUsers.Wyświetlenia,PostUsers.IdAktywności from PostUsers,Aktywność, Users where PostUsers.IdAktywności = Aktywność.IdAktywności and PostUsers.IdU = Users.IdU");
            return Ok(history);
        }

        [HttpGet("GetPostUser/{idU}")]
        public async Task<ActionResult<List<PostUser>>> GetPostUser(int idU)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            var history = await connection.QueryAsync<PostUser>("Select PostUsers.IdPost, Aktywność.Nazwa, Aktywność.Obrazek, PostUsers.Opis , PostUsers.DataWpisu, PostUsers.IdU, PostUsers.Wyświetlenia, PostUsers.Temat, PostUsers.IdAktywności, Users.Nick from PostUsers,Aktywność,Users where PostUsers.IdAktywności = Aktywność.IdAktywności and  PostUsers.IdU = Users.IdU  and Users.IdU=@IdU", new { IdU= idU});
            return Ok(history);
        }

        // Dodawanie 
        [HttpPost("/AddUserAccount")]
        
        public async Task<ActionResult<List<Users>>> AddUserAccount(Users users)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Insert into Users (Email, Password, Nick ) Values (@email, @password, @nick)",users);
            return Ok();
        }

        [HttpPost("/AddUserPost")]
        public async Task<ActionResult<List<Users>>> AddUserPost(PostUsers postmodels)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Insert Into PostUsers Values (@opis,@dataWpisu, @IdU, @IdAktywności, 0, @Temat)", postmodels);

            return Ok();
        }

        [HttpPost("/AddActive")]
        public async Task<ActionResult<List<AktywnośćUser>>> AddActive(AktywnośćUser active)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Insert into AktywnośćUser  Values (@GodzinaOd, @GodzinaDo, @Data, @Opis, @MiejsceDoc, @IlośćMiejsc, @IdAktywności, 0)", active);
            return Ok();
        }

       
        [HttpPost("/AddActiveUser")]
        public async Task<ActionResult<List<AktywnośćUsr>>> AddActiveUser(AktywnośćUsr usr)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Insert into AktywnośćUsr Values (@idU, @aktywnośćId)", usr);
            await connection.ExecuteAsync("Update AktywnośćUser set Count  = Count +1 where AktywnośćId = @id ", new { id = usr.AktywnośćId });

            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<List<Users>>> UpdateUsersInfo(Users usermodels)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update Users set Email = @email, Password = @password, Nick = @nick, Płeć = @płeć, Waga = @waga, Bmi = @bmi, RokUrodzenia = @rokUrodzenia, Wzrost=@wzrost where idU = @idU", usermodels);

            return Ok();
        }

        [HttpPut("UpdatePost")]
        public async Task<ActionResult<List<Users>>> UpdatePost(PostUsers post)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update PostUsers set Opis = @opis, DataWpisu = @dataWpisu, IdU = @idU, IdAktywności = @idAktywności, Wyświetlenia = @wyświetlenia, Temat = @temat where IdPost = @idPost", post);

            return Ok();
        }


        [HttpPut("AddWyświetlenie")]
        public async Task<ActionResult<List<PostUsers>>> AddWyświetlenie(PostUsers postWys)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Update PostUsers set Wyświetlenia = Wyświetlenia + 1 where IdPost = @idPost and IdU != @idU ", new { idPost = postWys.IdPost, idU=postWys.IdU });
            return Ok();
        }

        [HttpDelete("DeleteUserAccount/{Id}")]
        public async Task<ActionResult<List<Users>>> DeleteUser (int Id)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(" DELETE FROM Users WHERE IdU = @Id", new {Id= Id});

            return Ok();
        }

        [HttpDelete("DeletePostUser/{Id}/{PostId}")]
        public async Task<ActionResult<List<Users>>> DeletePostUser(int Id, int PostId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync(" Delete from PostUsers Where IdU = @idU and IdPost = @idPost", new { idU = Id, idPost = PostId });

            return Ok();
        }

        [HttpDelete("/DeleteActiveUser/{idU},{Aktywność}")]
        public async Task<ActionResult<List<AktywnośćUsr>>> DeleteActiveUser(int idU, int Aktywność)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("Delete AktywnośćUsr where IdU = @idU and AktywnośćId= @AktywnośćId", new { idU = idU, AktywnośćId = Aktywność });
            await connection.ExecuteAsync("Update AktywnośćUser set count  = count -1 where AktywnośćId = @AktywnośćId  ", new { AktywnośćId = Aktywność });
            return Ok();
        }


        [HttpDelete("/DeleteUserPost/{AktywnośćId}")]
        public async Task<ActionResult<List<AktywnośćUsr>>> DeleteUserPost(int AktywnośćId)
        {
            using var connection = new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            await connection.ExecuteAsync("delete from AktywnośćUser where AktywnośćId = @AktywnośćId  ", new { AktywnośćId = AktywnośćId });
            return Ok();
        }
    }
}
