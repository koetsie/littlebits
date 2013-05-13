
// The goal is to return a strong pseudo Random Digit between 0 and 9.
// using the RandomNumberGenerator RNGCryptoServiceProvider from the
// System.Security.Cryptography Namespace. 
//
// if we take the int 0,1,2,3 and pick Randomly 3 x One then the sum of the 
// three draws wil by a minimum value of 0 and a Maximum value of 9. 
// 
// If we just look at the fact if a Byte value is odd or even and use 2 bytes per draw
// we should get a fair Psuedo Random result. I think :-)
//
//====================================================================================
//  So lets assume we have a Byte Array X={a,b,c,d,e,f} values{1,34,55,147,255,6}
//
//           [a,b]  = odd/even   [c,d]          =odd/odd       [e,f]    = odd/even
//           [1][34]= 3 (r1)     [55][147]      =3  (r2)       [255][6] = 2 (r3)
//
// Result: The sum of the 3x2 byte pairs is 3 + 3 + 2 = >>>>> 8
//=====================================================================================
//
// Each round we opposite the Odd/even values. For better distibution
// This was also done in the Hot Bits Radiation project. Ater testing the 
// Bell curve was better distributed.
//
//
// first round  2 bytes
//0	00	25%         if   byte(0) odd     &&    byte(1) odd  =0
//1	01	25%         if   byte(0) even    &&    byte(1) odd  =1
//2	10	25%         if   byte(0) even    &&    byte(1) even =2  
//3	11	25%         if   byte(0) odd     &&    byte(1) even =3 
//----- > sum1 = [0-3]
// second  round 2 bytes (notice we reverse the values)
//0	00	25%         if   byte(0) odd     &&    byte(1) odd  =3
//1	01	25%         if   byte(0) even    &&    byte(1) odd  =2
//2	10	25%         if   byte(0) even    &&    byte(1) even =1  
//3	11	25%         if   byte(0) odd     &&    byte(1) even =0 
 
//----- >sum2 = [0-3]
// third round  2 byte (notice we reverse the values)
//0	00	25%         if   byte(0) odd     &&    byte(1) odd  =1
//1	01	25%         if   byte(0) even    &&    byte(1) odd  =3
//2	10	25%         if   byte(0) even    &&    byte(1) even =0  
//3	11	25%         if   byte(0) odd     &&    byte(1) even =2 
//----- > sum3 =  [0-3]
//
//
// the integer of value between 0-9 is  (sum1 + sum 2 + sum 3)
//
// If you like it please mention my name
// Marc Koutzarov The Netherlands
// For comments ######






using System;
using System.Security.Cryptography;
using System.Text;

namespace ps.security
{
    public class RandomDigits
    {

        

        ///<summary>
        /// Returns One random Digit of a value between 0 and 9.
        ///</summary>
        
        
        public static int getDigit(){
            int[] Result = { 0, 0, 0 }; // this stores sum1 , sum2 and sum3 
            int R = 0; // Will hold the resulting integer Sum of Result{3}
            int b1 = 0; // byte one
            int b2 = 0; // byte two 
            int offset = 0; // need an offset to loop 3 times. Each time i get 2 bytes to compare.
          
            //get 6 random bytes
            RandomNumberGenerator rng = new RNGCryptoServiceProvider();
            
            byte[] b = new byte[6];
             
            rng.GetNonZeroBytes(b);


            for (int i =0; i <=2; i++)  // Loop 3 time  i=2 i=1 and i=0
               { 

               //get the offset 
                if (i == 2) {
                    offset = 4; // 5th and 6th byte 
                }else if(i == 1) {
                    offset = 2; // 3th and 4th byte 
                }else if (i == 0){
                    offset = 0; // 1th and 2th byte
                };




                //(b1,2 MOD 2) b1 and b2 will be odd number is 1 and even if 0
                b1 = ((int)b[offset] % 2);
                b2 = ((int)b[offset+1] % 2);


                if ((i == 0))
                {
                    if ((b1 == 1) && (b2 == 1))
                    {
                        Result[i] = 0;              //odd-odd   = 0
                    }
                    else if ((b1 == 0) && (b2 == 1))
                    {
                        Result[i] = 1;              //even-odd  = 1
                    }
                    else if ((b1 == 0) && (b2 == 0))
                    {
                        Result[i] = 2;              //even-even = 2  
                    }
                    else if ((b1 == 1) && (b2 == 0))
                    {
                        Result[i] = 3;
                    };           //odd-even  = 3
                }
                else if (i==1) {
                    // on second round flip the values 
                    if ((b1 == 1) && (b2 == 1))
                    {
                        Result[i] = 3;              //odd-odd   = 3
                    }
                    else if ((b1 == 0) && (b2 == 1))
                    {
                        Result[i] = 2;              //even-odd  = 2
                    }
                    else if ((b1 == 0) && (b2 == 0))
                    {
                        Result[i] = 1;              //even-even = 1  
                    }
                    else if ((b1 == 1) && (b2 == 0))
                    {
                        Result[i] = 0;          //odd-even  = 0
                    };

                }
                else if (i == 2) {

                    // on 3 round  flip the values once more
                    if ((b1 == 1) && (b2 == 1))
                    {
                        Result[i] = 1;              //odd-odd   = 1
                    }
                    else if ((b1 == 0) && (b2 == 1))
                    {
                        Result[i] = 3;              //even-odd  = 3
                    }
                    else if ((b1 == 0) && (b2 == 0))
                    {
                        Result[i] = 0;              //even-even = 0  
                    }
                    else if ((b1 == 1) && (b2 == 0))
                    {
                        Result[i] = 2;          //odd-even  = 2
                    };

                }


                b1 = 0;
                b2 = 0;

            };// end 3 for loops




            //Sum the results of round 1,2 and 3 together
            //this will make an Int between 0 - 9 
            R =(int)(Result[0] + Result[1] + Result[2]);

            //Return a Int valued between 0 - 9
            return R;

        }


       
    }
}
