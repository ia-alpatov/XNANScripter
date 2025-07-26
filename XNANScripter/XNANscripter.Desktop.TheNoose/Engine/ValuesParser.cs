using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using XNANScripter.Engine.Config;
using XNANScripter.Engine.Sprites;
using XNANScripter.Engine.Variables.Alias;
using System.Threading.Tasks;
using The_noose.Engine.TextDisplaying;

namespace XNANScripter.Engine
{
    internal static class ValuesParser
    {
        //Этот код разбирает параметры в строке по маске

        #region Masked params parser

        /* Типы масок
         *
         * 1 тип. Парсим и разбираем как есть
         * 2 тип. Парсим, разбираем и проверяем отставшуюся часть на наличие опциональной части и если она есть то разбираем её
         * 3 тип. Парсим, разбираем и в цикле проверяем сет масок При этом первое вхождение обязательно
         * 4 тип. Парсим, разбираем и в цикле проверяем сет масок, но наличие их необязательно
         *
         */

        internal static List<string> GetParams(string value, List<string> mask)
        {
            char type = mask[mask.Count - 1][0];

            int stableIndex;

            List<string> resRegExps = null;

            //В зависимости от типа маски получаем нужные части и транслитируем их в регэкпс
            if (type == '1')
            {
                stableIndex = mask.Count - 2;
                resRegExps = GetTranslatedMask(stableIndex, mask);
            }
            else
            {
                stableIndex = int.Parse(mask[mask.Count - 2]);
                resRegExps = GetTranslatedMask(stableIndex, mask);
            }

            //В зависимости от типа маски парсим реэкспом нужным методом
            List<string> paramsList;

            paramsList = RegExpParse(value, resRegExps);

            //Проверяем подходит ли полученная строка под маску
            if (paramsList != null)
            {
                value = paramsList[paramsList.Count - 1];

                List<string> temp;
                switch (type)
                {
                    case '2':

                        #region Второй тип

                        mask.RemoveRange(0, stableIndex + 1);
                        resRegExps = GetTranslatedMask(mask.Count - 3, mask);
                        temp = RegExpParse(value, resRegExps);
                        if (temp != null)
                        {
                            paramsList.RemoveAt(paramsList.Count - 1);
                            for (int i = 0; i < temp.Count; i++)
                            {
                                paramsList.Add(temp[i]);
                            }
                        }

                        #endregion Второй тип

                        break;

                    case '3':

                        #region Третий тип

                        mask.RemoveRange(0, stableIndex + 1);
                        resRegExps = GetTranslatedMask(mask.Count - 3, mask);
                        temp = RegExpParse(value, resRegExps);
                        if (temp != null)
                        {
                            for (int i = 0; i < temp.Count - 2; i++)
                            {
                                paramsList.Add(temp[i]);
                            }
                            value = paramsList[paramsList.Count - 1];
                        }
                        else
                        {
                            return null;
                        }
                        while (true)
                        {
                            temp = RegExpParse(value, resRegExps);
                            if (temp != null)
                            {
                                for (int i = 0; i < temp.Count - 2; i++)
                                {
                                    paramsList.Add(temp[i]);
                                }
                                value = paramsList[paramsList.Count - 1];
                            }
                            else
                            {
                                break;
                            }
                        }

                        #endregion Третий тип

                        break;

                    case '4':

                        #region Четвёртый тип

                        mask.RemoveRange(0, stableIndex + 1);
                        resRegExps = GetTranslatedMask(mask.Count - 3, mask);
                        while (true)
                        {
                            temp = RegExpParse(value, resRegExps);
                            if (temp != null)
                            {
                                paramsList.RemoveAt(paramsList.Count - 1);
                                for (int i = 0; i < temp.Count - 1; i++)
                                {
                                    paramsList.Add(temp[i]);
                                }
                                paramsList.Add(temp[temp.Count - 1]);
                                value = temp[temp.Count - 1];
                            }
                            else
                            {
                                break;
                            }
                        }

                        #endregion Четвёртый тип

                        break;
                }
            }
            else
            {
                return null;
            }

            return paramsList;
        }

        //Этот код разбирает строку по маске

        #region Regexp parser

        private static List<string> RegExpParse(string value, List<string> resRegExps)
        {
            List<string> paramsList = new List<string>();

            System.Text.RegularExpressions.Match m;

            for (int i = 0; i < resRegExps.Count; i++)
            {
                m = System.Text.RegularExpressions.Regex.Match(value, @"^[\s]*" + resRegExps[i]);

                if (m.Success)
                {
                    string val = m.Value.Trim();
                    if (val.Length > 0 && val[0] == ',')
                    {
                        val = val.Remove(0, 1);
                    }
                    paramsList.Add(val);

                    value = value.Remove(0, m.Value.Length);
                }
                else
                {
                    return null;
                }
            }

            if (value.Length > 0 && value[0] == ',')
            {
                value = value.Remove(0, 1);
            }

            paramsList.Add(value);

            return paramsList;
        }

