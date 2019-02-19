using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Obfuscation_1
{
    class ObfuscatedCode
    {
        /// <summary>
        /// Массив служебных символов
        /// </summary>
        string[] ArrayOperatorSymbols = { ",", ";", ".", "+", "-", "*", "^", "&", "=", "~", "!", "/", "<", ">", "(", ")", "[", "]", "|", "%", "?", "\'", "\"", ":", "{", "}" };

        /// <summary>
        /// Массив литералов (цифры, алфавит и нижнее подчеркивание)
        /// </summary>
        char[] ArrayLiterals = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '_' };

        /// <summary>
        /// Массив начальных литералов (Алфавит и нижнее подчеркивание)
        /// </summary>
        char[] ArrayStartLiterals = {'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '_' };

        /// <summary>
        /// Массив с цифрами
        /// </summary>
        char[] ArrayNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        /// <summary>
        /// Массив с возможными типами данных
        /// </summary>
        List<string> ArrayDataTypes = new List<string>
        {
            "auto",
            "char",
            "const",
            "double",
            "enum",
            "float",
            "int",
            "long",
            "short",
            "struct",
            "signed",
            "union",
            "unsigned",
            "_Bool",
            "_Complex",
            "_Imaginary"
        };

        /// <summary>
        /// Массив с зарезервированными в C словами
        /// </summary>
        List<string> ArrayReservedWords = new List<string>
        {
            "#define",
            "#elif",
            "#else",
            "#endif",
            "#error",
            "#if",
            "#ifdef",
            "#ifndef",
            "#include",
            "#line",
            "#pragma",
            "#undef",
            "auto",
            "break",
            "case",
            "char",
            "const",
            "continue",
            "default",
            "do",
            "double",
            "else",
            "enum",
            "extern",
            "float",
            "for",
            "goto",
            "if",
            "inline",
            "int",
            "long",
            "main",
            "register",
            "return",
            "short",
            "signed",
            "sizeof",
            "static",
            "struct",
            "switch",
            "typedef",
            "union",
            "unsigned",
            "void",
            "volatile",
            "while",
            "_Bool",
            "_Complex",
            "_Imaginary",
            "_Alignas",
            "_Alignof",
            "_Atomic",
            "_Generic",
            "_Noreturn",
            "_Static_assert",
            "_Thread_local"
        };

        /// <summary>
        /// Исходный код
        /// </summary>
        private StringBuilder Code;

        /// <summary>
        /// Копия исходного кода
        /// </summary>
        private StringBuilder CloneCode;

        // Конструктор
        public ObfuscatedCode(StringBuilder InputCode)
        {
            Code = InputCode;
        }

        /// <summary>
        /// Удаляет комментарии
        /// </summary>
        private void RemoveComments()
        {
            //const string CommOneString = "//";
            //const string CommStart = "/*";
            //const string CommEnd = "*/";

            StringBuilder CurrentChars = new StringBuilder();

            int DeletingLength = 2;

            for (int i = 0; i < Code.Length - 2; i++) /* До предпоследнего символа */
            {
                // Удаляемое к-во символов
                DeletingLength = 2;

                if ("//" == Code[i].ToString() + Code[i + 1].ToString())
                {
                    for (int j = i + 2; j < Code.Length - 2; j++) /* До предпоследнего символа */
                    {
                        DeletingLength++;
                        if ("\n" == Code[j].ToString())
                        {
                            break;
                        }
                        if ("\r\n" == Code[j].ToString() + Code[j + 1].ToString())
                        {
                            DeletingLength++;
                            break;
                        }
                    }

                    Code.Remove(i, DeletingLength);

                    i = 0;

                    continue;
                }

                if ("/*" == Code[i].ToString() + Code[i + 1].ToString())
                {
                    for (int j = i + 2; j < Code.Length - 2; j++) /* До предпоследнего символа */
                    {
                        DeletingLength++;
                        if ("*/" == Code[j].ToString() + Code[j + 1].ToString())
                        {
                            DeletingLength++;
                            break;
                        }
                    }

                    Code.Remove(i, DeletingLength);

                    i = 0;

                    continue;
                }
            }

        }

        /// <summary>
        /// Удаляем множественные пробелы
        /// </summary>
        private void RemoveMultipleSpaces()
        {
            for (int i = 0; i < Code.Length - 2; i++) /* До предпоследнего символа */
            {
                if ("  " == Code[i].ToString() + Code[i + 1].ToString())
                {
                    Code.Remove(i + 1, 1);
                    i = 0;
                    continue;
                }
            }
        }

        /// <summary>
        /// Удаляет символы форматирования
        /// </summary>
        private void RemoveFormatSymbols()
        {
            // Удаляем знаки табуляции
            Code.Replace("\t", " ");
            // Удаляем переносы строк
            Code.Replace("\n", " ");
            Code.Replace("\r", " ");
            //Code.Replace("\r\n", " ");
            Code.Replace("\v", " ");
            // Удаляем пробелы возле служебных символов
            for (int j = 0; j < ArrayOperatorSymbols.Length - 1; j++)
            {
                // Удаляем пробелы после служебных символов
                for (int i = 0; i < Code.Length - 2; i++)
                {
                    // Исключаем выражения в кавычках и "<>"
                    IgnoreQuotes(ref i);
                    if (i == Code.Length-1)
                    {
                        Obfuscator.MainWindow.ShowExceptionMessageBox();
                        return;
                    }
                    if ((ArrayOperatorSymbols[j] == Code[i].ToString()) &&
                        (" " == Code[i + 1].ToString()))
                    {
                        Code.Remove(i + 1, 1);
                        i = 0;
                        continue;
                    }

                    // Удаляем пробелы перед служебными символами
                    for (int k = 1; k < Code.Length - 2; k++)
                    {
                        // Исключаем выражения в кавычках и "<>"
                        IgnoreQuotes(ref k);

                        if ((ArrayOperatorSymbols[j] == Code[k].ToString()) &&
                              (" " == Code[k - 1].ToString()))
                        {
                            Code.Remove(k - 1, 1);
                            k = 1;
                            continue;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверка на начальный литерал в функциях и переменных
        /// </summary>
        /// <param name="c">Проверяемый символ</param>
        /// <returns></returns>
        private bool IsStartLiteral(char c)
        {
            foreach (char i in ArrayStartLiterals)
            {
                if (i == c) return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на литерал
        /// </summary>
        /// <param name="c">Проверяемый символ</param>
        /// <returns></returns>
        private bool IsLiteral(char c)
        {
            foreach (char i in ArrayLiterals)
            {
                if (i == c) return true;
            }
            return false;
        }

        /// <summary>
        /// Является ли слово зарезервированным
        /// </summary>
        /// <param name="s">Проверяемая строка</param>
        /// <returns></returns>
        private bool IsReservedWord(string s)
        {
            foreach (string i in ArrayReservedWords)
            {
                if (i == s) return true;
            }
            return false;
        }

        /// <summary>
        /// Является ли символ оператором
        /// </summary>
        /// <param name="c">проверяемый символ</param>
        /// <returns></returns> 
        private bool IsOperatorSymbol(char c)
        {
            foreach (string i in ArrayOperatorSymbols)
            {
                if (i == c.ToString()) return true;
            }
            return false;
        }

        /// <summary>
        /// Проверка на цифру
        /// </summary>
        /// <param name="c">Проверяемый символ</param>
        /// <returns></returns>
        private bool IsNumber(char c)
        {
            foreach (char i in ArrayNumbers)
            {
                if (i == c) return true;
            }
            return false;
        }

        /// <summary>
        /// Объявленные идентификаторы
        /// </summary>
        private List<string> DeclaredIds;

        /// <summary>
        /// Вхождения объявленных идентификаторов
        /// </summary>
        private List<int> DeclaredIdsEntries;

        /// <summary>
        /// Уникальные объявленные идентификаторы
        /// </summary>
        private List<string> UniqueDeclaredIds;


        /// <summary>
        /// Исключаем выражения в кавычках 
        /// </summary>
        /// <param name="i"></param>
        private void IgnoreQuotes(ref int i)
        {
            // Если очередной символ "
            if ('"' == Code[i])
            {
                i++;
                // Пока следующий символ не "
                while (('"' != Code[i]) && (i < Code.Length - 1))
                {
                    // Если текущий символ \
                    if ('\\' == Code[i])
                    {
                        // Игнорируем
                        i++;
                    }

                    i++;

                }
            }

            //// Если очередной символ <
            //if ('<' == Code[i])
            //{
            //    i++;

            //    // Пока следующий символ не >
            //    while ('>' != Code[i])
            //    {
            //        i++;
            //    }
            //}

            // Если очередной символ '
            if ('\'' == Code[i])
            {

                i++;

                if ('\\' == Code[i])
                {
                    i++;
                }

                i += 2;
            }

            // Исключили выражения в кавычках
            // --------------------------
        }

        /// <summary>
        /// Находит объявленные идентификаторы (функции и переменные)
        /// </summary>
        private void FindDeclaredIds()
        {
            // Только имена объявленных идентификаторов
            UniqueDeclaredIds = new List<string>();

            // Инициализация списка объявленных идентификаторов
            DeclaredIds = new List<string>();

            // Инициализация списка вхождений объявленных идентификаторов
            DeclaredIdsEntries = new List<int>();

            List<string> vars = new List<string>();
            List<int> varEntries = new List<int>();
            List<bool> isDeclaredVar = new List<bool>();
            List<string> funcs = new List<string>();
            List<int> funcEntries = new List<int>();
            List<bool> isDeclaredFunc = new List<bool>();

            string tempName = "";
            int tempEntry = 0;

            for (int i = 0; i < Code.Length - 2; i++)
            {
                // Исключаем выражения в кавычках и "<>"
                IgnoreQuotes(ref i);

                // --------------------------
                // Находим функции и переменные
                if (IsStartLiteral(Code[i]) || (Code[i] == '#'))
                {
                    tempName = Code[i].ToString();
                    tempEntry = i;

                    i++;

                    while (IsLiteral(Code[i]))
                    {
                        tempName += Code[i];
                        i++;
                    }

                    // Выделение слова
                    if (IsReservedWord(tempName))
                    {
                        if ("#include" == tempName)
                        {
                            if ('<' == Code[i] || '<' == Code[i + 1])
                            {
                                while ('>' != Code[i])
                                {
                                    i++;
                                }
                            }
                        }
                        //// Чтобы не перескочить на следующий символ
                        //i--;
                        continue;
                    }

                    // Проверка на функцию
                    if ('(' == Code[i])
                    {
                        funcs.Add(tempName);
                        funcEntries.Add(tempEntry);
                        continue;
                    }
                    // Проверка на переменную
                    if ('(' != Code[i])
                    {
                        vars.Add(tempName);
                        varEntries.Add(tempEntry);
                        continue;
                    }
                }
                // Нашли функции и переменные
                // --------------------------
            }

            // -----------------------
            // Находим объявленные функции
            for (int currentFunc = 0; currentFunc < funcEntries.Count(); currentFunc++)
            {
                int i = 0;

                int openBracketCount = 0;
                int closeBracketCount = 0;

                for (i = funcEntries[currentFunc] + funcs[currentFunc].Length; i < Code.Length - 1; i++)
                {
                    // Подсчёт открытых скобок
                    if ('(' == Code[i]) openBracketCount++;

                    // Подсчёт закрытых скобок
                    if (')' == Code[i]) closeBracketCount++;

                    if ((';' == Code[i]) || ('{' == Code[i]))
                        break;
                }

                // Если это был простой вызов
                if ((';' == Code[i]) || (openBracketCount != closeBracketCount))
                {
                    isDeclaredFunc.Add(false);
                    continue;
                }

                // Если функция объявлена
                if ('{' == Code[i])
                {
                    isDeclaredFunc.Add(true);
                    continue;
                }

            }
            // Нашли объявленные функции
            // ----------------------- 


            // -----------------------
            // Записываем вхождения функций

            // Запись в список каждого идентификатора, который объявлен
            for (int i = 0; i < funcs.Count; i++)
            {
                if (isDeclaredFunc[i])
                {
                    UniqueDeclaredIds.Add(funcs[i]);
                }
            }

            // Проход по всему списку вхождений
            for (int funcCounter = 0; funcCounter < funcs.Count(); funcCounter++)
            {
                // Проход по каждому имени объявленного идентификатора
                foreach (string s in UniqueDeclaredIds)
                {
                    // Если такое имя встречается – записываем его и его вхождение в список идентификаторов
                    if (s == funcs[funcCounter])
                    {
                        DeclaredIds.Add(funcs[funcCounter]);
                        DeclaredIdsEntries.Add(funcEntries[funcCounter]);
                    }
                }
            }
            // Записали вхождения функций
            // -----------------------


            // -----------------------
            // Находим объявленные переменные
            for (int currentVar = 0; currentVar < varEntries.Count(); currentVar++)
            {
                string tempStr = "";
                string[] tempArr;

                for (int i = varEntries[currentVar]; i >= 0; i--)
                {
                    if (('{' == Code[i]) || ('(' == Code[i]) || (';' == Code[i]))
                        break;

                    tempStr += Code[i];
                }

                string t = "";
                for (int i = tempStr.Length - 1; i >= 0; i--)
                {
                    t += tempStr[i];
                }

                tempStr = t;

                List<char> splitSymbols = new List<char>();
                splitSymbols.Add(' ');
                foreach (var i in ArrayOperatorSymbols)
                {
                    splitSymbols.Add(i[0]);
                }

                tempArr = tempStr.Split(splitSymbols.ToArray());

                bool d = false;

                foreach (var word in tempArr)
                {
                    foreach (var dataType in ArrayDataTypes)
                    {
                        if (word == dataType)
                        {
                            d = true;
                            break;
                        }
                    }
                }
                isDeclaredVar.Add(d);
            }

            // Нашли объявленные переменные
            // -----------------------

            // -----------------------
            // Записываем вхождения переменных

            // Запись в список каждого идентификатора, который объявлен
            for (int i = 0; i < vars.Count - 1; i++)
            {
                if (isDeclaredVar[i])
                {
                    UniqueDeclaredIds.Add(vars[i]);
                }
            }

            // Проход по всему списку вхождений
            for (int varCounter = 0; varCounter < vars.Count(); varCounter++)
            {
                // Проход по каждому имени объявленного идентификатора
                foreach (string s in UniqueDeclaredIds)
                {
                    // Если такое имя встречается – записываем его и его вхождение в список идентификаторов
                    if (s == vars[varCounter])
                    {
                        DeclaredIds.Add(vars[varCounter]);
                        DeclaredIdsEntries.Add(varEntries[varCounter]);
                    }
                }
            }

            // Записали вхождения переменных
            // -----------------------

            // -----------------------
            // Сортировка списка идентификаторов

            for (int i = DeclaredIdsEntries.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (DeclaredIdsEntries[j] > DeclaredIdsEntries[j + 1])
                    {
                        tempEntry = DeclaredIdsEntries[j];
                        tempName = DeclaredIds[j];

                        DeclaredIdsEntries[j] = DeclaredIdsEntries[j + 1];
                        DeclaredIds[j] = DeclaredIds[j + 1];

                        DeclaredIdsEntries[j + 1] = tempEntry;
                        DeclaredIds[j + 1] = tempName;
                    }
                }
            }

            vars.Clear();
            varEntries.Clear();
            isDeclaredVar.Clear();
            funcs.Clear();
            funcEntries.Clear();
            isDeclaredFunc.Clear();
        }

        /// <summary>
        /// Заменяет объявленные функции и переменные на нелогичные
        /// </summary>
        private void RenameIds()
        {
            //Количество объявленных идентификаторов
            //int uniqueIdsCount = UniqueDeclaredIds.Count;

            // Новые имена объявленных идентификаторов
            string[] newUniqueIds = new string[UniqueDeclaredIds.Count];

            // Макс. длина идентификатора
            int MAXIDLENGTH = 10;

            // Вычисление макс. длины идентификатора
            foreach (string s in UniqueDeclaredIds)
            {
                if (s.Length > MAXIDLENGTH)
                    MAXIDLENGTH = s.Length;
            }

            // Создание новых нелогичных идентификаторов
            Random rnd = new Random();
            for (int idIter = 0; idIter < UniqueDeclaredIds.Count; idIter++)
            {
                string tempId = "";

                // Стартовый символ
                tempId += ArrayStartLiterals[rnd.Next(0, ArrayStartLiterals.Length - 1)];

                // Оставшиеся символы
                for (int i = 1; i < MAXIDLENGTH; i++)
                {
                    tempId += ArrayLiterals[rnd.Next(0, ArrayLiterals.Length - 1)];
                }

                // Проверка на одинаковые идентификаторы
                bool eq = false;
                for (int l = 0; l < newUniqueIds.Length-1; l++)
                {
                    if (tempId == newUniqueIds[l])
                    {
                        eq = true;
                        break;
                    }
                }

                if (eq)
                {
                    idIter--;
                    continue;
                }

                newUniqueIds[idIter] = tempId;
            }

            // Массив заменяемых идентификаторов
            string[] replIds = new string[DeclaredIds.Count];

            // Прохождение по объявленным идентификаторам
            for (int i = 0; i < DeclaredIds.Count; i++)
            {
                // Прохождение по уникальным именам идентификаторов
                for (int j = 0; j < UniqueDeclaredIds.Count; j++)
                {
                    // Если такой идентификатор встречается, то в массив заменяемых
                    // вставляем на его место обфусцированную версию
                    if (DeclaredIds[i] == UniqueDeclaredIds[j])
                    {
                        replIds[i] = newUniqueIds[j];
                    }
                }
            }

            // Вхождения новых идентификаторов
            int[] entrIds = DeclaredIdsEntries.ToArray();

            // Замена идентификаторов
            for (int i = DeclaredIdsEntries.Count - 1; i >= 0; i--)
            {
                Code.Remove(DeclaredIdsEntries[i], DeclaredIds[i].Length);
                Code.Insert(DeclaredIdsEntries[i], replIds[i]);
            }
        }
        /// <summary>
        /// Целочисленные константы
        /// </summary>
        List<string> intConst = new List<string>();
        /// <summary>
        /// Вхождения целочисленных констант
        /// </summary>
        List<int> iconstEntries = new List<int>();

        /// <summary>
        /// Поиск констант
        /// </summary>
        private void ConvertToHexadecimal()
        {
            string tempName = "";
            int tempEntry = 0;

            for (int i = 1; i < Code.Length - 2; i++)
            {
                // Исключаем выражения в кавычках и "<>"
                IgnoreQuotes(ref i);

                // --------------------------
                // Находим константы
                if (IsNumber(Code[i]) && ('0' != Code[i]) && !IsLiteral(Code[i-1]) && ('.' != Code[i-1]) && ('.' != Code[i + 1]))
                {
                    tempName = Code[i].ToString();
                    tempEntry = i;

                    i++;

                    while (IsNumber(Code[i]))
                    {
                        tempName += Code[i];
                        i++;
                    }

                    // Добавление целочисленной константы
                    intConst.Add(tempName);
                    iconstEntries.Add(tempEntry);
                    continue;
                }
            }
        }

        /// <summary>
        /// Заменяет константы из 10-ричной СС в 16-ричную
        /// </summary>
        private void ConvertConst()
        {
            // Массив конвертированных в 16-ричную СС значения констант
            string[] convertIntConst = new string[intConst.Count];
            
            // Перевод из 10 в 16 СС
            for (int iconstIter = 0; iconstIter < intConst.Count; iconstIter++)
            {
                string tempIconst = "0x";

                // Стартовый символ
                tempIconst += Convert.ToString(Convert.ToInt32(intConst[iconstIter]), 16);
                    
                convertIntConst[iconstIter] = tempIconst;
            }
           
            // Замена констант
            for (int i = iconstEntries.Count - 1; i >= 0; i--)
            {
                Code.Remove(iconstEntries[i], intConst[i].Length);
                Code.Insert(iconstEntries[i], convertIntConst[i]);
            }
            intConst.Clear();
            iconstEntries.Clear();
        }


        /// <summary>
        /// Возращает обфусцированный код
        /// </summary>
        /// <returns>Обфусцированный исходный код</returns>
        public StringBuilder GetObfuscatedCode()
        {
            try
            {
                // "Причёсывание" кода
                RemoveComments();
                // Удаляем множественные пробелы
                RemoveMultipleSpaces();
                // Удаляем символы форматирования
                RemoveFormatSymbols();
                // Ещё раз удаляем множественные пробелы
                RemoveMultipleSpaces();

                // Клонируем код
                CloneCode = new StringBuilder();
                CloneCode.Append(Code.ToString());

                FindDeclaredIds();
                RenameIds();
            
            // Переводим числовые значение в 16-ричную систему счисления
            ConvertToHexadecimal();
            ConvertConst();
            AddComments();
            }
            catch (OverflowException)
            {
                Obfuscator.MainWindow.ShowExceptionMessageBox();
            }
            return Code;
        }

        /// <summary>
        /// Ищет подключенные нестандартные библиотеки
        /// </summary>
        /// <returns>Список имен библиотек</returns>
        public List<string> GetIncludeLibs()
        {
            // "Причёсывание" кода
            RemoveComments();
            // Удаляем множественные пробелы
            RemoveMultipleSpaces();
            // Удаляем символы форматирования
            RemoveFormatSymbols();
            // Ещё раз удаляем множественные пробелы
            RemoveMultipleSpaces();

            List<string> Libs = new List<string>();

            const string INCL = "#include";
            string currLib = "";
            bool isLibExist = false;

            for (int i = 0; i < Code.Length - (INCL.Length + 1); i++)
            {
                currLib = "";
                isLibExist = false;

                if (INCL == Code.ToString().Substring(i, INCL.Length))
                {
                    i += INCL.Length;
                    if (('<' == Code[i]) || ('<' == Code[i + 1]))
                        continue;

                    if ('"' == Code[i])
                        i++;

                    if ('"' == Code[i + 1])
                        i += 2;
                    
                    while ('"' != Code[i])
                    {
                        isLibExist = true;
                        currLib += Code[i];
                        i++;
                    }
                }

                if (isLibExist)
                    Libs.Add(currLib);
            }

            return Libs; 
        }

        /// <summary>
        /// Удаляет подключенную библиотеку
        /// </summary>
        /// <param name="libName"></param>
        public void RemoveLib(string libName)
        {
            string currLib;
            bool isLibExist;
            const string INCL = "#include";
            int startIndex, length;

            for (int i = 0; i < Code.Length - (INCL.Length + 1); i++)
            {
                currLib = "";
                isLibExist = false;
                startIndex = 0;
                length = 0;

                if (INCL == Code.ToString().Substring(i, INCL.Length))
                {
                    startIndex = i;
                    i += INCL.Length;
                    if (('<' == Code[i]) || ('<' == Code[i + 1]))
                        continue;

                    if ('"' == Code[i])
                        i++;

                    if ('"' == Code[i + 1])
                        i += 2;

                    while ('"' != Code[i])
                    {
                        isLibExist = true;
                        currLib += Code[i];
                        i++;
                    }
                    length = i - startIndex + 1;
                }

                if ((isLibExist) || (currLib == libName))
                    Code.Remove(startIndex, length);
            }
        }

        string[] ArrayCommsnts = {
            "/* Increment */",
            "/* if array overflow */",
            "/* assign new value */",
            "/* get new value */",
            "/* divide by zero */",
            "/* return new value */",
            "/* array filling */",
            "/* exchange of values */",
            "/* function value evalution */",
            "/* take me to church */",
            "/* we are the champions */",
            "/* hello, world */",
            "/* decrement */",
            "/* ininialization */",
            "/* complication of a table of variables */",
            "/* new function */",
            "/* rename a variable */",
            "/* remove value */",
            "/* find solution */",
            "/* new array */"
        };

        // добавление различных комментариев в код
        private void AddComments()
        {
            for (int i = 0; i < Code.Length-1; i++)
            {
                Random rnd = new Random();
                if (';' == Code[i])
                {
                    Code.Insert(i + 1, ArrayCommsnts[rnd.Next(0, ArrayCommsnts.Length - 1)]);
                }
            }
        }

    }
}