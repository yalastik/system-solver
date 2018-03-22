using System;


namespace SolutionLibrary
{
    public class MethodsToSolve
    {
        //конструктор, через который здаются p, [][]A,[]B
        public MethodsToSolve(int p, int[][] a, int[] b)
        {
            InTheField P = new InTheField(p);
            //проверим, что ключевой полином - неприводимый, тривиальный многочлен
            if (!IsTrivial(P))
            {
                throw new ArgumentException("порождающий полином должен быть неприводимым!");
            }
            this.P = P;
            //поля матриц заполняем перепарсенными матрицами (ранее интовыми)
            this.A = AParsing(a, p);
            this.B = BParsing(b, p);
        }
        /// <summary>
        /// переводит матрицу интов в объекты инзефилд
        /// </summary>
        /// <param name="A"></param>
        /// <returns></returns>
        InTheField[][] AParsing(int[][] A, int p)
        {
            int n = A.GetLength(0);
            InTheField[][] a = new InTheField[n][];
            for (int i = 0; i < n; i++)
            {
                a[i] = new InTheField[A[i].Length];
                for (int j = 0; j < A[i].Length; j++)
                {
                    a[i][j] = new InTheField(A[i][j], p);
                }
            }
            return a;
        }
        InTheField[] BParsing(int[] B, int p)
        {
            int n = B.GetLength(0);
            InTheField[] b = new InTheField[n];
            for (int i = 0; i < n; i++)
            {
                b[i] = new InTheField(B[i], p);
            }
            return b;
        }
        int[][] ReturnA()
        {
            int n = A.GetLength(0);
            int[][] a = new int[n][];
            for (int i = 0; i < n; i++)
            {
                a[i] = new int[A[i].Length];
                for (int j = 0; j < A[i].Length; j++)
                {
                    a[i][j] = A[i][j].Geta();
                }
            }
            return a;
        }
        public int[] ReturnB()
        {
            int n = B.Length;
            int[] b = new int[n];
            for (int i = 0; i < n; i++)
            {
                b[i] = B[i].Geta();
            }
            return b;
        }
        //код порождающего многочлена
        public static InTheField p;
        InTheField[][] A;
        InTheField[] B;
        InTheField P { get { return p; } set { p = value; } }
        InTheField GetInverse(InTheField a)
        {
            InTheField x = new InTheField();
            InTheField y = new InTheField();
            InTheField r = new InTheField();
            InTheField s = new InTheField();
            InTheField gx = new InTheField(a.Geta());
            InTheField d = Ext_GCD(gx, p, ref x, ref y, ref r, ref s);
            //should be inversable!!!
            if (d.Geta() != 1) throw new Exception("Нет решений");
            return x;
        }
        /// <summary>
        /// зануляем элементы в столбце над главным
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="i"></param>
        void ToCanon(ref InTheField[][] A, ref InTheField[] B, int i)
        {
            //проверим нулевая ли строка нам подается (b тоже нуль)
            bool allAiZero = true;
            foreach (InTheField el in A[i]) if (el.Geta() != 0) allAiZero = false;
            allAiZero &= B[i].Geta() == 0;
            if (allAiZero) throw new Exception("Система линейно зависима!");
            //находим обратный элемент для диагонального (главного в матрице)
            InTheField inverse_a = GetInverse(A[i][i]);
            //домножаме на него i-ую строку
            for (int j = 0; j < A[i].Length; j++)
            {
                A[i][j].BinToInt = A[i][j] * inverse_a;
            }
            B[i].BinToInt = B[i] * inverse_a;
            //все остальные над i-ой ксорим с ней, чтобы занулить все элементы над главным
            for (int j = 0; j < i; j++)
            {
                InTheField Ajitemp = new InTheField(A[j][i].Geta());
                InTheField BA = new InTheField(B[i] * Ajitemp);
                B[j].BinToInt = B[j] + BA;
                for (int k = 0; k < A[j].Length; k++)
                {
                    InTheField AA = new InTheField(A[i][k] * Ajitemp);
                    A[j][k].BinToInt = A[j][k] + AA;
                }
            }

        }
        /// <summary>
        /// метод для определение приводим ли полином, false - приводимый, нам не подходит
        /// </summary>
        /// <param name="p">код полинома</param>
        /// <returns></returns>