        #endregion Regexp parser

        //Этот код преобразует полученную маску в регэксп

        #region Mask translation circle

        //Функци по переводу полной маски в регэксп
        private static List<string> GetTranslatedMask(int stableIndex, List<string> mask)
        {
            List<string> result = new List<string>();

            string comma = "";

            for (int i = 0; i <= stableIndex; i++)
            {
                if (i > 0) { comma = @",[\s]*"; };
                result.Add(comma + TranslateToRegExp(mask[i]));
            }

            return result;
        }

        #region Mask to regexp translator

        //Эта функция подготавливает и транслитирует маску в регэксп
        private static string TranslateToRegExp(string p)
        {
            string result = null;

            // Определяем нужна ли дополнительная подготовка и обработка для маски
            if (p.Contains('|') || p[0] == '(')
            {
                //Проверяем перечисление ли это
                if (p[0] == '(')
                {
                    result = p;
                }
                else
                {
                    //Проверяем ИЛИ ли это
                    if (p.Contains('|'))
                    {
                        result += "(";

                        string buf = null;
                        for (int i = 0; i < p.Length; i++)
                        {
                            if (i == p.Length - 1)
                            {
                                result += TranslateSwitch(buf);
                            }
                            else
                            {
                                if (p[i] == '|')
                                {
                                    result += TranslateSwitch(buf);
                                    result += '|';
                                    buf = null;
                                }
                                else
                                {
                                    buf += p[i];
                                }
                            }
                        }

                        result += ")";
                    }
                }
            }
            else
            {
                result = TranslateSwitch(p);
            }

            return result;
        }

        //Эта функция транслирует подготовленную маску в регэксп
        private static string TranslateSwitch(string p)
        {
            string result = null;

            //Проверяем ссылочные ли это переменные
            if (p.Length == 5 && p.Contains('%'))
            {
                switch (p.Remove(2))
                {
                    case "i%":
                        result = @"i%[a-zA-Z0-9_]+";
                        break;

                    case "s%":
                        result = @"s%[a-zA-Z0-9_]+";
                        break;

                    case "$%":
                        result = @"\$%[a-zA-Z0-9_]+";
                        break;

                    case "%%":
                        result = @"%%[a-zA-Z0-9_]+";
                        break;
                }
            }
            else
            {
                switch (p)
                {
                    case "$VAR":
                        result = @"(\$[a-zA-Z0-9_]+[\+]{0,1}|""[^""']*""[\+]{0,1}|'[^""']*'[\+]{0,1}|[a-zA-Z0-9_]+[\+]{0,1})+";
                        break;

                    case "%VAR":
                        result = @"(%[a-zA-Z0-9_]+[\+\*\\-]{0,1}|\?[a-zA-Z0-9_]+\[[0-9]+\](\[[0-9]+\]){0,1}[\+\*\\-]{0,1}|[-]{0,1}[0-9]+[\+\*\\-]{0,1}|0x[a-zA-Z0-9]+[\+\*\\-]{0,1}|[a-zA-Z0-9_]+[\+]{0,1})+";
                        break;

                    case "LABEL":
                        result = @"\*[0-9A-Za-z_]+";
                        break;

                    case "NAME":
                        result = "[0-9A-Za-z_]+";
                        break;

                    case "COLOR":
                        result = "#[0-9A-Za-z]{6}";
                        break;

                    default:
                        result = p;
                        break;
                }
            }

            return result;
        }

        #endregion Mask to regexp translator

        #endregion Mask translation circle

        #endregion Masked params parser

        // multi  =  %var1  +  ?asd[0]  -  10 * 0x2A + 2
        //Этот код преобразует строку с числами и переменными в число

        #region Get Number

