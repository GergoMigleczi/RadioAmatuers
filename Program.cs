using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace RadioAmatuers
{
    struct Message
    {
        public int day;
        public int IdOfReciever;
        public string message;

        public Message(int day, int IdOfReciever, string message)
        {
            this.day = day;
            this.IdOfReciever = IdOfReciever;
            this.message = message;
        }
    }
    class RadioAmatue
    {
        static List<Message> messages = new List<Message>();

        //Read and store the data
        static void Task1()
        {
            StreamReader sr = new StreamReader("veetel.txt");

            /*
             The file looks like this:
                    1 12         --> day=1, reciever=12
                    1/12 bla#b#ah#lah --> message=thisLine wolves=1, babywolves=12      # =  unkown chars that the reciever did not recieve.
                    1 10                                                                        There is one message per day, so another reciever might recieved this char and
                    .....                                                                       later in task 5 the messages will be recovered
                    ...
             */
            while (!sr.EndOfStream)
            {
                string[] dayAndIdOfReciever = sr.ReadLine().Split();
                int day = int.Parse(dayAndIdOfReciever[0]);
                int IdOfReciever = int.Parse(dayAndIdOfReciever[1]);
                string message = sr.ReadLine();

                Message item = new Message(day, IdOfReciever, message);
                messages.Add(item);
            }

            sr.Close();
        }

        //Print who recieved the first and last message in the list
        static void Task2()
        {
            Console.WriteLine("Task 2");
            Console.WriteLine($"The reciever of the first message: {messages[0].IdOfReciever}");
            Console.WriteLine($"The reciever of the last message: {messages[messages.Count - 1].IdOfReciever}");
        }

        //Print the day and id of the reciever of the messge that contains "farkas" (farkas = wolf) 
        static void Task3()
        {
            Console.WriteLine("Task 3");

            foreach (Message item in messages)
            {
                if (item.message.Contains("farkas"))
                {
                    Console.WriteLine($"{item.day}. day: {item.IdOfReciever} reciever");
                }
            }
        }

        //Print how many reciever has reciueved a message each day
        //The task sheet stated that there were 11 days
        static void Task4()
        {
            Console.WriteLine("Task 4");
            //days = 1-11

            for (int i = 1; i < 12; i++)
            {
                int numberOfRecievers = 0;
                foreach (Message item in messages)
                {
                    if (item.day == i)
                        numberOfRecievers++;
                }
                Console.WriteLine($"{i}. day: {numberOfRecievers} reciever");
            }

        }

        //Recover the messages
        //The task sheet stated that there was only one message per day.
        static void Task5()
        {
            StreamWriter sw = new StreamWriter("adaas.txt");

            for (int i = 1; i < 12; i++)
            {
                string recovered = "";
                foreach (Message item in messages)
                {
                    if (item.day == i)
                    {
                        if (recovered == "")
                        {
                            recovered = item.message;
                        }
                        else
                        {
                            for (int j = 0; j < 90; j++)
                            {
                                if (recovered[j] == '#')
                                {
                                    char[] ch = recovered.ToCharArray();
                                    ch[j] = item.message[j];
                                    recovered = new string(ch);
                                }
                            }
                        }
                    }
                }
                sw.WriteLine($"{i}. day: {recovered}");
            }

            sw.Flush();
            sw.Close();
        }

        //Create a method that checks if a string is a number or not (szame = szám-e? = is it a number)
        static bool szame(string szo)
        {
            bool szame = true;
            for (int i = 0; i < szo.Length; i++)
            {
                if (szo[i] < '0' || szo[i] > '9')
                    szame = false;
            }
            return szame;
        }

        //The user inputs a day and a reciever
        //The code shall tell how many wolves were seen on that day (wolves and babywolves together)
        //if # follows the number then the code shall print the there is no information
        static void Task7()
        {
            Console.WriteLine("Task 7");

            Console.WriteLine("A day= ");
            int day = int.Parse(Console.ReadLine());

            Console.Write("An Id of a reciever= ");
            int IdOfReciever = int.Parse(Console.ReadLine());

            bool ListContainsThisRecord = false;
            bool ThereIsInformation = false;
            int wolves = 0;

            int totalOfWolves = 0;
            foreach (Message item in messages)
            {
                if (item.day == day && item.IdOfReciever == IdOfReciever)
                {
                    ListContainsThisRecord = true;
                    if (item.message.Contains('/'))
                    {
                        ThereIsInformation = true;
                        string[] split = item.message.Split('/');
                        string kolyok = split[0];
                        string felnott = split[1];
                        if (kolyok.Length == 1)
                        {
                            if (szame(kolyok))
                            {
                                totalOfWolves += int.Parse(kolyok);
                            }
                        }
                        else
                        {
                            ThereIsInformation = true;
                            for (int i = 0; i < kolyok.Length - 1; i++)
                            {

                                if (szame(kolyok[i].ToString()) && kolyok[i + 1] != '#')
                                {

                                }
                                else { ThereIsInformation = false; }
                            }
                            if (ThereIsInformation)
                            {
                                totalOfWolves += int.Parse(kolyok);
                            }
                        }

                        if (felnott.Length == 1)
                        {
                            if (szame(felnott))
                            {
                                totalOfWolves += int.Parse(felnott);
                            }
                        }
                        else
                        {
                            ThereIsInformation = true;
                            for (int i = 0; i < felnott.Length - 1; i++)
                            {

                                if (szame(felnott[i].ToString()) && felnott[i + 1] != '#')
                                {
                                    char[] ch = new char[90];
                                    ch[i] = felnott[i];
                                    string atvaltas = new string(ch);
                                    wolves = int.Parse(atvaltas);
                                    if (felnott[i + 1] == ' ')
                                    {
                                        break;
                                    }
                                }
                                else { ThereIsInformation = false; }
                            }
                            if (ThereIsInformation)
                            {
                                totalOfWolves += wolves;
                            }
                        }

                    }


                }

            }

            if (ListContainsThisRecord)
            {
                if (ThereIsInformation)
                {
                    Console.WriteLine($"Number {IdOfReciever} reciever recorded that {totalOfWolves} wolves were seen");
                }
                else
                {
                    Console.WriteLine("There is no clear information of the number of wolves seen");
                }
            }
            else
            {
                Console.WriteLine("There is no such record in the list");
            }
        }
        static void Main(string[] args)
        {
            Task1();
            Task2();
            Console.WriteLine();
            Task3();
            Console.WriteLine();
            Task4();
            Console.WriteLine();
            Task5();
            Console.WriteLine();
            Task7();
            Console.ReadKey();
        }
    }
}
