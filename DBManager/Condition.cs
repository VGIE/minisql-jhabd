using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using DbManager;

namespace DbManager
{
    public class Condition
    {
        public string ColumnName { get; private set; }
        public string Operator { get; private set; }
        public string LiteralValue { get; private set; }

        public Condition(string column, string op, string literalValue)
        {
            //TODO DEADLINE 1A: Initialize member variables

            ColumnName = column;
            Operator = op;
            LiteralValue = literalValue;
            
        }


        public bool IsTrue(string value, ColumnDefinition.DataType type)
        {
            //TODO DEADLINE 1A: return true if the condition is true for this value
            //Depending on the type of the column, the comparison should be different:
            //"ab" < "cd
            //"9" > "10"
            //9 < 10
            //Convert first the strings to the appropriate type and then compare (depending on the operator of the condition)
            

            switch (type)
            {
                case ColumnDefinition.DataType.String:
                if (Operator == "<")
                    {
                        if (value.CompareTo(LiteralValue) < 0)
                            return true;
                    }
                    else if (Operator == ">")
                    {
                        if (value.CompareTo(LiteralValue) > 0)
                            return true;
                    }
                    else if (Operator == "<=")
                    {
                        if (value.CompareTo(LiteralValue) <= 0)
                            return true;
                    }
                    else if (Operator == ">=")
                    {
                        if (value.CompareTo(LiteralValue) >= 0)
                            return true;
                    }
                    else if (Operator == "=")
                    {
                        if (value == LiteralValue)
                            return true;
                    }
                    else if (Operator == "!=")
                    {
                        if (value != LiteralValue)
                            return true;
                    }
                    return false;
                
                case ColumnDefinition.DataType.Int:

                if (int.TryParse(value, out int intValue) && 
                        int.TryParse(LiteralValue, out int intLiteral))
                    {
                        if (Operator == "<")
                        {
                            if (intValue < intLiteral)
                                return true;
                        }
                        else if (Operator == ">")
                        {
                            if (intValue > intLiteral)
                                return true;
                        }
                        else if (Operator == "<=")
                        {
                            if (intValue <= intLiteral)
                                return true;
                        }
                        else if (Operator == ">=")
                        {
                            if (intValue >= intLiteral)
                                return true;
                        }
                        else if (Operator == "=")
                        {
                            if (intValue == intLiteral)
                                return true;
                        }
                        else if (Operator == "!=")
                        {
                            if (intValue != intLiteral)
                                return true;
                        }
                    }
                    return false;

                case ColumnDefinition.DataType.Double:
                    if (double.TryParse(value, CultureInfo.InvariantCulture, out double doubleValue) && 
                        double.TryParse(LiteralValue, CultureInfo.InvariantCulture, out double doubleLiteral))
                    {
                        if (Operator == "<")
                        {
                            if (doubleValue < doubleLiteral)
                                return true;
                        }
                        else if (Operator == ">")
                        {
                            if (doubleValue > doubleLiteral)
                                return true;
                        }
                        else if (Operator == "<=")
                        {
                            if (doubleValue <= doubleLiteral)
                                return true;
                        }
                        else if (Operator == ">=")
                        {
                            if (doubleValue >= doubleLiteral)
                                return true;
                        }
                        else if (Operator == "=")
                        {
                            if (doubleValue == doubleLiteral)
                                return true;
                        }
                        else if (Operator == "!=")
                        {
                            if (doubleValue != doubleLiteral)
                                return true;
                        }
                    }
                    return false;

            }

            
            
    
                return false;
        }
    }
    
}