        //функция получает конечное значение аргумента, учитывая + - * / , но пока не учитывая порядок действий
        internal static string GetNumber(string multi)
        {
            int endRes = 0;

            int j = 1;
            string buf = multi[0].ToString();
            char s = '+';

            while (true)
            {
                if (j == multi.Length)
                {
                    switch (s)
                    {
                        case '+':
                            endRes += int.Parse(NumVar(buf));
                            break;

                        case '-':
                            endRes -= int.Parse(NumVar(buf));
                            break;

                        case '*':
                            endRes *= int.Parse(NumVar(buf));
                            break;

                        case '/':
                            endRes /= int.Parse(NumVar(buf));
                            break;
                    }
                    break;
                }
                else
                {
                    switch (multi[j])
                    {
                        case '+':
                            endRes += int.Parse(NumVar(buf));
                            s = multi[j];
                            buf = null;
                            break;

                        case '-':
                            endRes -= int.Parse(NumVar(buf));
                            s = multi[j];
                            buf = null;
                            break;

                        case '*':
                            endRes *= int.Parse(NumVar(buf));
                            s = multi[j];
                            buf = null;
                            break;

                        case '/':
                            endRes /= int.Parse(NumVar(buf));
                            s = multi[j];
                            buf = null;
                            break;

                        default:
                            buf += multi[j];
                            break;
                    }

                    j++;
                }
            }

            return endRes.ToString();
        }

        #region Numeric variables

