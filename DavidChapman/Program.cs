using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
/* The original grammar given in the instructions was:
 * E->E+T | E-T | T
 * T->T*F | T/F | F
 * F->n | (E) | v
 * Left recursion must be removed, which leaves the following:
 * E->TEp
 * Ep-> +TEp |-TEp 
 * T->FTp
 * Tp-> *FTp | /FTp 
 * F-> n | v | (E)
 * This grammar is the basis of this application.
 */

namespace DavidChapman
{
    class Program
    {
        //This queue will hold the test string.  As each character is consumed, it is dequeued.
        static Queue<char> parseQueue = new Queue<char>();
       //result will tell whether or not the string is accepted
        static bool result = true;
        //Regular expression to look for any Letters Uppercase or lower case
        static Regex variable = new Regex(@"[A-Za-z]");
        //Regular expression to look for any Numbers 0-9
        static Regex integer = new Regex(@"[0-9]");
        //Variable to check if a number or variable has been seen
        static bool varNumCheck = false;
        //Check for signs/operators
        static bool signCheck = false;
        static bool opCheck = true;
        //Check for decimals
        static bool decCheck = true;


        public static void Main(string[] args)
        {
            //Add chars from args[0] into the queue
            foreach (char c in args[0]) parseQueue.Enqueue(c);
            //Run through the grammar
            E();
            //At the end checks to see if the expression was accepted or rejected
            if (parseQueue.Count == 0) Console.WriteLine(result);
            else Console.WriteLine("False");
        }
        //Grammar rules:  This is the starting rule
        public static void E()
        {
            T();
            Ep();
        }
        public static void Ep()
        {
            //Only runs when I see a + or -
            if (parseQueue.Count > 0 && matchEp())
            {
                parseQueue.Dequeue();
                T();
                Ep();
            }
        }
        public static void T()
        {
            F();
            Tp();
        }
        public static void Tp()
        {
            //Only runs when I see a * or /
            if (parseQueue.Count > 0 && matchTp())
            {
                parseQueue.Dequeue();
                F();
                Tp();
            }
        }
        public static void F()
        {
            //Make sure the queue is not empty
            if (parseQueue.Count > 0 && parseQueue.Peek() == '(')
            {
                parseQueue.Dequeue();
                E();
                if (parseQueue.Count == 0) result = false;
                else if (parseQueue.Peek() != ')')
                {
                    //If there is no )
                    parseQueue.Dequeue();
                    result = false;
                }
                else
                {
                    parseQueue.Dequeue();
                    result = true;
                }
            }
            //If a ) comes before a (
            else if (parseQueue.Count > 0 && parseQueue.Peek() == ')')
            {
                //E();
                while (parseQueue.Count > 0) parseQueue.Dequeue();
                result = false;
            }
            //If there are no parenthesis
            else if (match()){ }
        }
        //match() checks for valid variables and integers
        public static bool match()
        {
            //Make sure the queue is not empty
            if (parseQueue.Count > 0)
            {
                //Check for valid variables
                if (variable.IsMatch(parseQueue.Peek().ToString()))
                {
                    varNumCheck = true;
                    result = true;
                    signCheck = false;
                    parseQueue.Dequeue();
                    //Remove valid variable names
                    while (parseQueue.Count != 0 && (variable.IsMatch(parseQueue.Peek().ToString()) || integer.IsMatch(parseQueue.Peek().ToString())))
                    {
                        parseQueue.Dequeue();
                    }
                    return true;
                }
                //Check for integers
                else if (integer.IsMatch(parseQueue.Peek().ToString()) || (parseQueue.Peek() == '.'))
                {
                    varNumCheck = true;
                    result = true;
                    signCheck = false;
                    //Record if a decimal has been seen
                    if (parseQueue.Count > 0 && parseQueue.Peek() == '.' && decCheck)
                    {
                        decCheck = false;
                    }
                    parseQueue.Dequeue();
                    //Make sure the queue is not empty 
                    if (parseQueue.Count > 0)
                    {
                        //Read in multi-digit integers
                        while (parseQueue.Count > 0 && integer.IsMatch(parseQueue.Peek().ToString()) || parseQueue.Peek() == '.' && decCheck)
                        {
                            //Record if a decimal has been seen
                            if (parseQueue.Peek() == '.') decCheck = false;
                            parseQueue.Dequeue();
                        }
                    }
                    if (parseQueue.Count > 0 && (variable.IsMatch(parseQueue.Peek().ToString()) || parseQueue.Peek() == '.' && !decCheck))
                    {
                        //If the variable name starts with a number or an integer has more than one decimal
                        result = false;
                        while (parseQueue.Count > 0) parseQueue.Dequeue();
                        return false;
                    }
                    decCheck = true;
                    return true;
                }
                else result = false;
            }
            return false;
        }
        //matchEp() checks for + or -
        static public bool matchEp()
        {
            //Make sure the queue is not empty
            if (parseQueue.Count > 0)
                if (signCheck)
                {
                    //If the previous character was a sign
                    result = false;
                    while (parseQueue.Count > 0) parseQueue.Dequeue();
                    return false;
                }
                else
                {
                    if (opCheck)
                    {
                        if ((parseQueue.Peek() == '+' || parseQueue.Peek() == '-') && varNumCheck)
                        {
                            signCheck = true;
                            return true;
                        }
                    }
                    else
                    {
                        while (parseQueue.Count > 0)
                        {
                            parseQueue.Dequeue();
                            opCheck = false;
                        }
                    }

                }
            return false;
        }
        //matchTp() checks for * or /
        static public bool matchTp()
        {
            //Make sure the queue is not empty
            if (parseQueue.Count > 0)
                if (signCheck)
                {
                    //If the previous character was a sign
                    result = false;
                    while (parseQueue.Count > 0) parseQueue.Dequeue();
                    return false;
                }
                else
                {
                    if ((parseQueue.Peek() == '*' || parseQueue.Peek() == '/') && varNumCheck)
                    {
                        signCheck = true;
                        return true;
                    }
                }
        
            return false;
        }

    }
}
