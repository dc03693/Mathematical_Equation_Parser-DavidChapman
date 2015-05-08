using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
/*This application is based on the attached PDA.  It will return true or false
 * based on whether or not the test string is a valid arithmetic expression*/

namespace DavidChapman
{
    class Program
    {
        //parseQueue holds the test string.  As characters are consumed, they are dequeued
        static Queue<char> parseQueue = new Queue<char>();
        //pdaStack will be used as the stack for the PDA
        static Stack<char> pdaStack = new Stack<char>();
        //result holds the boolean result of the test string
        static bool result = true;
        //Regular expression for variables
        static Regex variable = new Regex(@"[A-Za-z]");
        //Regular expression for integers
        static Regex integer = new Regex(@"[0-9]");
        static void Main(string[] args)
        {
            //Add chars from arg[0] into parseQueue
            foreach (char c in args[0]) parseQueue.Enqueue(c);
            //Transition to the start state
            startState();
            //Check the pdaStack
            checkStack();
            //Print out the result when the queue is empty
            if (parseQueue.Count == 0) Console.WriteLine(result);
            else Console.WriteLine("False");

        }
        //The start state
        public static void startState()
        {
            //Make sure the queue is not empty
            if (parseQueue.Count > 0)
            {
                //If I see a ( add a ( to the stack
                if (parseQueue.Peek() == '(')
                {
                    pdaStack.Push('(');
                    parseQueue.Dequeue();
                    startState();
                }
                //Move to fail state
                else if (pdaStack.Count==0 && parseQueue.Peek() == ')')
                {
                    result = false;
                    while (parseQueue.Count != 0) parseQueue.Dequeue();
                    return;
                }
                //If I see a valid variable name
                else if (variable.IsMatch(parseQueue.Peek().ToString()) && parseQueue.Count>0)
                {
                    result = true;
                    parseQueue.Dequeue();
                    if (parseQueue.Count > 0)
                    {
                        result = true;
                        //Check for valid variable names with letters before integers
                        while (parseQueue.Count != 0 && (variable.IsMatch(parseQueue.Peek().ToString()) || integer.IsMatch(parseQueue.Peek().ToString())))
                        {
                            parseQueue.Dequeue(); 
                        }
                    }
                    else return;
                    //Transition to state2
                    state2();
                }
                //If I see a valid number
                else if (integer.IsMatch(parseQueue.Peek().ToString()) && parseQueue.Count>0)
                {
                    //Check for invalid variable names
                    if (variable.IsMatch(parseQueue.Peek().ToString()) && parseQueue.Count>0)
                    {
                        result = false;
                        while (parseQueue.Count > 0) parseQueue.Dequeue();
                    }
                    //Check for integers longer than one character
                    else if (integer.IsMatch(parseQueue.Peek().ToString()))
                    {
                        result = true;
                        parseQueue.Dequeue();
                        if (parseQueue.Count > 0)
                        {
                            while (parseQueue.Count != 0 && integer.IsMatch(parseQueue.Peek().ToString())) 
                            {
                                parseQueue.Dequeue(); 
                            }
                        }
                        //Check for decimal
                        if (parseQueue.Count != 0 && parseQueue.Peek() == '.')
                        {
                            //Decimal state is not final
                            result = false;
                            parseQueue.Dequeue();
                            if (parseQueue.Count > 0)
                            {
                                //If a decimal is read in in the decimal state, move to fail state
                                if (parseQueue.Peek() == '.')
                                {
                                    result = false;
                                    while (parseQueue.Count != 0)
                                    {
                                        parseQueue.Dequeue();
                                    }
                                }
                                else if (parseQueue.Count != 0 && integer.IsMatch(parseQueue.Peek().ToString()))
                                {
                                    result = true;
                                    while (parseQueue.Count != 0 && integer.IsMatch(parseQueue.Peek().ToString()))
                                    {
                                        parseQueue.Dequeue();
                                    }
                                    state2();
                                }
                            }
                        }//End Decimal check
                        state2();
                    }
                    else { state2(); }
                    state2();
                }
                else
                {
                    while (parseQueue.Count > 0) parseQueue.Dequeue();
                    result =  false;
                }
            }
            else { return; }
        }
        public static void state2()
        {
            if(parseQueue.Count>0)
            {
                //If a ) is read in
                if (pdaStack.Count!=0 && parseQueue.Peek() == ')')
                {
                    //Make sure there is a ( on the stack
                    if (pdaStack.Peek() == '(')
                    {
                        pdaStack.Pop();
                        parseQueue.Dequeue();
                        state2();
                    }
                    //If there is not a ( on the stack
                    else
                    {
                        result = false;
                        while (parseQueue.Count != 0) parseQueue.Dequeue();
                        return;
                    }
                }
                //An operator moves back to the start state
                else if (parseQueue.Peek() == '+' || parseQueue.Peek() == '-' || parseQueue.Peek() == '*' || parseQueue.Peek() == '/')
                {
                    parseQueue.Dequeue();
                    result = false;
                    startState();
                }
            }
            else { return; }
        }
        //Make sure the stack is empty
        public static void checkStack()
        {
            if (pdaStack.Count == 0 && result) result = true;
            else result = false;
        }

    }
}