        //Эта функция оперделяет и берёт значение конкретной переменной
        private static string NumVar(string vari)
        {
            switch (vari[0])
            {
                case '%':
                    //Определяем не ссылка ли это

                    #region Userr ref and num varuables getter

                    if (vari.StartsWith("%%"))
                    {
                        //Перделать с учётом уровней
                        int startIndex = UserDefine.UserFunctionsVariablesIndexsList[UserDefine.UserFunctionsVariablesIndexsList.Count - 1 - UserDefine.UserFunctionsLevel];
                        UserFunctionsVariables.UserFunctionsVariables VarNames = UserDefine.UserFunctionsVariablesList.GetRange(startIndex, UserDefine.UserFunctionsVariablesList.Count - startIndex).First(var => var.CurrentLevelVarName == vari);

                        if (VarNames != null)
                        {
                            //Устанавливаем уровень выше и запускаем рекурсию
                            UserDefine.UserFunctionsLevel--;
                            vari = NumVar(VarNames.UpLevelVarName);
                            //Возвращаем уровень
                            UserDefine.UserFunctionsLevel++;
                        }
                    }
                    else
                    {
                        string VarName = vari.Remove(0, 1);
                        int number;
                        //Проверяем номер слота ли это
                        if (int.TryParse(VarName, out number))
                        {
                            vari = UserVariables.NumVarList.First(var => var.Key.number == number).Value.ToString();
                        }
                        else
                        {
                            vari = UserVariables.NumVarList.First(var => var.Key.name == VarName).Value.ToString();
                        }
                    }

                    #endregion Userr ref and num varuables getter

                    break;

                case '?':

                    #region Array getter

                    vari = vari.Remove(0, 1);
                    string name = null, n1 = null;

                    int j = 0;

                    while (true)
                    {
                        if (vari[j] != '[')
                        {
                            name += vari[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    vari = vari.Remove(0, j - 1);

                    j = 0;

                    while (true)
                    {
                        if (vari[j] != ']')
                        {
                            n1 += vari[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (vari[j + 1] == '[')
                    {
                        string n2 = null;

                        vari = vari.Remove(0, j - 1);
                        j = 0;

                        while (true)
                        {
                            if (vari[j] != ']')
                            {
                                n2 += vari[j];
                                j++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        vari = UserVariables.ArrayVarList[name][int.Parse(n1), int.Parse(n2)].ToString();
                    }
                    else
                    {
                        vari = UserVariables.ArrayVarList[name][0, int.Parse(n1)].ToString();
                    }

                    #endregion Array getter

                    break;

                default:
                    if ((vari.Length > 2) && vari.StartsWith("0x"))
                    {
                        vari = int.Parse(vari).ToString();
                    }
                    else
                    {
                        int number;
                        //Проверяем номер слота ли это
                        if (!int.TryParse(vari, out number))
                        {
                            vari = UserVariables.NumVarList.First(var => var.Key.name == vari).Value.ToString();
                        }
                    }
                    break;
            }
            return vari;
        }

        #endregion Numeric variables

        #endregion Get Number

        // multi = $var2 + "ada" + ' 123123' = 123ada 123123
        // Этот код преобразует набор строк и переменных в одну строку

        #region Get String

        internal static string GetString(string multi)
        {
            string buf = multi[0].ToString(), result = null;
            int j = 1;

            while (true)
            {
                if (j == multi.Length)
                {
                    result += StringVar(buf);
                    break;
                }
                else
                {
                    if (multi[j] == '+')
                    {
                        result += StringVar(buf);
                        buf = null;
                    }
                    else
                    {
                        buf += multi[j];
                    }
                    j++;
                }
            }

            return result;
        }

        private static string StringVar(string vari)
        {
            vari = vari.Trim();

            switch (vari[0])
            {
                case '$':
                    if (vari.StartsWith("$%"))
                    {
                        //Перделать с учётом уровней
                        int startIndex = UserDefine.UserFunctionsVariablesIndexsList[UserDefine.UserFunctionsVariablesIndexsList.Count - 1 - UserDefine.UserFunctionsLevel];
                        UserFunctionsVariables.UserFunctionsVariables VarNames = UserDefine.UserFunctionsVariablesList.GetRange(startIndex, UserDefine.UserFunctionsVariablesList.Count - startIndex).First(var => var.CurrentLevelVarName == vari);

                        if (VarNames != null)
                        {
                            //Устанавливаем уровень выше и запускаем рекурсию
                            UserDefine.UserFunctionsLevel--;
                            vari = StringVar(VarNames.UpLevelVarName);
                            //Возвращаем уровень
                            UserDefine.UserFunctionsLevel++;
                        }
                    }
                    else
                    {
                        vari = UserVariables.StringVarList[vari.Remove(0, 1)];
                    }
                    break;

                case '"':
                    vari = vari.Remove(vari.Length - 1).Remove(0, 1);
                    break;

                case '\'':
                    vari = vari.Remove(vari.Length - 1).Remove(0, 1);
                    break;

                default:
                    vari = UserVariables.StringVarList[vari];
                    break;
            }

            return vari;
        }

        #endregion Get String

        //Этот код задаёт значение переменной (это может быть массив или числовая переменная)

        #region Set number

        internal static void SetNumber(string variable, string value)
        {
            switch (variable[0])
            {
                case '?':
                    //Определяем размерность массива и адрес
                    variable = variable.Remove(0, 1);
                    string name = null, n1 = null;

                    int j = 0;

                    while (true)
                    {
                        if (variable[j] != '[')
                        {
                            name += variable[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    variable = variable.Remove(0, j - 1);

                    j = 0;

                    while (true)
                    {
                        if (variable[j] != ']')
                        {
                            n1 += variable[j];
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (variable[j + 1] == '[')
                    {
                        string n2 = null;

                        variable = variable.Remove(0, j - 1);
                        j = 0;

                        while (true)
                        {
                            if (variable[j] != ']')
                            {
                                n2 += variable[j];
                                j++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        UserVariables.ArrayVarList[name][int.Parse(n1), int.Parse(n2)] = int.Parse(value);
                    }
                    else
                    {
                        UserVariables.ArrayVarList[name][0, int.Parse(n1)] = int.Parse(value);
                    }
                    break;

                case '%':
                    //Проверяем не ссылочная ли переменная
                    if (variable.StartsWith("%%"))
                    {
                        //Перделать с учётом уровней
                        int startIndex = UserDefine.UserFunctionsVariablesIndexsList[UserDefine.UserFunctionsVariablesIndexsList.Count - 1 - UserDefine.UserFunctionsLevel];
                        UserFunctionsVariables.UserFunctionsVariables VarNames = UserDefine.UserFunctionsVariablesList.GetRange(startIndex, UserDefine.UserFunctionsVariablesList.Count - startIndex).First(var => var.CurrentLevelVarName == variable);

                        if (VarNames != null)
                        {
                            //Устанавливаем уровень выше и запускаем рекурсию
                            UserDefine.UserFunctionsLevel--;
                            SetNumber(VarNames.UpLevelVarName, value);
                            //Возвращаем уровень
                            UserDefine.UserFunctionsLevel++;
                        }
                    }
                    else
                    {
                        NumVariable key;
                        string VarName = variable.Remove(0, 1);
                        int number;
                        //Проверяем номер слота ли это или название переменной
                        if (int.TryParse(VarName, out number))
                        {
                            key = UserVariables.NumVarList.First(var => var.Key.number == number).Key;
                        }
                        else
                        {
                            key = UserVariables.NumVarList.First(var => var.Key.name == VarName).Key;
                        }

                        UserVariables.NumVarList[key] = int.Parse(GetNumber(value));
                    }
                    break;

                default:
                    NumVariable k = UserVariables.NumVarList.First(var => var.Key.name == variable).Key;
                    UserVariables.NumVarList[k] = int.Parse(GetNumber(value));
                    break;
            }
        }

        #endregion Set number

        //Этот код проверяет условия

        #region Conditionds checker

        internal static List<string> GetConditionResult(string p)
        {
            //Получаем список условий, где поледним элементом идёт остаток
            List<string> Conditions = ParseConditions(p);

            List<string> result = new List<string>();

            //Проверяем все условия
            bool first = CheckCondition(Conditions[0]);
            for (int i = 1; i < Conditions.Count - 2; i++)
            {
                if (first != CheckCondition(Conditions[i]))
                {
                    first = false;
                    break;
                }
            }

            //Добавляем результат и остаток
            result.Add(first.ToString());
            result.Add(Conditions[Conditions.Count - 1]);

            return result;
        }

        private static bool CheckCondition(string p)
        {
            bool result = false;

            if (p.StartsWith("lchk") && p.StartsWith("fchk"))
            {
                throw new NotImplementedException();
            }
            else
            {
                System.Text.RegularExpressions.Match matcher = System.Text.RegularExpressions.Regex.Match(p, @"^[\s]*([%]{1,2}[a-zA-Z0-9_]+|\?[a-zA-Z0-9_]+\[[0-9]+\](\[[0-9]+\]){0,1}|0x[a-zA-Z0-9]+|[0-9]+)");

                if (matcher.Success)
                {
                    int firstVar = int.Parse(GetNumber(matcher.Value.Trim()));
                    p = p.Remove(0, matcher.Value.Length);

                    string condition;

                    if (p[1] == '=' || p[1] == '>')
                    {
                        condition = p.Remove(2);
                        p = p.Remove(0, 2);
                    }
                    else
                    {
                        condition = p.Remove(1);
                        p = p.Remove(0, 1);
                    }

                    int secondVar = int.Parse(GetNumber(p));

                    switch (condition)
                    {
                        case "=":
                            result = (firstVar == secondVar);
                            break;

                        case "==":
                            result = (firstVar == secondVar);
                            break;

                        case "!=":
                            result = (firstVar != secondVar);
                            break;

                        case "<>":
                            result = (firstVar != secondVar);
                            break;

                        case ">":
                            result = (firstVar > secondVar);
                            break;

                        case "<":
                            result = (firstVar < secondVar);
                            break;

                        case ">=":
                            result = (firstVar >= secondVar);
                            break;

                        case "<=":
                            result = (firstVar <= secondVar);
                            break;

                        default:
                            throw new Exception("Неверное условие");
                    }
                }
                else
                {
                    throw new Exception("Неверное число");
                }
            }

            return result;
        }

        private static List<string> ParseConditions(string p)
        {
            List<string> result = new List<string>();

            System.Text.RegularExpressions.Match matcher;

            while (true)
            {
                //Проверяем числовое ли это условие
                matcher = System.Text.RegularExpressions.Regex.Match(p, @"^[\s]*([%]{1,2}[a-zA-Z0-9_]+|\?[a-zA-Z0-9_]+\[[0-9]+\](\[[0-9]+\]){0,1}|0x[a-zA-Z0-9]+|[0-9]+)[\s]*(=|==|!=|<>|>|<|>=|<=)([%]{1,2}[a-zA-Z0-9_]+|\?[a-zA-Z0-9_]+\[[0-9]+\](\[[0-9]+\]){0,1}|[-]{0,1}0x[a-zA-Z0-9]+|[-]{0,1}[0-9]+) (&|&&){0,1}");

                if (matcher.Success)
                {
                    result.Add(matcher.Value.Replace('&', ' ').Replace(" ", string.Empty));

                    int CountToDelete = (matcher.Value.EndsWith("&")) ? matcher.Value.Length + 1 : matcher.Value.Length;

                    p = p.Remove(0, CountToDelete);
                }
                else
                {
                    //Проверяем текстовое ли это значение
                    matcher = System.Text.RegularExpressions.Regex.Match(p, @"^[\s]*(lchk|fchk)");
                    if (matcher.Success)
                    {
                        throw new NotImplementedException();

                        string buf = matcher.Value.Trim();
                        p = p.Remove(0, matcher.Value.Length);

                        matcher = System.Text.RegularExpressions.Regex.Match(p, @"^[\s]*([a-zA-Z0-9_]+|(\$[a-zA-Z0-9_]+[\+]{0,1}|"".*""[\+]{0,1}|'.*'[\+]{0,1})) (&|&&){0,1}");
                        if (matcher.Success)
                        {
                            //Удаляем знак &
                            if (matcher.Value[matcher.Value.Length - 1] == '&')
                            {
                            }

                            int CountToDelete = (matcher.Value.EndsWith("&")) ? matcher.Value.Length + 1 : matcher.Value.Length;

                            p = p.Remove(0, CountToDelete);
                        }
                        else
                        {
                            throw new Exception("Неверный формат условия");
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            result.Add(p);

            return result;
        }

        #endregion Conditionds checker

        //Этот код проебразовывает #FFFFFF в цвет

        #region Color

        public static Color GetColor(string value)
        {
            Color color = Color.White;
            value = value.Substring(1);
            uint hex = uint.Parse(value, System.Globalization.NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            color.R = (byte)(hex >> 16);
            color.G = (byte)(hex >> 8);
            color.B = (byte)(hex);

            return color;
        }

        #endregion Color

        //Этот код преобразует тэги в спрайт

        #region Image processing tag

        /*
         * 1 тип -  просто название картинки
         * 2 тип -  картинка с тэгом
         * 3 тип - строковая картинка
         * 4 тип - тэг для генерации картинки
         */

        public static async Task<Sprite> GetSprite(string value)
        {
            //Удаляем ""
            value = value.Remove(value.Length - 1).Remove(0, 1);

            Sprite sprite = new Sprite();

            //Определяем тип
            if (value[0] != ':' && value[0] != '>')
            {
                #region 1 type

                sprite.Type = 1;

                //Загружаем текстуру из файла
                Texture2D texture = Config.System.Content.Load<Texture2D>(value);


                #endregion 1 type
            }
            else
            {
                System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(value, @"^(:.+;){0,1}>");

                if (m.Success)
                {
                    #region 4 type

                    sprite.Type = 4;

                    //Определяем тэг

                    //Создаём картинку из заданных цветов и размера

                    #endregion 4 type
                }
                else
                {
                    if (value.StartsWith(":s") && (value[2] == '/' || value[2] == ';'))
                    {
                        #region 3 type

                        sprite.Type = 3;

                        //Создаём картинку из текста

                        #endregion 3 type
                    }
                    else
                    {
                        #region 2 type

                        sprite.Type = 2;

                        //Получаем тэг
                        if (value[2] == '/' || value[2] == ';')
                        {
                            //безмасковый, возможно анимационный тэг

                            //Получаем тип прозрачности
                            sprite.TransparencyType = value[1];

                            if (value[2] == ';')
                            {
                                //безанимационный тэг
                                sprite.IsAnimated = false;

                                value = value.Remove(0, 3);
                            }
                            else
                            {
                                //анимационный тэг
                                sprite.IsAnimated = true;

                                #region Опрделение типа цикла, количества ячеек и времени, которое показывается каждый кадр

                                value = value.Remove(0, 3);

                                m = System.Text.RegularExpressions.Regex.Match(value, @"^[0-9]+,");

                                sprite.CellNumber = int.Parse(m.Value.Remove(m.Value.Length - 1));

                                value = value.Remove(0, m.Value.Length);

                                //Определяем время, которое показывается каждая часть картинки
                                if (value[0] == '<')
                                {
                                    sprite.IsDelayFull = true;

                                    #region First delay

                                    m = System.Text.RegularExpressions.Regex.Match(value, @"^<[0-9]+,");

                                    if (m.Success)
                                    {
                                        sprite.delayList.Add(int.Parse(m.Value.Remove(m.Value.Length - 1).Remove(0, 1)));

                                        value = value.Remove(0, m.Value.Length);
                                    }
                                    else
                                    {
                                        throw new Exception("Неверные параметры");
                                    }

                                    #endregion First delay

                                    #region Other delays

                                    for (int i = 0; i < sprite.CellNumber - 2; i++)
                                    {
                                        m = System.Text.RegularExpressions.Regex.Match(value, @"^[0-9]+,");

                                        if (m.Success)
                                        {
                                            sprite.delayList.Add(int.Parse(m.Value.Remove(m.Value.Length - 1)));

                                            value = value.Remove(0, m.Value.Length);
                                        }
                                        else
                                        {
                                            throw new Exception("Неверные параметры");
                                        }
                                    }

                                    #endregion Other delays

                                    #region Last delay

                                    m = System.Text.RegularExpressions.Regex.Match(value, @"^[0-9]+>,");

                                    if (m.Success)
                                    {
                                        sprite.delayList.Add(int.Parse(m.Value.Remove(m.Value.Length - 2)));

                                        value = value.Remove(0, m.Value.Length);
                                    }
                                    else
                                    {
                                        throw new Exception("Неверные параметры");
                                    }

                                    #endregion Last delay
                                }
                                else
                                {
                                    sprite.IsDelayFull = false;

                                    m = System.Text.RegularExpressions.Regex.Match(value, @"^[0-9]+,");
                                    sprite.delayList.Add(int.Parse(m.Value.Remove(m.Value.Length - 1)));
                                    value = value.Remove(0, m.Value.Length);
                                }

                                //Тип цикла

                                sprite.LoopType = int.Parse(value.Remove(value.IndexOf(';')));

                                value = value.Remove(0, value.IndexOf(';') + 1);

                                #endregion Опрделение типа цикла, количества ячеек и времени, которое показывается каждый кадр
                            }
                        }
                        else
                        {
                            //масковый, возможно параметрический тэг

                            sprite.TransparencyType = 'm';

                            throw new NotImplementedException();
                        }

                        //Загружаем картинку
                        Texture2D texture = Config.System.Content.Load<Texture2D>(value);

                        

                        if (sprite.IsAnimated)
                        {
                            int cellWidth;

                            switch (sprite.TransparencyType)
                            {
                                case 'l':

                                    #region Загружаем картинку, делим её на части, устанавливая прозрачность соответствующую верхнему левому пикселю

                                    Color[] texData = new Color[texture.Width * texture.Height];

                                    texture.GetData<Color>(texData);

                                    cellWidth = texture.Width / sprite.CellNumber;

                                    List<Texture2D> textursList = new List<Texture2D>();
                                    List<Color[]> colorsList = new List<Color[]>();

                                    for (int i = 0; i < sprite.CellNumber; i++)
                                    {
                                        textursList.Add(new Texture2D(Drawing.GraphicsDevice, cellWidth, texture.Height));
                                        colorsList.Add(new Color[cellWidth * texture.Height]);
                                    }

                                    ushort cellCounter = 0;
                                    int widthBuf = 0, tempX = 0;
                                    byte alpha = GetAlpha(texData[0]);

                                    for (int y = 0; y < texture.Width; y++)
                                    {
                                        for (int x = 0; x < texture.Height; x++)
                                        {
                                            widthBuf++;
                                            if ((x - widthBuf * (cellCounter + 1)) > 0)
                                            {
                                                cellCounter++;
                                                alpha = GetAlpha(texData[x + y]);
                                                tempX = 0;
                                            }
                                            texData[x + y].A = alpha;
                                            colorsList[cellCounter][y + tempX] = texData[x + y];
                                            tempX++;
                                        }

                                        tempX = 0;
                                        cellCounter = 0;
                                        widthBuf = 0;
                                    }

                                    for (int i = 0; i < sprite.CellNumber; i++)
                                    {
                                        textursList[i].SetData<Color>(colorsList[i]);
                                    }

                                    colorsList.Clear();

                                    sprite.texturesList = textursList;
                                    textursList.Clear();

                                    #endregion Загружаем картинку, делим её на части, устанавливая прозрачность соответствующую верхнему левому пикселю

                                    break;

                                case 'r':
                                    throw new Exception();
                                    break;

                                case 'c':
                                    throw new Exception();
                                    break;

                                case 'a':

                                    cellWidth = texture.Width / sprite.CellNumber;

                                    throw new Exception();
                                    break;

                                case 'm':
                                    throw new Exception();
                                    break;
                            }
                        }
                        else
                        {
                            switch (sprite.TransparencyType)
                            {
                                case 'l':
                                    throw new Exception();
                                    break;

                                case 'r':
                                    throw new Exception();
                                    break;

                                case 'c':
                                    throw new Exception();
                                    break;

                                case 'a':

                                    int cellWidth = texture.Width / 2;

                                    Texture2D orTexture = new Texture2D(Drawing.GraphicsDevice, cellWidth, texture.Height);
                                    Color[] orColor = new Color[texture.Height * cellWidth];

                                    Color[] texData = new Color[texture.Width * texture.Height];
                                    texture.GetData<Color>(texData);

                                    for (int y = 0; y < texture.Height; y++)
                                    {
                                        for (int x = 0; x < texture.Width; x++)
                                        {
                                            if (x < cellWidth)
                                            {
                                                orColor[x + y * (cellWidth)] = texData[x + y * (texture.Width)];
                                            }
                                            else
                                            {
                                                orColor[(x - cellWidth) + y * (cellWidth)].A = GetAlpha(texData[x + y * (texture.Width)]);
                                            }
                                        }
                                    }

                                    orTexture.SetData<Color>(orColor);

                                    sprite.texturesList.Add(orTexture);

                                    break;

                                case 'm':
                                    throw new Exception();
                                    break;
                            }

                            //sprite.texturesList.Add();
                        }

                        #endregion 2 type
                    }
                }
            }

            return sprite;
        }

        private static byte GetAlpha(Color clr)
        {
            int alpha = clr.B + clr.G + clr.R;

            byte[] result;

            if (alpha != 0)
            {
                result = BitConverter.GetBytes((alpha - 765) * (-1) / 3);
            }
            else
            {
                result = new byte[1];
                result[0] = 255;
            }

            return result[0];
        }

        #endregion Image processing tag

        //Этот код подготавливает строку текста

        // `sad/ '

        public static bool GetText(string line)
        {
            line = line.TrimStart();
            if (line[0] == '`')
            {
                // 1-byte text - English, for example
                line = line.Remove(0,1);

                if (line[line.Length - 1] == '@')
                {
                    
                }
                else if (line[line.Length - 1] == '\\')
                {


                }
            }
            else if (line[line.Length - 1] == '@')
            { 
            
            }
            else if (line[line.Length - 1] == '\\')
            {
                
            }

            List<TextPart> parts = new List<TextPart>();
            var lineArray = line.Split('@').ToList();
            if (lineArray.Count == 1)
            {
                var array = line.Split('\\').ToList();
                if (array.Count == 1)
                {
                    if (!String.IsNullOrWhiteSpace(array[0]))
                    {
                        parts = GetColoredParts(array[0]);
                        parts[parts.Count - 1].IsEndingWithWait = false;
                        parts[parts.Count - 1].IsEndingWithNewLine = false;

                        TextWindow.TextParts.AddRange(parts);
                        TextWindow.IsShowing = true;
                        

                        return true;
                    }
                }
                else if (array.Count == 2 && String.IsNullOrWhiteSpace(array[1]))
                {
                    parts = GetColoredParts(array[0]);
                    parts[parts.Count - 1].IsEndingWithWait = false;
                    parts[parts.Count - 1].IsEndingWithNewLine = true;

                    TextWindow.TextParts.AddRange(parts);
                    TextWindow.IsShowing = true;

                    return true;
                }
                else
                {
                    foreach (string c in array)
                    {
                        if (String.IsNullOrWhiteSpace(c))
                        {
                            parts.AddRange(GetColoredParts(array[0]));
                            parts[parts.Count - 1].IsEndingWithWait = false;
                            parts[parts.Count - 1].IsEndingWithNewLine = true;
                        }
                    }
                }
               
            }
            else if (lineArray.Count == 2 && String.IsNullOrWhiteSpace(lineArray[1]))
            {
                parts = GetColoredParts(lineArray[0]);
                parts[parts.Count - 1].IsEndingWithWait = true;
                parts[parts.Count - 1].IsEndingWithNewLine = false;

                TextWindow.TextParts.AddRange(parts);
                TextWindow.IsShowing = true;

                return true;
            }
            else
            {
                foreach (string ch in lineArray)
                {
                    var array = ch.Split('\\').ToList();
                    if (array.Count == 1)
                    {
                        if (!String.IsNullOrWhiteSpace(array[0]))
                        {
                            parts.AddRange(GetColoredParts(array[0]));
                            parts[parts.Count - 1].IsEndingWithWait = true;
                        }
                    }
                    else if (array.Count == 2 && String.IsNullOrWhiteSpace(array[1]))
                    {
                        parts.AddRange(GetColoredParts(array[0]));
                        parts[parts.Count - 1].IsEndingWithWait = false;
                        parts[parts.Count - 1].IsEndingWithNewLine = true;
                    }
                    else
                    {
                        foreach (string c in array)
                        {
                            if (String.IsNullOrWhiteSpace(c))
                            {
                                parts.AddRange(GetColoredParts(array[0]));
                                parts[parts.Count - 1].IsEndingWithWait = false;
                                parts[parts.Count - 1].IsEndingWithNewLine = true;
                            }
                        }
                    }
                }
            }

            //temp hack
            if (TextWindow.TextParts != null && TextWindow.TextParts.Count > 0 && string.IsNullOrWhiteSpace(TextWindow.TextParts.Last().Text) && TextWindow.TextParts.Last().IsEndingWithWait)
            {
                TextWindow.TextCurrentPosition = Vector2.Zero;
            }

            TextWindow.TextParts = parts;
            TextWindow.IsShowing = true;

            return true;
        }

        private static List<TextPart> GetColoredParts(string line)
        {
            List<TextPart> result = new List<TextPart>();
            var matcher = System.Text.RegularExpressions.Regex.Match(line, "#[A-Za-z0-9]{6,6}");

            if (matcher.Success)
            {
                if (matcher.Groups.Count > 1)
                {
                    for(int i = 0; i < matcher.Groups.Count; i++)
                    {
                        var text = line.Replace(matcher.Value, String.Empty);
                        if(matcher.Groups.Count > i+1)
                        {
                            text = text.Remove(text.IndexOf(matcher.Groups[i+1].Value));
                        }
                        result.Add(new TextPart() { IsColoredPart = true, Text = text, TextColor = GetColor(matcher.Value), Position = TextWindow.TextPointer });
                        line = line.Remove(0,text.Length + matcher.Value.Length);
                    }
                }
                else
                {
                    result.Add(new TextPart() { IsColoredPart = true, Text = line.Replace(matcher.Value, String.Empty), TextColor = GetColor(matcher.Value), Position = TextWindow.TextPointer });
                }
            }
            else
            {
                result.Add(new TextPart() { IsColoredPart = false, Text = line, TextColor = TextWindow.TextColor, Position = TextWindow.TextPointer });
            }
            return result;
        }
    }
}