using System;
using System.Collections.Generic;
using System.Text;

namespace FileIoDemoForPiPi
{
    public class Student
    {
        public int Id;
        public string Name;
        public double History;
        public double Math;
        public double Physics;
        public double Geography;

        /// <summary>
        /// 如果文字行中包含Id，则使用该构造方法创建对象
        /// </summary>
        /// <param name="line"></param>
        public Student(string line)
        {
            string[] fields = line.Split(",");
            Id = int.Parse(fields[0].Trim());
            Name = fields[1].Trim();
            History = double.Parse(fields[2].Trim());
            Math = double.Parse(fields[3].Trim());
            Physics = double.Parse(fields[4].Trim());
            Geography = double.Parse(fields[5].Trim());
        }

        /// <summary>
        /// 如果文字行中不包含Id，则使用更改构造方法创建对象
        /// </summary>
        /// <param name="id"></param>
        /// <param name="lineWithoutId"></param>
        public Student(int id, string lineWithoutId)
        {
            Id = id;
            string[] fields = lineWithoutId.Split(",");
            Name = fields[0].Trim();
            History = double.Parse(fields[1].Trim());
            Math = double.Parse(fields[2].Trim());
            Physics = double.Parse(fields[3].Trim());
            Geography = double.Parse(fields[4].Trim());
        }

        public void Update(string lineWithoutId)
        {
            string[] fields = lineWithoutId.Split(",");
            Name = fields[0].Trim();
            History = double.Parse(fields[1].Trim());
            Math = double.Parse(fields[2].Trim());
            Physics = double.Parse(fields[3].Trim());
            Geography = double.Parse(fields[4].Trim());
        }

        /// <summary>
        /// 重写该方法，返回一个格式化后的字符串，该格式化功能指定了每个字段的列宽。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0,-6}{1,-12}{2,12}{3,12}{4,12}{5,12}", Id, Name, History, Math, Physics, Geography);
        }
        /// <summary>
        /// 返回对象被转换后的字符串，用来保存到文件中的一行。
        /// </summary>
        /// <returns></returns>
        public string ToStringForSave()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", Id, Name, History, Math, Physics, Geography);
        }
        /// <summary>
        /// 获取格式化后的Header行，用来输出到控制台。
        /// </summary>
        /// <returns></returns>
        public static string GetHeaderLineForPrint()
        {
            return string.Format("{0,-6}{1,-12}{2,12}{3,12}{4,12}{5,12}", "Id", "Name", "History", "Math", "Physics", "Geography");
        }
        /// <summary>
        /// 获取Header行，用来储存到文件中。
        /// </summary>
        /// <returns></returns>
        public static string GetHeaderLineForSave()
        {
            return string.Format("{0},{1},{2},{3},{4},{5}", "Id", "Name", "History", "Math", "Physics", "Geography");
        }
    }
}
