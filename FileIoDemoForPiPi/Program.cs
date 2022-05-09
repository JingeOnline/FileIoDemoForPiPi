using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace FileIoDemoForPiPi
{

    class Program
    {
        /// <summary>
        /// 文件在硬盘中的路径
        /// </summary>
        static readonly string FilePath = @"C:\CSV Output Folder\Students.csv";
        /// <summary>
        /// 全局变量，学生对象列表
        /// </summary>
        static List<Student> StudentList = new List<Student>();

        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to FileIoDemoForPiPi app!\n");
            ReadFile();
            while (true)
            {
                GetUserOrder();
            }
        }

        /// <summary>
        /// 读取文件，把学生信息读取到全局的StudentList中
        /// </summary>
        /// <returns></returns>
        public static void ReadFile()
        {
            List<string> lineList = new List<string>();
            try
            {
                string[] lines = File.ReadAllLines(FilePath);
                lineList = new List<string>(lines);
                lineList.RemoveAt(0);
            }
            catch
            {
                ConsoleWriteYellowLine("Cannot find the file in " + FilePath+", the app will create it automatically.\n");
                CreateFile();
                ReadFile();
            }
            try
            {
                foreach (string line in lineList)
                {
                    Student student = new Student(line);
                    StudentList.Add(student);
                }
            }
            catch
            {
                ConsoleWriteRedLine("Cannot parse records, the file format is not match with [Id],[Name],[History],[Math],[Physics],[Geography].");
            }


        }

        /// <summary>
        /// 显示指令
        /// </summary>
        public static void showInstruction()
        {
            Console.WriteLine("[0] Show Student List");
            Console.WriteLine("[1] Create new record");
            Console.WriteLine("[2] Delete record by Id");
            Console.WriteLine("[3] Update record by Id");
            Console.WriteLine("[4] Query record by Id");
            Console.WriteLine("[S] Save and Exit");
            Console.WriteLine("[Q] Exit without Saving ");
            Console.WriteLine();
            Console.WriteLine("Any thing I can help you?");
        }

        /// <summary>
        /// 获取用户选择的指令
        /// </summary>
        public static void GetUserOrder()
        {
            showInstruction();
            //前面添加一个空行，为了美观
            Console.WriteLine();
            string userInput = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            switch (userInput)
            {
                case "0": ShowStudentList(); break;
                case "1": Create(); break;
                case "2": Delete(); break;
                case "3": Update(); break;
                case "4": Query(); break;
                case "s":
                case "S": SaveAndExit(); break;
                case "q":
                case "Q": ExitWithoutSaving(); break;
                default: GetUserOrder(); break;
            }
        }

        /// <summary>
        /// 显示所有的学生列表
        /// </summary>
        public static void ShowStudentList()
        {
            ConsoleWriteYellowLine(Student.GetHeaderLineForPrint());
            foreach (Student s in StudentList)
            {
                ConsoleWriteYellowLine(s.ToString());
            }
            //后面添加一个空行，为了美观
            Console.WriteLine();
        }

        /// <summary>
        /// 新增一条学生记录
        /// </summary>
        public static void Create()
        {
            Console.WriteLine("Please follow the format [Name],[History],[Math],[Physics],[Geography]...");
            string line = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            try
            {
                //获得当前学生列表中，最大的学生Id
                int maxId = StudentList.Max(x => x.Id);
                Student student = new Student(maxId + 1, line);
                StudentList.Add(student);
                //Console.WriteLine("Create successfully.\n");
                ConsoleWriteYellowLine("Create successfully.\n");
            }
            catch
            {
                ConsoleWriteRedLine("Create fail. Please check your input formate and enter again.\n");
                Create();
            }
        }

        /// <summary>
        /// 删除一条学生记录
        /// </summary>
        public static void Delete()
        {
            Console.WriteLine("Please enter the Id your want to delete...");
            string line = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            //指定Id，获取该学生对象
            Student student = StudentList.FirstOrDefault(x => x.Id.ToString() == line);
            if (student == null)
            {
                ConsoleWriteRedLine("Cannot find the student Id. Please check your input and enter again.\n");
                Delete();
            }
            else
            {
                StudentList.Remove(student);
                ConsoleWriteYellowLine("Delete Successfully.\n");
            }

        }

        /// <summary>
        /// 修改一条学生记录
        /// </summary>
        public static void Update()
        {
            Student student = QueryStudentForUpdate();
            UpdateStudent(student);
        }

        /// <summary>
        /// 查询某个学生的信息
        /// </summary>
        public static void Query()
        {
            Console.WriteLine("Please enter the Id or Name your want to query...");
            string line = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            //指定Id，获取该学生对象
            Student student = StudentList.FirstOrDefault(x => x.Id.ToString() == line || x.Name.ToUpper() == line.ToUpper());
            if (student == null)
            {
                ConsoleWriteRedLine("Cannot find the student. Please check your input and enter again.\n");
                Query();
            }
            else
            {
                ConsoleWriteYellowLine(Student.GetHeaderLineForPrint());
                ConsoleWriteYellowLine(student.ToString());
                //后面添加一个空行，为了美观
                Console.WriteLine();
            }
        }

        /// <summary>
        /// 保存并退出
        /// </summary>
        public static void SaveAndExit()
        {
            try
            {
                List<string> lines = StudentList.Select(x => x.ToStringForSave()).ToList();
                lines.Insert(0, Student.GetHeaderLineForSave());
                File.WriteAllLines(FilePath, lines);
                ConsoleWriteYellowLine("See you!");
                //结束应用程序的执行,即关闭程序返回操作系统。
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                ConsoleWriteRedLine("Save Error:\n" + ex.Message);
            }

        }

        /// <summary>
        /// 不保存退出
        /// </summary>
        public static void ExitWithoutSaving()
        {
            ConsoleWriteYellowLine("See you!");
            //结束应用程序的执行,即关闭程序返回操作系统。
            Environment.Exit(0);
        }

        /// <summary>
        /// 也是查询一个学生是否存在，与Query()方法相比，只是第一行的提示文字不同, 并且该方法有返回值，返回查询到的Student对象。
        /// </summary>
        public static Student QueryStudentForUpdate()
        {
            Console.WriteLine("Please enter the Id or Name your want to update...");
            string line = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            //指定Id，获取该学生对象
            Student student = StudentList.FirstOrDefault(x => x.Id.ToString() == line || x.Name.ToUpper() == line.ToUpper());
            if (student == null)
            {
                ConsoleWriteRedLine("Cannot find the student. Please check your input and enter again.\n");
                return QueryStudentForUpdate();
            }
            else
            {
                ConsoleWriteYellowLine(Student.GetHeaderLineForPrint());
                ConsoleWriteYellowLine(student.ToString());
                //后面添加一个空行，为了美观
                Console.WriteLine();
                return student;
            }
        }

        /// <summary>
        /// 更新一个学生对象
        /// </summary>
        public static void UpdateStudent(Student student)
        {
            Console.WriteLine("You can follow the format [Name],[History],[Math],[Physics],[Geography] to update the record...");
            string line = Console.ReadLine();
            //后面添加一个空行，为了美观
            Console.WriteLine();
            try
            {
                student.Update(line);
                ConsoleWriteYellowLine("Update successfully.\n");
            }
            catch
            {
                ConsoleWriteRedLine("Update fail. Please check your input formate and enter again.\n");
                UpdateStudent(student);
            }
        }

        /// <summary>
        /// 为了方便显示，自己包装了一个输出红色字体的方法
        /// </summary>
        /// <param name="s"></param>
        public static void ConsoleWriteRedLine(string s)
        {
            //设置控制台文字的颜色
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(s);
            //显示完了再把颜色设置回来
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// 为了方便显示，自己包装了一个输出黄色字体的方法
        /// </summary>
        /// <param name="s"></param>
        public static void ConsoleWriteYellowLine(string s)
        {
            //设置控制台文字的颜色
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(s);
            //显示完了再把颜色设置回来
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// 当用户电脑中没有该文件的时候，自动在路径中创建该文件。
        /// </summary>
        public static void CreateFile()
        {
            try
            {
                List<string> lines = new List<string>();
                lines.Add(Student.GetHeaderLineForSave());
                lines.Add("1,Mike,60,70,60,70");
                lines.Add("2,Lucy,88,75,90,80");
                lines.Add("3,Richard,61,71,70,88");
                //创建文件夹
                Directory.CreateDirectory(Path.GetDirectoryName(FilePath));
                //创建文件
                File.WriteAllLines(FilePath, lines);
                ConsoleWriteYellowLine($"Create file in {FilePath} successfully.\n");
            }
            catch(Exception ex)
            {
                ConsoleWriteRedLine("Create file failed.\n" + ex.Message);
                ConsoleWriteYellowLine("\nThe app have automatically exit.");
                Environment.Exit(0);
            }

        }
    }
}
