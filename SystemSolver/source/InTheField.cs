using System;
using System.Collections.Generic;


namespace SolutionLibrary
{
    public class InTheField
    {
        /// <summary>
        /// нахождение остатка
        /// </summary>
        /// <param name="a"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static int Modul(int a, int p)
        {
            //if (a < p) return a;
            int f = a;
            InTheField F = new InTheField(f);
            InTheField P = new InTheField(p);
            while (F.binNumb.Length - 1 >= P.binNumb.Length - 1)
            {
                int m = F.binNumb.Length - P.binNumb.Length;
                F.BinToInt = F.Geta() ^ (P.Geta() << m);
            }
            return F.Geta();
        }
        /// <summary>
        /// метод вычисляет деление двоичных чисел в столбик
        /// </summary>
        /// <param name="a">рассматриваемое число</param>
        /// <param name="b">код порождающего многочлена</param>
        /// <returns></returns>
        public static int ForThatThield(int a, int b, out int[] div)
        {
            //представим данные числа в виде инзефилд
            InTheField A = new InTheField(a);
            InTheField B = new InTheField(b);
            if (a < b)
            {
                div = new int[] { 0 };
                return a;
            }
            //найдем их бинарные представления
            int[] ara = A.binNumb;
            //это нужно для махинаций над массивом ара
            int[] copyara = ara;
            int[] arb = B.binNumb;
            //то, что мы будем в столбике ксорить
            int xor = 0;
            //сюда пихаем результат деления
            div = new int[0];
            for (int i = 0; i <= ara.Length - arb.Length;)
            {
                InTheField COPYARA1 = new InTheField();
                COPYARA1.binNumb = copyara;
                if (copyara.Length < arb.Length)
                {
                    return COPYARA1.BinToInt;
                }
                //это нужно для того, чтобы такие коды как 011 превращались в 11
                COPYARA1 = new InTheField(COPYARA1.BinToInt);
                //бинарная запись той части ара, которая по размеру как арб
                int[] tempa = COPYARA1.binNumb;
                //нужно если размер копиарра был не как у арб
                Array.Resize(ref tempa, arb.Length);
                //создадим на основе этой записи объект инзефилд, чтобы потом получить десятичную запись и сксорить ее с десятичной записью В
                InTheField TempA = new InTheField();
                TempA.binNumb = tempa;
                TempA.a = TempA.BinToInt;
                //получили ксор (в начале этого числа будет ноль, но он автоматически убирается)
                xor = TempA + B;
                Array.Resize(ref div, div.Length + 1);
                div[div.Length - 1] = 1;
                //эти действия делаются, если в ара еще остались 1/0 в разрядах дальше, чем у арб
                //получим объект инзефилд на основе ксор
                InTheField Xor = new InTheField(xor);
                //теперь временный массив с которым ксорим арб будет равен ксору(его двоичной записи)
                copyara = Xor.binNumb;
                //только к нему еще одна цифра 1/0 добавляются от ара
                int k = 0, t = 0;
                do
                {
                    if (arb.Length + i < ara.Length)
                    {
                        Array.Resize(ref copyara, copyara.Length + 1);
                        copyara[copyara.Length - 1] = ara[arb.Length + i];
                        ++i;
                        if (k > 0)
                        {
                            Array.Resize(ref div, div.Length + 1);
                            div[div.Length - 1] = 0;
                        }
                        ++k;
                        for (int j = 0; j < copyara.Length; j++)
                        {
                            if (copyara[j] != 0)
                            {
                                t = j;
                                break;
                            }
                        }
                    }
                    else
                    {
                        InTheField COPYARA = new InTheField();
                        COPYARA.binNumb = copyara;
                        //это нужно для того, чтобы если ксор очередной не делится на в(то есть его разрядность меньше разрядности в), то в див добавляется ноль
                        if (copyara[0] == 0 && copyara.Length > 1 || COPYARA.Geta() < b && k > 0)
                        {
                            Array.Resize(ref div, div.Length + 1);
                            div[div.Length - 1] = 0;
                        }
                        //объкт инзефилд от копиара, чтобы его уже вывести как остаток, если больше нет разрядов в ара


                        return COPYARA.BinToInt;
                    }
                } while (copyara.Length - t != arb.Length);
            }
            return xor;
        }
        //пустой конструктор
        public InTheField() { }
        //конструктор, который создает объекты В ПОЛЕ
        public InTheField(int a, int p)
        {
            this.a = Modul(a, p);
        }
        /// <summary>
        /// умножение, но не в поле
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="P"></param>
        /// <returns></returns>
        public static int Multiplication(InTheField A, InTheField B)
        {
            int a = A.Geta();
            int b = B.Geta();
            int res = 0;
            while (a > 0)
            {
                if (a % 2 == 1)
                    res ^= b;
                a >>= 1;
                b <<= 1;
            }
            return res;
        }
        public static int operator *(InTheField a, InTheField b)
        {
            int c = 0;
            int[] bina = a.binNumb;
            int[] binb = b.binNumb;
            int[] copyOfa;
            InTheField prevsum = new InTheField();
            for (int i = binb.Length - 1; i >= 0; i--, c++)
            {
                //при умножении на b каждая единичка b дает слагаемое - копию a
                copyOfa = binb[i] == 1 ? bina : new int[bina.Length];
                //по этой копии(либо нулям, если в b встречаются нули) создадим объект в поле, чтобы знать его десятичное знач
                InTheField tempa = new InTheField();
                tempa.binNumb = copyOfa;
                //в зависимости от разряда b (в цикле i) мы сдвигаем полученное слагаемое, т.е. домножаем на 2
                tempa.BinToInt = tempa.BinToInt << c;
                //ксорим предыдущюю сумму(изначально к нулевому полю а объекта prevsum добавляем tempa.a) c новым сдвинутым слагаемым 
                prevsum.a = prevsum + tempa;
            }
            //просто значение prevsum.Geta() может быть за границами пораждающего многочлена, нужно в поле
            //int[] empty = null;//пустой массив, так требуется этот параметр
            //Console.WriteLine("p inthefield" + p);
            return Modul(prevsum.Geta(), p);
            //return ForThatThield(prevsum.Geta(), p, out empty);//(can be used just a)
            //return prevsum.Geta();
        }
        public static int operator +(InTheField a, InTheField b)
        {
            // int[] empty = null;//пустой массив, так требуется этот параметр
            //if (a.Geta() == b.Geta())
            return Modul(a.Geta() ^ b.Geta(), p);
            //return a.Geta() ^ b.Geta();
            //return ForThatThield(a.Geta() ^ b.Geta(), p, out empty);
        }
        //поле а
        int a;
        //поле бинарного представления а
        int[] bina;
        //поле десятичного представления порождающего многочлена
        public static int p;
        public InTheField(int a)
        {
            this.a = a;
        }
        //the dec of the number from bin
        public int Geta()
        {
            return a;
        }
        public int BinToInt
        {

            get
            {
                int mult = 1;
                int converted = 0;
                for (int i = bina.Length - 1; i >= 0; i--)
                {
                    converted += bina[i] * mult;
                    mult *= 2;
                }
                return converted;
            }
            set { a = value; }
        }
        //the bin of the number from dec
        public int[] binNumb
        {
            get
            {
                if (a == 0) return new int[] { 0 };
                int tmp = a;
                int temp1 = 0;
                List<int> s = new List<int>();
                while (tmp > 0)
                {
                    temp1 = tmp % 2;
                    tmp = tmp / 2;
                    s.Add(temp1);
                }
                return obrat(s);
            }
            set
            {
                bina = value;
                this.a = this.BinToInt;
            }
        }
        //переворачивает число и возвращает прямую запись двоичного числа.
        private int[] obrat(List<int> norm)
        {
            int[] s = new int[norm.Count];
            for (int i = norm.Count - 1; i >= 0; i--)
            {
                s[norm.Count - 1 - i] = norm[i];
            }
            return s;
        }
    }
}
