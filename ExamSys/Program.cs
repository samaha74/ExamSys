using System.ComponentModel.DataAnnotations;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace ExamSys
{ 

    abstract class Question
    {
        public Question()
        {
        }

        public Question(string header, int defLvl, int marks, string type)
        {
            Header = header;
            DefLvl = defLvl;
            Marks = marks;
            Type = type;
        }

        public string Header { get;set;}
        public int DefLvl { get;set; }
        public int Marks { get;set; }
        public string Type{ get;set; }

        public abstract int DisplayQ();
        public abstract bool CheckAns(int a);
    }

    class TrueFalse : Question
    {
        public TrueFalse(string header, int defLvl, int marks, string type, int ans) : base(header, defLvl, marks, type)
        {
            Header = header;
            DefLvl = defLvl;
            Marks = marks;
            Type = type;
            answer = ans;
        }

        public int answer { get;set; }

        public override int DisplayQ()
        {
            Console.WriteLine(this.Header);
            Console.WriteLine("Enter ans(true -> 1 , False -> 0)");
            return Convert.ToInt32(Console.ReadLine());
        }
        public override bool CheckAns(int a)
        {
            return a == answer;
        }
        
    }
    class ChooseOne : Question
    {
        public int Answer { get; set; }
        public List<string> ChoiceHeader = new List<string>();

        public ChooseOne(string header, int defLvl, int marks, string type,int answer,List<string> choiceHeader) : base(header, defLvl, marks, type)
        {
            Header = header;
            DefLvl= defLvl;
            Marks= marks;
            Type = type;
            Answer = answer;
            ChoiceHeader = choiceHeader;
        }

        public override int DisplayQ()
        {
            Console.WriteLine(this.Header);
            int i = 1;
            foreach(string a  in ChoiceHeader) Console.WriteLine( i++ + a);
            Console.WriteLine("\n Enter answer number: ");
            return Convert.ToInt32(Console.ReadLine());
        }
        public override bool CheckAns(int a)
        {
            return a == Answer;
        }
    }

    class MultChoice : Question
    {
        public List<int> Answers = new List<int>();
        public List<string> ChoiceHeader = new List<string>();

        public MultChoice(string header, int defLvl, int marks, string type, List<int> answers, List<string> choiceHeader) : base(header, defLvl, marks, type)
        {
            Header =header;
            DefLvl=defLvl;
            Marks=marks;
            Type = type;
            Answers = answers;
            ChoiceHeader = choiceHeader;
        }

        public override int DisplayQ()
        {
            Console.WriteLine(this.Header);
            int i = 1;
            foreach (string a in ChoiceHeader) Console.WriteLine(i++ + a);
            Console.WriteLine("Enter choices(eg. 1,3 / 2,3)");
            string tmp = Console.ReadLine();
            List<int> ll = new List<int>();
            foreach (string s in tmp.Split(","))
                ll.Add(Convert.ToInt32(s));

            int k = 0;
            int j = 1;
            foreach (int x in Answers)
            {
                k += x * i;
                j *= 10;
            }

            return k;
        }
        public override bool CheckAns(int a)
        {
            bool tmp = true;
            int k = 0;
            int i = 1;
        foreach (int x in Answers)
            {
                k += x * i;
                i *= 10;
            }
            return a == k;
        }
    }
    class Doctor
    {
        public static void DocMenu(List<List<Question>> q)
        {
            Console.WriteLine("Choose number of Question: ");
            int n = Convert.ToInt32(Console.ReadLine());

            for(int i = 0; i < n; i++)
            {
                Console.WriteLine("Select Question type(T/F -> 1 , MultChoice -> 2 , ChooseOne -> 3)");
                int t = Convert.ToInt32(Console.ReadLine());
                Question tmp = GetQuestion(t);
                q[tmp.DefLvl - 1].Add(tmp);
            }
        }

        private static Question GetQuestion(int x)
        {
            Console.WriteLine("Enter level (Easy -> 1, Medium -> 2, Hard -> 3).");
            int d = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter Header: ");
            string h = Console.ReadLine();
            Console.WriteLine("Enter Marks:");
            int m = Convert.ToInt32(Console.ReadLine());

            

            switch (x)
            {
                case 1:
                    Console.WriteLine("Enter Answer(T -> 1 , F -> 0): ");
                    int c = Convert.ToInt32(Console.ReadLine());
                    return new TrueFalse(h,d,m,"t/f", c);
                case 2:
                    Console.WriteLine("Enter Choice headers: ");
                    List<string> l = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        l.Add(Console.ReadLine());
                    }
                    Console.WriteLine("Enter correct choices(eg. 1,3 / 2,3)");
                    string tmp = Console.ReadLine();
                    List<int> ll = new List<int>(); 
                    foreach (string s in tmp.Split(","))
                        ll.Add(Convert.ToInt32(s));
                    return new MultChoice(h,d,m,"mc", ll,l);
                case 3:
                    Console.WriteLine("Enter Choice headers: ");
                    List<string> l1 = new List<string>();
                    for (int i = 0; i < 4; i++)
                    {
                        l1.Add(Console.ReadLine());
                    }
                    Console.WriteLine("Enter correct choices(1 --> 4)");
                    int ans = Convert.ToInt32(Console.ReadLine());
                    return new ChooseOne(h, d, m, "co", ans, l1);
                default:
                    return null;
            }
        }
    }

    class Student
    {
        public static void studMenu(List<List<Question>> q)
        {
            Console.WriteLine("1. practical Exam\n" +
                "2. Final Exam");
            int c = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("Enter level (Easy -> 1, Medium -> 2, Hard -> 3).");
            int d = Convert.ToInt32(Console.ReadLine());

            int t, s;
            t = s = 0;
            switch (c)
            {
                case 1:
                    for(int i =0; i< q[d-1].Count()/2; i++)
                    {
                        Question l = q[d-1][i];
                        bool r = l.CheckAns(l.DisplayQ());
                        if (r)
                            s += l.Marks;
                        t += l.Marks;

                        Console.WriteLine("\n\n==========================================\n\n");
                    }

                    Console.WriteLine($"Your Resault is {s} / {t}");
                    Console.WriteLine("\n\n==========================================\n\n");
                    break;

                case 2:
                    foreach (Question l in q[d - 1])
                    {
                        bool r = l.CheckAns(l.DisplayQ());
                        if (r)
                            s += l.Marks;
                        t += l.Marks;

                        Console.WriteLine("\n\n==========================================\n\n");
                    }

                    Console.WriteLine($"Your Resault is {s} / {t}");
                    Console.WriteLine("\n\n==========================================\n\n");
                    break;


            }
        }
    }

    internal class Program
    {
        private static int MainMenu()
        {
            Console.WriteLine("1. Doctor Mode\r\n2. Student Mode\r\n3. Exit");
            Console.WriteLine("Enter Choice");
            return Convert.ToInt32(Console.ReadLine());
        }
        static void Main()
        {
            List<List<Question>> q = new List<List<Question>>();
            q.Add(new List<Question>());
            q.Add(new List<Question>());
            q.Add(new List<Question>());
            

            while (true) {

                switch(MainMenu()){
                    case 1:
                        Doctor.DocMenu(q);
                        break;
                    case 2:
                        Student.studMenu(q);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}
