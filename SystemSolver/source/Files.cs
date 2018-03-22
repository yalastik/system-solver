using System;
using System.Collections.Generic;
using System.IO;

namespace SolutionLibrary
{
    public class Files
    {
        /// <summary>
        /// форммирует массив строк с ответами для записи в файл
        /// </summary>
        /// <param name="B"></param>
        /// <returns></returns>
        static string SolToString(int[] B)
        {
            int n = B.Length;
            string result = string.Empty;
            for (int i = 0; i < n; i++)
            {
                result += $"x{i + 1}={B[i]}; ";
            }
            return result;

        }


        /// <summary>
        /// возвращает массив строк: превая строка - p=...; следующие - уравнения системы
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="pCode"></param>
        /// <returns></returns>
        static string[] SysToString(int[][] a, int[] b, string pCode)
        {
            int n = a.GetLength(0);
            string[] system = new string[n + 1];
            system[0] = pCode;
            for (int i = 0, k = 1; i < n && k < n + 1; i++, k++)
            {
                if (a[i][a[i].Length - 1] == 0) Array.Resize(ref a[i], a[i].Length - 1);
                for (int j = 0; j < a[i].Length; j++)
                {
                    //добавочная часть включает коэф-т и х с индексом
                    string add = a[i][j] == 0 ? null : a[i][j] == 1 ? $"x{j + 1}" : $"{a[i][j]}x{j + 1}";
                    //если это не последний индекс строки матрицы, то добавляется еще плюс, иначе равно
                    system[k] += j == a[i].Length - 1 ? add + '=' : add == null ? add : add + '+';
                }
                system[k] += b[i].ToString();
            }
            return system;
        }

        public static void ResultsOut(string solution, string path)
        {
              using (StreamWriter SW = new StreamWriter(path))
                {
                    SW.WriteLine(solution);
                }
           
        }
        public static void SaveToFile(string[] lines, string path)
        {
           
                using (StreamWriter SW = new StreamWriter(path))
                {
                    foreach (string s in lines)
                    {
                        SW.WriteLine(s);
                    }
                }
            
        }

        public static string GetResult(string[] equations, int p)
        {
            string[] eqs = equations;
            //индексы иксов
            int[][] numbsX = new int[eqs.Length][];
            //коэфициенты
            int[][] koefs = new int[eqs.Length][];
            //столбик ответов
            int[] res = new int[eqs.Length];
            Methods handler = new Methods();
            int[][] matrix = null;
            MethodsToSolve solution;
            string solutionToFile = string.Empty;

            //разбор каждого уравнения
            for (int i = 0; i < eqs.Length; i++)
            {
                handler.Formation(ref eqs[i]);
                handler.Sorting(eqs[i], out numbsX[i], out koefs[i], out res[i]);
            }
            //готовая матрица 
            matrix = handler.GetMatrix(numbsX, koefs);
            solution = new MethodsToSolve(p, matrix, res);
            InTheField.p = MethodsToSolve.p.Geta();
            solution.Whole();
            solutionToFile = SolToString(solution.ReturnB());
            //столбик с результатами           
            return solutionToFile;
        }

        //перенос содержимого файла в массив строк
        public static string[] StringsFromFile(string path)
        {
            List<string> AllLinesList = new List<string>();
            using (StreamReader sr = File.OpenText(path))
            {
                string newline;
                while ((newline = sr.ReadLine()) != null && newline != "")
                    AllLinesList.Add(newline);
            }
            string[] AllLines = AllLinesList.ToArray();
            return AllLines;
        }


        public static string Poly(string[] arr)
        {
            return arr[0];
        }

        public static string Equals(string[] arr)
        {
            string all = string.Empty;
            for (int i = 1; i < arr.Length; i++)
            {
                all += arr[i];
                all += i != arr.Length - 1 ? "\r\n" : string.Empty;
            }
            return all;
        }

    }
}
