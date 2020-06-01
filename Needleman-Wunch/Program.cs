using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Needleman_Wunch
{
    class Program
    {
        public static void Main(string[] args)
        {
            string[] seqSokuma = File.ReadAllLines("seqS.txt");
            string[] seqTokuma = File.ReadAllLines("SeqT.txt");
            string SeqS = seqSokuma[1];
            string SeqT = seqTokuma[1];
            int GAP;
            int MATCH;
            int MISSMATCH;
            int SeqSCnt = SeqS.Length + 1;
            int SeqTCnt = SeqT.Length + 1;
            int[,] NwMatrix = new int[SeqTCnt, SeqSCnt];

            //kullanıcıdan Gap Match ve Missmatch deperlerini alalım

            Console.WriteLine("Gap penalty:");
            GAP = int.Parse(Console.ReadLine());
            Console.WriteLine("Match point:");
            MATCH = int.Parse(Console.ReadLine());
            Console.WriteLine("MissMatch penalty:");
            MISSMATCH = int.Parse(Console.ReadLine());


            //ilk başta tabloyu sıfırlar ile doldurdum.
            for (int i = 0; i < SeqTCnt; i++)
            {
                for (int j = 0; j < SeqSCnt; j++)
                {
                    NwMatrix[i, j] = 0;
                }
            }



            //Matri'in elemanlarının doldurulması.
            //Her bir elemanın solu , üstü ve sol çaprazı kontrol edilip max olan sayı yazılacak.
            TableFilling(SeqSCnt, SeqTCnt, SeqS, SeqT, NwMatrix, MATCH, MISSMATCH, GAP); // Table Filling Function
            //Doldurma evresi sonu.



            //Matrix'in tabloya yazılma fonksiyonu.
            Table_to_BlackBoard(SeqTCnt, SeqSCnt, NwMatrix); // Table To Blackboard Function
            //Console.ReadLine();
            //Matrix son.


            //TraceBack evresi
            char[] SeqSArray = SeqS.ToCharArray();
            char[] SeqTArray = SeqT.ToCharArray();

            string NewSeqA = string.Empty; // s için
            string NewSeqB = string.Empty; // t için

            int b = SeqSCnt - 1; //s için
            int a = SeqTCnt - 1; //t için

            TraceBackNw(a, b, NewSeqA, NewSeqB, SeqSArray, SeqTArray, NwMatrix, GAP, MATCH, MISSMATCH); //Traceback Function

            //Display the result
            Console.Write(Environment.NewLine);
            Console.ReadLine();

        }
        static void TraceBackNw(int a, int b, string NewSeqA, string NewSeqB, char[] SeqSArray, char[] SeqTArray, int[,] NwMatrix, int GAP, int MATCH, int MISSMATCH)
        {
            while (a > 0 || b > 0)
            {
                int scoreTemp = 0;

                if (a == 0 && b > 0)
                {
                    NewSeqA = SeqSArray[b - 1] + NewSeqA;
                    NewSeqB = "-" + NewSeqB;
                    b = b - 1;
                }
                else if (b == 0 && a > 0)
                {
                    NewSeqA = "-" + NewSeqA;
                    NewSeqB = SeqTArray[a - 1] + NewSeqB;
                    a = a - 1;
                }
                else
                {
                    if (SeqTArray[a - 1] == SeqSArray[b - 1])
                        scoreTemp = MATCH;
                    else
                        scoreTemp = MISSMATCH;

                    if (a > 0 && b > 0 && NwMatrix[a, b] == NwMatrix[a - 1, b - 1] + scoreTemp)
                    {
                        if (NwMatrix[a, b] == NwMatrix[a, b - 1] + GAP)
                        {
                            NewSeqA = SeqSArray[b - 1] + NewSeqA;
                            NewSeqB = "-" + NewSeqB;
                            b = b - 1;
                        }
                        else
                        {
                            NewSeqA = SeqSArray[b - 1] + NewSeqA;
                            NewSeqB = SeqTArray[a - 1] + NewSeqB;
                            a = a - 1;
                            b = b - 1;
                        }

                    }
                    else if (b > 0 && NwMatrix[a, b] == NwMatrix[a, b - 1] + GAP)
                    {
                        NewSeqA = SeqSArray[b - 1] + NewSeqA;
                        NewSeqB = "-" + NewSeqB;
                        b = b - 1;
                    }
                    else
                    {
                        NewSeqA = "-" + NewSeqA;
                        NewSeqB = SeqTArray[a - 1] + NewSeqB;
                        a = a - 1;
                    }
                }
            }
            Console.WriteLine(NewSeqA);
            Console.WriteLine(NewSeqB);
        }
        static void TableFilling(int SCount ,int TCount ,string S ,string T,int [,]NwMatrix,int MATCH,int MISSMATCH,int GAP)
        {
            for (int i = 1; i < TCount; i++)
            {
                for (int j = 1; j < SCount; j++)
                {
                    int scoretemp = 0;
                    if (S.Substring(j - 1, 1) == T.Substring(i - 1, 1))//eğer eşleşirse
                    {
                        scoretemp = NwMatrix[i - 1, j - 1] + MATCH;
                    }
                    else                                               //eşleşmez ise
                    {
                        scoretemp = NwMatrix[i - 1, j - 1] + MISSMATCH;
                    }
                    int ScoreLeft = NwMatrix[i, j - 1] + GAP;
                    int ScoreUp = NwMatrix[i - 1, j] + GAP;
                    int maxScore = Math.Max(Math.Max(scoretemp, ScoreLeft), ScoreUp);
                    NwMatrix[i, j] = maxScore;
                }
            }
        }
        static void Table_to_BlackBoard(int SeqTCnt,int SeqSCnt , int [,] NwMatrix)
        {
            for (int i = 0; i < SeqTCnt; i++)
            {
                for (int j = 0; j < SeqSCnt; j++)
                {
                    if (NwMatrix[i, j] >= 10)
                    {
                        Console.Write(" ");
                    }
                    else if (NwMatrix[i, j] < 0)
                    {
                        Console.Write(" ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                    Console.Write(NwMatrix[i, j]);
                }
                Console.Write(Environment.NewLine);
            }
        }
    }
}
