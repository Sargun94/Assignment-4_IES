using XML.Models;
using XML.Models.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;  
using System.Xml.Linq;
namespace XML
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> directories = new List<string>();
            directories = FTP.GetDirectory(Constants.FTP.BaseUrl);
             
            List<Student> stud_list = new List<Student>();

            // Print Each Directory
            foreach (var directory in directories)
            {
                Console.WriteLine("Directory: " + directory);
            }
            
            //Iterate over all Directories and print student information

            foreach (var directory in directories)
            {
                Student student = new Student() { AbsoluteUrl = Constants.FTP.BaseUrl };
                student.FromDirectory(directory);

                string infoFilePath = student.FullPathUrl + "/" + Constants.Locations.Info_File;

                //Check if File exists
                bool fileExists = FTP.FileExists(infoFilePath);
                if (fileExists == true)
                {

                    Console.WriteLine("Found info file:");

                    byte[] bytes = FTP.DownloadFileData(infoFilePath);

                    string csvData = Encoding.Default.GetString(bytes);

                    string[] csvlines = csvData.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
                    if (csvlines.Length != 2)
                    {
                        Console.WriteLine("Error in CSV format.");
                    }
                    else
                    {
                        student.FromCSV(csvlines[1]);
                    }
                }
                else
                {
                    Console.WriteLine("Could not find info file:");
                }

                //Print Info File Path
                Console.WriteLine("Info File Path::: " + infoFilePath);

                string imageFilePath = student.FullPathUrl + "/" + Constants.Locations.Image_File;

                bool imageFileExists = FTP.FileExists(imageFilePath);

                if (imageFileExists == true)
                {

                    Console.WriteLine("Found image file:");
                    Console.WriteLine("Image File Path::: " + imageFilePath);
                }
                else
                {
                    Console.WriteLine("Could not find image file:");
                }

                Console.WriteLine("Image File Path::: " + imageFilePath);

                stud_list.Add(student);
            }


            //-----------------JSON------------------------------------
            List<JsonModel> jsons = new List<JsonModel>();

            using (StreamWriter fs = new StreamWriter(Constants.Locations.CSV_File))
            {
              
                fs.WriteLine((nameof(Student.StudentId)) + ',' + (nameof(Student.FirstName)) + ',' + (nameof(Student.LastName)) + ',' + (nameof(Student.Age)) + ',' + (nameof(Student.DateOfBirth)) + ',' + (nameof(Student.MyRecord)) + ',' + (nameof(Student.ImageData)));
                foreach (var student in stud_list)
                {
                    fs.WriteLine(student.ToCSV());
                    Console.WriteLine("CSV : " + student.ToCSV());
                    Console.WriteLine("String : " + student.ToString());

                    JsonModel model = new JsonModel();
                    model.Student(student);
                    jsons.Add(model);



                }
            }


            string json = System.Text.Json.JsonSerializer.Serialize(jsons);
            File.WriteAllText(Models.Constants.Locations.JSON_File, json);

            string[] source = File.ReadAllLines(Constants.Locations.CSV_File);
            source = source.Skip(1).ToArray();



            XElement xElement = new XElement("Root",
                from str in source
                let fields = str.Split(',')
                select new XElement("Students",
                  new XAttribute("StudentID", fields[0]),

                    new XAttribute("FirstName", fields[1]),
                    new XElement("LastName", fields[2]),
                    new XElement("Age", fields[3]),
                    new XElement("DateOfBirth", fields[4]),
                    new XElement("ImageData", fields[5])

                    )
            );
            Console.WriteLine(xElement);
            xElement.Save(Constants.Locations.XML_File);
            Console.WriteLine("Total Item in List Count: " + stud_list.Count());

            int count_startswith = 0;

            //Iterate over Each Student In stud_list
            foreach (var list in stud_list)
            {
                if (list.FirstName.StartsWith("S"))
                {
                    count_startswith++;
                    Console.WriteLine("Starts With S: " + list);
                }
            }

            Console.WriteLine("Count Starts With S: " + count_startswith);

            //Search my record

            Student searchMyId = stud_list.Find(x => x.StudentId == "200450515");
            Console.WriteLine("My Record : " + searchMyId);

            //Print Min,Average,Max age
            var average_age = stud_list.Average(x => x.Age);
            var minimum_age = stud_list.Min(x => x.Age);
            var maximum_age = stud_list.Max(x => x.Age);

            Console.WriteLine("Average Age: " + average_age);

            Console.WriteLine("Minimum Age:" + minimum_age);
            Console.WriteLine("Maximum Age:" + maximum_age);

            //Create Word Document
            CreateWordDoc.CreateWordprocessingDocument(Constants.Locations.DOC_File, Constants.Locations.img_path, stud_list);
           
            //Create Spreadsheet
            CreateSpreadSheet.CreateSpreadsheetWorkbook(Constants.Locations.Excel_File, stud_list);

            //Upload all files on FTP
            FTP.UploadFile(Constants.Locations.CSV_File, Constants.FTP.CSVUploadLocation);
            FTP.UploadFile(Constants.Locations.XML_File, Constants.FTP.XMLUploadLocation);
            FTP.UploadFile(Constants.Locations.JSON_File, Constants.FTP.JSONUploadLocation);
            FTP.UploadFile(Constants.Locations.DOC_File, Constants.FTP.DocUploadLocation);     
            FTP.UploadFile(Constants.Locations.Excel_File, Constants.FTP.ExcelUploadLocation);

            Console.WriteLine( "Files Uploaded Successfully" );
            return;

        }

    }
}
       
