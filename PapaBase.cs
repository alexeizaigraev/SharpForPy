using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpForPy

{
    partial class Papa : Program
    {
        protected static string dataPath = DataPath();
        protected static string dataConfigPath = Path.Combine(dataPath, "Config") + "\\";

        protected static string dataConfigDirPath = Path.Combine(DataPath(), "ConfigDir") + "\\";
        protected static string dataInPath = Path.Combine(DataPath(), "InData") + "\\";
        protected static string dataOutPath = Path.Combine(DataPath(), "OutData") + "\\";

        protected static string kabinetPath = FileToVec(dataConfigPath + "ConfigKabinetPath.txt")[0];
        protected static string myDataPath = dataConfigPath + "comon_data.csv";

        protected static string outLine = "";
        protected static string outText = "";
        protected static string header = "";

        protected static string[] workVec;

        protected static void OpenNote(string fileName)
        {
            try { Process.Start("notepad.exe", fileName); }
            catch { Sos("Err Open Notepad", fileName); }
        }

        protected static void Sos(string error, string fact)
        {
            pRed("\n\t" + error + "\n");
            pBlue(fact);
            pGray("\n\t[Enter] MainMenu");
            Console.ReadKey();
            //MenuMain();
        }

        public static void Alarm(string error, string fact)
        {
            pGreen("\n\t" + error + " ");
            pBlue(fact + "\n");

        }

        public static Dictionary<string, string> FileToDict(int colNum)
        {
            List<string[]> data = FileToArr(myDataPath);
            Dictionary<string, string> d = new Dictionary<string, string>();
            foreach (string[] line in data)
            {
                try { d[line[0]] = line[colNum]; }
                catch { Sos("Err FileToDict", line[0]); }
            }
            return d;
        }

        public static Dictionary<string, string> VecToHash(string[] header, string[] vec)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            for (int i = 0; i < header.Length; i++)
            {
                try { dict[header[i]] = vec[i]; }
                catch { Alarm("Err VecToHash", String.Format(" column {0}", i)); }
            }
            return dict;
        }

        public static List<string> DictToList(Dictionary<string, string> dict)
        {
            List<string> rez = new List<string>();
            foreach (string unit in dict.Values)
            {
                rez.Add(unit);
            }
            return rez;
        }

        public static List<string> DictToHead(Dictionary<string, string> dict)
        {
            List<string> rez = new List<string>();
            foreach (string unit in dict.Keys)
            {
                try { rez.Add(unit); }
                catch { Sos("Err DictToHead", unit); }
            }
            return rez;
        }

        public static Dictionary<string, Dictionary<string, string>> ArrToHash(string[] head, List<string[]> list, int keyColNum)
        {
            Dictionary<string, Dictionary<string, string>> hashTab = new Dictionary<string, Dictionary<string, string>>();
            Dictionary<string, string> hash = new Dictionary<string, string>();
            foreach (string[] line in list)
            {
                try
                {
                    hash = VecToHash(head, line);
                    string keyLine = head[keyColNum];
                    string keyArr = hash[keyLine];
                    hashTab[keyArr] = hash;
                }
                catch { Sos("Err ArrToHash", line[0]); }
            }
            return hashTab;
        }

        public static Dictionary<string, Dictionary<string, string>> FileToHashTab(string fName, int keyColNum)
        {
            List<string[]> arr = FileToArr(fName);
            string[] head = arr[0];
            List<string[]> list = SubList(arr, 1, arr.Count - 1);
            Dictionary<string, Dictionary<string, string>> hashTab = new Dictionary<string, Dictionary<string, string>>();
            try { hashTab = ArrToHash(head, list, keyColNum); }
            catch { Sos("Err FileToHashTab", fName); }
            return hashTab;
        }

        public static List<string[]> SubList(List<string[]> list, int start, int finish)
        {
            List<string[]> myList = new List<string[]>();
            for (int i = start; i <= finish; i++)
            {
                try { myList.Add(list[i]); }
                catch { Sos("Err Sublist", String.Format(" row {0}", i)); }
            }
            return myList;
        }
        public static void MyDelete(string fName)
        {
            try
            {
                File.Delete(fName);
                pGray(" Dlelete " + fName);
            }
            catch { Sos("Ошибка удаления", fName); }
        }


        public static void MoveOneFile(string oldName, string newName)
        {
            FileInfo fileInfo = new FileInfo(newName);
            if (fileInfo.Exists) MyDelete(newName);
            try
            {
                File.Move(oldName, newName);
                pBlue(" Move " + newName);
            }
            catch { Alarm("Err Move", newName); }
        }

        public static void CopyOneFile(string oldName, string newName)
        {
            FileInfo fileInfo = new FileInfo(newName);
            if (fileInfo.Exists) MyDelete(newName);
            try
            {
                File.Copy(oldName, newName);
                pBlue(" Copy " + newName);
            }
            catch { Alarm("Err copy", newName); }
        }


        public static void Coper(string dirInCopy, string dirOutCopy)
        {
            DirectoryInfo d = new DirectoryInfo(dirInCopy);
            FileInfo[] infos = d.GetFiles("*.*");
            foreach (FileInfo f in infos)
            {
                string oldName = f.FullName;
                string newName = Path.Combine(dirOutCopy, f.Name);
                CopyOneFile(oldName, newName);
            }
        }

        public static void Mover(string dirInMove, string dirOutMove)
        {
            DirectoryInfo d = new DirectoryInfo(dirInMove);
            FileInfo[] infos = d.GetFiles("*.*");
            foreach (FileInfo f in infos)
            {
                string oldName = f.FullName;
                string newName = Path.Combine(dirOutMove, f.Name);
                MoveOneFile(oldName, newName);
            }
        }

        public static void Clon(string dirInClone, string dirOutClone)
        {
            DirectoryInfo d = new DirectoryInfo(dirInClone);
            FileInfo[] infos = d.GetFiles("*.*");
            foreach (FileInfo f in infos)
            {
                string oldName = f.FullName;
                string newName = Path.Combine(dirOutClone, f.Name);
                CopyOneFile(oldName, newName);

            }
        }

        public static void ClonAccess(string dirInClone, string dirOutClone)
        {
            DirectoryInfo d = new DirectoryInfo(dirInClone);
            FileInfo[] infos = d.GetFiles("*.*");
            foreach (FileInfo f in infos)
            {
                string oldName = f.FullName;
                string newName = Path.Combine(dirOutClone, f.Name);
                CopyOneFile(oldName, newName);
            }
        }
        public static void ClearFolder(string path)
        {
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] infos = d.GetFiles("*.*");
            foreach (FileInfo f in infos)
            {
                string delName = f.FullName;
                MyDelete(delName);
            }
        }

        public static string DataPath()
        {
            string path = "";
            try { path = File.ReadAllLines(Path.Combine("Config", "ConfigDataPath.txt"))[0]; }
            catch { Sos("Err read DataPath", Path.Combine("Config", "ConfigDataPath.txt")); }
            return path;
        }
        public static string[] FileToVec(string fName)
        {
            FileInfo fileInfo = new FileInfo(fName);
            if (!fileInfo.Exists) Sos("No File", fName);
            string[] vec = File.ReadAllLines(fName);
            return vec;
        }

        public static void TextToFile(string fName, string text)
        {
            try
            {
                File.WriteAllText(fName, text);
                pBlue("\n\t" + fName + "\n");
            }
            catch { Sos("Err TextToFile", fName); }
        }

        public static void ArrToFile(string fName, List<string[]> arr)
        {
            string text = ArrToText(arr);
            TextToFile(fName, text);
        }

        public static string ArrToText(List<string[]> arr)
        {
            string text = "";
            foreach (string[] vec in arr)
            {
                text += String.Join(";", vec) + "\n";
            }
            return text;
        }

        public static List<string[]> FileToArr(string fName)
        {
            string[] vec;
            List<string[]> arr = new List<string[]>();
            try
            {
                vec = File.ReadAllLines(fName);
                foreach (string vecLine in vec)
                {
                    string[] vecLineSplit = vecLine.Split(';');
                    arr.Add(vecLineSplit);
                }
            }
            catch { Sos("Err FileToArr", fName); }
            return arr;
        }

        public static string FileToText(string fName)
        {
            string text = "";
            try { text = File.ReadAllText(fName); }
            catch { Sos("Err RFileToText", fName); }
            return text;
        }

        public static List<string> MkNatasha()
        {
            var arr = FileToArr(Path.Combine(dataInPath, "natasha.csv"));
            List<string> vec = new List<string>();
            string sign = "Відділення № ";
            foreach (var line in arr)
            {
                try
                {
                    foreach (var el in line)
                    {
                        if (el.IndexOf(sign) > -1)
                            vec.Add(el.Replace(sign, "").Replace(" ", ""));
                    }
                }
                catch { Sos("Err mkNatasha", "natasha.csv"); }

            }
            return vec;
        }

        public static Dictionary<string, string> MkComonHash(int keyColNum)
        {
            Dictionary<string, string> rez = new Dictionary<string, string>();
            List<string[]> data = new List<string[]>();
            try { data = FileToArr(myDataPath); }
            catch { Sos("Err read", myDataPath); }

            try
            {
                foreach (string[] splitLine in data)
                {
                    rez[splitLine[0]] = splitLine[keyColNum];
                }
            }
            catch { Sos("Err MkComonHash", String.Format("KeyColNum {0}", keyColNum)); }
            return rez;
        }

        public static string DateNowLine() { return (DateTime.Today).ToString("yyyy-MM-dd").Replace(".", ""); }


    }
}
