using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
namespace eumk2
{
    class Program
    {
        static int n;
        static List<string> text = new List<string>();
        static List<KeyValuePair<int, KeyValuePair<string, string>>>pairs = new List<KeyValuePair<int, KeyValuePair<string, string>>>();
        static List<string> wordsExclusion = new List<string>();
        static List<KeyValuePair<int, string>> oftenWords = new List<KeyValuePair<int, string>>();
             
        static void Input()
        {   //считываем число нужных пар
            var file = new StreamReader("n.txt");
            n = int.Parse(file.ReadLine());
            file.Close();
            
            //считываем текст(книгу)
            //Spit - разбиение строки на слова (подстроки)
            string[] words = File.ReadAllText(@"text10000(4).txt").Split(new char[]{ ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            text.AddRange(words);

            //считываем слова-исключения
            words = File.ReadAllText(@"word-exclusion.txt").Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            wordsExclusion.AddRange(words);
        }
        


        static void CreateOftenWords()
        {
            var words = new Dictionary<string,int>();
            //формируем словарь слов с их количесвом в тексте
            foreach(var i in text)
                //если слово не принадлежит словам-исключениям
                if (!wordsExclusion.Exists(x => x.StartsWith(i)))//???
                {
                    try 
                    {
                        words[i]++;
                    }
                    catch (Exception) 
                    {
                        words.Add(i, 1);
                    }
                }
                    
            //инициализируем список часто встречающихся слов
            foreach (var i in words)
            {
                var j = new KeyValuePair<int, string>(i.Value, i.Key);
                oftenWords.Add(j);
            }
            //сортировка по убыванию

            oftenWords.Sort(delegate (KeyValuePair<int, string> x, KeyValuePair<int, string> y)
            {
                return y.Key.CompareTo(x.Key);
            });
        }

        unsafe static void AddPair(ref Dictionary<KeyValuePair<string,string>,int> a, int idL, int idR)
        {
            var j = new KeyValuePair<string, string>(text[idL], text[idR]);
            try
            {
                a[j]++;
            }
            catch (Exception)
            {
                a.Add(j, 1);
            }
        }
        unsafe static void CreatePairs()
        {
            var a = new Dictionary<KeyValuePair<string,string>,int>();
            
            for (int i = 0; i < n && i<oftenWords.Count; i++) 
            {   //обработка первого элемента
                if (text[0] == oftenWords[i].Value && !wordsExclusion.Exists(x => x.StartsWith(text[1])))
                    AddPair(ref a, 0, 1);
                //обработка последнего элемента
                if (text[text.Count - 1] == oftenWords[i].Value && !wordsExclusion.Exists(x => x.StartsWith(text[text.Count - 1])))
                    AddPair(ref a, text.Count - 2, text.Count - 1);

                for (int k = 1; k < text.Count-1; k++)
                    if (text[k] == oftenWords[i].Value) 
                    {   //пара слева
                        if (!wordsExclusion.Exists(x => x.StartsWith(text[k - 1])))
                            AddPair(ref a, k - 1, k);
                        //пара справа
                        if (!wordsExclusion.Exists(x => x.StartsWith(text[k + 1])))
                            AddPair(ref a, k, k + 1);
                    }

                var b  = new KeyValuePair<string, string> (" ", " ");
                var res = new KeyValuePair<KeyValuePair<string,string>,int>(b, 0);
                foreach (var p in a) if (p.Value > res.Value) res = p;
                var z = new KeyValuePair<int, KeyValuePair<string,string>>(res.Value, res.Key);
         
                pairs.Add(z);
                a.Clear();
            }
        }
        static void Output()
        {
            pairs.Sort(delegate (KeyValuePair<int, KeyValuePair<string, string>> x, KeyValuePair<int, KeyValuePair<string, string>> y)
            {
                return y.Key.CompareTo(x.Key);
            });
            string text = "";
            foreach (var i in pairs)
                text += i.Key + " " + i.Value.Key + " " + i.Value.Value +"\r\n";
            File.WriteAllText(@"result.txt", text);

        }
        static void Main(string[] args)
        {
            Input();

            var timer =  Stopwatch.StartNew();
            CreateOftenWords();
            CreatePairs();
            File.WriteAllText(@"time.txt",$"time: {timer.ElapsedMilliseconds} ms");

            Output();
        }
    }
}

/*string s = Cinsole.Readline // прочитать строку
//прочитать число
int i = int.Parse(Console.Readline());
int i = Convert.ToInt32(Console.Readline());
 * //вывод 
 * //$-интерполяция
 * //:D004 - число ровно 4 знака
 * //:F4 - число знаков после ЗАПЯТОЙ
 * 
Console.WriteLine(&"i = {i:D004}");
 * //Подключение библиотек
 * //.
CultureInfo.DefaultThreadCurrentCulture = CultureInfo.Invariant();
 * //@- не подменяет слэши
string[] words File.ReadAllText(@"C:\test.txt).Split('\n', ' ');
//язык исходно компилируется не в машинные коды, компиляция в машинные коды происходит при запуске
//управление памятью - сбощик мусора (нет делитов, ссылок на объект)
//нет деструкторов  в явном виде
// ТИПЫ:
//все тип имеют общего предка (есть функции у числа О_О порождаются типом object)
//referenceType - относятся типы к классам
//valueType (нельзя поменять)
//массивы:
string[] words = new[] {"a", "bbb"};
 * //местное auto:
var words = new[] {"a", "bbb"};
 * //Length - длина
var array = new int[int.Parse(Console.ReadLine())];
 
var list = new List<string>();
 * //Add
 * //AddRange
 * //Capacity
 * //ToArray (копирование)
//сортировка по возрастанию 
list.Sort();
 * //
var dic = new Dictionary<string,int>();
 * //аналог map
var dic = new SortedDictionary<string, int>();

var timer = Stopwatch.StartNew();
 * //часы, минуты, миллисек
 * //.ElapsedMilliseconds -только миллисек
Console.WriteLIne($"time: {timer.Elapsed}");
 
//перечисление
static IEnumerable<int> Numbers()
 * {
 *  int i = 0;
 *  while(true)
 *  //прерывание функции, восстановление с того же места после запроса 
 *      yield return i++;
 * }
 string.Jone(", ",Numbers().Skip(10).Take(10));
 * //вместо Numbers() - Enumerable.Range(0,100)
var set = new HashSet<string>();
var sortedSet = new SortedSet<string>();

 * 
 */