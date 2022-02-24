using System;
using System.Collections.Generic;
namespace Bulls
{
    class Program
    {
        // ЧИТ-КОД!!! (God of Bulls and Cows)
        static string cheat = "GB&C";

        /// <summary>
        /// Функция создающия все варианты level-значных чисел, у которых все цифры меньше level
        /// </summary>
        /// <param name="level"></param>
        /// <returns>Все варианты</returns>
        static public List<string> MakeAllNumbers(uint level)
        {
            //Однозначные 
            if (level == 1)
            {
                // Если число однозначное, то это только 1(0 - с ведущим 0)
                List<string> allVariants = new List<string>() { "1" };
                return allVariants;
            }
            else
            {
                List<string> allVariants = new List<string>();
                // Рекурсивно создаём все (level - 1) - значные числа, у которых все цифры < (level - 1)
                List<string> previousAllVariants = MakeAllNumbers(level - 1);
                for (int i = ((level % 10 != 0) ? 0 : 1)  /* костыль, чтобы 0 не был ведущим */; i < level; ++i)
                {
                    // Добавляем цифру - level, во все места каждого числа
                    for (int j = 0; j < previousAllVariants.Count; ++j)
                    {
                        string newVariant = previousAllVariants[j].Insert(i, Convert.ToString(level % 10));
                        allVariants.Add(newVariant);
                    }
                }
                return allVariants;
            }
        }
        /// <summary>
        /// // Функция возвращающая рандомное level-значное число
        /// // Эту функцию можно написать подругому. Генерировать случайную цифру, и проверить есть ли в нашей переменной ответа эта цифра(если есть то добавлять, иначе брать другую). Изначально наша переменная пустая. Делаем эту процедуру level раз.
        /// </summary>
        /// <param name="level">уровень</param>
        /// <returns>Случайный ответ</returns>
        static public string MakeRandomNumber(uint level)
        {
            // Создаём все возможные 10-значные варианты
            List<string> allVariants = MakeAllNumbers(10);
            Random rand = new Random();
            // Индекс случайного варианта
            int ind = rand.Next(allVariants.Count);
            string randonNumber = "";
            // Этот цикл из случайного 10-значного числа, делает level-значное
            for (int i = 0; i < level; ++i)
            {
                randonNumber += allVariants[ind][i];
            }
            return randonNumber;
        }
        /// <summary>
        /// Функция проверяющая строку на повторы
        /// </summary>
        /// <param name="try_">попытка</param>
        /// <returns></returns>
        static public bool CheckRepetitions(string try_)
        {
            // Перебираем все пары символов и проверяем
            for (int i = 0; i < try_.Length; ++i)
            {
                for (int j = 0; j < i; ++j)
                {
                    if (try_[i] == try_[j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Функция выводит быков и коров в правельной форме
        /// </summary>
        /// <param name="bulls">быки</param>
        /// <param name="cows">коровы</param>
        static public void Print(int bulls, int cows)
        {
            if (bulls % 10 == 1)
            {
                Console.Write(bulls + " бык ");
            }
            else if (bulls % 10 >= 2 && bulls % 10 <= 4)
            {
                Console.Write(bulls + " быка ");
            }
            else
            {
                Console.Write(bulls + " быков ");
            }
            if (cows % 10 == 1)
            {
                Console.WriteLine(cows + " корова");
            }
            else if (cows % 10 >= 2 && cows % 10 <= 4)
            {
                Console.WriteLine(cows + " коровы");
            }
            else
            {
                Console.WriteLine(cows + " коров");
            }
        }

        /// <summary>
        /// Сам ответ на запрос
        /// </summary>
        /// <param name="right">правельный ответ</param>
        /// <param name="try_">попытка</param>
        /// <param name="countTry">количество попыток</param>
        static public void GetBullsandCows(string right, string try_, int countTry)
        {
            int bulls = 0, cows = 0;
            for (int i = 0; i < right.Length; ++i)
            {
                if (try_[i] == right[i])
                {
                    bulls++;
                }
                else if (right.Contains(Convert.ToString(try_[i])))
                {
                    cows++;
                }
            }
            if (bulls != right.Length)
                Print(bulls, cows);
            // Это снова вывод с правельной формой слов
            else if (countTry == 1)
                Console.WriteLine($"ВЕРНО!!! Вы победели за {countTry} попытку");
            else if (countTry >= 2 && countTry <= 4)
                Console.WriteLine($"ВЕРНО!!! Вы победели за {countTry} попытки");
            else
                Console.WriteLine($"ВЕРНО!!! Вы победели за {countTry} попыток");
        }

        /// <summary>
        /// Функция которая не отстанет, пока вы не сделайте корректную попытку ввода
        /// </summary>
        /// <param name="level">уровень</param>
        /// <param name="countTry">количество попыток</param>
        /// <returns>Верная Попытка</returns>
        static public string MakeTry(uint level, int countTry)
        {
            Console.WriteLine("Попытка № " + countTry);
            string try_ = Console.ReadLine();
            // Проверка на корректность
            if (try_ != "give up" && try_ != "GB&C" && (try_.Length != level || try_[0] <= '0' || try_[0] > '9' || try_[try_.Length - 1] < '0' ||
                try_[try_.Length - 1] > '9' || !ulong.TryParse(try_, out ulong buff)/*просто проверка на число(переменная buff далее не нужна)*/
                || CheckRepetitions(try_)))
            {
                Console.WriteLine("Неверный ввод, попрубуйте снова");
                // Если Ввод некорректный, то вызов ещё раз
                return MakeTry(level, countTry);
            }
            return try_;
        }

        /// <summary>
        /// Сама игра
        /// </summary>
        /// <param name="level">уровень</param> 
        static public void Game(uint level)
        {
            string right = MakeRandomNumber(level);
            // Количество попыток
            int countTry = 1;
            string try_ = "";
            while (try_ != right && try_ != "give up")
            {
                // Ваша попытка
                try_ = MakeTry(level, countTry);
                // Проверка на чит
                if (try_ == cheat)
                {
                    Console.WriteLine(right);
                    continue;
                }
                // Проверка на сдачу
                if (try_ != "give up")
                    GetBullsandCows(right, try_, countTry);
                countTry++;
            }
            if (try_ != right)
            {
                Console.WriteLine("Очень жаль, в следующий раз обязательно получится");
            }
        }
        /// <summary>
        /// Начало игры(выбор уровня сложности)
        /// </summary>
        /// <param name="args"></param>
        static public void Main(string[] args)
        {
            while (true)
            {
                // Первый запрос, на то хочет ли вообще человек играть

                Console.WriteLine("Введите start, чтобы начать. Или finish, чтобы выйти");
                string startcommand = Console.ReadLine();
                while (startcommand != "start" && startcommand != "finish")
                {
                    Console.WriteLine("Вы ввели другую команду, попробуйте снова");
                    startcommand = Console.ReadLine();
                }
                if (startcommand == "finish")
                {
                    break;
                }

                // Выбор уровня сложности

                Console.WriteLine("Выберите уровень сложности(Введите число от 1 до 10)");
                uint level = 11;
                while (!uint.TryParse(Console.ReadLine(), out level) || level > 10 || level == 0)
                {
                    Console.WriteLine("Вы ввели что-то другое, попробуйте снова");
                }

                Game(level);

            }
        }
    }
}