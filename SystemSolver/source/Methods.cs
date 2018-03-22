using System;
using System.Collections.Generic;


namespace SolutionLibrary
{
    public class Methods
    {
        //рабивает по массивам коэффициенты у иксов, их индексы и столбик ответов
        public void Sorting(string s, out int[] numbsX, out int[] koefs, out int res)
        {

            List<int> numbX = new List<int>();
            List<int> koef = new List<int>();
            int ind = -1;
            do
            {
                ind++;
                if (!char.IsDigit(s[ind])) koef.Add(1);
                else
                {
                    string a = "";
                    while (char.IsDigit(s[ind])) { a += s[ind]; ind++; }
                    koef.Add(int.Parse(a));
                }
                ind++;
                string b = "";
                while (char.IsDigit(s[ind])) { b += s[ind]; ind++; }
                numbX.Add(int.Parse(b));
                if (s[ind] == '+') continue;
                if (s[ind] == '=') break;
            } while (true);
            ind++;
            res = int.Parse(s.Substring(ind));
            numbsX = numbX.ToArray();
            koefs = koef.ToArray();
        }

        //проверка строки с уравнением
        public void Formation(ref string str)
        {
            str = str.Replace(" ", ""); //?
            //проверка на то, что в строке есть только лишь допустимые символы
            string norm = "0123456789x+=";
            for (int i = 0; i < str.Length; i++)
                if (norm.IndexOf(str[i]) == -1)
                    throw new Exception("Присутствуют недопустимые символы.");
            //проверка на то, что нет никакого вхождения таких недопустимых комбинаций
            if (str.IndexOf("x+") + str.IndexOf("x=") + str.IndexOf("=x") + str.IndexOf("++") + str.IndexOf("==") + str.IndexOf("xx") != -6)
                throw new Exception("Ошибки в формате введенных данных.");
        }

        //возвращает максимальный индекс икса в массиве массивов
        int GetMax(int[][] ar)
        {
            int max = 0;
            foreach (int[] a in ar)
                foreach (int b in a)
                    if (b > max) max = b;
            return max;
        }

        // проверка на то, что количество неизвестных равно количеству уравнений в системе
         bool SystemIsCorrect(int NumbEq, int maxInd)
        {
            return (NumbEq == maxInd);
        }

        //получаем готовую матрицу
        public int[][] GetMatrix(int[][] numbsX, int[][] koefs)
        {
            if (!SystemIsCorrect(numbsX.Length, GetMax(numbsX)))
                throw new Exception("Количество уравнений не совпадает с количеством переменных.");

            int[][] count = CountIndexes(numbsX);

            if (!CountDifferentIndexes(count))
                throw new Exception("Количество уравнений не совпадает с количеством переменных.");
            if (!IndexesAreDifferent(count))
                throw new Exception("Повторился икс");
            //количество уравнений и иксов
            int k = GetMax(numbsX);
            int[][] res = new int[k][];
            for (int i = 0; i < k; i++)
                res[i] = new int[k];
            for (int i = 0; i < k; i++)
                for (int j = 0; j < numbsX[i].Length; j++)
                    //индекс коэф-та - индекс индекса икса минус оин
                    res[i][numbsX[i][j] - 1] = koefs[i][j];
            return res;
        }

        /// <summary>
        /// подсчитывает количество повторений индесов в каждой строке
        /// </summary>
        /// <param name="numbsX"></param>
        int[][] CountIndexes(int[][] numbsX)
        {
            int maxInd = GetMax(numbsX);
            int[][] count = new int[maxInd][];
            for (int i = 0; i < maxInd; i++)
            {
                //сколько раз повторился индекс в каждой строчке
                count[i] = new int[maxInd];
                for (int j = 0; j < numbsX[i].Length; j++)
                    count[i][numbsX[i][j] - 1] += 1;
            }
            return count;
        }

        /// <summary>
        /// проверка на то, что в одной строчке нет повторяющихся индексов
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        bool IndexesAreDifferent(int[][] count)
        {
            foreach (int[] str in count)
                foreach (int i in str)
                    if (i > 1) return false;
            return true;
        }

        /// <summary>
        /// возвращает true, если нет столбца из нулей
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        bool CountDifferentIndexes(int[][] count)
        {
            int k = count.Length;
            for (int i = 0; i < k; i++)
            {
                bool flag = false;
                for (int j = 0; j < k; j++)
                    flag = flag | ((count[j][i]) != 0);
                if (!flag) return flag;
            }
            return true;
        }

    }
}
