using back_aspnet.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;


namespace back_aspnet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantsController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public RestaurantsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select RestaurantId, RestaurantName, RestaurantAddress, RestaurantDescription, RestaurantPhoto from dbo.Restaurant
            ";

            DataTable table=new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("myDb");
            SqlDataReader myReader;
            using(SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Restaurant rest)
        {
            string query = @"
                            insert into dbo.Restaurant 
                            (RestaurantName, RestaurantAddress, RestaurantDescription, RestaurantPhoto)
                     values (@RestaurantName, @RestaurantAddress, @RestaurantDescription, @RestaurantPhoto)
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("myDb");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@RestaurantName", rest.RestaurantName);
                    myCommand.Parameters.AddWithValue("@RestaurantAddress", rest.RestaurantAddress);
                    myCommand.Parameters.AddWithValue("@RestaurantDescription", rest.RestaurantDescription);
                    myCommand.Parameters.AddWithValue("@RestaurantPhoto", rest.RestaurantPhoto);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Added succesfully");
        }

        [HttpPut]
        public JsonResult Put(Restaurant rest)
        {
            string query = @"
                            update dbo.Restaurant 
                        set RestaurantName = @RestaurantName,
                            RestaurantAddress = @RestaurantAddress,
                            RestaurantDescription = @RestaurantDescription,
                            RestaurantPhoto = @RestaurantPhoto
                            where RestaurantId = @RestaurantId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("myDb");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@RestaurantId", rest.RestaurantId);
                    myCommand.Parameters.AddWithValue("@RestaurantName", rest.RestaurantName);
                    myCommand.Parameters.AddWithValue("@RestaurantAddress", rest.RestaurantAddress);
                    myCommand.Parameters.AddWithValue("@RestaurantDescription", rest.RestaurantDescription);
                    myCommand.Parameters.AddWithValue("@RestaurantPhoto", rest.RestaurantPhoto);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Updated succesfully");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                           delete from dbo.Restaurant
                            where RestaurantId = @RestaurantId
                            ";

            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("myDb");
            SqlDataReader myReader;
            using (SqlConnection myConn = new SqlConnection(sqlDataSource))
            {
                myConn.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myConn))
                {
                    myCommand.Parameters.AddWithValue("@RestaurantId", id);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myConn.Close();
                }
            }

            return new JsonResult("Deleted succesfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception)
            {

                return new JsonResult("anonymous.png");
            }
        }
    }
}
