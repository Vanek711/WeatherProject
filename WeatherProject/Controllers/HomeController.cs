using Microsoft.AspNetCore.Mvc;
using WeatherProject.Models;
using System.Diagnostics;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using EFCore.BulkExtensions;
using System.Data.SqlClient;
using WeatherProject.Database;
using WeatherProject.Models.WeatherData;
using WeatherProject.Models.Pager;
using System.Drawing;
using static System.Runtime.InteropServices.JavaScript.JSType;
using NPOI.POIFS.Crypt.Dsig;

namespace WeatherProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly DatabaseContext _dbocontext;

        public HomeController(DatabaseContext context)
        {
            _dbocontext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public ActionResult ClearDatabase()
        {
            string connectionString = "data source=IVAND;initial catalog=WeatherDatabase;integrated security=true;TrustServerCertificate=True";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand("TRUNCATE TABLE Weather", connection);
                int rowsAffected = command.ExecuteNonQuery();
            }

            // Возвращаемся на главную страницу или на другую страницу после очистки базы данных
            return RedirectToAction("Index");
        }

        //Метод необходимый для множественной загрузки ( пока что просто перебирает много раз одиночную загрузку)
        public IActionResult MultiUploadToDb([FromForm] IFormCollection ExcelFiles)
        {
            try
            {
                IFormFileCollection files = ExcelFiles.Files;

                //string connectionString = "data source=IVAND;initial catalog=WeatherDatabase;integrated security=true;TrustServerCertificate=True";
                //////Очистка базы данных
                //SqlConnection connection = new SqlConnection(connectionString);
                //connection.Open();
                //SqlCommand command = new SqlCommand("TRUNCATE TABLE Weather", connection);
                //int rowsAffected = command.ExecuteNonQuery();
                //connection.Close();
                string filExamination = string.Empty;
                foreach (var file in files)
                {
                    if(UploadToDb(file))
                    {
                        filExamination += "Загружено:"+file.FileName;
                    }
                    else
                    {
                        filExamination += "Обновлено:" + file.FileName;
                    }
                }
                return StatusCode(StatusCodes.Status200OK, new { message = $"{filExamination}" });
            }
            catch (NullReferenceException e)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { message = "Возникла ошибка" });

            }
        }

        //Логика загрузки в БД считывается по строчно каждая книга Excel
        
        public bool UploadToDb( IFormFile ArchivoExcel)
        {
            Stream stream = ArchivoExcel.OpenReadStream();
            int countRows = 0;
            IWorkbook MiExcel = null;
            
            if (Path.GetExtension(ArchivoExcel.FileName) == ".xlsx")
            {
                MiExcel = new XSSFWorkbook(stream);
            }
            else
            {
                MiExcel = new HSSFWorkbook(stream);
            }
            

            for (int j = 0; j < 12; j++)
            {
                ISheet NewSheet = MiExcel.GetSheetAt(j);

                int quantityRows = NewSheet.LastRowNum;

                List<WeatherTypes> list = new List<WeatherTypes>();

                for (int i = 4; i <= quantityRows; i++)
                {

                    IRow file = NewSheet.GetRow(i);

                    list.Add(new WeatherTypes
                    {
                        Id = i + countRows,
                        Data = file.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Time = file.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Temperature = file.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Relativehumidity = file.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Dewpoint = file.GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Atmosphericpressure = file.GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Directionofthewind = file.GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Windspeed = file.GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Cloudiness = file.GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        H = file.GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Vv = file.GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                        Weatherconditions = file.GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString(),
                    });
                }
                if (_dbocontext.Weather.Any(o => o.Data == list[0].Data)) return false;
                if (_dbocontext.Database.CanConnect())
                {
                    _dbocontext.BulkInsert(list);
                }
                countRows = quantityRows + countRows;

            }
            return true;
        }

        //Логика пагинации + логика поиска 
        //Если в поиске пусто - отображать все, иначе отображать все, что начинается с Search
        public IActionResult DisplayDb(int pg = 1, int pageSize = 10, string Search = "", string SearchMonth= "", string SearchYear ="")
        {
            List<WeatherTypes> weatherings;
            List<WeatherTypes> data = new List<WeatherTypes>();
            if (!string.IsNullOrEmpty(SearchMonth))
            {
                Search = SearchMonth;
            }
            if (!string.IsNullOrEmpty(SearchYear))
            {
                Search += SearchYear;
            }
            if (pg < 1) pg = 1;

            if (string.IsNullOrEmpty(Search))
            {
                weatherings = _dbocontext.Weather.ToList();

            }
            else
            {
                weatherings = _dbocontext.Weather.Where(p => p.Data.Contains(Search)).ToList();
            }
            int resCount = weatherings.Count();
            if (resCount == 0) return View(data);

            Pager pager = new Pager();
            int recSkip = 0;
            if (!string.IsNullOrEmpty(Search))
            {
                pager = new Pager(resCount, pg, resCount);

                recSkip = (pg - 1) * resCount;
            }
            else
            {
                pager = new Pager(resCount, pg, pageSize);

                recSkip = (pg - 1) * pageSize;

            }

            data = weatherings.Skip(recSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;

            return View(data);
        }

        public IActionResult StartPage()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}