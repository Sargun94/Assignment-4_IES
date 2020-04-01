using System;

namespace XML.Models
{
    public class Constants
    {

        public readonly Student Student = new Student { StudentId = "200450515", FirstName = "SargunSingh", LastName = "Walia" };

        public class Locations
        {
            public readonly static string DesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);


            public readonly static string ContentFolder = $"{ContentFolder}\\..\\..\\..\\Content";
            public readonly static string DataFolder = $"{ContentFolder}\\Data";
            public readonly static string ImagesFolder = $"{ContentFolder}\\Images";
            public readonly static string FilesFolder = $"{ContentFolder}\\Files";
            public readonly static string DOC_File = "C:/Users/sargu/Desktop/Assignment-4_IES/info.docx";
            public readonly static string Excel_File = "C:/Users/sargu/Desktop/Assignment-4_IES/info.xlsx";

            public const string Info_File = "info.csv";
            public const string Image_File = "myimage.jpg";

            public static readonly string img_path = "C:/Users/sargu/Desktop/Assignment-4_IES/myimage.jpg";
            public readonly static string CSV_File = "C:/Users/sargu/Desktop/Assignment-4_IES/student.csv";
            public readonly static string JSON_File = "C:/Users/sargu/Desktop/Assignment-4_IES/students.json";
            public static readonly string XML_File = "C:/Users/sargu/Desktop/Assignment-4_IES/students.xml";
          
        }

        public class FTP
        {
            public const string Username = @"bdat100119f\bdat1001";
            public const string Password = "bdat1001";

            public const string BaseUrl = "ftp://waws-prod-dm1-127.ftp.azurewebsites.windows.net/bdat1001-20914";
            public const string MyDirectory = "/200450515 SargunSingh Walia";
            public const string CSVUploadLocation = BaseUrl + MyDirectory + "/students.csv";
            public const string JSONUploadLocation = BaseUrl + MyDirectory + "/students.json";
            public const string XMLUploadLocation = BaseUrl + MyDirectory + "/students.xml";
            public const string DocUploadLocation = BaseUrl + MyDirectory + "/info.docx";
            public const string ExcelUploadLocation = BaseUrl + MyDirectory + "/info.xlsx";
            public const int OperationPauseTime = 10000;

        }
    }
}
