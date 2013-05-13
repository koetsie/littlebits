using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ps.security;
using System.Diagnostics; 





    class Program
    {




        static void Main(string[] args){
            
            
            
            Console.WriteLine("");
            Console.WriteLine("Test nr 1. Testing frequency of appearance");
            Program.DrawNumbersPerDigitTest();
            Console.WriteLine("Any key to start next test");
            Console.ReadKey();
             
             Console.WriteLine("");
             
            
            Console.WriteLine("Test 2 draw 10.000 digits");
            Program.DrawDigitTest(10000);
             
            Console.WriteLine("Done. Any key to start next test");
            Console.ReadKey();





             Console.WriteLine("");
             Console.WriteLine("50 x Random 4 digit pin");
             for (int i = 50; i >= 1; i--)
             {
                 Console.WriteLine(GetPin(4));
             }

             Console.WriteLine("Done.");
             Console.ReadKey();  
         }




        // returns digits as string c is the number of digits
        public static string GetPin(int c) {
            StringBuilder b = new StringBuilder();

          
            for (int i = c; i >= 1; i--)
            {
              
                int n = ps.security.RandomDigits.getDigit();
                b.Append(n.ToString()); 

            }
            return b.ToString();
        }


   
        // Draws a the digits 0-9 
        // each digit(from 0-9) is being drawn untill it appears
        // At the end it shows the Total draws needed for all didigts to appear.
        // and the avg number of draws per digit.

     public static int[] DrawNumbersPerDigitTest()
      {

            Console.WriteLine("Random digits  0-9 generator test.");

            double frequency = System.Diagnostics.Stopwatch.Frequency;
            double nanosecPerTick = (1000 * 1000 * 1000) / frequency;

            int totalItter = 0;
            int[] Totaldistri = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int AllDigits = 9; AllDigits >= 0; AllDigits--)
            {


                Stopwatch timer = new Stopwatch();

                int avg = 0;
                int itters = 0;
                int[] distri = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

                timer.Start();

                for (int i = 1000000; i >= 1; i--)
                {

                    int n = ps.security.RandomDigits.getDigit();

                    avg = (avg + n);
                    distri[n]++;
                    itters++;
                    if (n == AllDigits)
                    {
                        break;
                    }
                };
                timer.Stop();
                
                double durNanosec = timer.ElapsedTicks * nanosecPerTick;
                double durSecs = timer.ElapsedTicks * 1000 / frequency;
                
                Console.WriteLine("");
                Console.WriteLine("|----------------------------------|");
                Console.WriteLine("Digit nr:\t" + AllDigits);
                Console.WriteLine("");
                Console.WriteLine("|  int\t|  freq\t percentage\t   |");
                Console.WriteLine("|----------------------------------|");

                for (int i = 9; i >= 0; i--)
                {
                    double p =Math.Round( ((double)distri[i])/((double)itters / 100),3);
                    Console.WriteLine("|  " + i + "\t|  " + +distri[i] + "\t " + p + "%\t");
                }
                Console.WriteLine("|----------------------------------|");
                Console.WriteLine("");
                Console.WriteLine("  Sum     : >\t" + avg.ToString());
                Console.WriteLine("  Average : >\t" + ((double)avg / (double)itters).ToString());
                Console.WriteLine("  Count   : >\t" + (itters).ToString());
                Console.WriteLine("");
                Console.WriteLine("  nSecs   :\t" + Math.Round(durNanosec, 3));
                Console.WriteLine("  mSecs   :\t" + Math.Round(durSecs, 9));

                totalItter += (itters);
                Totaldistri[AllDigits] = itters;
            };

            Console.WriteLine("");
            Console.WriteLine("|----------------------------------|");
            Console.WriteLine("Average + total draws per Digit:");
            Console.WriteLine("|----------------------------------|");
            Console.WriteLine("");
            Console.WriteLine("  total draws\t: >\t" + totalItter.ToString());
            Console.WriteLine("  avg\t\t: >\t" + Math.Round((double)totalItter / 10, 3));

            Console.WriteLine("|  int\t|  freq\t ");
            Console.WriteLine("|----------------------------------|");

            for (int i = 9; i >= 0; i--)
            {
                Console.WriteLine("|  " + i + "\t|  " + +Totaldistri[i] + "\t \t");
            }


            return Totaldistri;

      }



     // Draws a the digits 0-9 
     // each digit(from 0-9) is being drawn untill it appears
     // At the end it shows the Total draws needed for all didigts to appear.
     // and the avg number of draws per digit.

     public static void DrawDigitTest(int nrOfDigits)
     {

         Console.WriteLine("Random digits  0-9 generator test.");

         double frequency = System.Diagnostics.Stopwatch.Frequency;
         double nanosecPerTick = (1000 * 1000 * 1000) / frequency;
        
         Stopwatch timer = new Stopwatch();

             int avg = 0;
             int itters = 0;
             int[] distri = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

             timer.Start();

             for (int i = nrOfDigits; i >= 1; i--)
             {

                 int n = ps.security.RandomDigits.getDigit();

                 avg = (avg + n);
                 distri[n]++;
                 itters++;
                 
             };
             timer.Stop();

             double durNanosec = timer.ElapsedTicks * nanosecPerTick;
             double durSecs = timer.ElapsedTicks * 1000 / frequency;

            
             Console.WriteLine("");
             Console.WriteLine("| int\t|  freq\t percentage\t   |");
             Console.WriteLine("|----------------------------------|");

             for (int i = 9; i >= 0; i--)
             {
                 double p = Math.Round(((double)distri[i]) / ((double)itters / 100), 1);
                 Console.WriteLine("| " + i + "\t| " + +distri[i] + "\t\t" + p + "%.");
             }
             Console.WriteLine("|----------------------------------|");
             Console.WriteLine("");
             Console.WriteLine("  #Digits : >\t\t" + (itters).ToString());
             Console.WriteLine("  Sum     : >\t\t" + avg.ToString());
             Console.WriteLine("  Average : >\t\t" + ((double)avg / (double)itters).ToString());
             
             Console.WriteLine("");
             Console.WriteLine("  nSecs   : >\t" + Math.Round(durNanosec, 3));
             Console.WriteLine("  mSecs   : >\t" + Math.Round(durSecs, 9));
     
     }


       

     }


        


    