        bool IsTrivial(InTheField p)
        {
            //найдем степень полинома
            int m = p.binNumb.Length - 1;
            //объект инзефилд 
            InTheField ux = new InTheField();
            InTheField ux_x = new InTheField();
            //изначально ux=x, т.е. двоичная запись - 10
            ux.binNumb = new int[] { 1, 0 };
            for (int i = 1; i <= m / 2; i++)
            {
                //сдвиг влево (умножаем на 2, так как двоичные полиномиальные поля)
                ux.BinToInt = InTheField.Multiplication(ux, ux);
                //считаем остаток от деление на полином
                ux.BinToInt = InTheField.Modul(ux.Geta(), p.Geta());
                ux_x = new InTheField(ux.Geta() + 2);
                InTheField d = GCD(p, ux_x);
                if (d.Geta() != 1) return false;
            }
            return true;

        }
        public void Whole()
        {

            int n = A.GetLength(0);
            for (int i = 0; i < n; i++)
            {
                for (int j = i + 1; j < n; j++)
                {
                    InTheField x = new InTheField();
                    InTheField y = new InTheField();
                    InTheField r = new InTheField();
                    InTheField s = new InTheField();
                    InTheField d = Ext_GCD(A[i][i], A[j][i], ref x, ref y, ref r, ref s);
                    if (A[j][i].Geta() == 0) continue;
                    LinearChangesInMartix(ref A[i], ref A[j], ref B[i], ref B[j], x, y, r, s);
                }
                ToCanon(ref A, ref B, i);

            }
        }
        /// <summary>
        /// домножаем 2 строки на коэ-ты Безу, найденные в GCD и складываем их
        /// </summary>
        /// <param name="A1">i-ая строка ма-цы</param>
        /// <param name="A2">j-ая строка ма-цы</param>
        /// <param name="b1">i-ый эл-т вектора В</param>
        /// <param name="b2">j-ый эл-т вектора В</param>
        /// <param name="x">коэф Безу</param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="s"></param>
        void LinearChangesInMartix(ref InTheField[] A1, ref InTheField[] A2, ref InTheField b1, ref InTheField b2, InTheField x, InTheField y, InTheField r, InTheField s)
        {
            int n = A1.Length;
            //создаем объекты инзефилд, чтобы учсть двоичное умножение и сложение (ксор)
            InTheField A1Field;
            InTheField A2Field;
            int[] A1t = new int[n];//temporary result
            int[] A2t = new int[n];//-//-
            for (int i = 0; i < n; i++)
            {
                A1Field = new InTheField(A1[i].Geta());
                A2Field = new InTheField(A2[i].Geta());
                //чтобы потом плюс считался как ксор, применим его к объектам инзефилд
                InTheField mult11 = new InTheField(A1Field * x);
                InTheField mult12 = new InTheField(A2Field * y);
                InTheField mult21 = new InTheField(A1Field * r);
                InTheField mult22 = new InTheField(A2Field * s);

                A1t[i] = mult11 + mult12;
                A2t[i] = mult21 + mult22;
            }
            for (int i = 0; i < n; i++)
            {
                A1[i] = new InTheField(A1t[i]);
                A2[i] = new InTheField(A2t[i]);
            }
            //то же самое для матрицы В
            InTheField B1t = new InTheField();
            InTheField B2t = new InTheField();
            InTheField mul11 = new InTheField(b1 * x);
            InTheField mul12 = new InTheField(b2 * y);
            InTheField mul21 = new InTheField(b1 * r);
            InTheField mul22 = new InTheField(b2 * s);
            B1t.BinToInt = mul11 + mul12;
            B2t.BinToInt = mul21 + mul22;
            b1 = B1t;
            b2 = B2t;
        }
        /// <summary>
        /// нерасширенный алго евклида, то есть только НОД 2-х чисел
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        InTheField GCD(InTheField a, InTheField b)
        {
            InTheField A = new InTheField(a.Geta());
            InTheField B = new InTheField(b.Geta());
            int m = 0;
            while (A.Geta() != 0 && B.Geta() != 0)
            {
                if (A.Geta() >= B.Geta())
                {
                    m = A.binNumb.Length - B.binNumb.Length;
                    A.BinToInt = A.Geta() ^ (B.Geta() << m);
                }
                else
                {
                    m = B.binNumb.Length - A.binNumb.Length;
                    B.BinToInt = B.Geta() ^ (A.Geta() << m);
                }
            }
            InTheField res = new InTheField(A.Geta() + B.Geta());
            return res;
        }
        InTheField Ext_GCD(InTheField a, InTheField b, ref InTheField x, ref InTheField y, ref InTheField x1, ref InTheField y1)
        {
            //чтобы работать в поле
            InTheField A = new InTheField(a.Geta());
            InTheField B = new InTheField(b.Geta());
            InTheField Q = new InTheField();
            InTheField R = new InTheField();
            InTheField X2 = new InTheField();
            InTheField Y2 = new InTheField();
            InTheField d;
            if (B.Geta() == 0)
            {
                d = A; x.BinToInt = 1; y.BinToInt = 0;
                return d;
            }
            X2.BinToInt = 1; x1.BinToInt = 0; Y2.BinToInt = 0; y1.BinToInt = 1;
            int[] arrForQ;
            while (B.Geta() != 0)
            {
                //найденный остаток в методе класса инзефилд присваиваем нашему R
                R.BinToInt = InTheField.ForThatThield(A.Geta(), B.Geta(), out arrForQ);
                Q.binNumb = arrForQ;

                //объекты инзефилд равные произведению двоичных чисел (чтобы потом использовать перегруженный +)
                InTheField QX1 = new InTheField(Q * x1);
                InTheField QY1 = new InTheField(Q * y1);
                //ксор вместо стандартного вычитания
                x.BinToInt = X2 + QX1; y.BinToInt = Y2 + QY1;
                A.BinToInt = B.Geta(); B.BinToInt = R.Geta();
                X2.BinToInt = x1.Geta(); x1.BinToInt = x.Geta(); Y2.BinToInt = y1.Geta(); y1.BinToInt = y.Geta();
            }
            d = A; x = X2; y = Y2;
            return d;
        }
    }
}
